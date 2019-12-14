using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System;
using TMPro;
using ClinicalTools.SimEncountersOld;

public class EditTabScript : MonoBehaviour
{

    GameObject tabEditPrefab;
    TextMeshProUGUI tabName;
    GameObject BG, parentBG;
    WriterHandler ds;
    TabManager tm;
    private TextMeshProUGUI inputText;
    private string TitleValuePath;

    // Use this for initialization
    void Start()
    {
        parentBG = GameObject.Find("GaudyBG");
        BG = transform.gameObject;
        ds = WriterHandler.WriterInstance;
        tm = transform.GetComponent<TabManager>();
        TitleValuePath = "TabEditorPanel/Content/Row0/TMPInputField/TitleValue";
    }

    /**
	 * Open's the edit tab panel. Pass in display text value
	 */
    public void OpenTabPanel(TextMeshProUGUI t)
    {
        //string name = Regex.Split (t.text, "[0-9]*$") [0];
        if (BG == null) {
            Start();
        }
        tabName = t;

        tabEditPrefab = Instantiate(Resources.Load("Writer/Prefabs/Panels/TabEditorBG")) as GameObject;
        tabEditPrefab.transform.SetParent(parentBG.transform, false);
        //editTabPanel = transform.Find ("TabEditorBG").gameObject;
        tabEditPrefab.transform.SetAsLastSibling();
        tabEditPrefab.transform.SetSiblingIndex(tabEditPrefab.transform.GetSiblingIndex() - 1);
        tabEditPrefab.transform.Find(TitleValuePath).GetComponent<TMP_InputField>().text = "";

        //editTabPanel.gameObject.SetActive (true);

        var conditions = TabInfoScript.CurrentTab.Conditions;
        if (conditions != null) {
            var condGroup = tabEditPrefab.GetComponentInChildren<CondGroup>();

            condGroup.SetConditions(conditions);
        }
    }

    /**
	 * This method will convert special characters into an xml friendly format so they can be used
	 */
    private string ConvertName(string name)
    {
        foreach (char c in name) {
            string character = c + "";
            Debug.Log(character);
            if (!Regex.IsMatch(character, "[a-zA-Z0-9-. ]")) {
                int a = Convert.ToInt32(c);
                string hex = "_" + String.Format("{0:X}", a) + "_";
                Debug.Log(hex);
                name = name.Replace(character, hex);
                Debug.Log(name);
            }
        }
        //Debug.Log (name);
        return name;
    }

    /**
	 * Submits the changes from the edit tab panel
	 */
    public void SubmitChanges()
    {
        string newName = tabEditPrefab.transform.Find(TitleValuePath).GetComponent<TMP_InputField>().text;
        var conditions = tabEditPrefab.GetComponentInChildren<CondGroup>().ConditionSerials();
        TabInfoScript.CurrentTab.Conditions = conditions;

        if (!ds.IsValidName(newName, "Tab")) {
            //ds.ShowMessage ("Tab name not valid. Cannot use:\n*, &, <, >, or //", true);
            throw new Exception("Name not valid: Please choose a new name for your tab.");
        } else if (!newName.Equals(tabName.text)) {
            // TODO: make sure this didn't mess anything up
            //ds.EditTab(tabName.text, newName);
            string formattedNewName = newName;//.Replace(" ", "_") + "Tab";
            tabName.transform.parent.Find("TabButtonDisplayText").GetComponent<TextMeshProUGUI>().text = newName;
            //tObject.transform.parent.Find ("TabButtonLinkToText").GetComponent<Text> ().text = formattedNewName;
            tm.setCurrentTabName(newName + "Tab");//formattedNewName);
            tabName.transform.parent.name = formattedNewName/*.Replace(" ", "_")*/ + "TabButton";
        }
        tabEditPrefab.transform.Find(TitleValuePath).GetComponent<TMP_InputField>().text = "";
        tabEditPrefab.gameObject.SetActive(false);
        tabName.transform.parent.GetComponent<ScriptButtonFixScript>().FixTab();

        Destroy(tabEditPrefab);
    }

    /**
	 * Called when removing a tab from the edit tab panel
	 */
    public void removeTab()
    {
        //Debug.Log (tObject.text);
        tm.AddToDictionary();
        string tabData = ds.EncounterData.OldSections[tm.GetCurrentSectionKey()].GetData(tabName.text);
        ds.EncounterData.OldSections[tm.GetCurrentSectionKey()].Remove(tabName.text);
        tm.DestroyCurrentTab();
        tm.TabButtonContentPar.transform.Find(tabName.text + "TabButton");
        if (!tabName.text.Contains("/")) {
            Destroy(tm.TabButtonContentPar.transform.Find(tabName.text + "TabButton").gameObject);
        } else {
            for (int i = 0; i < tm.TabButtonContentPar.transform.childCount; i++) {
                if (tm.TabButtonContentPar.transform.GetChild(i).name.Equals(tabName.text + "TabButton")) {
                    Destroy(tm.TabButtonContentPar.transform.GetChild(i).gameObject);
                    break; //break out of the loop
                }
            }
        }
        if (ds.EncounterData.OldSections[tm.GetCurrentSectionKey()].GetTabList().Count != 0) {
            TabInfoScript newTabInfo = ds.EncounterData.OldSections[tm.GetCurrentSectionKey()]
                .GetTabInfo(ds.EncounterData.OldSections[tm.GetCurrentSectionKey()].GetTabList()[0]);
            tm.setTabName(newTabInfo.customName);
            tm.SwitchTab(ds.EncounterData.OldSections[tm.GetCurrentSectionKey()].GetTabList()[0]);
        } else {
            //BG.transform.Find ("TabSelectorBG").gameObject.SetActive (true);
            GameObject tabSelectorPrefab = Instantiate(Resources.Load("Writer/Prefabs/Panels/TabSelectorBG")) as GameObject;
            tabSelectorPrefab.transform.SetParent(BG.transform, false);

            if (tabSelectorPrefab.transform.Find("TabSelectorPanel/RowTitle/CancelButton")) {
                tabSelectorPrefab.transform.Find("TabSelectorPanel/RowTitle/CancelButton").gameObject.SetActive(false);
            }
        }
        //Debug.Log (ds.GetData (tm.getCurrentSection()).getTabList ()[0]);
        tabEditPrefab.transform.Find(TitleValuePath).GetComponent<TMP_InputField>().text = "";
        //tabEditPrefab.gameObject.SetActive (false);
        if (tabName.text.StartsWith("Background_InfoTab")) {
            ds.transform.Find("TabSelectorBG/TabSelectorPanel/Content/ScrollView/Viewport/Content/BackgroundInfoTabPanel").gameObject.SetActive(true);
        }

        Destroy(tabEditPrefab);

        List<string> keyList;
        foreach (string key in ds.EncounterData.Images.Keys) {
            if (tabData.Contains("<Image>" + key + "</Image>")) {
                Debug.Log("Removing Image: " + key);
                ds.EncounterData.Images.Remove(key);
                //ds.GetImages ().Remove (key);
            }
        }

        keyList = ds.GetDialogues().Keys.ToList();
        foreach (string key in keyList) {
            if (key.StartsWith(tm.GetCurrentSectionKey() + "/" + tabName.text + "Tab")) {
                ds.GetDialogues().Remove(key);
            }
        }

        keyList = ds.GetQuizes().Keys.ToList();
        foreach (string key in keyList) {
            if (key.StartsWith(tm.GetCurrentSectionKey() + "/" + tabName.text + "Tab")) {
                ds.GetQuizes().Remove(key);
            }
        }
    }

    /**
	 * Called when removing a certain tab
	 * Pass in the LinkToText
	 */
    public void removeTab(string tabName)
    {
        bool isCurrentTab = false;
        if (ds.EncounterData.OldSections[tm.GetCurrentSectionKey()].GetTabInfo(tabName).customName.Equals(tm.getCurrentTab())) {
            isCurrentTab = true;
        }
        ds.EncounterData.OldSections[tm.GetCurrentSectionKey()].Remove(tabName);
        tm.RemoveTab(tabName);
        Destroy(tm.TabButtonContentPar.transform.Find(tabName.Replace(" ", "_") + "Button").gameObject);
        if (isCurrentTab) {
            if (ds.EncounterData.OldSections[tm.GetCurrentSectionKey()].GetTabList().Count != 0) {
                tm.SwitchTab(ds.EncounterData.OldSections[tm.GetCurrentSectionKey()].GetTabList()[0]);
                UpdateTabPos();
            } else {
                BG.transform.Find("TabSelectorBG").gameObject.SetActive(true);
            }
        }

        //Debug.Log (ds.GetData (tm.getCurrentSection()).getTabList ()[0]);

        Destroy(tabEditPrefab);
    }

    private void UpdateTabPos()
    {
        //Update all other button's position
        List<Transform> buttons = new List<Transform>();
        for (int i = 0; i < tm.TabButtonContentPar.transform.childCount; i++) {
            if (!tm.TabButtonContentPar.transform.GetChild(i).name.Equals("placeholder") && !tm.TabButtonContentPar.transform.GetChild(i).name.Equals("Filler")) {
                buttons.Add(tm.TabButtonContentPar.transform.GetChild(i));
            }
        }

        foreach (Transform t in buttons) {
            ds.EncounterData.OldSections[tm.GetCurrentSectionKey()].GetTabInfo(t.Find("TabButtonLinkToText").GetComponent<TextMeshProUGUI>().text).SetPosition(t.GetSiblingIndex());
        }
    }
}
