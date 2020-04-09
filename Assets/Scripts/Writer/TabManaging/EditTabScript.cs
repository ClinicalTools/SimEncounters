using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
using System;
using TMPro;

public class EditTabScript : MonoBehaviour
{

    GameObject tabEditPrefab;
    TextMeshProUGUI tabName;
    GameObject BG, parentBG;
    TabManager tm;
    private TextMeshProUGUI inputText;
    private string TitleValuePath;

    // Use this for initialization
    void Start()
    {
        parentBG = GameObject.Find("GaudyBG");
        BG = transform.gameObject;
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

    }

    /**
	 * Called when removing a tab from the edit tab panel
	 */
    public void removeTab()
    {
    }

    /**
	 * Called when removing a certain tab
	 * Pass in the LinkToText
	 */
    public void removeTab(string tabName)
    {
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
    }
}
