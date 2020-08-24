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
    }

    /**
	 * Updates all the section images to those in the imgDict
	 */
    public void ChangeSectionImages()
    {
        foreach (Image t in SectionContentPar.GetComponentsInChildren<Image>()) {
            if (t.transform.name.Equals("Image")) {
                t.sprite = null;
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

        addTabButton.transform.SetAsLastSibling();

    }

    /**
	 * Updates the tab buttons. Currently not in use
	 */
    public void updateTabButtons()
    {
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
        return null;
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