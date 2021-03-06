using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;
using UnityEngine.Networking;

/**
 * This script focuses on loading the data from the DataScript into the fields in the editor
 * Every tab should have this script as a component or if not must call it in some way (would require slight edits)
 */
public class LoadData : MonoBehaviour {

	private XmlDocument xmlDoc;
	private DataScript ds;
	private TabManager tm;
    private GameObject BG;
    private string uid;

	// Use this for initialization
	void Start () {
		BG = GameObject.Find ("GaudyBG");
		ds = BG.GetComponent<DataScript> ();
		tm = BG.GetComponent<TabManager> ();
		LoadXML ();
	}

	/**
	 * Loads the data into xmlDoc to use
	 */
	public void LoadXML()
	{
		string text = ds.GetData (tm.getCurrentSection(), transform.name.Substring(0, transform.name.Length - 3));
		if (text == null) {
			Debug.Log ("No Data to load");
			ds.newTabs.Add (this.transform);
			return;
		}
		Debug.Log ("Loaded data: " + text);
		xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(text);
        //LoadXMLData();
		NextFrame.Function(LoadXMLData);
	}

	/**
	 * Directly loads the data into the data fields in the current tab
	 */
	private void LoadXMLData()
	{
        Transform[] allChildren = transform.gameObject.GetComponentsInChildren<Transform>(true);
        XmlNode node = xmlDoc.FirstChild;
		while (node.Value == null && !node.InnerText.Equals("")) {
			//Debug.Log (node.Name);
			if (node.Name.Equals ("EntryData")) {
				if (node.NextSibling != null)
					node = node.NextSibling;
				else {
					while (node.ParentNode.NextSibling == null) 
						node = node.ParentNode;					
					node = node.ParentNode.NextSibling;
					if (node == xmlDoc.DocumentElement.LastChild)
						return;
				}
			}
			node = AdvNode (node);
			if (node == null) {
				Debug.Log ("No Data to load.");
				return;
			}
		}
        
		//Assign values in spaces
        bool addDialogue = false;
        bool addQuiz = false;
        bool addFlag = false;
        bool addEvent = false;
        foreach (Transform child in allChildren) {
            //Debug.Log("CHILD: " + child.name);

            if (node != null && (child.name.Equals(node.ParentNode.Name) || (child.name.Equals(node.Name) && node.InnerXml.Equals(""))))
            {
                try
                {
                    if (node.Value != null)
                    {
                        if (child.gameObject.GetComponent<InputField>() != null)
                        {
                            child.gameObject.GetComponent<InputField>().text = UnityWebRequest.UnEscapeURL(node.Value);
                        }
                        else if (child.gameObject.GetComponent<Text>() != null)
                        {
                            child.gameObject.GetComponent<Text>().text = UnityWebRequest.UnEscapeURL(node.Value);
                        }
                        else if (child.gameObject.GetComponent<Dropdown>() != null)
                        {
                            int indexValue = 0;

                            foreach (Dropdown.OptionData myOptionData in child.gameObject.GetComponent<Dropdown>().options)
                            {
                                if (myOptionData.text.Equals(UnityWebRequest.UnEscapeURL(node.Value)))
                                {
                                    break;
                                }
                                indexValue++;
                            }
                            child.gameObject.GetComponent<Dropdown>().value = indexValue;
                        }
                        else if (child.gameObject.GetComponent<Toggle>() != null)
                        {
                            child.gameObject.GetComponent<Toggle>().isOn = bool.Parse(node.Value);

                            //child.gameObject.GetComponent<Text>().text = WWW.UnEscapeURL(node.Value);
                        }
                        else if (child.gameObject.GetComponent<Text>() != null)
                        {
                            child.gameObject.GetComponent<Text>().text = UnityWebRequest.UnEscapeURL(node.Value);
                        }
                    }
                    node = AdvNode(node);

                    while (node != null && node.Value == null && !node.InnerText.Equals(""))
                    {
                        if (Regex.IsMatch(node.Name.ToLower(), "tab[0-9]*$"))
                            break;

                        if (node.Name.Equals("EntryData") || node.Name.Equals("DialoguePin") || node.Name.Equals("QuizPin") || node.Name.Equals("FlagPin") || (node.Name.Equals("EventPin")))
                        {
                            findUID(node); // Find uid node and use it as the key
                            if (node.Name.Equals("DialoguePin"))
                            {
                                addDialogue = true;
                                ds.AddDialogue(uid, node.OuterXml);
                            }
                            if (node.Name.Equals("QuizPin"))
                            {
                                addQuiz = true;
                                XmlNode tempNode = node;
                                while (!tempNode.Name.Equals("Parent"))
                                {
                                    tempNode = AdvNode(tempNode);
                                }
                                ds.AddQuiz(tempNode.InnerText, node.InnerXml);
                            }
                            if (node.Name.Equals("FlagPin"))
                            {
                                addFlag = true;
                                ds.AddFlag(uid, node.OuterXml);
                            }
                            if (node.Name.Equals("EventPin"))
                            {
                                addEvent = true;
                            }
                            if (node.NextSibling != null)
                            {
                                node = node.NextSibling;
                            }
                            else
                            {
                                if (node == null)
                                {
                                    break;
                                }
                                if (node == xmlDoc.DocumentElement.LastChild)
                                {
                                    break;
                                }
                                node = node.ParentNode.NextSibling;
                                
                            }
                        }
                        else
                        {
                            node = AdvNode(node);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }

            if (child.name.Equals("PinArea"))
            {
                if (addDialogue)
                {
                    GameObject pinObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/DialoguePinIcon") as GameObject;
                    pinObj = Instantiate(pinObj, child);
                    Button b = pinObj.AddComponent<Button>();
                    b.onClick.AddListener(delegate {
                        /*b.onClick.AddListener(delegate {*/ ButtonListenerFunctionsScript.OpenDialogueEditor(b); //});
                        //b.onClick.AddListener(delegate { ButtonListenerFunctionsScript.OpenDialogueEditor(b, instantiatePanel("DialogueEditorBG")); });
                    });
                    pinObj.tag = "Value";
                    pinObj.name = "Dialogue" + "Pin";

                    addDialogue = false;
                }
                if (addQuiz)
                {
                    GameObject pinObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/QuizPinIcon") as GameObject;
                    pinObj = Instantiate(pinObj, child);
                    Button b = pinObj.AddComponent<Button>();
                    b.onClick.AddListener(delegate {
                        //ButtonListenerFunctionsScript.OpenQuizEditor(b);
						//Delegate d = b.onClick.GetPersistentEventCount();
						//d.GetInvocationList();
						/*b.onClick.AddListener(delegate { */ButtonListenerFunctionsScript.OpenQuizEditor(b, instantiatePanel("QuizEditorBG")); //});
                    });
                    pinObj.tag = "Value";
                    pinObj.name = "Quiz" + "Pin";

                    addQuiz = false;
                }
                if (addFlag)
                {
                    GameObject pinObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/FlagPinIcon") as GameObject;
                    pinObj = Instantiate(pinObj, child);
                    Button b = pinObj.AddComponent<Button>();
                    b.onClick.AddListener(delegate {
                        //ButtonListenerFunctionsScript.OpenFlagEditor(b);
                        b.onClick.AddListener(delegate { ButtonListenerFunctionsScript.OpenFlagEditor(b, instantiatePanel("FlagEventEditorBG")); });
                    });
                    pinObj.tag = "Value";
                    pinObj.name = "Flag" + "Pin";

                    addFlag = false;
                }
                if (addEvent)
                {
                    GameObject pinObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/EventPinIcon") as GameObject;
                    pinObj = Instantiate(pinObj, child);
                    Button b = pinObj.AddComponent<Button>();
                    b.onClick.AddListener(delegate {
                        ButtonListenerFunctionsScript.OpenEventEditor(b, instantiatePanel("EventEditorBG"));
                    });
                    pinObj.tag = "Value";
                    pinObj.name = "Event" + "Pin";

                    addEvent = false;
                }
            }



		}
	}

    // Recrusive function to read all nodes until uid is found
    private void findUID(XmlNode node)
    {
        if (node.Name == "uid")
            uid =  node.InnerText;

        foreach(XmlNode n in node.ChildNodes)
        {
            findUID(n);
        }
    }

    private GameObject instantiatePanel(string panelName)
    {
        GameObject pinPanelPrefab = Instantiate(Resources.Load("Writer/Prefabs/Panels/" + panelName)) as GameObject;
        pinPanelPrefab.transform.SetParent(BG.transform, false);
        return pinPanelPrefab;
    }

    /**
	 * Advance the node to the correct next node in the XmlDoc. Just like Depth first search
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
}
