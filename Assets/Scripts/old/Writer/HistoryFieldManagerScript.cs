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

public class HistoryFieldManagerScript : MonoBehaviour
{

    public GameObject[] loadedPrefabs;
    public String[] loadedPrefabsDisplayNames;
    private List<LabEntryScript> entryList;
    public GameObject parentTab;
    private XmlDocument xmlDoc;
    private TabManager tm;
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
        //FindUniqueParent();
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
            uniquePath = tm.GetCurrentSectionKey() + "/" + uniquePath;
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
            uniquePath = tm.GetCurrentSectionKey() + "/" + uniquePath;
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
            return;
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
            while (tempNode != null) { //&& tempNode.ParentNode.HasChildNodes
                XmlNode a = xmlDoc.AdvNode(tempNode);
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
            return;
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
            node = xmlDoc.AdvNode(node);

            if (node == null) {
                Debug.Log("No Data to load.");
                return;
            }
        }
        node = xmlDoc.AdvNode(node); //Enter the EntryData. This will put Node on "Parent". The entries are all siblings now

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
                answerNode = xmlDoc.AdvNode(answerNode);

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

        string data = getData(idx);
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
    }

    /**
	 * Adds a specified entry from the AddEntryPanel
	 */
    public void AddEntryFromPanel(string name)
    {
        addData = false;
        AddEntry(name, true);
        foreach (LabEntryScript entry in entryList) {
            //if ((move = entry.gObject.GetComponentInChildren<MoveableObjectCursorScript>()) != null)
                //move.UpdateEntryList();
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
        //MoveableObjectCursorScript move;
        foreach (LabEntryScript entry in entryList) {
            //if ((move = entry.gObject.GetComponentInChildren<MoveableObjectCursorScript>()) != null)
                //move.UpdateEntryList();
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
        //MoveableObjectCursorScript move;
        foreach (LabEntryScript entry in entryList) {
            //if ((move = entry.gObject.GetComponentInChildren<MoveableObjectCursorScript>()) != null)
                //move.UpdateEntryList();
        }
        NextFrame.Function(ScrollToEntry);
    }

    public void RemoveEntries(Transform parent)
    {
        foreach (Transform child in parent) {
            Destroy(child.gameObject);
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
            newEntry = new LabEntryScript(entryList.Count, newEntryObject, loadedPrefabs[0].name);
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
                newEntry = new LabEntryScript(entryList.Count, newEntryObject, PrefabName);
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
                    newEntry = new LabEntryScript(entryList.Count, newEntryObject, PrefabName);
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
                newEntry = new LabEntryScript(entryList.Count, newEntryObject, PrefabName);
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

        foreach (LabEntryScript entry in entryList) {
            //if ((move = entry.gObject.GetComponentInChildren<MoveableObjectCursorScript>()) != null)
                //move.UpdateEntryList();
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
    }


    /**
	 * Saves the quiz data to the quiz dictionary
	 */
    public void AddToQuizDictionary()
    {
    }

    /**
	 * Call this after all data has been retrieved/dealt with
	 */
    public void ReorderDictionaries()
    {
    }

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

    // Grabs the pin associated with the panel (dialogue, quiz, event, etc)
    public void GrabPin(Transform pin)
    {
        currentPin = pin;
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
            return;
        }

        //Navigate to the data of the first entry

        bool inEntryData = false;

        // Search through nodes searching for an entry.
        while (!node.Name.Equals("PanelData") || !inEntryData) {
            if (node.Name.StartsWith("Entry")) {
                inEntryData = true;
            }
            node = xmlDoc.AdvNode(node);

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
                    uid = node.FindUID(); // Get the uid used for dialogue key
            
                    if (node.Name.Equals("EventPin")) {
                        addEvent = true;
                    }
                    if (node.NextSibling != null) {
                        node = node.NextSibling;
                    } else {
                        node = xmlDoc.AdvNode(node);
                    }

                } else {
                    node = xmlDoc.AdvNode(node);
                }
                if (node == null) {
                    return;
                }
            }

            foreach (Transform child in allChildren) {
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
                        node = xmlDoc.AdvNode(node);

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
                node = xmlDoc.AdvNode(node);
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
                node = xmlDoc.AdvNode(node);

                if (node == null) {
                    break;
                }
            }
        }
        ToggleEntry(0);
    }
}