using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Linq;
using TMPro;
using SimEncounters;

public class AddTabScript : MonoBehaviour {

    public WriterHandler ds;
    public TabManager tm;
    public GameObject TabButtonContentPar;
    public GameObject SectionButtonContentPar;
    public TextMeshProUGUI tabToSpawn;
    public GameObject Content;
    private Transform BGInfoOption, OverviewInfoOption;
    private GameObject BG;

    private string tabsFileName = "Tabs.tsv";
    private char splitChar = '\t';

    private int CATEGORY = 0;
    private int DISPLAY = 1;
    private int PREFAB = 2;
    private int DESCRIPTION = 3;

    // Use this for initialization
    void Start()
    {
        BG = GameObject.Find("GaudyBG");
        if (BG != null) {
            ds = WriterHandler.WriterInstance;
            tm = BG.GetComponent<TabManager>();
            TabButtonContentPar = BG.transform.Find("ContentPanel/TabButtonsPanel/Scroll View/Viewport/TabButtonContent").gameObject;
            SectionButtonContentPar = BG.transform.Find("ContentPanel/SectionButtonsPanel/Scroll View/Viewport/SectionButtonContent").gameObject;
        }
        // ew
        if (transform.name.StartsWith("Section")) {
            UpdateColorChoice();
            transform.Find("SectionCreatorPanel/Content/ScrollView").GetComponent<ScrollRect>().verticalScrollbar.value = 1;
        }
        //BGInfoOption = ds.transform.Find ("TabSelectorBG/TabSelectorPanel/Content/ScrollView/Viewport/Content/BackgroundInfoTabPanel");

        //This is what I get from not seperating this into 2 scripts a long time ago
        if (transform.name.Equals("SectionCreatorBG")) {
            return;
        }

        System.IO.StreamReader reader = new System.IO.StreamReader(Application.streamingAssetsPath + "/Instructions/" + tabsFileName);
        Transform listParent = transform.Find("TabSelectorPanel/Content/Row2/ScrollView1/Viewport/Content");
        string[] split;
        split = reader.ReadLine().Split(splitChar); //Skip the first line
        Object prefab = Resources.Load("Writer/Prefabs/Panels/TabTemplatePanel");
        while (!reader.EndOfStream) {
            split = reader.ReadLine().Split(splitChar);
            if (split.Length < 3) {
                continue;
            }
            split[CATEGORY] = split[CATEGORY].TrimStart('"').TrimEnd('"');
            if (split[CATEGORY].Equals("END OF FILE")) {
                break;
            }
            if (split[CATEGORY].Equals("N/A")) {
                GameObject baseObj = Instantiate(prefab, listParent) as GameObject;
                baseObj.name = (split[DISPLAY] + "TabPanel");//.Replace(" ", "");
                baseObj.transform.Find("Selected/TabTemplateTitle").GetComponent<TextMeshProUGUI>().text = split[DISPLAY];
                baseObj.GetComponent<Toggle>().group = baseObj.transform.parent.GetComponent<ToggleGroup>();
                baseObj.GetComponent<Toggle>().onValueChanged.AddListener((on) => {
                    if (on) {
                        PopulateDescription(baseObj.transform);
                    }
                });
            } else if (listParent.Find(split[CATEGORY] + "Category") == null) {
                GameObject category = Instantiate(prefab, listParent) as GameObject;
                category.name = split[CATEGORY] + "Category";
                //category.transform.Find ("Selected/TabTemplateTitle").GetComponent<TextMeshProUGUI> ().text = split [CATEGORY] + " ▶";
                category.transform.Find("Selected/TabTemplateTitle").GetComponent<TextMeshProUGUI>().text = split[CATEGORY];
                category.transform.Find("Selected/RightArrow").gameObject.SetActive(true);
                category.GetComponent<Toggle>().group = category.transform.parent.GetComponent<ToggleGroup>();
                category.GetComponent<Toggle>().onValueChanged.AddListener((on) => {
                    if (on) {
                        ExpandSelected(category.transform);
                    }
                });
            }
        }
        reader.Close();
    }

    void OnEnable()
    {
        tabToSpawn.transform.GetComponentInParent<TMP_InputField>().text = "";
    }

    /**
	 * Returns the text of the selected Tab
	 */
    private TextMeshProUGUI FindSelected()
    {
        if (transform.Find("TabSelectorPanel/Content/Row2/ScrollView").gameObject.activeInHierarchy) {
            print("Original List");
            Transform originalList = transform.Find("TabSelectorPanel/Content/Row2/ScrollView");
            foreach (Transform t in originalList.GetComponentsInChildren<Transform>()) {
                if (t.parent == originalList.transform) {
                    if (t.GetComponent<Toggle>().isOn) {
                        return t.Find("Selected/TabTemplateTitle").GetComponent<TextMeshProUGUI>();
                    }
                }
            }
        }

        foreach (Transform t in Content.GetComponentsInChildren<Transform>()) {
            if (t.parent == Content.transform) {
                if (t.GetComponent<Toggle>().isOn) {
                    print(t.Find("Selected/TabTemplateTitle").GetComponent<TextMeshProUGUI>().text);
                    return t.Find("Selected/TabTemplateTitle").GetComponent<TextMeshProUGUI>();
                }
            }
        }
        Transform listParent = transform.Find("TabSelectorPanel/Content/Row2/ScrollView1/Viewport/Content");
        foreach (Transform t in listParent.GetComponentsInChildren<Transform>()) {
            if (t.parent == listParent.transform) {
                if (t.GetComponent<Toggle>().isOn) {
                    return t.Find("Selected/TabTemplateTitle").GetComponent<TextMeshProUGUI>();
                }
            }
        }
        return null;
    }

    /**
	 * Adds a tab from the TabSelectorBG
	 * Pass in the display name
	 */
    public void addTab(TextMeshProUGUI tabName)
    {
        if (!ds.IsValidName(tabName.text, "Tab")) {
            //ds.ShowMessage("Tab name not valid. Cannot use:\n*, &, <, >, or //", true);
            ds.ShowMessage("Name not valid: Please rename your tab", true);
            throw new System.Exception("Name not valid: Please rename your tab");
        }
        var tabTemplate = FindSelected();
        if (tabTemplate == null) {
            ds.ShowMessage("A tab template must be selected.", true);
            throw new System.Exception("A tab template must be selected");
        }

        string tabCustomName;
        List<string> tempTabList = ds.EncounterData.OldSections[tm.GetCurrentSectionKey()].GetTabList();
        //Do not remove last compare below. It's looking for a zero width space (unicode 8203 / Alt+200B)
        if (tabName.text == null || tabName.text.Equals("") || tabName.text.ToCharArray()[0].Equals(GlobalData.EMPTY_WIDTH_SPACE)) {
            tabCustomName = tabTemplate.text;//.Replace(" ", "_") + "Tab";
        } else {
            if (tabToSpawn != null) {
                tabName.text = tabToSpawn.text;
            }
            tabCustomName = tabName.text;//.Replace(" ", "_") + "Tab";
        }
        tabCustomName = tabCustomName.Replace(GlobalData.EMPTY_WIDTH_SPACE + "", "");
        //Debug.Log(tabCustomName + "--------------");
        //tabName = "Test History";

        if (tempTabList.Contains(tabCustomName)) {
            ds.ShowMessage("Cannot name two tabs the same in one step!", true);
            throw new System.Exception("Cannot name two tabs the same in one step!");
        }

        string selected = tabTemplate.text;
        string prefabName = selected;
        print("Selected: " + selected);
        System.IO.StreamReader reader = new System.IO.StreamReader(Application.streamingAssetsPath + "/Instructions/" + tabsFileName);
        string[] split;
        split = reader.ReadLine().Split(splitChar); //Skip the first line
        while (!reader.EndOfStream) {
            split = reader.ReadLine().Split(splitChar);
            if (split.Length < 3) {
                continue;
            }
            //print ("split 1: " + split[DISPLAY]);
            if (split[DISPLAY].Equals(selected)) {
                //print ("split 2: " + split[PREFAB]);
                prefabName = split[PREFAB];
                break;
            }
        }
        reader.Close();

        string xmlTabName = prefabName;//.Replace (" ", "_") + "Tab";
        string xml = "<data></data>";

        //Debug.Log (GlobalData.resourcePath + "Prefabs/Tabs/" + prefabName + " Tab" + "/" + prefabName.Replace (" ", string.Empty) + "Tab");
        GameObject test = Resources.Load(GlobalData.resourcePath + "/Prefabs/Tabs/" + prefabName + " Tab" + "/" + prefabName.Replace(" ", string.Empty) + "Tab") as GameObject;
        if (test == null) {
            Debug.Log("Cannot load tab prefab " + prefabName);
            Debug.Log(GlobalData.resourcePath + "Prefabs/Tabs/" + prefabName + " Tab" + "/" + prefabName.Replace(" ", string.Empty) + "Tab");
            return;
        }
        Debug.Log("-----: " + xmlTabName + ", " + tabCustomName);
        ds.AddSectionTabData(tm.GetCurrentSectionKey(), xmlTabName, tabCustomName, xml);


        GameObject newTab = Resources.Load(GlobalData.resourcePath + "/Prefabs/TabButton") as GameObject;
        TextMeshProUGUI[] children = newTab.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI child in children) {
            if (child.name.Equals("TabButtonLinkToText")) { //Where the button links to
                child.text = tabCustomName;
            } else if (child.name.Equals("TabButtonDisplayText")) { //What the button displays
                child.text = tabCustomName;//.Replace("_", " ").Substring(0, tabCustomName.Length - 3);
                                           //This converts the custom name back to display format since tabName.text can be left blank
            }
        }
        //The button's position
        newTab = Instantiate(newTab, TabButtonContentPar.transform);
        newTab.name = tabCustomName/*.Replace(" ", "_")*/ + "TabButton";
        if (tabName.text.Equals("Background Info") || tabName.text.Equals("Case Overview")) {
            newTab.transform.SetSiblingIndex(0);
            if (!BGInfoOption) {
                BGInfoOption = ds.transform.Find("TabSelectorBG/TabSelectorPanel/Content/ScrollView/Viewport/Content/BackgroundInfoTabPanel");
            }

            if (!OverviewInfoOption) {
                OverviewInfoOption = ds.transform.Find("TabSelectorBG/TabSelectorPanel/Content/ScrollView/Viewport/Content/CaseOverviewTab");
            }
            BGInfoOption.gameObject.SetActive(false);
            UpdateTabPos();
            //tm.SwitchTab (xmlTabName + tabCount);
        }
        tm.setTabName(tabCustomName);
        //Debug.Log (xmlTabName);
        //tm.SwitchTab (xmlTabName);
        tm.SwitchTab(tabCustomName);
        Destroy(BG.transform.Find("TabSelectorBG(Clone)").gameObject);
    }

    /**
	 * Adds a new section from the SectionCreatorBG
	 * Pass in the display name
	 */
    public void addSection(TextMeshProUGUI sectionName)
    {
        if (sectionName.text.Equals("") || sectionName.text.Length == 0) {
            ds.ShowMessage("Cannot leave step name blank", true);
            return;
        }
        if (!ds.IsValidName(sectionName.text, "Section")) {
            //ds.ShowMessage ("Section name not valid. Cannot use:\n*, &, <, >, or //", true);
            throw new System.Exception("Name not valid: Please rename your step");
        }
        string xmlSectionName = sectionName.text.Replace(" ", "_").Replace(GlobalData.EMPTY_WIDTH_SPACE + "", "") + "Section";

        //Try to add the section. Return if any errors
        try {
            xmlSectionName = ds.EncounterData.OldSections.Add(new SectionDataScript());
        } catch (System.Exception e) {
            ds.ShowMessage("Cannot name two steps the same", true);
            Debug.Log(e.Message);
            return;
        }
        //tm.SetCurrent(xmlSectionName);
        //Create the section button and link it accordingly
        GameObject newSection = Resources.Load(GlobalData.resourcePath + "/Prefabs/SectionButton") as GameObject;
        TextMeshProUGUI[] children = newSection.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI child in children) {
            if (child.name.Equals("SectionLinkToText")) { //Where the button links to
                child.text = xmlSectionName;
            } else if (child.name.Equals("SectionDisplayText")) { //What the button displays
                child.text = sectionName.text;
            }
        }
        newSection.GetComponentInChildren<TextMeshProUGUI>().text = sectionName.text;

        //Spawn in the section button
        newSection = Instantiate(newSection, SectionButtonContentPar.transform);
        newSection.name = xmlSectionName.Replace(" ", "_") + "Button";
        SectionButtonContentPar.transform.Find("AddSectionButton").SetAsLastSibling();
        SectionButtonContentPar.transform.Find("Filler").SetAsLastSibling();

        //Spawns/Removes the Background Info tab as needed
        bool spawn = transform.Find("SectionCreatorPanel/Content/Row1").GetComponentInChildren<Toggle>().isOn;
        string tabName = ds.EncounterData.OldSections[tm.GetCurrentSectionKey()].GetTabList().Find((string obj) => obj.StartsWith("Background_InfoTab"));
        bool tabExists = true;
        if (tabName == null || tabName.Equals("")) {
            tabExists = false;
        }
        if (BGInfoOption != null) {
            if (!spawn && tabExists) {
                ds.GetComponent<EditTabScript>().removeTab(tabName);

                //reactivate the option to select BG Info from the TabSelector
                BGInfoOption.gameObject.SetActive(true);
            } else if (spawn && !tabExists) {
                AddBGInfoTab("Background Info", xmlSectionName);

                //deactivate the option to select BG Info from the TabSelector
                BGInfoOption.gameObject.SetActive(false);
            }
        } else if (OverviewInfoOption != null) {
            if (!spawn && tabExists) {
                ds.GetComponent<EditTabScript>().removeTab(tabName);

                //reactivate the option to select BG Info from the TabSelector
                OverviewInfoOption.gameObject.SetActive(true);
            } else if (spawn && !tabExists) {
                AddBGInfoTab("Case Overview", xmlSectionName);

                //deactivate the option to select BG Info from the TabSelector
                OverviewInfoOption.gameObject.SetActive(false);
            }
        }

        //Set the section button's icon as the user's choice
        Sprite spr = null;
        string imgRefName = "";
        Transform[] sectionIcons = transform.Find("SectionCreatorPanel/Content/ScrollView/Viewport/Content").GetComponentsInChildren<Transform>();
        foreach (Transform t in sectionIcons) {
            Toggle tog;
            if ((tog = t.GetComponent<Toggle>()) != null && tog.isOn) {
                spr = t.Find("Icon").GetComponent<Image>().sprite;
                imgRefName = t.Find("Icon").GetComponent<Image>().name;
                break;
            }
        }
        if (spr != null) {
            newSection.transform.Find("Image").GetComponent<Image>().sprite = spr;
        }

        //Sets the section button's color as the selected color.
        Toggle[] colorButtons = transform.Find("SectionCreatorPanel/Content/Row3/Column0").GetComponentsInChildren<Toggle>();
        foreach (Toggle tog in colorButtons) {
            if (tog.isOn) {
                newSection.transform.GetComponent<Image>().color = tog.GetComponent<Image>().color;
                ds.AddImg(xmlSectionName, imgRefName);
                var img = ds.EncounterData.Images[xmlSectionName];
                img.useColor = true;
                img.color = tog.GetComponent<Image>().color;
            }
        }

        //This script is parented to the add section panel, so let's disable it
        gameObject.SetActive(false);

        bool startEnabled = true; //Whether you want to switch to tabs upon creation or not


        //Either switches to the newly created section or stays on the current section
        if (startEnabled) {
            tm.SwitchSection(xmlSectionName);
        } else {
            Transform[] components = newSection.GetComponentsInChildren<Transform>();
            foreach (Transform c in components) {
                if (!c.name.Equals(newSection.name)) {
                    c.gameObject.SetActive(false);
                }
            }
            newSection.transform.GetChild(0).gameObject.SetActive(true); //The image
            newSection.GetComponent<Button>().interactable = true;
        }
    }

    /**
	 * Called when selecting a premade color. Updates the color sliders to match
	 */
    public void UpdateColorChoice()
    {
        Toggle[] toggles = transform.Find("SectionCreatorPanel/Content/Row3").GetComponentsInChildren<Toggle>();
        Transform colorSlidersParent = transform.Find("SectionCreatorPanel/Content/Row3/Column1");
        foreach (Toggle tog in toggles) {
            if (tog.isOn) {
                Image img = tog.GetComponent<Image>();
                //Set the slider values to match the section's current color
                colorSlidersParent.Find("Row0/RSlider").GetComponent<Slider>().value = img.color.r;
                colorSlidersParent.Find("Row1/GSlider").GetComponent<Slider>().value = img.color.g;
                colorSlidersParent.Find("Row2/BSlider").GetComponent<Slider>().value = img.color.b;
                string newTextString = "";
                newTextString += ((int)(img.color.r * 255)).ToString("X").PadLeft(2, '0');
                newTextString += ((int)(img.color.g * 255)).ToString("X").PadLeft(2, '0');
                newTextString += ((int)(img.color.b * 255)).ToString("X").PadLeft(2, '0');
                //Debug.Log (newTextString);

                Transform customColorParent = transform.Find("SectionCreatorPanel/Content/Row3/Column0/RowCustomColor/TMPInputField/ColorHexValue");
                customColorParent.GetComponent<TMP_InputField>().text = newTextString;
            }
        }
    }

    public void UpdateColorSlidersFromHex(TMP_InputField hex)
    {
        Transform colorSlidersParent = transform.Find("SectionCreatorPanel/Content/Row3/Column1");
        //Set the slider values to match the section's current color
        colorSlidersParent.Find("Row0/RSlider").GetComponent<Slider>().value = System.Convert.ToInt32(hex.text.Substring(0, 2), 16) / 255.0f;
        colorSlidersParent.Find("Row1/GSlider").GetComponent<Slider>().value = System.Convert.ToInt32(hex.text.Substring(2, 2), 16) / 255.0f;
        colorSlidersParent.Find("Row2/BSlider").GetComponent<Slider>().value = System.Convert.ToInt32(hex.text.Substring(4, 2), 16) / 255.0f;
    }

    /**
	 * Adjusts the custom color circle in the SectionCreatorBG
	 */
    public void EditColor(int index)
    {
        float value;
        string newTextString;
        Transform colorSlidersParent = transform.Find("SectionCreatorPanel/Content/Row3/Column1");
        Transform customColorParent = transform.Find("SectionCreatorPanel/Content/Row3/Column0/RowCustomColor/TMPInputField/ColorHexValue");

        if (customColorParent.GetComponent<TMP_InputField>().text.Length != 6) {
            customColorParent.GetComponent<TMP_InputField>().text = "000000";
        }

        switch (index) {
            case 0:
                value = colorSlidersParent.Find("Row0/RSlider").GetComponent<Slider>().value * 255;
                colorSlidersParent.Find("Row0/RValue").GetComponent<TextMeshProUGUI>().text = "" + (int)value;
                newTextString = customColorParent.GetComponent<TMP_InputField>().text;
                newTextString = ((int)value).ToString("X").PadLeft(2, '0') + newTextString.Substring(2);
                //Debug.Log (newTextString);
                customColorParent.GetComponent<TMP_InputField>().text = newTextString;
                break;
            case 1:
                value = colorSlidersParent.Find("Row1/GSlider").GetComponent<Slider>().value * 255;
                colorSlidersParent.Find("Row1/GValue").GetComponent<TextMeshProUGUI>().text = "" + (int)value;
                newTextString = customColorParent.GetComponent<TMP_InputField>().text;
                newTextString = newTextString.Substring(0, 2) + ((int)value).ToString("X").PadLeft(2, '0') + newTextString.Substring(4);
                //Debug.Log (newTextString);
                customColorParent.GetComponent<TMP_InputField>().text = newTextString;
                break;
            case 2:
                value = colorSlidersParent.Find("Row2/BSlider").GetComponent<Slider>().value * 255;
                colorSlidersParent.Find("Row2/BValue").GetComponent<TextMeshProUGUI>().text = "" + (int)value;
                newTextString = customColorParent.GetComponent<TMP_InputField>().text;
                newTextString = newTextString.Substring(0, 4) + ((int)value).ToString("X").PadLeft(2, '0');
                //Debug.Log (newTextString);
                customColorParent.GetComponent<TMP_InputField>().text = newTextString;
                break;
            default:
                return;
        }
        float r = (float)(int.Parse(newTextString.Substring(0, 2), System.Globalization.NumberStyles.AllowHexSpecifier)) / 255;
        float g = (float)(int.Parse(newTextString.Substring(2, 2), System.Globalization.NumberStyles.AllowHexSpecifier)) / 255;
        float b = (float)(int.Parse(newTextString.Substring(4, 2), System.Globalization.NumberStyles.AllowHexSpecifier)) / 255;
        customColorParent.parent.parent.GetChild(0).GetComponent<Image>().color = new Color(r, g, b);
    }

    /**
	 * Pass in the Display Text
	 */
    private void AddBGInfoTab(string tabName, string sectionName)
    {
        string prefabName = tabName + " Tab";
        string tabCustomName = tabName;
        string xmlTabName = tabName;//.Replace (" ", "_") + "Tab";
        string xml = "<data></data>";

        //Debug.Log (GlobalData.resourcePath + "Prefabs/Tabs/" + prefabName + "/" + prefabName.Replace (" ", string.Empty));
        GameObject test = Resources.Load(GlobalData.resourcePath + "/Prefabs/Tabs/" + prefabName + "/" + prefabName.Replace(" ", string.Empty)) as GameObject;
        if (test == null) {
            Debug.Log("Cannot load tab prefab");
            return;
        }
        ds.AddSectionTabData(sectionName, xmlTabName, tabCustomName, xml);


        GameObject newTab = Resources.Load(GlobalData.resourcePath + "/Prefabs/TabButton") as GameObject;
        newTab = Instantiate(newTab, tm.TabButtonContentPar.transform);
        TextMeshProUGUI[] children = newTab.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI child in children) {
            if (child.name.Equals("TabButtonLinkToText")) { //Where the button links to
                child.text = xmlTabName;
            } else if (child.name.Equals("TabButtonDisplayText")) { //What the button displays
                child.text = tabCustomName;
            }
        }
        //The button's position
        newTab.name = tabCustomName/*.Replace (" ", "_")*/ + "TabButton";
        if (tabName.Equals("Background Info")) {
            newTab.transform.SetSiblingIndex(0);
        } else if (tabName.Equals("Case Overview")) {
            newTab.transform.SetSiblingIndex(0);
        }
    }

    private void UpdateTabPos()
    {
        //Update all other button's position
        List<Transform> buttons = new List<Transform>();
        for (int i = 0; i < TabButtonContentPar.transform.childCount; i++) {
            if (!TabButtonContentPar.transform.GetChild(i).name.Equals("placeholder") && !TabButtonContentPar.transform.GetChild(i).name.Equals("Filler")) {
                buttons.Add(TabButtonContentPar.transform.GetChild(i));
            }
        }

        foreach (Transform t in buttons) {
            ds.EncounterData.OldSections[tm.GetCurrentSectionKey()].GetTabInfo(t.Find("TabButtonLinkToText").GetComponent<TextMeshProUGUI>().text).SetPosition(t.GetSiblingIndex());
        }
    }

    public void delectTab()
    {
        Destroy(this.gameObject);
    }

    public string RefactorTabName(TextMeshProUGUI selected)
    {
        return selected.transform.parent.Find("TabPrefabTitle").GetComponent<TextMeshProUGUI>().text;

        /*System.IO.StreamReader reader = new System.IO.StreamReader (Application.streamingAssetsPath + "/Instructions/" + tabsFileName);
		string line;
		while (!reader.EndOfStream) {
			line = reader.ReadLine ();
			if (line.Split (splitChar) [1].TrimStart(' ').Equals (selected.text)) {
				reader.Close ();
				return line.Split (splitChar) [2].TrimStart(' ');
			}
		}
		reader.Close ();
		return selected.text; //Couldn't find a replacement, so just return the selected text and hope for the best
		*/
    }

    public void ExpandSelected(Transform selected)
    {
        var toggleGroup = selected.parent.GetComponent<ToggleGroup>();
        if (toggleGroup)
            toggleGroup.allowSwitchOff = false;

        if (!selected.GetComponent<Toggle>().isOn) {
            return;
        }
        System.IO.StreamReader reader = new System.IO.StreamReader(Application.streamingAssetsPath + "/Instructions/" + tabsFileName);
        Dictionary<string, string> selectedList = new Dictionary<string, string>();
        string[] split;
        while (!reader.EndOfStream) {
            split = reader.ReadLine().Split(splitChar);
            if (split.Length < 3) {
                continue;
            }
            if (selected.name.Equals(split[CATEGORY].TrimStart('"').TrimEnd('"') + "Category")) {
                selectedList.Add(split[DISPLAY], split[DESCRIPTION]);
            }
        }
        reader.Close();



        Transform listParent = transform.Find("TabSelectorPanel/Content/Row2/ScrollView2/Viewport/Content");
        listParent.GetComponent<ToggleGroup>().allowSwitchOff = true;

        Object prefab = Resources.Load("Writer/Prefabs/Panels/TabTemplatePanel");

        for (int i = 0; i < listParent.childCount; i++) {
            Destroy(listParent.GetChild(i).gameObject);
        }
        //Reset tab info text
        transform.Find("TabSelectorPanel/Content/Row2/ScrollView3/Viewport/Content/Text").GetComponent<TMPro.TextMeshProUGUI>().text = "";

        foreach (string name in selectedList.Keys.ToList()) {
            GameObject baseObj = Instantiate(prefab, listParent) as GameObject;
            baseObj.name = (name + "TabPanel");//.Replace(" ", "");
            baseObj.transform.Find("Selected/TabTemplateTitle").GetComponent<TextMeshProUGUI>().text = name;
            baseObj.GetComponent<Toggle>().group = baseObj.transform.parent.GetComponent<ToggleGroup>();
            baseObj.GetComponent<Toggle>().onValueChanged.AddListener((on) => {
                if (on) {
                    PopulateDescription(baseObj.transform);
                }
            });
        }
    }

    public void PopulateDescription(Transform selected)
    {
        var toggle = selected.GetComponent<Toggle>();

        if (!toggle.isOn) {
            return;
        }

        string key = selected.name.Remove(selected.name.Length - "TabPanel".Length); //remove TabPanel from the end of the name
                                                                                     //print(key);
                                                                                     //Text descriptionTextField = transform.Find("TabSelectorPanel/Content/Row2/ScrollView3/Viewport/Content/Text").GetComponent<TextMeshProUGUI>();
        string descriptionText = "";

        System.IO.StreamReader reader = new System.IO.StreamReader(Application.streamingAssetsPath + "/Instructions/" + tabsFileName);
        string[] split;
        while (!reader.EndOfStream) {
            split = reader.ReadLine().Split(new char[] { splitChar }, 4);
            if (key.Equals(split[DISPLAY])) {
                char[] quotes = new char[] { '"' };
                descriptionText = split[DESCRIPTION].TrimStart(quotes).TrimEnd(quotes);
                descriptionText = $"<b>{split[PREFAB]}</b>\n{descriptionText}".Trim();
                break;
            }
        }
        reader.Close();

        transform.Find("TabSelectorPanel/Content/Row2/ScrollView3/Viewport/Content/Text").GetComponent<TMPro.TextMeshProUGUI>().text = descriptionText;
    }

    public void OpenAddSectionPanel()
    {
        if (ds == null) {
            Start();
        }
        gameObject.SetActive(true);

        //Check to see if 8 sections already exist. If so, notify the user and prevent more from being added
        if (ds.EncounterData.OldSections.Count >= 8) {
            transform.Find("SectionCreatorPanel/RowTitle/CreateButton").GetComponent<Button>().interactable = false;
            transform.Find("SectionCreatorPanel/Content/StepCountWarning").gameObject.SetActive(true);
        } else {
            transform.Find("SectionCreatorPanel/RowTitle/CreateButton").GetComponent<Button>().interactable = true;
            transform.Find("SectionCreatorPanel/Content/StepCountWarning").gameObject.SetActive(false);
        }
    }
}
