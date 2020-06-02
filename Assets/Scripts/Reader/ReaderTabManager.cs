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
#if UNITY_EDITOR
using UnityEditor;
#endif


/**
 * This class manages Sections and Tabs
 * It also has some functions for storing/loading data
 */
public class ReaderTabManager : MonoBehaviour
{

    private Transform[] allChildren;    //Used to hold all transform children when loading and saving.
    private ReaderDataScript ds;            //Reference to DataScript
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


    void Start()
    {
        sameTab = false;
        MethodToCall = null;
        ID = new DefaultDataScript();
        BG = GameObject.Find("GaudyBG");
        ds = BG.GetComponentInChildren<ReaderDataScript>();
    }

    /**
	 * Updates all the section images to those in the imgDict
	 */
    public void ChangeSectionImages()
    {
        foreach (Image t in SectionContentPar.GetComponentsInChildren<Image>()) {
            if (t.transform.name.Equals("Image")) {
                t.sprite = null;
                t.sprite = ds.GetImage(t.transform.parent.Find("SectionLinkToText").GetComponent<Text>().text).sprite;
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
                if (t.GetComponent<Image>().sprite.name != null && !t.GetComponent<Image>().sprite.name.Equals("")) {
                    string s = t.GetComponent<Image>().sprite.name;
                    SpriteHolderScript shs = new SpriteHolderScript(s);
                    ds.AddImg(t.parent.Find("SectionLinkToText").GetComponent<Text>().text, t.GetComponent<Image>().sprite);
                    foreach (Image img in shs.iconHolder.GetComponentsInChildren<Image>()) {
                        if (img.sprite.name.Equals(s)) {
                            ds.AddImg(t.parent.Find("SectionLinkToText").GetComponent<Text>().text, img.transform.parent.name);
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
        return; //Dont save data in the reader
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


    private bool AllTabsVisited()
    {
        return ds.GetData(currentSection).AllTabsVisited();
    }

    /**
	 * Updates the tab buttons. Currently not in use
	 */
    public void updateTabButtons()
    {
        List<string> sectionTabs = ds.GetData(currentSection).GetTabList();
        //Debug.Log (sectionTabs.Count + ", " + string.Join (", ", sectionTabs.ToArray ()));
        foreach (string tabName in sectionTabs) {
            GameObject newTab = Resources.Load(GlobalData.resourcePath + "/Prefabs/TabButton") as GameObject;
            Text[] children = newTab.GetComponentsInChildren<Text>();
            foreach (Text child in children) {
                if (child.name.Equals("TabButtonLinkToText")) { //Where the button links to
                    child.text = tabName;
                } else if (child.name.Equals("TabButtonDisplayText")) { //What the button displays
                    child.text = tabName.Replace("_", " ").Substring(0, Regex.Split(tabName, "[0-9]*$")[0].Length - "Tab".Length);
                    ds.GetData(currentSection).GetTabInfo(tabName).customName = child.text;
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
	 * Returns the current section name
	 */
    public string getCurrentSection()
    {
        return currentSection;
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

        List<string> keyList = ds.GetImageKeys();
        string tabData = ds.GetData(currentSection, tabName);
        foreach (string key in keyList) {
            if (tabData.Contains("<Image>" + key + "</Image>")) {
                Debug.Log("Removing Image: " + key);
                ds.RemoveImage(key);
            } else if (key.StartsWith(getCurrentSection() + "." + tabName + "Tab")) {
                ds.RemoveImage(key);
            }
        }

        keyList = ds.GetDialogues().Keys.ToList();
        foreach (string key in keyList) {
            if (key.StartsWith(getCurrentSection() + "/" + tabName + "Tab")) {
                ds.GetDialogues().Remove(key);
            }
        }

        keyList = ds.GetQuizes().Keys.ToList();
        foreach (string key in keyList) {
            if (key.StartsWith(getCurrentSection() + "/" + tabName + "Tab")) {
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

    /// <summary>
    /// Switches the tab
    /// </summary>
    /// <param name="tabName">Formatted tab name</param>
    public void SwitchTab(string tabName)
    {
        //Transform[] tabButtonsList = TabButtonContentPar.GetComponentsInChildren<Transform>();
        List<Transform> tabButtonsList = new List<Transform>();
        for (int i = 0; i < TabButtonContentPar.transform.childCount; i++) {
            var trans = TabButtonContentPar.transform.GetChild(i);
            if (trans.gameObject.activeSelf)
                tabButtonsList.Add(TabButtonContentPar.transform.GetChild(i));
        }

        /* For debugging
		string str = "";
		foreach (Transform t in tabButtonsList) {
			str += t.name + ", ";
		}
		Debug.Log (str);*/

        string tName = null;
        string section = currentSection;
        string tabType;

        float tempScrollPos = 0;

        //Check if the tab is specified. If not, let the user choose their tab
        TabInfoScript tabScript = null;
        //int n = -1;
        if (tabName == null || tabName.Equals("")) { //If no provided tab name
            tabScript = ds.GetData(section).GetCurrentTab(); //Check to see if there were any active tabs for the current section
            if (tabScript == null || tabScript.type == "") { //If no active tab
                                                             //n = 0;
                SectionDataScript sds = ds.GetData(section);
                if (sds.GetTabList().Count > 0) { //If the section has any tabs at all
                    string tempTabName = sds.GetTabList()[0];
                    tempTabName = tempTabName.Replace('_', ' ').Substring(0, tempTabName.Length - "Tab".Length);
                    sds.SetCurrentTab(tempTabName);
                    tabType = tempTabName;
                    sds.GetTabInfo(tabType).Visit();
                    if (sds.AllTabsVisited()) {
                        //Track that this section has had all tabs visited
                        Tracker.RecordData(
                            Tracker.DataType.progress,
                            GlobalData.caseObj.recordNumber,
                            new Tracker.ProgressData(sds.GetPosition(), -1));// ds.GetSectionsList().FindIndex((string sec) => sec.Equals(section))));

                    } else {
                        //Track the tab visit
                        Tracker.RecordData(
                            Tracker.DataType.progress,
                            GlobalData.caseObj.recordNumber,
                            new Tracker.ProgressData(sds.GetPosition(), sds.GetTabInfo(tabType).n));
                    }
                    Tracker.PrintAllData();
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
            //Switching away from a tab
            if (currentTab != null) {
                if ((tabName + "Tab").Equals(currentTab.name) && section.Equals(currentSection)) {
                    return;
                } else {

                    //This line throws errors with a "/" in the name
                    //TabButtonContentPar.transform.Find(currentTab.name + "Button").GetComponent<Button>().interactable = true;

                    tName = currentTab.name/*.Replace (" ", "_")*/ + "Button";
                    foreach (Transform t in tabButtonsList) {
                        if (t.name.Equals(tName)) {
                            t.GetComponent<Button>().interactable = true;
                            //Enable visited mark
                            if (ds.forceInOrder) {
                                //Enabling something (like the visited notice) resets the scrollbar back to 0 (to the left)
                                //NextFrame.Function(delegate { TabButtonContentPar.transform.parent.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = tempScrollPos; });
                                tempScrollPos = TabButtonContentPar.transform.parent.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition;
                                t.Find("Visited").gameObject.SetActive(true);
                            }
                            break;
                        }
                    }

                    Destroy(currentTab);
                    currentTab = null;
                }
            }
            //Debug.Log(string.Join(",", ds.GetData (currentSection).GetTabList().ToArray()));
            tabType = ds.GetData(currentSection).GetTabInfo(tabName).type;
            ds.GetData(currentSection).GetTabInfo(tabName).Visit();
            if (ds.GetData(currentSection).AllTabsVisited()) {
                //Track that this section has had all tabs visited
                int sectionIdx = ds.GetSectionsList().FindIndex((string sec) => sec.Equals(section));

                //If the last section has been completed, mark the case as visited
                if (sectionIdx == ds.GetSectionsList().Count - 1) {
                    Tracker.RecordData(
                        Tracker.DataType.finishedCase,
                        GlobalData.caseObj.recordNumber,
                        true);
                } else {
                    //Record the completed section
                    Tracker.RecordData(
                        Tracker.DataType.progress,
                        GlobalData.caseObj.recordNumber,
                        new Tracker.ProgressData(ds.GetData(currentSection).GetPosition(), ds.GetData(currentSection).GetTabInfo(tabName).n));
                }

				//Remove the lock on the next step
				if (ds.forceInOrder) {
					int nextPos = ds.GetData(currentSection).GetPosition() + 1;
					if (nextPos < ds.GetSectionsList().Count) {
						Transform button = ds.SectionButtonPar.transform.GetChild(nextPos);
						button.GetComponent<Button>().interactable = true;
						if (button.Find("Overlay")) {
							button.Find("Overlay").GetComponent<Image>().color = new Color(0, 0, 0, (float)50 / 255);
							button.Find("Overlay").GetComponent<HideOnMouseHoverScript>().enabled = true;
						}
						button.Find("Lock").gameObject.SetActive(false);
						ds.SectionButtonPar.transform.GetChild(ds.GetData(currentSection).GetPosition() + 1).GetComponent<Button>().interactable = true;
					}
				}

				//Apply the checkmark when the last tab is visited
				Transform currentButton = ds.SectionButtonPar.transform.GetChild(ds.GetData(currentSection).GetPosition());
				currentButton?.Find("AllTabsVisitedCheck")?.gameObject.SetActive(true);
			} else {
                //Track the tab visit
                Tracker.RecordData(
                    Tracker.DataType.progress,
                    GlobalData.caseObj.recordNumber,
                    new Tracker.ProgressData(ds.GetData(currentSection).GetPosition(), ds.GetData(currentSection).GetTabInfo(tabName).n));
            }
            Tracker.PrintAllData();


            
            if (tabType.ToLower().EndsWith("tab")) { //Remove the "Tab" from the end of the type
                tabType = tabType.Substring(0, tabType.Length - 3);
            }
        }
        //Debug.Log ("Section: " + currentSection + ", Name: " + tabName + ", Type: " + tabType); //Type is _ + Tab, but should be prefab folder - " Tab"



        //Load in the specified tab
        //string name = tabName;
        //string folder = tabType + " Tab";
        //tabType = tabType.Replace (" ", string.Empty) + "Tab";
        //Folder should be the same as the prefab folder. Tab should equal the prefab name. Name should be the link to text

        //Debug.Log ("Prefabs/Tabs/" + folder + "/" + tabType);
        //GameObject newPrefab = Resources.Load(GlobalData.resourcePath + "/Prefabs/Tabs/" + folder + "/" + tabType) as GameObject;
        //GameObject newTab = Instantiate (newPrefab, TabContentPar.transform);

        //GameObject newPrefab = Resources.Load(GlobalData.resourcePath + "/Prefabs/Tabs/" + tabType + " Tab/" + tabType.Replace(" ", string.Empty) + "Tab") as GameObject;
        GameObject newTab = Instantiate(Resources.Load(GlobalData.resourcePath + "/Prefabs/Tabs/" + tabType + " Tab/" + tabType.Replace(" ", string.Empty) + "Tab") as GameObject, TabContentPar.transform);
        newTab.name = tabName + "Tab";

        if (GlobalData.GDS.isMobile) {
            newTab.transform.GetComponentInChildren<Scrollbar>().targetGraphic.enabled = true;
            newTab.transform.GetComponentInChildren<Scrollbar>().GetComponent<Image>().enabled = true;
        }

        //newTab.AddComponent<Text> ().text = "" + n;
        currentTab = newTab;
        ds.GetData(section).SetCurrentTab(tabName);

        //Adjust the buttons
        int tNameHash = (currentTab.name/*.Replace (" ", "_")*/ + "Button").GetHashCode();
		int j = 0;
        foreach (Transform t in tabButtonsList) {
            //if (t.name.Equals (tName)) {
            if (t.name.GetHashCode() == tNameHash) {
                //Writer code to disable trash button for persistant tabs was here. Changing to visited code now
                //Enable the visited check
                t.GetChild(t.childCount - 1).gameObject.SetActive(true);
                t.GetComponent<Button>().interactable = false;
                t.GetComponent<ScriptButtonFixScript>().FixTab();

                //Find the value that we need to auto-scroll to
                float viewportWidth = TabButtonContentPar.transform.parent.GetComponent<RectTransform>().rect.width;
                float tabWidth = t.GetComponent<RectTransform>().rect.width;
                float contentParOffset = TabButtonContentPar.GetComponent<RectTransform>().anchoredPosition.x;
                float tabButtonOffset = t.transform.localPosition.x;
                float leftSideTabButtonLocal = tabButtonOffset - tabWidth / 2 + contentParOffset;
                float rightSideTabButtonLocal = tabButtonOffset + tabWidth / 2 + contentParOffset;
                if (leftSideTabButtonLocal < 0) {
                    //Past left side
                    print("Past left");
                    float tabHorizontalPos = Mathf.Clamp01((tabButtonOffset - tabWidth / 2) / (TabButtonContentPar.GetComponent<RectTransform>().rect.width - viewportWidth));
                    TabButtonContentPar.transform.parent.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = tabHorizontalPos;
                } else if (rightSideTabButtonLocal > viewportWidth) {
                    //past right side
                    print("Past right");
                    float tabHorizontalPos = Mathf.Clamp01((tabButtonOffset + tabWidth / 2 - viewportWidth) / (TabButtonContentPar.GetComponent<RectTransform>().rect.width - viewportWidth));
                    TabButtonContentPar.transform.parent.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = tabHorizontalPos;
                }
                /*
				//If the tab's left side is to the left of the tab viewport
				if (t.position.x - tabWidth / 2 < viewportX - viewportWidth / 2) {
					float tabHorizontalPos = (t.localPosition.x - t.GetComponent<RectTransform>().rect.width / 2 - viewportWidth) / (TabButtonContentPar.GetComponent<RectTransform>().rect.width - viewportWidth);
					TabButtonContentPar.transform.parent.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = tabHorizontalPos;
				//Else if the right side of the tab is past the right side of the tab viewport
				} else if (t.position.x + tabWidth / 2 > viewportX + viewportWidth / 2) {
					float tabHorizontalPos = (t.localPosition.x + t.GetComponent<RectTransform>().rect.width / 2 - viewportWidth) / (TabButtonContentPar.GetComponent<RectTransform>().rect.width - viewportWidth);
					TabButtonContentPar.transform.parent.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = tabHorizontalPos;
				}
				*/
                break;
            }
			j++;
        }
		if (ds.forceInOrder && tabButtonsList.Count > j+1) {
			//tabButtonsList[j + 1].Find("Locked").gameObject.SetActive(false);
			//tabButtonsList[j + 1].GetComponent<Button>().enabled = true;
		}
        print(tempScrollPos);
        if (tempScrollPos > 0) {
            TabButtonContentPar.transform.parent.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = tempScrollPos;
            NextFrame.Function(delegate {
                //Can't do next frame, because that messes up the code that alligns to a tab off the screen
                //TabButtonContentPar.transform.parent.parent.GetComponent<ScrollRect>().horizontalNormalizedPosition = tempScrollPos;
            });
        }
        /*
		foreach(TabScrollButtonScript scrollButton in TabButtonContentPar.transform.parent.parent.parent.GetComponentsInChildren<TabScrollButtonScript>()) {
			if (scrollButton.name.EndsWith("Right")) {
				//scrollButton.gameObject.SetActive(true);
			}
		}*/

        //Debug.Log(ds.GetData(getCurrentSection()).GetAllData());
        //Resources.UnloadUnusedAssets();
    }

    /**
	 * Switches the active section.
	 * Pass in the LinkToText
	 */
    public void SwitchSection(string sectionName, bool backwards = false)
    {
    }
}