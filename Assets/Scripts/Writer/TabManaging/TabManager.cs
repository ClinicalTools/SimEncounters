using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.Networking;
using ClinicalTools.SimEncountersOld;

#if UNITY_EDITOR
using UnityEditor;
#endif


/**
 * This class manages Sections and Tabs
 * It also has some functions for storing/loading data
 */
public class TabManager : MonoBehaviour
{
    public static TabManager Instance;

    private Transform[] allChildren;    //Used to hold all transform children when loading and saving.
    private WriterHandler ds;          //Reference to DataScript
    private GameObject currentTab;  //Holds the current tab
    private string currentSection;  //Holds the current section
    private DefaultDataScript ID;   //Default data to load (not used really)
    public GameObject SectionContentPar;    //The parent of Section Buttons
    public GameObject TabButtonContentPar;  //The parent of Tab buttons
    public GameObject TabContentPar;        //The parent of Tab Content
    public delegate void DelegateMethod();  //Data on the page when we dont want the xml to read it
    public DelegateMethod MethodToCall;     //A pointer to an alternative AddToDictionary method. 
    public bool sameTab;			//Whether or not you are calling AddToDictionary on the current tab
    private GameObject BG;
    private string tabName;
    public GameObject addTabButton;


    void Start()
    {
        sameTab = false;
        MethodToCall = null;
        ID = new DefaultDataScript();
        BG = GameObject.Find("GaudyBG");
        ds = WriterHandler.WriterInstance;
    }

    /**
	 * Updates all the section images to those in the imgDict
	 */
    public void ChangeSectionImages()
    {
        foreach (Image t in SectionContentPar.GetComponentsInChildren<Image>()) {
            if (t.transform.name.Equals("Image")) {
                t.sprite = null;
                t.sprite = ds.EncounterData.Images[t.transform.parent.Find("SectionLinkToText").GetComponent<TextMeshProUGUI>().text].sprite;
            }
        }
    }

    /**
	 * Adds the section buttons' icons to the imgDict
	 */
    public void GetSectionImages()
    {
        allChildren = SectionContentPar.GetComponentsInChildren<Transform>();
        foreach (Transform t in allChildren) {
            if (t.name.Equals("Image")) {
                if (t.GetComponent<Image>().sprite == null) {
                    continue;
                }
                if (t.GetComponent<Image>().sprite.name != null && !t.GetComponent<Image>().sprite.name.Equals("")) {
                    string s = t.GetComponent<Image>().sprite.name;
                    SpriteHolderScript shs = new SpriteHolderScript(s);
                    var imgHolder = ds.EncounterData.Images[t.parent.Find("SectionLinkToText").GetComponent<TextMeshProUGUI>().text.Replace(GlobalData.EMPTY_WIDTH_SPACE + "", "")];
                    imgHolder.sprite = t.GetComponent<Image>().sprite;

                    foreach (Image img in shs.iconHolder.GetComponentsInChildren<Image>()) {
                        if (img.sprite.name.Equals(s)) {
                            ds.AddImg(t.parent.Find("SectionLinkToText").GetComponent<TextMeshProUGUI>().text.Replace(GlobalData.EMPTY_WIDTH_SPACE + "", ""), img.transform.parent.name);
                            break;
                        }
                    }
                }
            }
        }
        //Debug.Log (string.Join (", ", ds.GetImages ().Keys.ToArray ()));
        //Debug.Log (string.Join ("-----", ds.GetImages ().Select (x => x.Value.GetXMLText()).ToArray ()));
    }

    /**
	 * Adds the active tab to the dictionary
	 */
    public void AddToDictionary()
    {
        //GetSectionImages ();

        //If another tab has specified another AddToDictionary method, use that one
        if (MethodToCall != null) {
            MethodToCall();
            if (!sameTab) {
                MethodToCall = null;
            }
            return;
        } else {
            if (currentTab == null) {
                return;
            }

            string xml = "<data>";
            string imageData = "<images>";
            int imgCount = 0;

            //string customName = ds.GetData (currentSection).getTabInfo (currentTab.name + currentTab.GetComponent<Text>().text).customName;
            //xml += "<customName>" + customName + "</customName><data>";

            //Sets up the XML string to be passed into the dictionary
            allChildren = currentTab.GetComponentsInChildren<Transform>();
            Transform nextChild = null;
            string UID = "";
            foreach (Transform child in allChildren) {
                //Debug.Log(child.name);
                if (child != null) {
                    if (child.tag.Equals("Image")) { //Handle images
                        Sprite img = child.GetComponent<Image>().sprite;
                        imageData += "<image" + imgCount + ">";

                        UID = child.GetComponent<OpenImageUploadPanelScript>().GetGuid();
                        imageData += "<key>" + UID + "</key>";
                        //imageData += "<key>" + currentSection + currentTab.transform.Find ("TabButtonLinkToText").GetComponent<Text>().text + "</key>";
                        string imgRef;
                        if ((imgRef = ds.EncounterData.Images[currentSection + currentTab.transform.Find("TabButtonLinkToText").GetComponent<TextMeshProUGUI>().text].referenceName) != null) {
                            imageData += imgRef;
                        } else {
                            imageData += "<width>" + img.texture.width + "</width><height>" + img.texture.height + "</height>";
                            Texture2D dictTexture = img.texture;
                            Texture2D newTexture = new Texture2D(dictTexture.width, dictTexture.height, TextureFormat.ARGB32, false);
                            newTexture.SetPixels(0, 0, dictTexture.width, dictTexture.height, dictTexture.GetPixels());
                            newTexture.Apply();
                            byte[] bytes = newTexture.EncodeToPNG();
                            string base64Data = Convert.ToBase64String(bytes);
                            imageData += "<data>" + base64Data + "</data>";
                        }
                        imageData += "</image" + imgCount + ">";
                        imgCount++;
                    } else if (child.GetComponent<HistoryFieldManagerScript>() != null) {
                        xml += child.GetComponent<HistoryFieldManagerScript>().getData();
                        Transform tempChild = child;
                        while (nextChild == null) {
                            if (tempChild.GetSiblingIndex() + 1 == tempChild.parent.childCount)
                                tempChild = tempChild.parent;
                            else
                                nextChild = tempChild.parent.GetChild(tempChild.GetSiblingIndex() + 1);
                        }
                    } else if (nextChild == null && (child.name.ToLower().EndsWith("value") || child.name.ToLower().EndsWith("toggle") || child.tag.Equals("Value"))) { //Input object/field
                        if (child.gameObject.GetComponent<Toggle>() != null) {
                            if (true) {// || child.gameObject.GetComponent<Toggle> ().isOn) { //Leaving as true for now. May change later
                                xml += "<" + child.name + ">";
                                xml += child.gameObject.GetComponent<Toggle>().isOn;
                                xml += "</" + child.name + ">";
                            }
                        } else if (child.name.ToLower().EndsWith("toggle") || child.name.ToLower().EndsWith("radio")) {
                            continue;
                        } else {
                            xml += "<" + child.name + ">";

                            //Handle reading the child
                            if (child.gameObject.GetComponent<InputField>() != null) {
                                xml += UnityWebRequest.EscapeURL(child.gameObject.GetComponent<InputField>().text);
                            } else if (child.gameObject.GetComponent<Text>() != null) {
                                xml += UnityWebRequest.EscapeURL(child.gameObject.GetComponent<Text>().text);
                            } else if (child.gameObject.GetComponent<Dropdown>() != null) {
                                xml += UnityWebRequest.EscapeURL(child.gameObject.GetComponent<Dropdown>().captionText.text);
                            } else if (child.name.Equals("DialoguePin") || child.name.Equals("QuizPin") || child.name.Equals("FlagPin") || child.name.Equals("EventPin")) {
                                Transform uniqueParent = child;
                                string path = "";
                                while (uniqueParent.parent != null && !uniqueParent.parent.name.Equals("Content")) {
                                    uniqueParent = uniqueParent.parent;
                                }
                                path = uniqueParent.name;
                                while (!uniqueParent.name.EndsWith("Tab")) {//Once you hit the Tab container
                                    uniqueParent = uniqueParent.parent;
                                    path = uniqueParent.name + "/" + path;
                                }
                                path = GetCurrentSectionKey() + "/" + path;

                                //If the dialogue entry has been moved, adjust the dictionary accordingly
                                if (child.name.Equals("DialoguePin") && ds.GetDialogues().ContainsKey(path)) {
                                    string dialogueXML = ds.GetDialogues()[path]; //Dialogue data
                                    ds.AddDialogue(path, dialogueXML);

                                    xml += dialogueXML;
                                } else if (child.name.Equals("DialoguePin")) {
                                    xml = xml.Substring(0, xml.Length - ("<" + child.name + ">").Length);
                                    continue;
                                }

                                //If the quiz entry has been moved, adjust the dictionary accordingly
                                if (child.name.Equals("QuizPin") && ds.GetQuizes().ContainsKey(path)) {
                                    string quizXML = ds.GetQuizes()[path]; //Quiz data
                                    ds.AddQuiz(path, quizXML);
                                    xml += quizXML;
                                } else if (child.name.Equals("QuizPin")) {
                                    xml = xml.Substring(0, xml.Length - ("<" + child.name + ">").Length);
                                    continue;
                                }

                                if (child.name.Equals("FlagPin") && ds.GetFlags().ContainsKey(path)) {
                                    string flagXML = ds.GetFlags()[path]; //Dialogue data
                                    ds.AddFlag(path, flagXML);

                                    xml += flagXML;
                                } else if (child.name.Equals("FlagPin")) {
                                    xml = xml.Substring(0, xml.Length - ("<" + child.name + ">").Length);
                                    continue;
                                }



                            }

                            xml += "</" + child.name + ">";
                        }
                    }
                    if (child == nextChild) {
                        nextChild = null;
                    }
                }
            }
            imageData += "</images>";
            xml += "</data>";

            //Add the current Tab to the Dictionary. Replace any existing tab if it exists
            if (ds == null) {
                ds = WriterHandler.WriterInstance;
            }
            //ds.AddData (currentSection, tabName, xml);
            ds.AddSectionTabData(currentSection, currentTab.name.Substring(0, currentTab.name.Length - 3), xml);
            Debug.Log("Saved value: " + xml);
        }
    }

    /**
	 * Clears all data. Useful when loading new file.
	 */
    public void ClearAll()
    {
        foreach (Transform obj in TabButtonContentPar.GetComponentsInChildren<Transform>()) {
            if (!obj.name.Equals(TabButtonContentPar.name)) {
                Destroy(obj.gameObject);
            }
        }
        foreach (Transform obj in SectionContentPar.GetComponentsInChildren<Transform>()) {
            if (!obj.name.Equals(SectionContentPar.name) && !obj.name.Equals("Filler")) {
                Destroy(obj.gameObject);
            }
        }
        foreach (Transform obj in TabContentPar.GetComponentsInChildren<Transform>()) {
            if (!obj.name.Equals(TabContentPar.name)) {
                Destroy(obj.gameObject);
            }
        }
        if (currentTab != null) {
            Destroy(currentTab);
            currentTab = null;
        }
        currentSection = null;

    }

    /**
	 * Switches the active tab
	 * Pass in the formatted tabName
	 */
    public void SwitchTab(string tabName)
    {
        Transform[] tabButtonsList = TabButtonContentPar.GetComponentsInChildren<Transform>();

        /* For debugging
		string str = "";
		foreach (Transform t in tabButtonsList) {
			str += t.name + ", ";
		}
		Debug.Log (str);*/

        string tName = null;
        string section = currentSection;
        string tabType;

        //Check if the tab is specified. If not, let the user choose their tab
        TabInfoScript tabScript = null;
        //int n = -1;
        if (tabName == null || tabName.Equals("")) { //If no provided tab name
            tabScript = ds.EncounterData.OldSections[section].GetCurrentTab(); //Check to see if there were any active tabs for the current section
            if (tabScript == null || tabScript.type == "") { //If no active tab
                                                             //n = 0;
                SectionDataScript sds = ds.EncounterData.OldSections[section];
                if (sds.GetTabList().Count > 0) { //If the section has any tabs at all
                    string tempTabName = sds.GetTabList()[0];
                    tempTabName = tempTabName.Replace('_', ' ').Substring(0, tempTabName.Length - "Tab".Length);
                    sds.SetCurrentTab(tempTabName);
                    tabType = tempTabName;
                } else { //Section has no tabs. New Section. Let user pick tab
                         //transform.Find("TabSelectorBG").gameObject.SetActive(true);
                    return;
                    //This will set the tab as the default specified in the Default data script
                    //sds.SetCurrentTab (new TabInfoScript(0, ID.defaultTab));
                    //tab = ID.defaultTab;
                }
            } else { //Set tab to the section's last active tab
                tabType = tabScript.type;
            }
            Destroy(currentTab);
            currentTab = null;
        } else {
            //Remove the number at the end of tab and store it as n
            /*string[] tabSplit = Regex.Split (tab, @"[0-9]*$");
			n = int.Parse (tab.Substring (tabSplit [0].Length, tab.Length - tabSplit [0].Length));
			tab = tabSplit [0];*/


            /* Debugging test
			GameObject test = Resources.Load(gData.resourcesPath + "/Prefabs/Tabs/" + tab.Replace("_", " ").Substring(0, tab.Length - 3) + " Tab" + "/" + tab.Replace (" ", String.Empty).Replace("_", String.Empty)) as GameObject;
			if (test == null) {
				Debug.Log ("Cannot load tab prefab");
				//return;
			}
			*/

            //If we're switching away from a tab, save the data
            if (currentTab != null) {
                if ((tabName + "Tab").Equals(currentTab.name) && section.Equals(currentSection)) {
                    sameTab = true;
                    AddToDictionary();
                    sameTab = false;
                    return;
                } else {
                    AddToDictionary();

                    tName = currentTab.name/*.Replace (" ", "_")*/ + "Button";
                    foreach (Transform t in tabButtonsList) {
                        if (t.name.Equals(tName)) {
                            t.GetChild(t.childCount - 1).gameObject.SetActive(false);
                            t.GetComponent<Button>().interactable = true;
                        }
                    }
                    Destroy(currentTab);
                    currentTab = null;
                }
            }
            //Debug.Log(string.Join(",", ds.GetData (currentSection).GetTabList().ToArray()));
            tabType = ds.EncounterData.OldSections[currentSection].GetTabInfo(tabName).type;
            if (tabType.ToLower().EndsWith("tab")) { //Remove the "Tab" from the end of the type
                tabType = tabType.Substring(0, tabType.Length - 3);
            }
        }
        Debug.Log("Section: " + currentSection + ", Name: " + tabName + ", Type: " + tabType); //Type is _ + Tab, but should be prefab folder - " Tab"



        //Load in the specified tab
        string name = tabName;
        string folder = tabType + " Tab";
        tabType = tabType.Replace(" ", string.Empty) + "Tab";

        /*if (tabType.Contains (" ")) {
			//name = tabName.Replace (" ", "_") + "Tab";
			tabType = tabType.Replace (" ", String.Empty);
			tabType = tabType + "Tab";
			folder = tabType + " Tab";
		} else {
			//name = tabName.Replace (" ", "_") + "Tab";
			folder = tabType.Replace ("_", " "); 
			if (!folder.EndsWith (" Tab") && folder.EndsWith ("Tab")) {
				folder = folder.Substring (0, folder.Length - 3) + " Tab"; 
			} else if (!folder.EndsWith ("Tab")) {
				folder = folder + " Tab";
			}
			tabType = tabType.Replace ("_", String.Empty); 
			if (!tabType.EndsWith ("Tab")) {
				tabType = tabType + "Tab";
			}
			/*if (!name.EndsWith ("Tab")) {
				name = name + "Tab";
			}
		}*/
        name = tabName;
        //Folder should be the same as the prefab folder. Tab should equal the prefab name. Name should be the link to text

        GameObject par = TabContentPar;
        //Debug.Log ("Prefabs/Tabs/" + folder + "/" + tabType);
        GameObject newPrefab = Resources.Load(GlobalData.resourcePath + "/Prefabs/Tabs/" + folder + "/" + tabType) as GameObject;
        GameObject newTab = Instantiate(newPrefab, par.transform);
        newTab.name = name + "Tab";
        //newTab.AddComponent<Text> ().text = "" + n;
        currentTab = newTab;
        ds.EncounterData.OldSections[section].SetCurrentTab(name);

        if (newTab.transform.Find("AddFieldButton") && ds.EncounterData.Images[GetCurrentSectionKey()] != null) {
            newTab.transform.Find("AddFieldButton").GetComponent<Image>().color = ds.EncounterData.Images[GetCurrentSectionKey()].color;
        }

        //Adjust the buttons
        tName = currentTab.name/*.Replace (" ", "_")*/ + "Button";
        foreach (Transform t in tabButtonsList) {
            if (t.name.Equals(tName)) {
                if (!ds.EncounterData.OldSections[currentSection].IsPersistant(name)) {
                    t.GetChild(t.childCount - 1).gameObject.SetActive(true);
                }
                t.GetComponent<Button>().interactable = false;
                t.GetComponent<ScriptButtonFixScript>().FixTab();
                t.GetComponent<ScriptButtonFixScript>().FixTab();


            }
        }

        if (addTabButton != null) {
            addTabButton.transform.SetAsLastSibling();
            if (ds.EncounterData.OldSections[currentSection].GetTabList().Count >= 8) {
                addTabButton.SetActive(false);
            } else {
                addTabButton.SetActive(true);
            }
        }

        //Debug.Log(ds.GetData(getCurrentSection()).GetAllData());
        Resources.UnloadUnusedAssets();
    }
    
    // temporary
    public void SetCurrent(string sectionName)
    {
        currentSection = sectionName;
    }

    /**
	 * Switches the active section.
	 * Pass in the LinkToText
	 */
    public void SwitchSection(string sectionName)
    {
        string sName = null;
        //Disable the text and edit button of the section we're switching away from (show only the icon)
        List<Button> sectionButtonsList = SectionContentPar.GetComponentsInChildren<Button>().ToList();
        sectionButtonsList.RemoveAt(sectionButtonsList.Count - 1); //Remove the AddSectionButton
        if (currentSection != null && !currentSection.Equals("")) {
            sName = currentSection.Replace(" ", "_") + "Button";
            foreach (Button t in sectionButtonsList) {
                if (t.transform.name.Equals(sName)) {
                    Transform[] components = t.transform.GetComponentsInChildren<Transform>();
                    foreach (Transform c in components) {
                        if (!c.name.Equals(t.name) && !c.name.Equals("SectionDisplayTMP")) {
                            c.gameObject.SetActive(false);
                        }
                        if (c.name.Equals("SectionDisplayTMP") && c.GetComponent<TextMeshProUGUI>().preferredWidth > 85) {
                            c.GetComponent<LayoutElement>().preferredWidth = 85;
                        }
                    }
                    t.transform.GetChild(0).gameObject.SetActive(true);
                    t.interactable = true;
                }
            }
        }

        if (sectionName == null || sectionName.Equals("")) { //No section provided. Use default
            if (ID == null) {
                ID = new DefaultDataScript();
            }
            sectionName = ID.defaultSection;
            print(string.Join(",", sectionButtonsList.Select(b => b.name)));
            if (sectionButtonsList.Count > 0 && sectionButtonsList[0].name.Length > 0) {
                sectionName = sectionButtonsList[0].name.Remove(sectionButtonsList[0].name.Length - "Button".Length);
            }
            print(sectionName);
        } else if (sectionName.Equals(currentSection)) { //Trying to switch to current section
            return;
        }

        if (currentSection != null && !currentSection.Equals("")) { //Save data before switching away
            AddToDictionary();
            currentSection = null;
            currentTab = null;
        }

        currentSection = sectionName;
        //Destroy Tab buttons and content
        foreach (Transform child in TabContentPar.GetComponentInChildren<Transform>()) {
            Destroy(child.gameObject);
        }

        foreach (Transform child in TabButtonContentPar.GetComponentInChildren<Transform>()) {
            if (child.name != "AddTabButton") {
                Destroy(child.gameObject);
            }
        }
        //Enable the new section's text and edit button
        sName = currentSection.Replace(" ", "_") + "Button";
        foreach (Button t in sectionButtonsList) {
            if (t.name.Equals(sName) && !t.name.Equals("AddSectionButton")) {
                Transform[] components = t.GetComponentsInChildren<Transform>(true);
                foreach (Transform c in components) {
                    if (c.name.Equals("SectionDisplayText")) {
                        continue;
                    }
                    c.gameObject.SetActive(true);
                    if (c.name.Equals("SectionDisplayTMP")) {
                        if (c.GetComponent<TextMeshProUGUI>().preferredWidth > 270) {
                            c.GetComponent<LayoutElement>().preferredWidth = 270;
                        } else {
                            c.GetComponent<LayoutElement>().preferredWidth = -1;
                        }
                    }
                }
                t.GetComponent<Button>().interactable = false;
                t.GetComponent<ScriptButtonFixScript>().FixTab();
            }
        }

        //Load in the Tab buttons for each tab
        List<string> sectionTabs = null;
        if (ds.EncounterData.OldSections[currentSection] != null) {
            sectionTabs = ds.EncounterData.OldSections[currentSection].GetTabList();
        }
        int i = 0;
        if (sectionTabs == null || sectionTabs.Count == 0) { //Spawn a default tab

            //transform.Find("TabSelectorBG").gameObject.SetActive(true); //Let the user choose their new tab
            GameObject savePrefab = Instantiate(Resources.Load("Writer/Prefabs/Panels/TabSelectorBG")) as GameObject;
            savePrefab.transform.SetParent(BG.transform, false);

            if (savePrefab.transform.Find("TabSelectorPanel/RowTitle/CancelButton")) {
                savePrefab.transform.Find("TabSelectorPanel/RowTitle/CancelButton").gameObject.SetActive(false);
            }
        } else {
            //Spawn in the section's tab buttons
            Debug.Log("TABS = " + string.Join(",", sectionTabs.ToArray()));
            foreach (string tabName in sectionTabs) {
                GameObject newTab = Resources.Load(GlobalData.resourcePath + "/Prefabs/TabButton") as GameObject;
                TextMeshProUGUI[] children = newTab.GetComponentsInChildren<TextMeshProUGUI>();
                foreach (TextMeshProUGUI child in children) {
                    if (child.name.Equals("TabButtonLinkToText")) { //Where the button links to
                        child.text = tabName;
                    } else if (child.name.Equals("TabButtonDisplayText")) { //What the button displays
                        string customName = ds.EncounterData.OldSections[currentSection].GetTabInfo(tabName).customName;
                        if (customName != null) {// && customName != tabName) {
                            child.text = customName;
                        } else {
                            if (child.text.Contains(" ")) {
                                //child.text = tabName.Replace ("_", " ").Substring (0, tabName.Length - "Tab".Length); //Unformat it
                                //child.text = child.text.Replace(" ", "_") + "Tab";
                            }
                            child.text = tabName;
                            ds.EncounterData.OldSections[currentSection].GetTabInfo(tabName).customName = child.text;
                        }
                    }
                }
                //The button's position
                i++;
                newTab = Instantiate(newTab, TabButtonContentPar.transform);
                newTab.name = tabName/*.Replace (" ", "_")*/ + "TabButton";
            }
            //transform.Find ("TabSelectorBG/TabSelectorPanel/Content/ScrollView/Viewport/Content/BackgroundInfoTabPanel").gameObject.SetActive (activateBGInfoOption);
            for (int j = 0; j < TabButtonContentPar.transform.childCount; j++) {
                //foreach (string tabName in sectionTabs) {
                //TabButtonContentPar.transform.Find (tabName/*.Replace (" ", "_")*/ + "TabButton").GetComponent<TabAndSectionDragScript> ().UpdateEntryList ();
                if (!TabButtonContentPar.transform.GetChild(j).name.Equals("AddTabButton")) {
                    //TabButtonContentPar.transform.GetChild(j).GetComponent<TabAndSectionDragScript>().UpdateEntryList();
                }
            }
        }
        //If the section had a custom icon image (and can load it), then set the tab background bar color to the icon image color
        if (currentSection != null && ds.EncounterData.Images.ContainsKey(currentSection)) {
            if (ds.EncounterData.Images[currentSection].color.a > 0) {
                GameObject.Find("TabButtonsPanel").GetComponent<Image>().color = ds.EncounterData.Images[currentSection].color;
            } else {
                GameObject.Find("TabButtonsPanel").GetComponent<Image>().color = ID.defaultColor;
            }
        } else if (currentSection == ID.defaultSection) {
            GameObject.Find("TabButtonsPanel").GetComponent<Image>().color = ID.defaultColor;
        }

        addTabButton.transform.SetAsLastSibling();

        //Switch to the default/last active tab of the new section
        TabInfoScript tab = ds.EncounterData.OldSections[currentSection].GetCurrentTab();
        if (tab != null) {
            string formattedName = tab.customName;//.Replace (" ", "_") + "Tab";
                                                  //Debug.Log ("CUSTOM NAME: " + tab.customName);
            setTabName(formattedName);
            SwitchTab(formattedName);
        } else if (ds.EncounterData.OldSections[GetCurrentSectionKey()].GetTabList().Count <= 0) {
            return;
        } else {
            //Debug.Log (string.Join (",", ds.GetData (getCurrentSection ()).GetTabList ().ToArray ()));
            TabInfoScript newTabInfo = ds.EncounterData.OldSections[GetCurrentSectionKey()].GetTabInfo(ds.EncounterData.OldSections[GetCurrentSectionKey()].GetTabList()[0]);
            string formattedName = newTabInfo.customName;//.Replace (" ", "_") + "Tab";
                                                         //Debug.Log ("DEFAULT'S CUSTOM NAME: " + newTabInfo.customName);
            setTabName(formattedName);
            SwitchTab(formattedName);
            //SwitchTab (ds.GetData (getCurrentSection ()).GetTabList () [0]);

        }
    }

    /**
	 * Updates the tab buttons. Currently not in use
	 */
    public void updateTabButtons()
    {
        List<string> sectionTabs = ds.EncounterData.OldSections[currentSection].GetTabList();
        Debug.Log(sectionTabs.Count + ", " + string.Join(", ", sectionTabs.ToArray()));
        foreach (string tabName in sectionTabs) {
            GameObject newTab = Resources.Load(GlobalData.resourcePath + "/Prefabs/TabButton") as GameObject;
            TextMeshProUGUI[] children = newTab.GetComponentsInChildren<TextMeshProUGUI>();
            foreach (TextMeshProUGUI child in children) {
                if (child.name.Equals("TabButtonLinkToText")) { //Where the button links to
                    child.text = tabName;
                } else if (child.name.Equals("TabButtonDisplayText")) { //What the button displays
                    child.text = tabName.Replace("_", " ").Substring(0, Regex.Split(tabName, "[0-9]*$")[0].Length - "Tab".Length);
                    ds.EncounterData.OldSections[currentSection].GetTabInfo(tabName).customName = child.text;
                }
            }
            //The button's position
            Instantiate(newTab, TabButtonContentPar.transform);
        }
    }

    /**
	 * Called when starting. Loads default data.
	 */
    public void FirstTimeLoad()
    {
        Start();
        string loadDefault = "";
        currentSection = "";
        SwitchSection(loadDefault);
    }

    /**
	 * Returns the current tab name (will maybe add the number to the end of this)
	 * Returns TabName now
	 */
    public string getCurrentTab()
    {
        //return currentTab.name;
        return tabName;
    }

    /**
	 * Sets the currentTab gameObject name
	 * Pass in the formatted tabName
	 */
    public void setCurrentTabName(string tabName)
    {
        currentTab.name = tabName;
    }

    // Gets the tab button name 
    public string getTabName()
    {
        return tabName;
    }

    /**
	 * Returns the current section key
	 */
    public string GetCurrentSectionKey()
    {
        return currentSection;
    }
    
    /**
     * Returns the current section name
     */
    public SectionDataScript GetCurrentSection()
    {
        return EncounterHandler.Instance.EncounterData.OldSections[currentSection];
    }

    // Sets the tab button's name on tab switch
    public void setTabName(string newTabName)
    {
        tabName = newTabName;
    }

    /**
	 * Sets the current section
	 * Pass in the LinkToName
	 */
    public void setCurrentSection(string sectionLinkToName)
    {
        currentSection = sectionLinkToName;
    }

    /**
	 * Destroys the current tab object
	 */
    public void DestroyCurrentTab()
    {
        Destroy(currentTab);
        currentTab = null;
        MethodToCall = null;
    }

    /**
	 * Destroys the specified tab object
	 * Pass in the LinkToText
	 */
    public void RemoveTab(string tabName)
    {
        if (currentTab.name.Equals(tabName)) {
            MethodToCall = null;
            currentTab = null;
        }
        Destroy(TabButtonContentPar.transform.Find(tabName.Replace(" ", "_") + "Button").gameObject);

        string tabData = ds.GetTabData(currentSection, tabName);
        foreach (string key in ds.EncounterData.Images.Keys) {
            if (tabData.Contains("<Image>" + key + "</Image>")) {
                Debug.Log("Removing Image: " + key);
                ds.EncounterData.Images.Remove(key);
            } else if (key.StartsWith(GetCurrentSectionKey() + "." + tabName + "Tab")) {
                ds.EncounterData.Images.Remove(key);
            }
        }

        var keyList = ds.GetDialogues().Keys.ToList();
        foreach (string key in keyList) {
            if (key.StartsWith(GetCurrentSectionKey() + "/" + tabName + "Tab")) {
                ds.GetDialogues().Remove(key);
            }
        }

        keyList = ds.GetQuizes().Keys.ToList();
        foreach (string key in keyList) {
            if (key.StartsWith(GetCurrentSectionKey() + "/" + tabName + "Tab")) {
                ds.GetQuizes().Remove(key);
            }
        }
    }

    /**
	 * Sets the currentSection and currentTab variables to null
	 */
    public void RemoveCurrentSection()
    {
        currentSection = null;
        currentTab = null;
        MethodToCall = null;
    }
}