using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Linq;
using TMPro;

public class AddTabScript : MonoBehaviour {
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
    }

    /**
	 * Adds a new section from the SectionCreatorBG
	 * Pass in the display name
	 */
    public void addSection(TextMeshProUGUI sectionName)
    {
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
        //string xml = "<data></data>";

        //Debug.Log (GlobalData.resourcePath + "Prefabs/Tabs/" + prefabName + "/" + prefabName.Replace (" ", string.Empty));
        GameObject test = Resources.Load(GlobalData.resourcePath + "/Prefabs/Tabs/" + prefabName + "/" + prefabName.Replace(" ", string.Empty)) as GameObject;
        if (test == null) {
            Debug.Log("Cannot load tab prefab");
            return;
        }

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
    }
}
