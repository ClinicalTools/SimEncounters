using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine.Networking;

public class HistoryFieldManagerScript : MonoBehaviour {

    public GameObject[] loadedPrefabs;
    public String[] loadedPrefabsDisplayNames;
    private List<LabEntryScript> entryList;
    public GameObject parentTab;
    private XmlDocument xmlDoc;
    private TabManager tm;
    private DataScript ds;
    public bool isNested;
    public bool isQuizPanel;
    public bool isStatic;
    private Transform quizPin;
    private bool populating;
    private string uniquePath;
    private bool willScroll = false;
    private Transform currentPin;
    private GameObject BG;
    private string uid;
    private bool addData;
    private HistoryFieldManagerScript parentManager;

    /**
	 * This script is used to manage any tab that has addable entries/panels to it. These can be nested as well
	 * (nested has only been tested one level deep)
	 */


    void Awake()
    {
        entryList = new List<LabEntryScript>();
        BG = GameObject.Find("GaudyBG");
        tm = BG.GetComponent<TabManager>();
        ds = BG.GetComponent<DataScript>();
    }


    // Use this for initialization
    void Start()
    {
        if (isQuizPanel && !populating) {
            return;
        }
        populating = false;
        GameObject BG = GameObject.Find("GaudyBG");
        tm = BG.GetComponent<TabManager>();
        ds = BG.GetComponent<DataScript>();

        if (isQuizPanel && quizPin == null) {
            return;
        }
        //Finds the tab this manager belongs to
        /*if (false && isQuizPanel) {
			parentTab = quizPin.GetComponentsInParent<Transform>().ToList().Find((Transform x) => x.name.EndsWith("Tab")).gameObject;
            Debug.Log(parentTab.transform.name);
		} else {*/
        Transform tempTrans = transform;
        while (parentTab == null && tempTrans != null) {
            if (tempTrans.parent != null && tempTrans.parent.GetComponent<HistoryFieldManagerScript>() != null) {
                parentTab = tempTrans.parent.GetComponent<HistoryFieldManagerScript>().parentTab;
                break;
            } else {
                tempTrans = tempTrans.parent;
            }
        }
        //}
        if (parentTab == null) {
            Debug.Log("Cannot access parent tab!");
            return;
        }
        parentManager = transform.parent.GetComponentInParent<HistoryFieldManagerScript>();
        addData = true;
        FindUniqueParent();
    }

    /**
	 * returns the pin that was clicked to open the quiz dialogue
	 */
    public GameObject GetPin()
    {
        return currentPin.gameObject;
    }

    /**
	 * Call this to refresh the uniquePath variable. Useful when renaming sections/tabs
	 */
    public string RefreshUniquePath()
    {
        uniquePath = "";
        Transform tempPin = null;
        if (isQuizPanel) {// && !isNested) {
            tempPin = quizPin;
        } else {
            tempPin = transform;
        }

        if (isQuizPanel && quizPin != null && !quizPin.GetComponentInParent<HistoryFieldManagerScript>()) {
            Transform uniqueParent;
            uniqueParent = quizPin;
            uniquePath = "";
            while (uniqueParent.parent != null && !uniqueParent.parent.name.Equals("Content")) {
                if (uniqueParent.parent.GetComponent<HistoryFieldManagerScript>() != null) {
                    break;
                }
                uniqueParent = uniqueParent.parent;
            }
            uniquePath = uniqueParent.name;
            if (parentTab == null && isNested && !isQuizPanel) {
                parentTab = transform.parent.GetComponentInParent<HistoryFieldManagerScript>().parentTab;
            } else if (parentTab == null) {
                parentTab = uniqueParent.GetComponentsInParent<Transform>().ToList().Find((Transform x) => x.name.EndsWith("Tab")).gameObject;
            }
            //Debug.Log (parentTab.name);
            while (!uniqueParent.name.Equals(tm.getCurrentTab() + "Tab")) {
                uniqueParent = uniqueParent.parent;
                if (uniqueParent == null) {
                    break;
                }
                uniquePath = uniqueParent.name + "/" + uniquePath;
            }
            uniquePath = tm.getCurrentSection() + "/" + uniquePath;
        } else {
            while (tempPin != null) {
                if (tempPin.name.StartsWith("LabEntry:")) {
                    if (!isQuizPanel) {
                        uniquePath = "LabEntry: " + tempPin.GetSiblingIndex() + uniquePath;
                    } else {
                        if (!string.IsNullOrEmpty(uniquePath))
                            uniquePath = tempPin.name + uniquePath;
                        else
                            uniquePath = tempPin.name;
                    }
                    //Debug.Log("UNIQUE PATh: " + uniquePath);

                }
                if (tempPin.name.EndsWith("Tab")) {
                    uniquePath = tempPin.name + "/" + uniquePath;
                    //Debug.Log("UNIQUE PATh: " + uniquePath);

                }
                tempPin = tempPin.parent;
            }
            uniquePath = tm.getCurrentSection() + "/" + uniquePath;
        }

        if (isQuizPanel && isNested) {
            Transform tempTrans = transform;
            while (!tempTrans.parent.GetComponent<HistoryFieldManagerScript>()) {
                tempTrans = tempTrans.parent;
            }
            uniquePath = uniquePath + "::" + Regex.Split(tempTrans.name, "[0-9]*$")[0] + tempTrans.GetSiblingIndex();
            //uniquePath = uniquePath + "::" + transform.GetComponentsInParent<Transform>().ToList().Find((Transform x) => x.name.StartsWith("LabEntry:")).name;
        }

        //Debug.Log("UNIQUE PATh: " + uniquePath);
        return uniquePath;
    }


    /**
	 * Used when loading to find the correct set of XML data to load
	 * Will also be used to load every new quiz that's opened
	 */
    public void FindUniqueParent()
    {
        var y = System.Diagnostics.Stopwatch.StartNew();

        //Gets the xml data for this tab
        string text = "<data></data>";

        //The contents of RefreshUniquePath used to be here
        //Debug.Log (tm.getCurrentTab () + parentTab.GetComponent<Text> ().text);

        RefreshUniquePath();

        if (!isQuizPanel) {
            text = ds.GetData(tm.getCurrentSection(), tm.getCurrentTab()); //+ parentTab.GetComponent<Text> ().text);
        } else {
            string topLevelPath = Regex.Split(uniquePath, "::")[0];
            if (GetComponentInParent<DialogueManagerScript>()) {
                if (ds.GetDialogues().ContainsKey(topLevelPath)) {
                    text = ds.GetDialogues()[topLevelPath];
                }
            } else {
                text = ds.GetQuizData(topLevelPath);
            }
        }

        bool addedNew = false;
        if (parentManager != null) {
            addedNew = !parentManager.CheckAddData();
            if (addedNew) {
                text = "";
                text = null;
            }
        }


        Debug.Log("LOADED TEXT: " + text);

        if (text == null || addedNew) {
            Debug.Log("No Data to load");

            //Open the entry selector panel. This should auto add an entry if there's only one type to add
            if (!isNested && !addedNew) {
                OpenAddEntryPanel();
            }
            return;
        }
        xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(text);
        XmlNode tempNode = xmlDoc.FirstChild;

        //Gets the manager's unique parent. This is necessary for nested managers. 
        int pos = -1;
        string path = "";
        XmlNode uniqueParentNode = tempNode;
        if (isNested) {
            //Finds the unique parent
            Transform uniqueParent = transform;
            /*path = "";
			while (uniqueParent.parent != null && !uniqueParent.parent.name.Equals ("Content")) {
				if (uniqueParent.parent.GetComponent<HistoryFieldManagerScript> () != null) {
					break;
				}
				uniqueParent = uniqueParent.parent;
			}
			pos = uniqueParent.GetSiblingIndex (); //The transform position of the unique parent (so 0 if the data is going to be in the first entry)

			bool fullNested = false;
			//This should set the path to point to Context aka the parent of the unique parent
			if (uniqueParent.parent.GetComponent<HistoryFieldManagerScript> () != null) {
				uniqueParent = uniqueParent.parent;
				fullNested = true;
			}
			path = uniqueParent.name;
			while (!uniqueParent.parent.name.Equals (parentTab.name)) {
				uniqueParent = uniqueParent.parent;
				path = uniqueParent.name + "/" + path;
			}
			//----------------------------------------------------------
			if (isQuizPanel && isNested && quizPin.GetComponentInParent<HistoryFieldManagerScript>()) {
				path = uniquePath + "::" + transform.GetComponentsInParent<Transform>().ToList().Find((Transform x) => x.name.StartsWith("LabEntry:")).name;
			}*/
            bool fullNested = false;
            //This should set the path to point to Context aka the parent of the unique parent
            if (uniqueParent.parent.GetComponent<HistoryFieldManagerScript>() != null) {
                uniqueParent = uniqueParent.parent;
                fullNested = true;
            }

            path = uniquePath;
            //Debug.Log (path);
            while (tempNode != null) { //Find if this is the correct info to load
                if (tempNode.Name.EndsWith("Pin")) {
                    if (tempNode.NextSibling != null) {
                        tempNode = tempNode.NextSibling;
                    }
                } else {
                    if (tempNode.Name.Equals("Parent")) {
                        //Debug.Log (transform.name + ", " + tempNode.InnerText + ", " + path);
                        if (tempNode.InnerText.Equals(path) && tempNode.NextSibling != null) { //If tempNode is currently pointing at the parent of the unique parent
                                                                                               //Debug.Log ("Name: " + tempNode.Name + ": " + tempNode.InnerXml);
                            if (fullNested) { //If this is a fully nested entry, we must account for the <Parent> tab being the first sibling
                                tempNode = tempNode.ParentNode.ChildNodes.Item(pos + 1); //Sets tempNode to it's sibling (the entry in the Parent of the unique parent that holds the info we need)
                                                                                         //if (tempNode != null)
                                                                                         //Debug.Log (tempNode.Name + tempNode.InnerXml);
                            } else {
                                tempNode = tempNode.ParentNode;
                            }
                            break;
                        }
                    }
                    tempNode = AdvNode(tempNode);
                }
            }
            uniqueParentNode = tempNode;
            tempNode = AdvNode(tempNode); //Advance inside the unique parent
        }

        if (tempNode == null) { //No data found
            return;
        }

        //Find the first entry while avoiding going into the next manager's data (if there are any)
        //Doing this to add and spwan in the Entries we'll need for loading into
        while (tempNode != null && !tempNode.Name.Equals("Entry0")) {
            tempNode = AdvNode(tempNode);
            if (uniqueParentNode.NextSibling != null && tempNode.Equals(uniqueParentNode.NextSibling)) {
                break;
            }
        }

        if (tempNode != null && !tempNode.Equals(uniqueParentNode.NextSibling)) {
            while (tempNode != null) { //&& tempNode.ParentNode.HasChildNodes
                XmlNode a = AdvNode(tempNode);
                AddEntry(a.InnerText, false);
                tempNode = tempNode.NextSibling;
            }
        }

        if (entryList.Count != 0 || transform.parent.GetComponent<DialogueManagerScript>()) {
            Load(uniqueParentNode);
        } else {
            //Open the entry selector panel. This should auto add an entry if there's only one type to add
            if (!isNested) {
                if (GetComponent<NewCaseSpawnPanelsScript>()) {
                    GetComponent<NewCaseSpawnPanelsScript>().CreateBlanks();
                } else {
                    OpenAddEntryPanel();
                }
            }
        }

        y.Stop();
        Debug.Log(y.ElapsedMilliseconds);
    }

    /**
	 * Loads the data from XMl
	 */
    private void Load(XmlNode uniqueParent)
    {
        if (entryList.Count == 0) {
            return;
        }
        XmlNode node = uniqueParent; //step inside the parent to begin

        // Tab is blank
        if (node == null) {
            Debug.Log("No data to load!");
            ds.newTabs.Add(parentTab.transform);
            return;
        }
        //-----------------------------------------------------------------------------
        //-------------------------New loading method below----------------------------
        //-----------------------------------------------------------------------------
        //First we need to set up the node so that it can walk along entries
        //We do this by setting it to the first "Entry#" and then referencing NextSibling

        //If the node isn't already on EntryData, get it there
        while (!node.Name.Equals("EntryData")) {
            node = AdvNode(node);

            if (node == null) {
                Debug.Log("No Data to load.");
                return;
            }
        }
        node = AdvNode(node); //Enter the EntryData. This will put Node on "Parent". The entries are all siblings now

        XmlDocument element = new XmlDocument();
        int ii = 0; //This is uesd for referencing labEntries

        var quizId = "";
        while (node.NextSibling != null) {
            node = node.NextSibling; //Move laterally across entries. Node will sit on the "Entry#" nodes.
            element.LoadXml(node.OuterXml);
            LabEntryScript labNodee = entryList[ii];

            //We need to know ahead of time if we need to spawn any pin objects when we reach the PinArea transform
            bool spawnQuiz = false;
            bool spawnDialogue = false;
            bool spawnFlag = false;
            bool spawnEvent = false;
            XmlNode tempNodee;
            if ((tempNodee = element.GetElementsByTagName("QuizPin").Item(0)) != null) {
                spawnQuiz = true;
                XmlNode findParent = tempNodee;
                while (!findParent.Name.Equals("Parent")) {
                    findParent = AdvNode(findParent);
                }
                quizId = findParent.InnerText;
                ds.AddQuiz(findParent.InnerText, tempNodee.InnerXml);
                tempNodee.ParentNode.RemoveChild(tempNodee);
                CreateQuestion(tempNodee.OuterXml); //Tests loading quiz pins into easy to reference objects.
                                                    //Could just load straight into the quizes if desired too. 
            }
            if ((tempNodee = element.GetElementsByTagName("DialoguePin").Item(0)) != null) {
                spawnDialogue = true;
                findUID(node);
                ds.AddDialogue(uid, element.GetElementsByTagName("DialoguePin").Item(0).InnerXml);
                tempNodee.ParentNode.RemoveChild(tempNodee);
                //GiveOutline (labNodee.gObject);
            }
            if ((tempNodee = element.GetElementsByTagName("FlagPin").Item(0)) != null) {
                spawnFlag = true;
            }
            if ((tempNodee = element.GetElementsByTagName("EventPin").Item(0)) != null) {
                spawnEvent = true;
            }


            //If there is a node in the lab entry named "data" that means that there are sub-entries.
            //In that case, we remove the xml for the sub-entries and leave the sub-entries to load its data
            //(The data is only removed as far as the current lab entry is concerned)
            if ((tempNodee = element.GetElementsByTagName("data").Item(0)) != null) {
                tempNodee.ParentNode.RemoveChild(element.GetElementsByTagName("data").Item(0));
            }

            //Iterate over all transform children of the entry to find where we need to load in data
            List<Transform> allChildrenn = labNodee.gObject.GetComponentsInChildren<Transform>(true).ToList<Transform>();
            Transform nestedParent = null;
            foreach (Transform child in allChildrenn) {

                //This helps the transforms ignore sub-entries as well. 
                //If a child transform has a HFMS attached, then we ignore everything that is a child of that child.
                if (child.GetComponent<HistoryFieldManagerScript>() && child.GetComponent<HistoryFieldManagerScript>().isNested) {
                    nestedParent = child;
                    continue;
                }
                if (nestedParent != null && child.IsChildOf(nestedParent)) {
                    continue;
                }

				//New code for EntryValue
				EntryValue.EntryValue ev;
				if ((ev = child.GetComponent<EntryValue.EntryValue>()) != null) {
					print("Loading entry value for " + child.name);
					//This checks the amount of xml tags with the name of the child we're populating and handles things accordingly
					int pos = 0;
					if (element.GetElementsByTagName(ev.GetTag()).Count == 0) {
						continue;
					} else if (element.GetElementsByTagName(ev.GetTag()).Count > 1) {
						//If there is more than one match, find where the current child sits compared to its siblings which share the same name
						List<EntryValue.EntryValue> sameValues = new List<EntryValue.EntryValue>(labNodee.gObject.GetComponentsInChildren<EntryValue.EntryValue>());
						sameValues = sameValues.FindAll((EntryValue.EntryValue val) => val.GetTag().Equals(ev.GetTag()));
						pos = sameValues.FindIndex((EntryValue.EntryValue val) => val.Equals(ev));
						/*
						List<Transform> sameNames = allChildrenn.FindAll((Transform obj) => obj.name.Equals(child.name));
						pos = sameNames.FindIndex((Transform obj) => obj.Equals(child));
						print(child.name + ", " + pos + ", " + labNodee.panelType);*/
					}

					//Assign the value in the XML to a string to make the code easier to read
					string xmlValue = element.GetElementsByTagName(ev.GetTag()).Item(pos)?.InnerText;

					//Set the data. Value is unescaped by the EntryValue type if needed
					ev.SetValue(xmlValue);
				} else {
					//Check to see if the child is actually something we need to populate with data.
					if (child.tag.Equals("Value") || child.tag.Equals("Image") || child.name.EndsWith("Value") || child.name.ToLower().EndsWith("toggle")) {
						//This checks the amount of xml tags with the name of the child we're populating and handles things accordingly
						int pos = 0;
						if (element.GetElementsByTagName(child.name).Count == 0) {
							continue;
						} else if (element.GetElementsByTagName(child.name).Count > 1) {
							//If there is more than one match, find where the current child sits compared to its siblings which share the same name
							List<Transform> sameNames = allChildrenn.FindAll((Transform obj) => obj.name.Equals(child.name));
							pos = sameNames.FindIndex((Transform obj) => obj.Equals(child));
							print(child.name + ", " + pos + ", " + labNodee.panelType);
						}

						//Assign the value in the XML to a string to make the code easier to read
						string xmlValue = element.GetElementsByTagName(child.name).Item(pos).InnerText;
						if (child.name.Equals("characterName")) {
							//int INeedABreakpoint = 0;
						}
						//Set the data according to the type of data field
						if (child.gameObject.GetComponent<InputField>() != null) {
							child.gameObject.GetComponent<InputField>().text = UnityWebRequest.UnEscapeURL(xmlValue);
							if (child.GetComponent<InputFieldResizer>()) {
								child.GetComponent<InputFieldResizer>().ResizeField();
							}
						} else if (child.gameObject.GetComponent<Dropdown>() != null) {
							int indexValue = 0;
							foreach (Dropdown.OptionData myOptionData in child.gameObject.GetComponent<Dropdown>().options) {
								if (myOptionData.text.Equals(UnityWebRequest.UnEscapeURL(xmlValue))) {
									break;
								}
								indexValue++;
							}
							child.gameObject.GetComponent<Dropdown>().value = indexValue;
						} else if (child.gameObject.GetComponent<Toggle>() != null && xmlValue != null && !xmlValue.Equals("")) {
							child.gameObject.GetComponent<Toggle>().isOn = bool.Parse(xmlValue);
						} else if (child.gameObject.GetComponent<Text>() != null) {
							child.gameObject.GetComponent<Text>().text = UnityWebRequest.UnEscapeURL(xmlValue);
						} else if (child.gameObject.GetComponent<TMP_InputField>() != null) {
							child.gameObject.GetComponent<TMP_InputField>().text = UnityWebRequest.UnEscapeURL(xmlValue);
							child.gameObject.GetComponent<TMP_InputField>().onEndEdit.Invoke(UnityWebRequest.UnEscapeURL(xmlValue));
						} else if (child.gameObject.GetComponent<TextMeshProUGUI>() != null) {
							child.gameObject.GetComponent<TextMeshProUGUI>().text = UnityWebRequest.UnEscapeURL(xmlValue);
						} else if (child.gameObject.GetComponent<TMP_Dropdown>() != null) {
							int indexValue = 0;
							foreach (TMP_Dropdown.OptionData myOptionData in child.gameObject.GetComponent<TMP_Dropdown>().options) {
								if (myOptionData.text.Equals(UnityWebRequest.UnEscapeURL(xmlValue))) {
									break;
								}
								indexValue++;
							}
							child.gameObject.GetComponent<TMP_Dropdown>().value = indexValue;
						} else if (child.name.Equals("Image") && child.GetComponent<OpenImageUploadPanelScript>()) {
							Debug.Log("LOADING IMAGE: " + xmlValue);
							child.GetComponent<OpenImageUploadPanelScript>().SetGuid(xmlValue);
							child.GetComponent<OpenImageUploadPanelScript>().LoadData(xmlValue);

							/*
							Image img = child.GetComponent<Image>();
							img.sprite = null;

							if (tm.transform.GetComponent<DataScript>().GetImageKeys().Contains(element.GetElementsByTagName(child.name).Item(0).InnerText)) { //Load image
								img.sprite = tm.transform.GetComponent<DataScript>().GetImage(xmlValue).sprite;
							}

							if (img.sprite == null) {
								img.GetComponent<CanvasGroup>().alpha = 0f;
								img.transform.parent.GetComponent<Image>().enabled = true;
							} else {
								img.GetComponent<CanvasGroup>().alpha = 1f;
								img.transform.parent.GetComponent<Image>().enabled = false;
							}
							*/
						}
					}
				}

                //If the PinArea is found, add pin objects as appropriate
                if (child.name.Equals("PinArea")) {
                    if (child.Find("DialoguePin")) {
                        //Have pins pulled out
                        //This will be set by a custom script attached to the buttons
                        /*GameObject pinObj = child.Find("DialoguePin").gameObject;
						Button b = pinObj.GetComponent<Button>();
						b.onClick.AddListener(delegate
						{
							ButtonListenerFunctionsScript.OpenDialogueEditor(b);
						});
						pinObj = child.Find("QuizPin").gameObject;
						Button b2 = pinObj.GetComponent<Button>();
						b2.onClick.AddListener(delegate
						{
							ButtonListenerFunctionsScript.OpenQuizEditor(b2, instantiatePanel("QuizEditorBG"));
						});*/
                    }

                    if (spawnDialogue) {
                        GameObject pinObj = child.Find("DialoguePin")?.gameObject;

                        if (!child.Find("DialoguePin")) {
                            //Spawning dialogue pins
                            pinObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/DialoguePinIcon") as GameObject;
                            pinObj = Instantiate(pinObj, child);
                            Button b = pinObj.AddComponent<Button>();
                            b.onClick.AddListener(delegate {
                                ButtonListenerFunctionsScript.OpenDialogueEditor(b);
                            });
                            pinObj.tag = "Value";
                            pinObj.name = "Dialogue" + "Pin";
                        } else if (pinObj && DialogueManagerScript.GetUID(tm.getCurrentSection(), pinObj.transform) == uid) {
                            //Change dialogue pin color
                            pinObj.transform.Find("Item Background Off").gameObject.SetActive(false);
                            pinObj.transform.Find("Item Background On").gameObject.SetActive(true);
                            pinObj.tag = "Value";
                            pinObj.name = "Dialogue" + "Pin";
                        }

                        spawnDialogue = false;
                    }
                    if (spawnQuiz) {
                        GameObject pinObj = child.Find("QuizPin")?.gameObject;

                        if (!child.Find("QuizPin")) {
                            pinObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/QuizPinIcon") as GameObject;
                            pinObj = Instantiate(pinObj, child);
                            Button b = pinObj.AddComponent<Button>();
                            b.onClick.AddListener(delegate {
                                ButtonListenerFunctionsScript.OpenQuizEditor(b, instantiatePanel("QuizEditorBG"));
                            });
                            pinObj.tag = "Value";
                            pinObj.name = "Quiz" + "Pin";
                        } else if (pinObj && DialogueManagerScript.GetUID(tm.getCurrentSection(), pinObj.transform) == quizId) {
                            pinObj = child.Find("QuizPin").gameObject;
                            pinObj.transform.Find("Item Background Off").gameObject.SetActive(false);
                            pinObj.transform.Find("Item Background On").gameObject.SetActive(true);
                            pinObj.tag = "Value";
                            pinObj.name = "Quiz" + "Pin";
                        }

                        spawnQuiz = false;
                    }
                    if (spawnFlag) {
                        GameObject pinObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/FlagPinIcon") as GameObject;
                        pinObj = Instantiate(pinObj, child);
                        Button b = pinObj.AddComponent<Button>();
                        b.onClick.AddListener(delegate {
                            b.onClick.AddListener(delegate { ButtonListenerFunctionsScript.OpenFlagEditor(b, instantiatePanel("FlagEventEditorBG")); });
                        });
                        pinObj.tag = "Value";
                        pinObj.name = "Flag" + "Pin";

                        if (GetComponent<ToolTipInfoScript>()) {
                            GetComponent<ToolTipInfoScript>().tooltipName = "OpenFlagPin";
                        }


                        ToolTipInfoScript tip = pinObj.AddComponent<ToolTipInfoScript>();
                        tip.tooltipName = "OpenFlagPin";

                        spawnFlag = false;
                    }
                    if (spawnEvent) {
                        GameObject pinObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/EventPinIcon") as GameObject;
                        pinObj = Instantiate(pinObj, child);
                        Button b = pinObj.AddComponent<Button>();
                        b.onClick.AddListener(delegate {
                            ButtonListenerFunctionsScript.OpenEventEditor(b, instantiatePanel("EventEditorBG"));
                        });
                        pinObj.tag = "Value";
                        pinObj.name = "Event" + "Pin";

                        if (GetComponent<ToolTipInfoScript>()) {
                            GetComponent<ToolTipInfoScript>().tooltipName = "OpenEventPin";
                        }


                        spawnEvent = false;
                    }
                }
            }

            //print(ds.GetData(tm.getCurrentSection()).GetCurrentTab().type + ", " + labNodee.gObject.transform.GetSiblingIndex());
            if (!isQuizPanel && labNodee.gObject.transform.GetSiblingIndex() == 0) {
                if (ds.GetData(tm.getCurrentSection()).GetCurrentTab().type.Equals("Personal Info")) {
                    if (GlobalData.caseObj.recordNumber != null && !GlobalData.caseObj.recordNumber.Equals("") && !GlobalData.fileName.StartsWith("[CHECKFORDUPLICATE]")) {
                        //print(GlobalData.caseObj.recordNumber);
                        labNodee.gObject.transform.Find("Row0/RecordValue").GetComponent<TextMeshProUGUI>().text = GlobalData.caseObj.recordNumber;
                    } else {
                        //To manually set the record number and prevent any slipups
                        labNodee.gObject.transform.Find("Row0/RecordValue").GetComponent<TextMeshProUGUI>().text = "######";
                    }
                }
            }

            if (labNodee.gObject.GetComponent<DialogueEntryScript>()) {
                DialogueEntryScript des = labNodee.gObject.GetComponent<DialogueEntryScript>();
                des.UpdateDialogue(des.GetComponentInChildren<TMP_InputField>());
                des.characterName = des.transform.Find("ParentDialogue/characterName").GetComponent<TextMeshProUGUI>().text;
                des.SetWriterColor(des.characterName);
                GetComponentInParent<DialogueManagerScript>().AddEntry(des);
            }

            ii++; //For itterating to the next entry in entryList
            if (ii >= entryList.Count) {
                break;
            }
        }
        ToggleEntry(0);
        return;
    }

    /**
	 * Parses through a quiz pin's xml to create an easy to use quiz question object list
	 * This is just a proof of concept right now, but will be useful in the reader
	 */
    public void CreateQuestion(string xml)
    {
        XmlDocument quiz = new XmlDocument();
        quiz.LoadXml(xml);
        XmlNode node = quiz.DocumentElement;

        //If the node isn't already on EntryData, get it there
        while (!node.Name.Equals("EntryData")) {
            node = AdvNode(node);

            if (node == null) {
                Debug.Log("No Data to load.");
                return;
            }
        }
        node = AdvNode(node); //Enter the EntryData. This will put Node on "Parent". The entries are all siblings now

        XmlDocument questionXml = new XmlDocument();

        List<QuizQuestionScript> questionList = new List<QuizQuestionScript>();
        while (node.NextSibling != null) {
            node = node.NextSibling;

            QuizQuestionScript currentQuestion = new QuizQuestionScript();
            questionList.Add(currentQuestion);

            string answerXML = node.OuterXml;
            //print (answerXML);
            //questionXml.LoadXml ("<Entry0><PanelType>QuizQuestionOption</PanelType><PanelData><OptionValue>1</OptionValue>" +
            //					 "<OptionTypeValue>Correct</OptionTypeValue><FeedbackValue></FeedbackValue></PanelData></Entry0>");
            questionXml.LoadXml(answerXML);


            currentQuestion.questionText = questionXml.GetElementsByTagName("QuestionValue").Item(0).InnerText;
            if (questionXml.GetElementsByTagName("DialoguePin").Count > 0) {
                XmlNode dialoguePin = questionXml.GetElementsByTagName("DialoguePin").Item(0);
                dialoguePin.ParentNode.RemoveChild(dialoguePin);
            }
            //print (questionXml.GetElementsByTagName ("data").Item (0).InnerXml);
            questionXml.LoadXml(questionXml.GetElementsByTagName("data").Item(0).InnerXml); //Prepare to traverse answers
            XmlNode answerNode = questionXml.DocumentElement;
            while (!answerNode.Name.Equals("EntryData")) {
                answerNode = AdvNode(answerNode);

                if (answerNode == null) {
                    Debug.Log("No Data to load.");
                    return;
                }
            }
            //print (answerNode.ChildNodes.Count - 1); //Num answers
            for (int i = 0; i < answerNode.ChildNodes.Count - 1; i++) {
                currentQuestion.AddAnswer(
                    questionXml.GetElementsByTagName("OptionValue").Item(i).InnerText, //Answer choice
                    questionXml.GetElementsByTagName("OptionTypeValue").Item(i).InnerText, //Correct, partially correct, or incorrect
                    questionXml.GetElementsByTagName("FeedbackValue").Item(i).InnerText //The descriptive feedback
                );
            }
        }
        foreach (QuizQuestionScript q in questionList) {
            //print (q.ToString ());
        }
    }

    public void GiveOutline(GameObject obj)
    {
        Color c = new Color(40 / 255f, 255 / 255f, 70 / 255f, 1f);
        Shadow s = obj.AddComponent<Shadow>();
        Outline o = obj.AddComponent<Outline>();
        s.effectColor = c;
        s.effectDistance = new Vector2(0f, 0f);
        o.effectColor = c;
        o.effectDistance = new Vector2(5f, 5f);
    }

    public void RemoveOutline(GameObject obj)
    {
        Destroy(obj.GetComponent<Outline>());
        Destroy(obj.GetComponent<Shadow>());
    }

    private GameObject instantiatePanel(string panelName)
    {
        GameObject pinPanelPrefab = Instantiate(Resources.Load("Writer/Prefabs/Panels/" + panelName)) as GameObject;
        pinPanelPrefab.transform.SetParent(BG.transform, false);
        return pinPanelPrefab;
    }

    /**
	 * Expands the first entry to make accessing data more easy
	 */
    void ToggleEntry(int n)
    {
        Toggle[] entryToggles = entryList[n].gObject.transform.GetComponentsInChildren<Toggle>();
        if ((entryToggles.Length > 0) && entryToggles.Length < n) {
            entryToggles[entryToggles.Length - 1].isOn = !entryToggles[entryToggles.Length - 1].isOn;
        }
    }

    /**
	 * Checks whether or not to add data for the given entry
	 */
    public bool CheckAddData()
    {
        return addData;
    }

    /**
	 * Removes an entry from the entry list and Destroy's the game object
	 */
    public void RemovePanelEntry(GameObject entry)
    {
        int idx = entry.transform.GetSiblingIndex();

        List<string> keys = ds.GetImageKeys();
        string data = getData(idx);
        Debug.Log("Removing data: " + data);
        foreach (string key in keys) {
            if (data.Contains("<Image>" + key + "</Image>")) {
                Debug.Log("Removing Image: " + key);
                ds.RemoveImage(key);
            }
        }

        entryList.RemoveAt(idx);
        foreach (LabEntryScript LES in entryList) {
            int siblingIdx = LES.gObject.transform.GetSiblingIndex();
            if (idx < siblingIdx) {
                LES.SetPosition(siblingIdx - 1);
            }

            if (LES.gObject.GetComponentInChildren<MoveableObjectCursorScript>())
                LES.gObject.GetComponentInChildren<MoveableObjectCursorScript>().RemoveFromEntryList(entry.transform);
        }
        ds.GetDialogues().Remove(uniquePath + entry.name);
        ds.correctlyOrderedDialogues.Remove(uniquePath + entry.name);
        ds.GetQuizes().Remove(uniquePath + entry.name);
        ds.correctlyOrderedQuizes.Remove(uniquePath + entry.name);
        if (entry.GetComponent<DiagnosisCountScript>()) {
            NextFrame.Function(ReorderDiagnosisEntries);
        }
        GameObject.Destroy(entry);
    }

    private void ReorderDiagnosisEntries()
    {

        foreach (DiagnosisCountScript child in GetComponentsInChildren<DiagnosisCountScript>()) {
            child.ReorderEntries();
        }
    }

    private void ScrollToBottom()
    {
        if (GetComponentInParent<DragOverrideScript>()) {
            transform.GetComponentInParent<DragOverrideScript>().verticalScrollbar.value = 0;
            transform.GetComponentInParent<DragOverrideScript>().verticalNormalizedPosition = 0;
        } else {
            transform.GetComponentInParent<ScrollRect>().verticalScrollbar.value = 0;
            transform.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 0;
        }
        willScroll = false;
    }

    public void ScrollToEntry(Transform entry)
    {
        float value = 0f;
        if (isNested) {
            value = (float)1 - (parentManager.transform.position.y - entry.position.y) / parentManager.GetComponent<RectTransform>().rect.height;
        } else {
            value = (float)1 - (transform.position.y - entry.position.y) / GetComponent<RectTransform>().rect.height;
        }
        print(value);
        if (GetComponentInParent<DragOverrideScript>()) {
            transform.GetComponentInParent<DragOverrideScript>().verticalScrollbar.value = value;
            transform.GetComponentInParent<DragOverrideScript>().verticalNormalizedPosition = value;
        } else {
            transform.GetComponentInParent<ScrollRect>().verticalScrollbar.value = value;
            transform.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = value;
        }
        willScroll = false;
    }

    public void ScrollToEntry()
    {
        Transform entry = transform.GetChild(transform.childCount - 1);
        float value = 0f;
        if (isNested) {
            return; //comment this to add scrolling-to to sub-entries
                    //value = (float)1 - (parentManager.transform.position.y - entry.position.y) / parentManager.GetComponent<RectTransform>().rect.height;
        } else {
            Vector3[] entryCorners = new Vector3[4];
            Vector3[] parentCorners = new Vector3[4];
            entry.GetComponent<RectTransform>().GetLocalCorners(entryCorners);
            GetComponent<RectTransform>().GetWorldCorners(parentCorners);

            //corners[0].y gives the bottom of the entry. corners[1] gives the top. We'll try to scroll so that it's at the bottom of the screen
            value = (float)1 - (GetComponent<RectTransform>().rect.height - entryCorners[0].y) / GetComponent<RectTransform>().rect.height;
        }
        print(value);
        if (value < 0) {
            value = 0;
        }
        if (GetComponentInParent<DragOverrideScript>()) {
            transform.GetComponentInParent<DragOverrideScript>().verticalScrollbar.value = value;
            transform.GetComponentInParent<DragOverrideScript>().verticalNormalizedPosition = value;
        } else {
            transform.GetComponentInParent<ScrollRect>().verticalScrollbar.value = value;
            transform.GetComponentInParent<ScrollRect>().verticalNormalizedPosition = value;
        }
        willScroll = false;
    }

    /**
	 * Opens the panel for the user to choose which kind of entry they'd like to add
	 */
    public void OpenAddEntryPanel()
    {
        addData = false;
        string tabType = ds.GetData(tm.getCurrentSection()).GetCurrentTab()?.type;
        if (tabType == null)
            return;

        if (tabType.Equals("Personal Info") || tabType.Equals("Family Social History")) {
            for (int i = 0; i < loadedPrefabs.Length; i++) {
                AddEntryFromPanel(loadedPrefabs[i].name);
            }
            return;
        }
        if (loadedPrefabs.Length == 1) {
            AddEntryFromPanel(loadedPrefabs[0].name);
            return;
        }
        GameObject dd = Instantiate(Resources.Load("Writer/Prefabs/Panels/EntrySelectorBG"), GameObject.Find("GaudyBG").transform) as GameObject;
        dd.name = "EntrySelectorBG";
        TMP_Dropdown ddc = dd.GetComponentInChildren<TMP_Dropdown>();
        List<string> prefabNames = new List<string>();
        foreach (string prefab in loadedPrefabsDisplayNames) {
            prefabNames.Add(prefab);
        }
        ddc.ClearOptions();
        ddc.AddOptions(prefabNames);
        dd.SetActive(true);
        dd.transform.parent.gameObject.SetActive(true);
    }

    /**
	 * Adds a specified entry from the AddEntryPanel
	 */
    public void AddEntryFromPanel(string name)
    {
        addData = false;
        AddEntry(name, true);
        MoveableObjectCursorScript move;
        foreach (LabEntryScript entry in entryList) {
            if ((move = entry.gObject.GetComponentInChildren<MoveableObjectCursorScript>()) != null)
                move.UpdateEntryList();
        }

        if (!willScroll && !isNested) {
            NextFrame.Function(ScrollToEntry);
            willScroll = true;
        }
        //ToggleEntry (entryList.Count - 1);
    }

    /**
	 * Adds an entry when no name is specified (aka from the create button on the panel)
	 */
    public void AddEntryFromPanel()
    {
        addData = false;
        if (loadedPrefabs.Length == 1) {
            AddEntry("According to all known laws of aviation, there is no way a bee should be able to fly.", true);

            if (!willScroll && !isNested) {
                NextFrame.Function(ScrollToEntry);
                willScroll = true;
            }

            ToggleEntry(entryList.Count - 1);
            return;
        }

        TMP_Dropdown ddc = tm.transform.Find("EntrySelectorBG").GetComponentInChildren<TMP_Dropdown>();
        AddEntry(ddc.GetComponentInChildren<TextMeshProUGUI>().text, true);
        MoveableObjectCursorScript move;
        foreach (LabEntryScript entry in entryList) {
            if ((move = entry.gObject.GetComponentInChildren<MoveableObjectCursorScript>()) != null)
                move.UpdateEntryList();
        }
        transform.GetComponentInParent<ScrollRect>().GraphicUpdateComplete();

        if (!willScroll && !isNested) {
            NextFrame.Function(ScrollToEntry);
            willScroll = true;
        }

        ToggleEntry(entryList.Count - 1);
    }


    /**
	 * Adds an entry from a button.
	 */
    public void AddEntryFromButton(string PrefabName)
    {
        AddEntry(PrefabName, true);
        MoveableObjectCursorScript move;
        foreach (LabEntryScript entry in entryList) {
            if ((move = entry.gObject.GetComponentInChildren<MoveableObjectCursorScript>()) != null)
                move.UpdateEntryList();
        }
        NextFrame.Function(ScrollToEntry);
    }

    public void RemoveEntries(Transform parent)
    {
        foreach (Transform child in parent) {
            GameObject.Destroy(child.gameObject);
        }
        entryList.Clear();
    }

    /**
	 * Adds an entry with the given prefab name
	 */
    public void AddEntry(string PrefabName, bool isNew)
    {
        GameObject newEntryObject = null;
        if (PrefabName == null || PrefabName.Equals("") || loadedPrefabs.Length == 1) {
            newEntryObject = Instantiate(loadedPrefabs[0], transform);
            LabEntryScript newEntry;
            newEntry = new LabEntryScript(entryList.Count, null, null, newEntryObject, loadedPrefabs[0].name);
            entryList.Add(newEntry);
            newEntryObject.name = "LabEntry: " + newEntry.GetPosition();
        } else {
            if (loadedPrefabsDisplayNames.Contains(PrefabName)) {
                int idx = 0;
                for (int i = 0; i < loadedPrefabsDisplayNames.Length; i++) {
                    if (loadedPrefabsDisplayNames[i].Equals(PrefabName)) {
                        idx = i;
                        break;
                    }
                }
                newEntryObject = Instantiate(loadedPrefabs[idx], transform);
                LabEntryScript newEntry;
                newEntry = new LabEntryScript(entryList.Count, null, null, newEntryObject, PrefabName);
                entryList.Add(newEntry);
                newEntryObject.name = "LabEntry: " + newEntry.GetPosition();
            }
            foreach (GameObject entry in loadedPrefabs) {
                if (entry == null) {
                    continue;
                }
                if (PrefabName.Equals(entry.name)) {
                    newEntryObject = Instantiate(entry, transform);
                    LabEntryScript newEntry;
                    newEntry = new LabEntryScript(entryList.Count, null, null, newEntryObject, PrefabName);
                    entryList.Add(newEntry);
                    newEntryObject.name = "LabEntry: " + newEntry.GetPosition();
                }
            }
        }

        //Check to see if this is a nested entry. If it is, then we don't change MethodToCall
        if (!isNested && !isQuizPanel) { //If not nested
            tm.MethodToCall = AddToDictionary;
        }

        //tm.MethodToCall = AddToDictionary;

        if (newEntryObject == null) {
            return;
        }

        //Need to set dialogue color and that's basically it
        if (newEntryObject.GetComponent<DialogueEntryScript>()) {
            //We pass in the character name for PrefabName for now I guess? Just to see if it works
            newEntryObject.GetComponent<DialogueEntryScript>().SetWriterColor(PrefabName);
        }

        //since the quiz panel is persistant, we have to manually restart each entry's inner HistoryFieldManagerScript
        else if (isQuizPanel && !isNested) {
            newEntryObject.GetComponentInChildren<HistoryFieldManagerScript>().PopulateQuiz(quizPin);
        }

        if (isNew) {
            if (newEntryObject.transform.Find("Row0/RowControls/CollapseButton")) {
                newEntryObject.transform.Find("Row0/RowControls/CollapseButton").GetComponent<CollapseContentScript>().SetTarget(true);
            }
        }
    }

    public void AddDialogue(string CharacterName)
    {
        string PrefabName = "DialogueEntry";// Test2";
        GameObject newEntryObject = null;
        foreach (GameObject entry in loadedPrefabs) {
            if (PrefabName.Equals(entry.name)) {
                newEntryObject = Instantiate(entry, transform);
                LabEntryScript newEntry;
                newEntry = new LabEntryScript(entryList.Count, null, null, newEntryObject, PrefabName);
                entryList.Add(newEntry);
                newEntryObject.name = "LabEntry: " + newEntry.GetPosition();
            }
        }

        //Check to see if this is a nested entry. If it is, then we don't change MethodToCall
        if (!isNested && !isQuizPanel) { //If not nested
            tm.MethodToCall = AddToDictionary;
        }

        //tm.MethodToCall = AddToDictionary;

        //Need to set dialogue color and that's basically it
        if (newEntryObject.GetComponent<DialogueEntryScript>()) {
            //We pass in the character name for PrefabName for now I guess? Just to see if it works
            newEntryObject.GetComponent<DialogueEntryScript>().characterName = CharacterName;
            newEntryObject.GetComponent<DialogueEntryScript>().SetWriterColor(CharacterName);
            newEntryObject.transform.Find("ParentDialogue/characterName").GetComponent<TextMeshProUGUI>().text = CharacterName;
        }

        GetComponentInParent<DialogueManagerScript>().AddEntry(newEntryObject.GetComponent<DialogueEntryScript>());

        MoveableObjectCursorScript move;
        foreach (LabEntryScript entry in entryList) {
            if ((move = entry.gObject.GetComponentInChildren<MoveableObjectCursorScript>()) != null)
                move.UpdateEntryList();
        }

        NextFrame.Function(ScrollToEntry);
    }

    /**
	 * 	Returns the data of a single entry, marked by its position
	 */
    public string getData(int position)
    {
        foreach (LabEntryScript entry in entryList) {
            if (entry.GetPosition() == position) {
                string s = "<Entry" + entry.GetPosition() + ">";
                s += "<PanelType>" + entry.GetPanelType() + "</PanelType><PanelData>";
                s += entry.getData() + "</PanelData></Entry" + entry.GetPosition() + ">";
                return s;
            }
        }
        return "";
    }

    /**
	 * 	Returns all data for all entries
	 */
    public string getData()
    {
        //string data = "<" + parentTab.name + "><data>";
        string data = "<data><EntryData>";

        if (transform.GetComponentInParent<DialogueManagerScript>()) {
            uniquePath = GetComponentInParent<DialogueManagerScript>().GetUID();
            if (isNested) {
                Transform tempTrans = transform;
                while (!tempTrans.parent.name.Equals("Content")) {
                    tempTrans = tempTrans.parent;
                }
                uniquePath = uniquePath + "::" + Regex.Split(tempTrans.name, "[0-9]*$")[0] + tempTrans.GetSiblingIndex();
                //uniquePath = uniquePath + "::" + transform.GetComponentsInParent<Transform>().ToList().Find((Transform x) => x.name.StartsWith("LabEntry:")).name;
            }
        } else {
            RefreshUniquePath();
        }

        data += "<Parent>" + uniquePath + "</Parent>";

        foreach (LabEntryScript entry in entryList) {
            data += getData(entry.GetPosition());
        }

        //The contents of ReorderDictionaries went here previously.

        //xmlDoc.Load (data);
        data += "</EntryData></data>";//</" + parentTab.name + ">";
                                      //Debug.Log(data);
        return data;
    }

    /**
	 * Adds data to the DataScript
	 */
    public void AddToDictionary()
    {
        if (parentTab != null) {
            ds.AddData(tm.getCurrentSection(), parentTab.name.Substring(0, parentTab.name.Length - 3), getData());// + parentTab.GetComponent<Text> ().text, getData ());
            ReorderDictionaries();
            //ReorderImages ();
        }
    }

    /**
	 * Used for rearranding quiz images to keep them assigned to the correct quiz
	 * Called after other dictionaries have already been handled
	 */
    /*
	private void ReorderImages() {
		ds.correctlyOrderedImages.Clear ();
		foreach(LabEntryScript entry in entryList) {
			Transform temp = entry.gObject.transform;
			string oldPath = "";
			string newPath = "";
			while (temp != null) {
				if (temp.name.StartsWith ("LabEntry:")) {
					//uniquePath = "LabEntry: " + tempPin.GetSiblingIndex() + uniquePath;
					oldPath = temp.name;
					newPath = "LabEntry: " + entry.GetPosition ();
				}
				if (temp.name.EndsWith ("Tab")) {
					oldPath = temp.name + "." + oldPath;
					newPath = temp.name + "." + newPath;
				}
				temp = temp.parent;
			}
			oldPath = tm.getCurrentSection () + "." + oldPath;
			newPath = tm.getCurrentSection () + "." + newPath;

			foreach (string key in ds.GetImages().Keys.ToList()) {
				if (key.Contains (oldPath + "::") && !ds.correctlyOrderedImages.ContainsKey (key.Replace (oldPath, newPath))) {
					SpriteHolderScript tempImg = new SpriteHolderScript(new Sprite());
					tempImg.sprite = ds.GetImages () [key].sprite;
					ds.correctlyOrderedImages.Add (key.Replace (oldPath, newPath), ds.GetImages () [key]);
					ds.GetImages ().Remove (key);
				}
			}
		}

		foreach (string key in ds.correctlyOrderedImages.Keys.ToList()) {
			ds.GetImages().Remove (key);
			ds.AddImg (key, ds.correctlyOrderedImages [key].sprite);
		}
		ds.correctlyOrderedImages.Clear ();

	}
	*/

    /**
	 * Saves the quiz data to the quiz dictionary
	 */
    public void AddToQuizDictionary()
    {
        string uniquePath = "";
        Transform tempPin = quizPin;
        if (!quizPin.GetComponentInParent<HistoryFieldManagerScript>()) {
            Transform uniqueParent = quizPin;
            string path = "";
            while (uniqueParent.parent != null && !uniqueParent.parent.name.Equals("Content")) {
                uniqueParent = uniqueParent.parent;
            }
            path = uniqueParent.name;
            while (!uniqueParent.name.EndsWith("Tab")) {//Once you hit the Tab container
                uniqueParent = uniqueParent.parent;
                path = uniqueParent.name + "/" + path;
            }
            uniquePath = tm.getCurrentSection() + "/" + path;
        } else {
            while (tempPin != null) {
                if (tempPin.name.StartsWith("LabEntry:")) {
                    //uniquePath = "LabEntry: " + tempPin.GetSiblingIndex() + uniquePath;
                    uniquePath = tempPin.name + uniquePath;
                }
                if (tempPin.name.EndsWith("Tab")) {
                    uniquePath = tempPin.name + "/" + uniquePath;
                }
                tempPin = tempPin.parent;
            }
            uniquePath = tm.getCurrentSection() + "/" + uniquePath;
        }

        RefreshUniquePath(); //maybe??
        Debug.Log("Unique path added to dictionary: " + uniquePath);
        ds.AddQuiz(uniquePath, getData());
        ReorderDictionaries();

        if (GetPin().transform.Find("Item Background Off")) {
            GetPin().transform.Find("Item Background Off").gameObject.SetActive(false);
            GetPin().transform.Find("Item Background On").gameObject.SetActive(true);
        }
    }

    /**
	 * Call this after all data has been retrieved/dealt with
	 */
    public void ReorderDictionaries()
    {
        foreach (string key in ds.correctlyOrderedDialogues.Keys) {
            //Debug.Log ("Dialogue key: " + key);
            ds.AddDialogue(key, ds.correctlyOrderedDialogues[key]);
        }
        ds.correctlyOrderedDialogues.Clear();

        foreach (string key in ds.correctlyOrderedQuizes.Keys) {
            //Debug.Log ("Quiz key: " + key);
            ds.AddQuiz(key, ds.correctlyOrderedQuizes[key]);
        }
        ds.correctlyOrderedQuizes.Clear();

        foreach (string key in ds.correctlyOrderedFlags.Keys) {
            //Debug.Log ("Flag key: " + key);
            ds.AddFlag(key, ds.correctlyOrderedFlags[key]);
        }
        ds.correctlyOrderedFlags.Clear();
    }

    // Need to setup a different method to get the unique path
    // rather than by Lab Entry. This script is not located in
    // the same part of the hierarchy as quiz & dialogue.
    /* 
     public void AddToFlagDictionary()
     {
         string uniquePath = "";
         while (tempPin != null)
         {
             if (tempPin.name.StartsWith("LabEntry:"))
             {
                 //uniquePath = "LabEntry: " + tempPin.GetSiblingIndex() + uniquePath;
                 uniquePath = tempPin.name;
             }
             if (tempPin.name.EndsWith("Tab"))
             {
                 uniquePath = tempPin.name + "." + uniquePath;
             }
             tempPin = tempPin.parent;
         }
         uniquePath = tm.getCurrentSection() + "." + uniquePath;
         Debug.Log("Unique path added to dictionary: " + uniquePath);
         ds.AddQuiz(uniquePath, getData());
     }
     */

    /**
	 * Called when loading quiz data
	 */
    public void PopulateQuiz(Transform pin)
    {
        quizPin = pin;
        entryList = new List<LabEntryScript>();
        populating = true;
        Start();
    }

    /*
    //Handles event pin color
    public void EventCompare(bool compare)
    {
        if (compare == true)
        {
            currentPin.GetComponent<Image>().color = new Color(94f / 255f, 175f / 255f, 172f / 255f);
        }
        else
        {
            currentPin.GetComponent<Image>().color = new Color(1f, 0f, 0f);
        }
    }
    */
    // Grabs the pin associated with the panel (dialogue, quiz, event, etc)
    public void GrabPin(Transform pin)
    {
        currentPin = pin;
    }

    // Recursive function to read all nodes until uid is found
    private string findUID(XmlNode node)
    {
        if (node.Name == "uid") {
            uid = node.InnerText;
            return uid;
        }

        foreach (XmlNode n in node.ChildNodes) {
            if (!findUID(n).Equals(""))
                return uid;
        }
        return "";
    }
    /**
	 * 	Advance the node to the correct next node in the XmlDoc. Just like Depth first search
	 */
    private XmlNode AdvNode(XmlNode node)
    {
        if (node == null)
            return node;
        if (node.HasChildNodes)
            node = node.ChildNodes.Item(0);
        else if (node.NextSibling != null)
            node = node.NextSibling;
        else {
            while (node.ParentNode.NextSibling == null) {
                node = node.ParentNode;
                if (node == xmlDoc.DocumentElement || node.ParentNode == null)
                    return null;
            }
            node = node.ParentNode.NextSibling;
            if (node == xmlDoc.DocumentElement.LastChild)
                return node;
        }
        return node;
    }

    /**
	 * From early testing. Move an entry down one position
	 */
    public void MoveDown(GameObject labEntry)
    {
        int idx = labEntry.transform.GetSiblingIndex();
        if (idx == entryList.Count - 1) {
            return;
        }
        LabEntryScript temp = entryList[idx];
        entryList[idx] = entryList[idx + 1];
        entryList[idx + 1] = temp;
        entryList[idx].SetPosition(idx);
        idx++;
        entryList[idx].SetPosition(idx);
        labEntry.transform.SetSiblingIndex(idx);
    }

    /**
	 * From early testing. Move an entry up one position
	 */
    public void MoveUp(GameObject labEntry)
    {
        int idx = labEntry.transform.GetSiblingIndex();
        if (idx == 0) {
            return;
        }
        LabEntryScript temp = entryList[idx];
        entryList[idx] = entryList[idx - 1];
        entryList[idx - 1] = temp;
        entryList[idx].SetPosition(idx);
        idx--;
        entryList[idx].SetPosition(idx);
        labEntry.transform.SetSiblingIndex(idx);
    }

    /**
	 * Move an entry from one index to another
	 * pos = index moving to
	 * beforePos = index moving from
	 */
    public void MoveTo(GameObject labEntry, int pos, int beforePos)
    {
        LabEntryScript temp = entryList[beforePos];
        entryList.RemoveAt(beforePos);
        entryList.Insert(pos, temp);

        int i = 0;
        foreach (LabEntryScript ls in entryList) {
            entryList[i].SetPosition(i);
            i++;
        }
        labEntry.transform.SetSiblingIndex(pos);
    }

    /**
	 * For debugging. Prints every entry to console
	 */
    public void PrintList()
    {
        string s = "";
        foreach (LabEntryScript entry in entryList) {
            s += "ENTRY: " + entry.GetPosition() + ", " + entry.getData();
        }
        Debug.Log(s);
    }

    /**
	 * Returns the List of entries
	 */
    public List<LabEntryScript> GetEntryList()
    {
        return entryList;
    }

    //This is the old loading method which is being used for reference
    private void OldLoadMethod(XmlNode uniqueParent)
    {
        if (entryList.Count == 0) {
            return;
        }
        XmlNode node = uniqueParent; //step inside the parent to begin

        // Tab is blank
        if (node == null) {
            Debug.Log("No data to load!");
            ds.newTabs.Add(parentTab.transform);
            return;
        }

        //Navigate to the data of the first entry

        bool inEntryData = false;

        // Search through nodes searching for an entry.
        while (!node.Name.Equals("PanelData") || !inEntryData) {
            if (node.Name.StartsWith("Entry")) {
                inEntryData = true;
            }
            node = AdvNode(node);

            if (node == null) {
                Debug.Log("No Data to load.");
                return;
            }
        }

        // Iterate through entries
        int i = 0;
        LabEntryScript labNode = entryList[i];
        while (labNode != null) {
            List<Transform> allChildren = labNode.gObject.GetComponentsInChildren<Transform>(true).ToList<Transform>();

            bool addDialogue = false;
            bool addQuiz = false;
            bool addFlag = false;
            bool addEvent = false;

            // For nodes without values check if they are a pin
            while (node.Value == null && !node.InnerText.Equals("")) {
                if (node.Name.Equals("DialoguePin") || node.Name.Equals("QuizPin") || node.Name.Equals("FlagPin") || node.Name.Equals("EventPin")) {
                    findUID(node); // Get the uid used for dialogue key
                    if (node.Name.Equals("DialoguePin")) {
                        addDialogue = true;
                        ds.AddDialogue(uid, node.InnerXml);
                    }
                    if (node.Name.Equals("QuizPin")) {
                        addQuiz = true;
                        XmlNode tempNode = node;
                        while (!tempNode.Name.Equals("Parent")) {
                            tempNode = AdvNode(tempNode);
                        }
                        ds.AddQuiz(tempNode.InnerText, node.InnerXml);

                    }
                    if (node.Name.Equals("FlagPin")) {
                        addFlag = true;
                        ds.AddFlag(node.FirstChild.InnerText, node.InnerXml);
                    }
                    if (node.Name.Equals("EventPin")) {
                        addEvent = true;
                    }
                    if (node.NextSibling != null) {
                        node = node.NextSibling;
                    } else {
                        node = AdvNode(node);
                    }

                } else {
                    node = AdvNode(node);
                }
                if (node == null) {
                    return;
                }
            }

            foreach (Transform child in allChildren) {


                /*if (child.tag.Equals ("Image")) {
					string startingUID = "";
					HistoryFieldManagerScript hfm = child.GetComponentInParent<HistoryFieldManagerScript> ();
					Transform tempChild = child.transform;
					if (hfm != null && hfm.isQuizPanel) {
						//tempChild = hfm.GetPin ().transform; //Might have to do this??? Dont think it'll cause problems but i'm not sure
					}
					while (tempChild != null) {
						if (tempChild.name.StartsWith("LabEntry:")) {
							startingUID = tempChild.name + startingUID;
						}
						if (tempChild.name.EndsWith("Tab")) {
							startingUID = tempChild.name + "Tab." + startingUID;
						}
						tempChild = tempChild.parent;
					}
					//Debug.Log (startingUID);
					//Debug.Log (ds.transform.GetComponent<TabManager> ().getCurrentSection ());
					//Debug.Log (ds.transform.GetComponent<TabManager> ().getTabName ());
					startingUID = ds.transform.GetComponent<TabManager> ().getCurrentSection () + "." + ds.transform.GetComponent<TabManager> ().getTabName () + "Tab." + startingUID;
					if (hfm != null && hfm.isQuizPanel) {
						//startingUID = startingUID + "::QuizPin.LabEntry:";
					}
					//Debug.Log ("Saved image: " + startingUID);

					if (ds.GetImages ().ContainsKey (startingUID)) {
						child.GetComponent<Image> ().sprite = ds.GetImages () [startingUID].sprite;
						child.GetComponent<CanvasGroup> ().alpha = 1f;
						child.transform.parent.GetComponent<Image> ().enabled = false;
					}

					continue;
				}*/

                //if (node == null && !addDialogue && !addQuiz)
                //break;

                // Check for input values and pins
                if (node != null && (child.name.Equals(node.ParentNode.Name) || (child.name.Equals(node.Name) && node.InnerXml.Equals("")))) {
                    try {
                        if (node.Value != null) {
                            if (child.gameObject.GetComponent<InputField>() != null) {
                                child.gameObject.GetComponent<InputField>().text = UnityWebRequest.UnEscapeURL(node.Value);
                                if (child.GetComponent<InputFieldResizer>()) {
                                    child.GetComponent<InputFieldResizer>().ResizeField();
                                }
                            } else if (child.gameObject.GetComponent<Dropdown>() != null) {
                                int indexValue = 0;

                                foreach (Dropdown.OptionData myOptionData in child.gameObject.GetComponent<Dropdown>().options) {
                                    if (myOptionData.text.Equals(UnityWebRequest.UnEscapeURL(node.Value))) {
                                        break;
                                    }
                                    indexValue++;
                                }

                                child.gameObject.GetComponent<Dropdown>().value = indexValue;
                            } else if (child.gameObject.GetComponent<Toggle>() != null) {
                                child.gameObject.GetComponent<Toggle>().isOn = bool.Parse(node.Value);
                            } else if (child.gameObject.GetComponent<Text>() != null) {
                                child.gameObject.GetComponent<Text>().text = UnityWebRequest.UnEscapeURL(node.Value);
                            } else if (child.name.Equals("Image") && child.GetComponent<OpenImageUploadPanelScript>()) {
                                Debug.Log("LOADING IMAGE: " + node.Value);
                                child.GetComponent<OpenImageUploadPanelScript>().SetGuid(node.Value);

                                Image img = child.GetComponent<Image>();
                                img.sprite = null;

                                if (tm.transform.GetComponent<DataScript>().GetImageKeys().Contains(node.Value)) { //Load image
                                    img.sprite = tm.transform.GetComponent<DataScript>().GetImage(node.Value).sprite;
                                }

                                if (img.sprite == null) {
                                    img.GetComponent<CanvasGroup>().alpha = 0f;
                                    img.transform.parent.GetComponent<Image>().enabled = true;
                                } else {
                                    img.GetComponent<CanvasGroup>().alpha = 1f;
                                    img.transform.parent.GetComponent<Image>().enabled = false;
                                }
                            }
                        }
                        //Advance node to the next value we need
                        node = AdvNode(node);

                        while (node != null && node.Value == null && !node.InnerText.Equals("")) {

                            if (Regex.IsMatch(node.Name.ToLower(), "tab[0-9]*$"))
                                break;
                            if (node.Name.Equals("DialoguePin") || node.Name.Equals("QuizPin") || node.Name.Equals("FlagPin") || node.Name.Equals("EventPin")) {
                                findUID(node); // Get the uid used for dialogue key
                                if (node.Name.Equals("DialoguePin")) {
                                    addDialogue = true;
                                    ds.AddDialogue(uid, node.InnerXml);
                                }
                                if (node.Name.Equals("QuizPin")) {
                                    addQuiz = true;
                                    XmlNode tempNode = node;
                                    while (!tempNode.Name.Equals("Parent")) {
                                        tempNode = AdvNode(tempNode);
                                    }
                                    ds.AddQuiz(tempNode.InnerText, node.InnerXml);
                                }
                                if (node.Name.Equals("FlagPin")) {
                                    addFlag = true;
                                    ds.AddFlag(node.FirstChild.InnerText, node.InnerXml);
                                }
                                if (node.Name.Equals("EventPin")) {
                                    addEvent = true;
                                }
                                if (node.NextSibling != null) {
                                    node = node.NextSibling;
                                    continue;
                                }
                            }

                            // If Entry Data is reached, back out to parent(s)
                            if (node.Name.Equals("EntryData")) {
                                while (node.ParentNode.NextSibling == null) {
                                    node = node.ParentNode;
                                    if (node == xmlDoc.DocumentElement || node.ParentNode == null) {
                                        node = null;
                                        break;
                                    }
                                }
                                if (node == null) {
                                    break;
                                }
                                node = node.ParentNode.NextSibling;
                                if (node == xmlDoc.DocumentElement.LastChild) {
                                    return;

                                }
                            } else {
                                node = AdvNode(node);
                            }
                        }
                    } catch (Exception e) {
                        Debug.Log(e.Message);
                    }
                }

                //If adding a dialogue, then add the pin button too
                if (child.name.Equals("PinArea")) {
                    if (addDialogue) {
                        GameObject pinObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/DialoguePinIcon") as GameObject;
                        pinObj = Instantiate(pinObj, child);
                        Button b = pinObj.AddComponent<Button>();
                        b.onClick.AddListener(delegate {
                            /*b.onClick.AddListener(delegate {*/
                            ButtonListenerFunctionsScript.OpenDialogueEditor(b); //});
                        });
                        pinObj.tag = "Value";
                        pinObj.name = "Dialogue" + "Pin";

                        addDialogue = false;
                    }
                    if (addQuiz) {
                        GameObject pinObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/QuizPinIcon") as GameObject;
                        pinObj = Instantiate(pinObj, child);
                        Button b = pinObj.AddComponent<Button>();
                        b.onClick.AddListener(delegate {
                            /* b.onClick.AddListener(delegate {*/
                            ButtonListenerFunctionsScript.OpenQuizEditor(b, instantiatePanel("QuizEditorBG")); //});
                        });
                        pinObj.tag = "Value";
                        pinObj.name = "Quiz" + "Pin";

                        addQuiz = false;
                    }
                    if (addFlag) {
                        GameObject pinObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/FlagPinIcon") as GameObject;
                        pinObj = Instantiate(pinObj, child);
                        Button b = pinObj.AddComponent<Button>();
                        b.onClick.AddListener(delegate {
                            b.onClick.AddListener(delegate { ButtonListenerFunctionsScript.OpenFlagEditor(b, instantiatePanel("FlagEventEditorBG")); });
                        });
                        pinObj.tag = "Value";
                        pinObj.name = "Flag" + "Pin";

                        if (GetComponent<ToolTipInfoScript>()) {
                            GetComponent<ToolTipInfoScript>().tooltipName = "OpenFlagPin";
                        }


                        ToolTipInfoScript tip = pinObj.AddComponent<ToolTipInfoScript>();
                        tip.tooltipName = "OpenFlagPin";

                        addFlag = false;
                    }

                    if (addEvent) {
                        GameObject pinObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/EventPinIcon") as GameObject;
                        pinObj = Instantiate(pinObj, child);
                        Button b = pinObj.AddComponent<Button>();
                        b.onClick.AddListener(delegate {
                            ButtonListenerFunctionsScript.OpenEventEditor(b, instantiatePanel("EventEditorBG"));
                        });
                        pinObj.tag = "Value";
                        pinObj.name = "Event" + "Pin";

                        if (GetComponent<ToolTipInfoScript>()) {
                            GetComponent<ToolTipInfoScript>().tooltipName = "OpenEventPin";
                        }


                        addEvent = false;
                    }
                }
            }
            i++;
            // If we have expended all of the entries
            if (i >= entryList.Count) {
                break;
            }
            labNode = entryList[i];
            bool newEntry = false;
            //Navigate to the next entry's data
            while (node != null && !node.Name.Equals("PanelData")) {
                //Debug.Log("NODE NAME: " + node.Name);

                node = AdvNode(node);
                newEntry = true;

                if (node == null) {
                    break;
                }
            }

            // New entry found - iterate again
            if (newEntry) {
                continue;
            }
            while (node != null && node.Value == null) {
                //Debug.Log("NODE NAME: " + node.Name);

                node = AdvNode(node);

                if (node == null) {
                    break;
                }
            }
        }
        ToggleEntry(0);
    }
}