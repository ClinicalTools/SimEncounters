using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using TMPro;
using UnityEngine.Networking;

public class ReaderEntryManagerScript : MonoBehaviour
{

    public GameObject[] loadedPrefabs;
    public String[] loadedPrefabsDisplayNames;
    private List<LabEntryScript> entryList;
    public GameObject parentTab;
    private XmlDocument xmlDoc;
    private ReaderTabManager tm;
    private ReaderDataScript ds;
    public bool isNested;
    public bool isQuizPanel;
    public bool isStatic;
    public bool isPuzzle;
    private Transform panelPin;
    private bool populating;
    private string uniquePath;
    private bool willScroll = false;
    private GameObject BG;
    private string uid;
    private bool addData;
    private ReaderEntryManagerScript parentManager;
    private List<QuizQuestionScript> questionList;

    /**
	 * This script is used to manage any tab that has addable entries/panels to it. These can be nested as well
	 * (nested has only been tested one level deep)
	 */


    void Awake()
    {
        entryList = new List<LabEntryScript>();
        BG = GameObject.Find("GaudyBG");
        tm = BG.GetComponent<ReaderTabManager>();
        ds = BG.GetComponent<ReaderDataScript>();
    }


    // Use this for initialization
    void Start()
    {
        if (isQuizPanel && !populating) {
            return;
        }
        populating = false;
        GameObject BG = GameObject.Find("GaudyBG");
        tm = BG.GetComponent<ReaderTabManager>();
        ds = BG.GetComponent<ReaderDataScript>();

        if (isQuizPanel && panelPin == null) {
            return;
        }
        //Finds the tab this manager belongs to
        if (isQuizPanel) {
            parentTab = panelPin.GetComponentsInParent<Transform>().ToList().Find((Transform x) => x.name.EndsWith("Tab")).gameObject;
            Debug.Log(parentTab.transform.name);
        } else {
            Transform tempTrans = transform;
            while (parentTab == null && tempTrans != null) {
                if (tempTrans.parent.GetComponent<ReaderEntryManagerScript>() != null) {
                    parentTab = tempTrans.parent.GetComponent<ReaderEntryManagerScript>().parentTab;
                    break;
                } else {
                    tempTrans = tempTrans.parent;
                }
            }
        }
        if (parentTab == null) {
            Debug.Log("Cannot access parent tab!");
            return;
        }
        questionList = new List<QuizQuestionScript>();
        parentManager = transform.parent.GetComponentInParent<ReaderEntryManagerScript>();
        addData = true;
        StartCoroutine(FindUniqueParent());
    }

    /**
	 * returns the pin that was clicked to open the quiz dialogue
	 */
    public GameObject GetPin()
    {
        return panelPin.gameObject;
    }

    /**
	 * Call this to refresh the uniquePath variable. Useful when renaming sections/tabs
	 */
    public string RefreshUniquePath()
    {
        uniquePath = "";
        Transform tempPin = null;
        if (isQuizPanel) {// && !isNested) {
            tempPin = panelPin;
        } else {
            tempPin = transform;
        }

        if (isQuizPanel && !panelPin.GetComponentInParent<ReaderEntryManagerScript>()) {
            Transform uniqueParent;
            uniqueParent = panelPin;
            uniquePath = "";
            while (uniqueParent.parent != null && !uniqueParent.parent.name.Equals("Content")) {
                if (uniqueParent.parent.GetComponent<ReaderEntryManagerScript>() != null) {
                    break;
                }
                uniqueParent = uniqueParent.parent;
            }
            uniquePath = uniqueParent.name;
            if (parentTab == null && isNested && !isQuizPanel) {
                parentTab = transform.parent.GetComponentInParent<ReaderEntryManagerScript>().parentTab;
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
                        uniquePath = tempPin.name + uniquePath;
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
            while (!tempTrans.parent.GetComponent<ReaderEntryManagerScript>()) {
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
    public IEnumerator FindUniqueParent()
    {
        //Gets the xml data for this tab
        string text = "";

        //The contents of RefreshUniquePath used to be here
        //Debug.Log (tm.getCurrentTab () + parentTab.GetComponent<Text> ().text);

        RefreshUniquePath();

        if (!isQuizPanel) {
            text = ds.GetData(tm.getCurrentSection(), tm.getCurrentTab()); //+ parentTab.GetComponent<Text> ().text);
        } else {
            string topLevelPath = Regex.Split(uniquePath, "::")[0];
            if (ds.GetDialogues().ContainsKey(topLevelPath) && GetComponentInParent<ReaderDialogueManagerScript>()) {
                text = ds.GetDialogues()[topLevelPath];
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


        //Debug.Log ("LOADED TEXT: "  + text);

        if (text == null || addedNew) {
            Debug.Log("No Data to load");

            //Open the entry selector panel. This should auto add an entry if there's only one type to add
            if (!isNested && !addedNew) {
                OpenAddEntryPanel();
            }
            yield break;
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

            bool fullNested = false;
            //This should set the path to point to Context aka the parent of the unique parent
            if (uniqueParent.parent.GetComponent<ReaderEntryManagerScript>() != null) {
                uniqueParent = uniqueParent.parent;
                fullNested = true;
            }

            path = uniquePath;
            Debug.Log(path);
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
                    tempNode = xmlDoc.AdvNode(tempNode);
                }
            }
            uniqueParentNode = tempNode;
            tempNode = xmlDoc.AdvNode(tempNode); //Advance inside the unique parent
        }

        if (tempNode == null) { //No data found
            Debug.Log("No nested data found");
            if (GetComponent<HideLabelScript>() != null) {
                GetComponent<HideLabelScript>().HideLabel();
            }
            yield break;
        }

        //Find the first entry while avoiding going into the next manager's data (if there are any)
        //Doing this to add and spwan in the Entries we'll need for loading into
        while (tempNode != null && !tempNode.Name.Equals("Entry0")) {
            tempNode = xmlDoc.AdvNode(tempNode);
            if (uniqueParentNode.NextSibling != null && tempNode.Equals(uniqueParentNode.NextSibling)) {
                break;
            }
        }

        if (tempNode != null && !tempNode.Equals(uniqueParentNode.NextSibling)) {
            //spawn in the number of entries required for this tab
            SectionDataScript currentData = ds.GetData(tm.getCurrentSection());
            if (!isNested && (!isQuizPanel && (currentData.GetCurrentTab().type.Equals("Quiz")) || (isQuizPanel && !GetComponentInParent<ReaderDialogueManagerScript>()))) {
                CreateQuestions(tempNode.ParentNode.OuterXml); //Temp node was on the first entry. We want the entries' parent.
            } else {
                while (tempNode != null) { //&& tempNode.ParentNode.HasChildNodes
                    XmlNode a = xmlDoc.AdvNode(tempNode);
                    if (a.InnerText.Equals("DialogueEntryTest2") || a.InnerText.Equals("DialogueEntry")) { //TODO update this to remove Test2 reference
                        while (!a.Name.StartsWith("character")) {
                            a = xmlDoc.AdvNode(a);
                        }
                        AddDialogue(a.InnerText);
                        /*if (WWW.UnEscapeURL(a.InnerText).Equals("Patient")) {
							AddEntry("DialogueEntryLeft", false);
						} else {
							AddEntry("DialogueEntryRight", false);
						}*/
                    } else {
                        AddEntry(a.InnerText, false);
                    }
                    tempNode = tempNode.NextSibling;
                }
            }

            //Adds the "Proceed to next tab/step" button
            string buttonType = "Tab";
            int tabPos = ds.GetData(tm.getCurrentSection()).GetCurrentTab().position + 1;
            //print(tabPos + "," + ds.GetData(tm.getCurrentSection()).GetTabList().Count);
            if (tabPos == ds.GetData(tm.getCurrentSection()).GetTabList().Count) {
                buttonType = "Step";
            }
            /*
			//Start the page at 1
			int page = 0;
			bool hitCurrentSection = false;
			int totalPages = 0;
			foreach(string section in ds.GetSectionsList()) {
				if (section.Equals(tm.getCurrentSection())) {
					hitCurrentSection = true;
				}
				if (!hitCurrentSection) {
					page += ds.GetData(section).GetTabList().Count;
				}
				totalPages += ds.GetData(section).GetTabList().Count;
			}
			page += tabPos;
			*/

            //Neater way to do this
            int page = GetCurrentPage();
            int totalPages = GetTotalPages();

            //Conditional for the last tab in the last section
            if (!isNested && !isQuizPanel) {
                Transform parentTransform = transform;
                while (parentTransform.parent != null && !parentTransform.name.Equals("Content")) {
                    parentTransform = parentTransform.parent;
                }

                string prefabPath = GlobalData.resourcePath + "/Prefabs/";
                if (GlobalData.GDS.isMobile) {
                    // Mobile buttons spawn at the bottom
                    parentTransform = GameObject.Find("Section/ColorBorder/TabBackGround/ButtonPanel").transform;
                    // Remove all old buttons
                    foreach (Transform child in parentTransform) {
                        Destroy(child.gameObject);
                    }
                    prefabPath += "Mobile";
                }

                if (buttonType.Equals("Step") && ds.GetSectionsList().FindIndex((string obj) => obj.Equals(tm.getCurrentSection())) + 1 == tm.SectionContentPar.transform.childCount - 1) {
                    //spawn a button for Complete Case?
                    Instantiate(Resources.Load(prefabPath + "LastSectionExit") as GameObject, parentTransform).transform.SetAsLastSibling();
                    //print("LAST AVAILABLE TAB IN LAST AVAILABLE SECTION");
                } else {
                    Instantiate(Resources.Load(prefabPath + "ProceedToNext" + buttonType) as GameObject, parentTransform).transform.SetAsLastSibling();
                }
                if (ds.forceInOrder && buttonType.Equals("Step") && !ds.GetData(tm.getCurrentSection()).AllTabsVisited()) {
                    parentTransform.GetChild(parentTransform.childCount - 1).GetComponentInChildren<Button>().interactable = false;
                }
                if (parentTransform.GetChild(parentTransform.childCount - 1).Find("PageCount")) {
                    parentTransform.GetChild(parentTransform.childCount - 1).Find("PageCount").GetComponent<TMPro.TextMeshProUGUI>().text = "Page: " + page + "/" + totalPages;
                }
            }
        }

        if (entryList.Count != 0 || transform.GetComponentInParent<ReaderDialogueManagerScript>()) {
            Load(uniqueParentNode);
        } else {
            //Open the entry selector panel. This should auto add an entry if there's only one type to add
            if (!isNested) {
                if (GetComponent<NewCaseSpawnPanelsScript>()) {
                    GetComponent<NewCaseSpawnPanelsScript>().CreateBlanks();
                } else {
                    //In the reader we don't want this to open
                    //OpenAddEntryPanel ();
                }
            }
        }
    }

    /**
	 * Loads the data from XMl
	 */
    private void Load(XmlNode uniqueParent)
    {
        if (entryList.Count == 0) {
            if (GetComponent<HideLabelScript>() != null) {
                GetComponent<HideLabelScript>().HideLabel();
            }
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
            node = xmlDoc.AdvNode(node);

            if (node == null) {
                Debug.Log("No Data to load.");
                return;
            }
        }
        node = xmlDoc.AdvNode(node); //Enter the EntryData. This will put Node on "Parent". The entries are all siblings now

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
                /*spawnQuiz = true;
				XmlNode findParent = tempNodee;
				while (!findParent.Name.Equals ("Parent")) {
					findParent = AdvNode (findParent);
				}
				ds.AddQuiz (findParent.InnerText, tempNodee.InnerXml);
				tempNodee.ParentNode.RemoveChild (tempNodee);*/
                //CreateQuestions (tempNodee.OuterXml); //Tests loading quiz pins into easy to reference objects.
                //Could just load straight into the quizes if desired too. 

                XmlDocument testEmptyQuiz = new XmlDocument();
                testEmptyQuiz.LoadXml(tempNodee.InnerXml);
                if (testEmptyQuiz.GetElementsByTagName("Entry0").Count == 0) {
                    spawnQuiz = false;
                } else {
                    spawnQuiz = true;
                    XmlNode findParent = tempNodee;
                    while (!findParent.Name.Equals("Parent")) {
                        findParent = xmlDoc.AdvNode(findParent);
                    }
                    quizId = findParent.InnerText;
                    ds.AddQuiz(findParent.InnerText, tempNodee.InnerXml);
                }
                tempNodee.ParentNode.RemoveChild(tempNodee);
            }
            if ((tempNodee = element.GetElementsByTagName("DialoguePin").Item(0)) != null) {
                XmlDocument testEmptyDialogue = new XmlDocument();
                testEmptyDialogue.LoadXml(element.GetElementsByTagName("DialoguePin").Item(0).InnerXml);
                if (testEmptyDialogue.GetElementsByTagName("Entry0").Count == 0) {
                    spawnDialogue = false;
                } else {
                    spawnDialogue = true;
                    uid = node.FindUID();
                    ds.AddDialogue(uid, element.GetElementsByTagName("DialoguePin").Item(0).InnerXml);
                    //GiveOutline (labNodee.gObject);
                }
                tempNodee.ParentNode.RemoveChild(tempNodee);
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
            List<Transform> allChildrenn = labNodee.gObject.GetComponentsInChildren<Transform>(true).ToList();

            //Establish commonly used variables as to limit the number of items for Garbage Collection
            Transform nestedParent = null;
            HideLabelScript hls = null;
            TextMeshProUGUI tmpUGUI;
            TMP_InputField tmpInput;

            foreach (Transform child in allChildrenn) {

                //This helps the transforms ignore sub-entries as well. 
                //If a child transform has a HFMS attached, then we ignore everything that is a child of that child.
                if (child.GetComponent<ReaderEntryManagerScript>() && child.GetComponent<ReaderEntryManagerScript>().isNested) {
                    nestedParent = child;
                    continue;
                }
                if (nestedParent != null && child.IsChildOf(nestedParent)) {
                    continue;
                }

                //Check to see if the child is actually something we need to populate with data.
                if (child.tag.Equals("Value") || child.tag.Equals("Image") || child.name.EndsWith("Value")) {
                    //This checks the amount of xml tags with the name of the child we're populating and handles things accordingly
                    int pos = 0;
                    if (element.GetElementsByTagName(child.name).Count == 0) {
                        if ((hls = child.GetComponent<HideLabelScript>()) != null) {
                            hls.HideLabel();
                        }
                        continue;
                    } else if (element.GetElementsByTagName(child.name).Count > 1) {
                        //If there is more than one match, find where the current child sits compared to its siblings which share the same name
                        List<Transform> sameNames = allChildrenn.FindAll((Transform obj) => obj.name.Equals(child.name));
                        pos = sameNames.FindIndex((Transform obj) => obj.Equals(child));
                        print(child.name + ", " + pos + ", " + labNodee.panelType);
                    }

                    //Assign the value in the XML to a string to make the code easier to read
                    string xmlValue = element.GetElementsByTagName(child.name).Item(pos).InnerText;

                    if ((hls = child.GetComponent<HideLabelScript>()) != null && (xmlValue.Equals("") || xmlValue.Equals(hls.customHide))) {
                        if (hls.displayNA) {
                            xmlValue = "N/A";
                        } else if (!string.IsNullOrEmpty(hls.customDisplay)) {
                            xmlValue = hls.customDisplay;
                        } else {
                            hls.HideLabel();
                        }
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
                        tmpInput = child.gameObject.GetComponent<TMP_InputField>();
                        tmpInput.text = UnityWebRequest.UnEscapeURL(xmlValue);
                        tmpInput.onEndEdit.Invoke(UnityWebRequest.UnEscapeURL(xmlValue));
                    } else if (child.gameObject.GetComponent<TextMeshProUGUI>() != null) {
                        tmpUGUI = child.gameObject.GetComponent<TextMeshProUGUI>();
                        //input.linkedTextComponent.autoSizeTextContainer = true;
                        tmpUGUI.text = UnityWebRequest.UnEscapeURL(xmlValue);
                        //input.linkedTextComponent.GetComponent<ContentSizeFitter>().enabled = true;
                        //input.fontSize = input.fontSize + 1;
                        if (tmpUGUI.linkedTextComponent != null) {
                            NextFrame.Function(delegate {
                                //input.fontSize = input.fontSize - 1;
                                tmpUGUI.text = tmpUGUI.text + " ";
                                if (!tmpUGUI.transform.parent.Find("ImageColumn/ImageBG").gameObject.activeInHierarchy) {
                                    print(tmpUGUI.renderedHeight + ", " + tmpUGUI.linkedTextComponent.renderedHeight + ", " + tmpUGUI.preferredHeight + ", " + tmpUGUI.rectTransform.sizeDelta.y);
                                    tmpUGUI.transform.parent.Find("ImageColumn").GetComponent<LayoutElement>().minHeight = (float)Math.Ceiling(tmpUGUI.preferredHeight);
                                    tmpUGUI.linkedTextComponent.GetComponent<LayoutElement>().preferredHeight = 2;// input.preferredHeight - input.transform.parent.Find("ImageColumn").GetComponent<RectTransform>().rect.height;
                                } else {
                                    print("tmpUGUI preferred height: " + tmpUGUI.preferredHeight + ", " + tmpUGUI.GetComponent<RectTransform>().rect.height + ", " + tmpUGUI.linkedTextComponent.preferredHeight);
                                    tmpUGUI.linkedTextComponent.GetComponent<LayoutElement>().preferredHeight = tmpUGUI.preferredHeight - tmpUGUI.transform.parent.Find("ImageColumn").GetComponent<RectTransform>().rect.height;
                                    //print(tmpUGUI.textInfo.linkInfo[0].linkTextLength + ", " + tmpUGUI.textInfo.linkInfo[0].linkTextfirstCharacterIndex);
                                }
                            });

                            //Attempting to remove some white space at the bottom
                            Rect r = tmpUGUI.linkedTextComponent.GetComponent<RectTransform>().rect;
                            r.size = new Vector2(tmpUGUI.linkedTextComponent.GetComponent<RectTransform>().rect.x, tmpUGUI.linkedTextComponent.GetComponent<RectTransform>().rect.y - tmpUGUI.GetComponent<RectTransform>().rect.y);
                        }
                    } else if (child.name.Equals("Image") && child.GetComponent<ReaderOpenImageUploadPanelScript>()) {
                        //Debug.Log("LOADING IMAGE: " + xmlValue);
                        child.GetComponent<ReaderOpenImageUploadPanelScript>().SetGuid(xmlValue);
                        child.GetComponent<ReaderOpenImageUploadPanelScript>().LoadData(xmlValue);

                        /*Image img = child.GetComponent<Image>();
						img.sprite = null;

						if (tm.transform.GetComponent<ReaderDataScript>().GetImageKeys().Contains(element.GetElementsByTagName(child.name).Item(0).InnerText)) { //Load image
							img.sprite = tm.transform.GetComponent<ReaderDataScript>().GetImage(xmlValue).sprite;
						}

						if (img.sprite == null) {
							img.GetComponent<CanvasGroup>().alpha = 0f;
							img.transform.parent.GetComponent<Image>().enabled = false;
							child.parent.gameObject.SetActive(false);
						} else {
							img.GetComponent<CanvasGroup>().alpha = 1f;
							img.transform.parent.GetComponent<Image>().enabled = false;
							child.parent.gameObject.SetActive(true); //Set the image's parent active
							img.GetComponent<ReaderOpenImageUploadPanelScript>().ForceStart();
						}*/
                    }
                }
            }

            if (!isQuizPanel && labNodee.gObject.transform.GetSiblingIndex() == 0) {
                if (ds.GetData(tm.getCurrentSection()).GetCurrentTab().type.Equals("Personal Info")) {
                    if (Application.platform == RuntimePlatform.WebGLPlayer) {
                        labNodee.gObject.transform.Find("BasicDetailsColumn/BasicDetailsRows/Row0/RecordValue").GetComponent<TextMeshProUGUI>().text = "######";
                        labNodee.gObject.transform.Find("BasicDetailsColumn/CharacterCreation").gameObject.SetActive(false);
                    } else {
                        if (GlobalData.caseObj.recordNumber != null && !GlobalData.caseObj.recordNumber.Equals("") && !GlobalData.fileName.StartsWith("[CHECKFORDUPLICATE]")) {// && !ds.GetReaderOnly()) {
                                                                                                                                                                               //print(GlobalData.caseObj.recordNumber);
                            labNodee.gObject.transform.Find("BasicDetailsColumn/BasicDetailsRows/Row0/RecordValue").GetComponent<TextMeshProUGUI>().text = GlobalData.caseObj.recordNumber;
                        } else {
                            //To manually set the record number and prevent any slipups
                            labNodee.gObject.transform.Find("BasicDetailsColumn/BasicDetailsRows/Row0/RecordValue").GetComponent<TextMeshProUGUI>().text = "######";
                        }
                    }
                }
            }

            //If the PinArea is found, add pin objects as appropriate
            if (spawnDialogue && DialogueManagerScript.GetUID(tm.getCurrentSection(), labNodee.gObject.transform) == uid) {
                Transform pinParent = labNodee.gObject.transform;
                var pinHandler = pinParent.GetComponent<PinHandler>();
                if (pinHandler) {
                    pinHandler.AddDialoguePin();
                } else {
                    if (labNodee.gObject.transform.Find("ResponseFeedback") != null) {
                        Debug.Log("HIDING DIALOGUE SOMEWHERE");
                        pinParent = labNodee.gObject.transform.Find("ResponseFeedback");
                    }
                    GameObject pinObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/ShowDialogue") as GameObject;
                    pinObj = Instantiate(pinObj, pinParent);
                    Button b = pinObj.transform.GetChild(0).GetComponent<Button>();
                    b.onClick.AddListener(delegate {
                        if (Input.GetMouseButton(1)) { //If right clicked
                            Debug.Log("OKAY");
                            return;
                        }
                        Transform t = Instantiate(Resources.Load(GlobalData.resourcePath + "/Prefabs/Panels/DialoguePopUp") as GameObject, GameObject.Find("Canvas").transform).transform;
                        //Transform t = Instantiate(Resources.Load(GlobalData.resourcePath + "/Prefabs/Panels/DialogueBG") as GameObject, GameObject.Find("Canvas").transform).transform;
                        t.gameObject.SetActive(true);
                        t.GetComponentInChildren<ReaderEntryManagerScript>().SetPin(b.transform);
                        t.GetComponentInChildren<ReaderDialogueManagerScript>().PopulateDialogue(b.transform);
                        if (t.Find("Image")) {
                            t.Find("Image").GetComponent<Image>().color = ds.GetImage(tm.getCurrentSection()).color; //ds.GetImage(tm.getCurrentSection()) != null ? ds.GetImage(tm.getCurrentSection()).color : GlobalData.GDS.defaultGreen;
                        }
                        /*
                        //Shows Cordelia's dialogue background
                        CanvasGroup gaudyBG = GameObject.Find("GaudyBG").GetComponent<CanvasGroup>();
                        gaudyBG.alpha = 0.0f;
                        gaudyBG.interactable = false;
                        gaudyBG.blocksRaycasts = false;
                        */
                    });
                    pinObj.tag = "Value";
                    pinObj.name = "Dialogue" + "Pin";
                }

                labNodee.gObject.SetActive(true);

                if (ds.GetData(tm.getCurrentSection()).GetCurrentTab().type.Equals("Quiz")) {
                    //labNodee.gObject.transform.Find("Line").gameObject.SetActive(true);
                }
                spawnDialogue = false;
            }
            if (spawnQuiz && DialogueManagerScript.GetUID(tm.getCurrentSection(), labNodee.gObject.transform) == quizId) { //Remove false when quiz pins are active
                Transform pinParent = labNodee.gObject.transform;
                var pinHandler = pinParent.GetComponent<PinHandler>();
                if (pinHandler) {
                    pinHandler.AddQuizPin();
                } else {
                    if (labNodee.gObject.transform.Find("ResponseFeedback") != null) {
                        Debug.Log("HIDING DIALOGUE SOMEWHERE");
                        pinParent = labNodee.gObject.transform.Find("ResponseFeedback");
                    }

                    GameObject pinObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/QuizPinIcon") as GameObject;
                    if (labNodee.gObject.transform.Find("DialoguePin")) {
                        pinObj = Instantiate(pinObj.transform.Find("ShowQuiz").gameObject, pinParent.Find("DialoguePin"));
                    } else {
                        pinObj = Instantiate(pinObj, pinParent);
                    }
                    //print((pinObj.GetComponent<Button>() == null) + ", " + (pinObj.GetComponentInChildren<Button>() == null));
                    Button b = pinObj.GetComponentInChildren<Button>();
                    b.onClick.AddListener(delegate {
                        //ButtonListenerFunctionsScript.OpenQuizEditor(b, instantiatePanel("QuizEditorBG"));
                        Transform t = Instantiate(Resources.Load(GlobalData.resourcePath + "/Prefabs/Panels/QuizPopUp") as GameObject, ds.transform).transform;
                        t.gameObject.SetActive(true);
                        t.GetComponentInChildren<ReaderEntryManagerScript>().SetPin(b.transform);
                        t.GetComponentInChildren<ReaderEntryManagerScript>().PopulatePanel(b.transform);
                        t.Find("Image").GetComponent<Image>().color = ds.GetImage(tm.getCurrentSection()).color;
                    });
                    pinObj.tag = "Value";
                    pinObj.name = "Quiz" + "Pin";
                }

                labNodee.gObject.SetActive(true);

                spawnQuiz = false;
            }
            if (spawnFlag) {
                GameObject pinObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/FlagPinIcon") as GameObject;
                pinObj = Instantiate(pinObj, labNodee.gObject.transform);
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
                pinObj = Instantiate(pinObj, labNodee.gObject.transform);
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

            if (labNodee.gObject.GetComponent<DialogueEntryScript>()) {
                DialogueEntryScript des = labNodee.gObject.GetComponent<DialogueEntryScript>();
                //des.dialogue = des.transform.Find("Image/Text").GetComponent<Text>().text;
                des.characterName = des.transform.Find("Character/characterName").GetComponent<TextMeshProUGUI>().text;
                //float[] cVals = ParseColor(des.transform.Find("Image/charColor").GetComponent<Text>().text); //Old, wide stretch
                //float[] cVals = ParseColor(des.transform.Find("TextContainer/Image/charColor").GetComponent<Text>().text); //Telegram style

                float[] cVals; //This setup is for the background shade
                if (des.transform.Find("TextContainer/BGFitter") != null) {
                    cVals = ParseColor(des.transform.Find("TextContainer/BGFitter/Image/charColor").GetComponent<TextMeshProUGUI>().text);
                } else {
                    cVals = ParseColor(des.transform.Find("TextContainer/Image/charColor").GetComponent<TextMeshProUGUI>().text);
                }

                /*
				float[] cVals; //This setup is for the background shade
				if (des.transform.Find("TextContainer/BGFitter") != null) {
					cVals = ParseColor(des.transform.Find("TextContainer/BGFitter/Image/charColor").GetComponent<Text>().text);
				} else {
					cVals = ParseColor(des.transform.Find("TextContainer/Image/charColor").GetComponent<Text>().text);
				}*/
                des.SetReaderColor(new Color(cVals[0], cVals[1], cVals[2], 1f));
                GetComponentInParent<ReaderDialogueManagerScript>().AddEntry(des);
            }

            ii++; //For itterating to the next entry in entryList
            if (ii >= entryList.Count) {
                break;
            }
        }
        ToggleEntry(0);

        //Handle hiding dialogue
        if (!isNested && GetComponentInParent<ReaderDialogueManagerScript>()) {
            for (int i = 0; i < transform.childCount; i++) {
                if (transform.GetChild(i).GetComponent<DialogueChoiceScript>()) {
                    transform.GetChild(i).GetComponent<DialogueChoiceScript>().HideBelow();
                    break;
                }
            }
        }

        if (isPuzzle) {
            if (entryList.Count > 1) {
                bool shuffled = false;
                System.Random rnd = new System.Random();
                while (!shuffled) {
                    bool entryMatched = false;

                    foreach (Transform entry in transform.Cast<Transform>().SelectMany(t => t.GetComponents<Transform>())) {
                        entry.SetSiblingIndex(rnd.Next(0, loadedPrefabs.Length + 1));
                    }

                    foreach (Transform entry in transform.Cast<Transform>().SelectMany(t => t.GetComponents<Transform>())) {
                        if (entry.Find("ReorderBar").GetComponent<ReaderMoveableObjectCursorScript>()) {
                            ReaderMoveableObjectCursorScript dragScript = entry.Find("ReorderBar").GetComponent<ReaderMoveableObjectCursorScript>();

                            if (dragScript.correctPosition == entry.GetSiblingIndex()) {
                                entryMatched = true;
                            }
                        }
                    }

                    if (!entryMatched) {
                        shuffled = true;
                    }
                }
            }

        }
        //Resources.UnloadUnusedAssets();
    }

    public int GetTotalPages()
    {
        int totalPages = 0;
        foreach (string section in ds.GetSectionsList()) {
            totalPages += ds.GetData(section).GetTabList().Count;
        }
        return totalPages;
    }

    public int GetCurrentPage()
    {
        int tabPosInStep = ds.GetData(tm.getCurrentSection()).GetCurrentTab().position + 1;
        int page = 0;
        bool hitCurrentSection = false;
        foreach (string section in ds.GetSectionsList()) {
            if (section.Equals(tm.getCurrentSection())) {
                hitCurrentSection = true;
            }
            if (!hitCurrentSection) {
                page += ds.GetData(section).GetTabList().Count;
            }
        }
        page += tabPosInStep;

        return page;
    }

    private float[] ParseColor(string color)
    {
        color = color.Replace("RGBA", "");
        color = color.Replace("(", "");
        color = color.Replace(")", "");
        string[] colorVals = color.Split(","[0]);

        float[] parsedColorValues = new float[4];
        for (int i = 0; i < 4; i++) {
            parsedColorValues[i] = float.Parse(colorVals[i]);
        }

        return parsedColorValues;
    }

    /**
	 * Parses through a quiz pin's xml to create an easy to use quiz question object list
	 * This is just a proof of concept right now, but will be useful in the reader
	 */
    public void CreateQuestions(string xml)
    {
        XmlDocument quiz = new XmlDocument();
        quiz.LoadXml(xml);
        XmlNode node = quiz.DocumentElement;

        //If the node isn't already on EntryData, get it there
        while (!node.Name.Equals("EntryData")) {
            node = xmlDoc.AdvNode(node);

            if (node == null) {
                Debug.Log("No Data to load.");
                return;
            }
        }
        node = xmlDoc.AdvNode(node); //Enter the EntryData. This will put Node on "Parent". The entries are all siblings now

        XmlDocument questionXml = new XmlDocument();

        questionList = new List<QuizQuestionScript>();
        while (node.NextSibling != null) {
            node = node.NextSibling;

            QuizQuestionScript currentQuestion = new QuizQuestionScript();

            string answerXML = node.OuterXml;
            //print (WWW.UnEscapeURL(answerXML));
            //print (answerXML);
            //questionXml.LoadXml ("<Entry0><PanelType>QuizQuestionOption</PanelType><PanelData><OptionValue>1</OptionValue>" +
            //					 "<OptionTypeValue>Correct</OptionTypeValue><FeedbackValue></FeedbackValue></PanelData></Entry0>");
            questionXml.LoadXml(answerXML);


            currentQuestion.questionText = UnityWebRequest.UnEscapeURL(questionXml.GetElementsByTagName("QuestionValue").Item(0).InnerText);
            var typeVals = questionXml.GetElementsByTagName("OptionTypeValue");
            foreach (XmlNode typeVal in typeVals) {
                currentQuestion.questionType = UnityWebRequest.UnEscapeURL(typeVal.InnerText);
                if (currentQuestion.questionType == "Multiple Choice" || currentQuestion.questionType == "Checkboxes")
                    break;
            }

            if (questionXml.GetElementsByTagName("Image").Count > 0) {
                currentQuestion.image = UnityWebRequest.UnEscapeURL(questionXml.GetElementsByTagName("Image").Item(0).InnerText);
            }

            if (questionXml.GetElementsByTagName("DialoguePin").Count > 0) {
                XmlNode dialoguePin = questionXml.GetElementsByTagName("DialoguePin").Item(0);
                dialoguePin.ParentNode.RemoveChild(dialoguePin);
            }
            //print (questionXml.GetElementsByTagName ("data").Item (0).InnerXml);
            questionXml.LoadXml(questionXml.GetElementsByTagName("data").Item(0).InnerXml); //Prepare to traverse answers
            XmlNode answerNode = questionXml.DocumentElement;
            while (!answerNode.Name.Equals("EntryData")) {
                answerNode = xmlDoc.AdvNode(answerNode);

                if (answerNode == null) {
                    Debug.Log("No Data to load.");
                    return;
                }
            }
            //print (answerNode.ChildNodes.Count - 1); //Num answers
            for (int i = 0; i < answerNode.ChildNodes.Count - 1; i++) {
                currentQuestion.AddAnswer(
                    UnityWebRequest.UnEscapeURL(questionXml.GetElementsByTagName("OptionValue").Item(i).InnerText), //answer: the answer choice
                    UnityWebRequest.UnEscapeURL(questionXml.GetElementsByTagName("OptionTypeValue").Item(i).InnerText), //feedback: Correct, partially correct, or incorrect
                    UnityWebRequest.UnEscapeURL(questionXml.GetElementsByTagName("FeedbackValue").Item(i).InnerText) //response: The descriptive feedback
                );
            }

            questionList.Add(currentQuestion);
        }
        foreach (QuizQuestionScript q in questionList) {
            //print (q.ToString ());
            string prefab = null;
            switch (q.questionType) {
                case "Multiple Choice":
                    prefab = "MultipleChoicePanel";
                    break;
                case "Checkboxes":
                    prefab = "CheckBoxPanel";
                    break;
                default:
                    break;
            }
            AddEntry(prefab, true);
        }
    }

    /**
	 * Spawns the feedback for quiz questions
	 */
    public void CheckQuizAnswers(Transform answerParent)
    {
        if (isNested) {
            parentManager?.CheckQuizAnswers(answerParent);
            //Make parent manager return bool on whether or not to expand then handle it here?
            //transform.GetComponent<CollapseContentScript>().SetTarget(true);
            return;
        }

        //FeedbackScript feedback = answerParent.parent.Find("SubmitButton").GetComponent<FeedbackScript>();
        FeedbackScript feedback = null;
        for (int i = 0; i < transform.childCount; i++) {
            if (answerParent.IsChildOf(transform.GetChild(i))) {
                feedback = transform.GetChild(i).Find("Submit/SubmitButton").GetComponent<FeedbackScript>();
                break;
            }
        }
        Transform feedbackParent = feedback.shownFeedback.transform.parent;
        for (int i = 0; i < feedbackParent.childCount; i++) {
            if (feedbackParent.GetChild(i).name.EndsWith("(Destroy)")) {
                Destroy(feedbackParent.GetChild(i).gameObject);
            }
        }

        QuizQuestionScript selected = questionList.ToArray()[answerParent.parent.parent.parent.parent.GetSiblingIndex()];
        Toggle[] togs = answerParent.GetComponentsInChildren<Toggle>();
        List<int> selectedIndexes = new List<int>();
        foreach (Toggle tog in togs) {
            if (!tog.isOn) {
                QuizQuestionScript.Answer answer =
                    selected.answers.Find((QuizQuestionScript.Answer ans) =>
                        ans.EqualsAnswer(tog.transform.Find("OptionValue").GetComponent<TextMeshProUGUI>().text));

                if (answerParent.GetComponent<ToggleGroup>() || !answer.feedback.Equals("Correct"))
                    continue;
            }

            string answerString = tog.transform.Find("OptionValue").GetComponent<TextMeshProUGUI>().text;

            feedback.Answer = answerString;
            feedback.OptionValue = tog.transform.parent.Find("Feedback/OptionTypeValue").GetComponent<TextMeshProUGUI>().text;
            // Update loaded feedback text (removes feedback redundancy)
            feedback.Feedback = selected.GetResponse(answerString);
            feedback.IsOn = tog.isOn;

            feedback.ReduceAlpha();
            feedback.ShowResponse();
            feedback.DefaultAlpha();

            //This duplicates the panel after it has been edited so that we can have as many panels as needed.
            GameObject newFeedback = Instantiate(feedback.shownFeedback, feedback.shownFeedback.transform.parent) as GameObject;
            newFeedback.SetActive(true);
            feedback.shownFeedback.gameObject.SetActive(false);
            newFeedback.name = "Feedback (Destroy)";

            if (tog.isOn) {
                selectedIndexes.Add(tog.transform.parent.GetSiblingIndex());
            } else {
                feedback.AddStripes(newFeedback.GetComponent<Image>());
            }
        }

        Tracker.RecordData(Tracker.DataType.quizData,
            GlobalData.caseObj.recordNumber,
            Tracker.GetQuizData(
                GetCurrentPage(),
                questionList.IndexOf(selected),
                selectedIndexes.ToArray())
            );
    }

    [SerializeField]
    private FeedbackScript feedback = null;
    /**
	 * Spawns the feedback for treatment questions
	 */
    public void CheckTreatmentAnswers(Transform answerParent)
    {
        TreatmentFeedback feedback = (TreatmentFeedback)this.feedback;

        Transform feedbackParent = feedback.shownFeedback.transform.parent;
        for (int i = 0; i < feedbackParent.childCount; i++) {
            if (feedbackParent.GetChild(i).name.EndsWith("(Destroy)")) {
                Destroy(feedbackParent.GetChild(i).gameObject);
            }
        }

        Toggle[] togs = answerParent.GetComponentsInChildren<Toggle>();
        List<int> selectedIndexes = new List<int>();
        foreach (Toggle tog in togs) {
            var parent = tog.transform.parent;
            var feedbackResponse = parent.Find("Feedback/OptionTypeValue").GetComponent<TextMeshProUGUI>().text;

            if (!tog.isOn && (answerParent.GetComponent<ToggleGroup>() || feedbackResponse != "Correct"))
                continue;

            feedback.OptionValue = feedbackResponse;
            feedback.entryTrans = parent;
            feedback.IsOn = tog.isOn;
            feedback.Answer = tog.transform.Find("TreatmentValue").GetComponent<TextMeshProUGUI>().text;
            // Update loaded feedback text (removes feedback redundancy)
            feedback.Feedback = parent.Find("Feedback/FeedbackValue").GetComponent<TextMeshProUGUI>().text;
            feedback.Outcome = parent.Find("Feedback/OutcomeValue").GetComponent<TextMeshProUGUI>().text;
            feedback.IsOn = tog.isOn;

            feedback.ReduceAlpha();
            feedback.ShowResponse();
            feedback.DefaultAlpha();

            //This duplicates the panel after it has been edited so that we can have as many panels as needed.
            GameObject newFeedback = Instantiate(feedback.shownFeedback, feedback.shownFeedback.transform.parent) as GameObject;
            newFeedback.SetActive(true);
            feedback.shownFeedback.gameObject.SetActive(false);
            newFeedback.name = "Feedback (Destroy)";

            if (tog.isOn) {
                selectedIndexes.Add(tog.transform.parent.GetSiblingIndex());
            } else {
                feedback.AddStripes(newFeedback.GetComponent<Image>());
            }
        }
    }
    /**
	 * Spawns the feedback for questions where the entry is shown below the option
	 */
    public void CheckAnswers(Transform answerParent)
    {
        Toggle[] togs = answerParent.GetComponentsInChildren<Toggle>();
        foreach (Toggle tog in togs) {
            var parent = tog.transform;
            while (parent.parent != answerParent)
                parent = parent.parent;

            var feedbackResponse = parent.Find("Feedback/OptionTypeValue").GetComponent<TextMeshProUGUI>().text;

            if (!tog.isOn && (answerParent.GetComponent<ToggleGroup>() || feedbackResponse != "Correct")) {
                parent.Find("Feedback").gameObject.SetActive(false);
                continue;
            }

            if (tog.isOn || feedbackResponse == "Correct") {
                var pinHandler = tog.GetComponentInParent<PinHandler>();
                if (pinHandler)
                    pinHandler.ShowPins();
            }

            feedback.entryTrans = parent;
            feedback.OptionValue = feedbackResponse;
            feedback.IsOn = tog.isOn;

            feedback.ShowResponse();
        }
    }

    //Testing dialogue option
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
            LES.gObject.GetComponentInChildren<MoveableObjectCursorScript>().RemoveFromEntryList(entry.transform);
        }
        ds.GetDialogues().Remove(uniquePath + entry.name);
        ds.correctlyOrderedDialogues.Remove(uniquePath + entry.name);
        ds.GetQuizes().Remove(uniquePath + entry.name);
        ds.correctlyOrderedQuizes.Remove(uniquePath + entry.name);
        GameObject.Destroy(entry);
    }

    private void ScrollToBottom()
    {
        transform.GetComponentInParent<ScrollRect>().verticalScrollbar.value = 0;
        willScroll = false;
    }

    /**
	 * Opens the panel for the user to choose which kind of entry they'd like to add
	 */
    public void OpenAddEntryPanel()
    {
        addData = false;
        string tabType = ds.GetData(tm.getCurrentSection()).GetCurrentTab().type;
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
        Dropdown ddc = dd.GetComponentInChildren<Dropdown>();
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
            NextFrame.Function(ScrollToBottom);
            willScroll = true;
        }
        ToggleEntry(entryList.Count - 1);
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
                NextFrame.Function(ScrollToBottom);
                willScroll = true;
            }

            ToggleEntry(entryList.Count - 1);
            return;
        }
        Dropdown ddc = tm.transform.Find("EntrySelectorBG").GetComponentInChildren<Dropdown>();
        AddEntry(ddc.GetComponentInChildren<TMPro.TextMeshProUGUI>().text, true);
        MoveableObjectCursorScript move;
        foreach (LabEntryScript entry in entryList) {
            if ((move = entry.gObject.GetComponentInChildren<MoveableObjectCursorScript>()) != null)
                move.UpdateEntryList();
        }
        transform.GetComponentInParent<ScrollRect>().GraphicUpdateComplete();

        if (!willScroll && !isNested) {
            NextFrame.Function(ScrollToBottom);
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
    }

    public void RemoveEntries(Transform parent)
    {
        foreach (Transform child in parent) {
            GameObject.Destroy(child.gameObject);
        }
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
            } else {
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
        }

        //Check to see if this is a nested entry. If it is, then we don't change MethodToCall
        if (!isNested && !isQuizPanel) { //If not nested
            tm.MethodToCall = AddToDictionary;
            if (isNew) {
                if (newEntryObject?.transform.Find("Row0/RowControls/CollapseButton") != null) {
                    newEntryObject.transform.Find("Row0/RowControls/CollapseButton").GetComponent<CollapseContentScript>().SetTarget(true);
                }
            }
        }
        //tm.MethodToCall = AddToDictionary;

        //since the quiz panel is persistant, we have to manually restart each entry's inner HistoryFieldManagerScript
        if (isQuizPanel && !isNested) {
            newEntryObject.GetComponentInChildren<ReaderEntryManagerScript>().PopulatePanel(panelPin);
        }
    }


    public void AddDialogue(string CharacterName)
    {
        GameObject newEntryObject = null;
        string PrefabName;
        if (UnityWebRequest.UnEscapeURL(CharacterName).Equals("Provider")) {
            PrefabName = "DialogueEntryRight";
        } else {
            PrefabName = "DialogueEntryLeft";
        }
        foreach (GameObject entry in loadedPrefabs) {
            if (PrefabName.Equals(entry.name)) {
                newEntryObject = Instantiate(entry, transform);
                LabEntryScript newEntry;
                newEntry = new LabEntryScript(entryList.Count, null, null, newEntryObject, PrefabName);
                entryList.Add(newEntry);
                newEntryObject.name = "LabEntry: " + newEntry.GetPosition();
                if (UnityWebRequest.UnEscapeURL(CharacterName).Equals("Instructor")) {
                    newEntryObject.transform.Find("Character/ProviderImage").gameObject.SetActive(true);
                    newEntryObject.transform.Find("Character/PatientImage").gameObject.SetActive(false);
                }
            }
        }

        //Check to see if this is a nested entry. If it is, then we don't change MethodToCall
        if (!isNested && !isQuizPanel) { //If not nested
                                         //tm.MethodToCall = AddToDictionary;
        }

        //tm.MethodToCall = AddToDictionary;

        //Need to set dialogue color and that's basically it
        if (newEntryObject.GetComponent<DialogueEntryScript>()) {
            //We pass in the character name for PrefabName for now I guess? Just to see if it works
            newEntryObject.GetComponent<DialogueEntryScript>().characterName = CharacterName;
            //newEntryObject.GetComponent<DialogueEntryScript>().SetReaderColor(CharacterName); //Color will be set later once we're sure the value is present
            newEntryObject.transform.Find("Character/characterName").GetComponent<TextMeshProUGUI>().text = CharacterName;
        }

        GetComponentInParent<ReaderDialogueManagerScript>().AddEntry(newEntryObject.GetComponent<DialogueEntryScript>());
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

        if (transform.GetComponentInParent<ReaderDialogueManagerScript>()) {
            uniquePath = GetComponentInParent<ReaderDialogueManagerScript>().GetUID();
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
        Transform tempPin = panelPin;
        if (!panelPin.GetComponentInParent<ReaderEntryManagerScript>()) {
            Transform uniqueParent = panelPin;
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
                    uniquePath = tempPin.name;
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
    public void PopulatePanel(Transform pin)
    {
        panelPin = pin;
        entryList = new List<LabEntryScript>();
        populating = true;
        Start();
    }

    public void DestroyPanel(GameObject panel)
    {
        Destroy(panel);
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
    public void SetPin(Transform pin)
    {
        panelPin = pin;
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

    public void ShowAllOptions()
    {
        Transform[] children = transform.gameObject.GetComponentsInDirectChildren<Transform>();

        foreach (Transform child in children) {
            CanvasGroup cg = child.GetComponent<CanvasGroup>();

            cg.alpha = 1.0f;
            cg.interactable = true;
            cg.blocksRaycasts = true;
            child.GetComponent<LayoutElement>().ignoreLayout = false;
            child.GetComponent<HorizontalLayoutGroup>().enabled = true;
        }
    }
}