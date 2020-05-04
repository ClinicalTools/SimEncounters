using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using TMPro;

public class EditSectionScript : MonoBehaviour
{

    GameObject editSectionPanel;
    TextMeshProUGUI tObject; //SectionLinkToText Text component
    GameObject BG, parentBG;
    TabManager tm;
    private Text inputText;
    private string TitleValuePath;
    Transform colorPanelParent;
    Transform customColorParent;
    Transform colorSlidersParent;
    Transform deleteButton;
    GlobalDataScript gData;
    Transform BGInfoOption; //The transform of the BG Info option in the tab selector
    CondGroup condGroup;

    private GameObject sectionEditPrefab;

    // Use this for initialization
    void Start()
    {
        BG = transform.gameObject;
        parentBG = GameObject.Find("GaudyBG");
        tm = transform.GetComponent<TabManager>();
        //editSectionPanel = transform.Find ("SectionEditorBG").gameObject;
        TitleValuePath = "SectionEditorPanel/Content/Row0/TMPInputField/TitleValue/";

        gData = transform.parent.GetComponent<GlobalDataScript>();
        BGInfoOption = transform.Find("TabSelectorBG/TabSelectorPanel/Content/ScrollView/Viewport/Content/BackgroundInfoTabPanel");
    }


    /**
	 * Opens the edit section panel. Pass in display text value
	 */
    public void OpenSectionPanel(TextMeshProUGUI t)
    {
        if (BG == null) {
            Start();
        }

        tObject = t;

        sectionEditPrefab = Instantiate(Resources.Load("Writer/Prefabs/Panels/SectionEditorBG")) as GameObject;
        sectionEditPrefab.transform.SetParent(parentBG.transform, false);
        //editSectionPanel = transform.Find ("SectionEditorBG").gameObject;
        sectionEditPrefab.transform.SetAsLastSibling();
        sectionEditPrefab.transform.SetSiblingIndex(sectionEditPrefab.transform.GetSiblingIndex() - 1);
        sectionEditPrefab.transform.Find(TitleValuePath).GetComponent<TMP_InputField>().text = t.text;
        colorPanelParent = sectionEditPrefab.transform.Find("SectionEditorPanel/Content/MainGroup/Row3");
        customColorParent = colorPanelParent.Find("Column0/RowCustomColor/TMPInputField/ColorHexValue");
        colorSlidersParent = colorPanelParent.Find("Column1");
        deleteButton = sectionEditPrefab.transform.Find("SectionEditorPanel/Content/MainGroup/Row5/DeleteSectionButton");

        // Checks if this tab has Patient Info, if so then this section cannot be deleted
        foreach (Transform child in tm.TabButtonContentPar.transform) {
            if (child.name == "Personal InfoTabButton") {
                deleteButton.gameObject.SetActive(false);
            }
        }


        //Check the Background Info box if the background info tab is already spawned
        //Debug.Log(string.Join(",", ds.GetData(tm.getCurrentSection()).GetTabList().ToArray()));
        Toggle bgInfoToggle = sectionEditPrefab.transform.Find("SectionEditorPanel/Content/MainGroup/Row1").GetComponentInChildren<Toggle>();
        bgInfoToggle.isOn = false;
    }

    public void UpdateColorSlidersFromHex(TMP_InputField hex)
    {
        //Set the slider values to match the section's current color
        colorSlidersParent.Find("Row0/RSlider").GetComponent<Slider>().value = System.Convert.ToInt32(hex.text.Substring(0, 2), 16) / 255.0f;
        colorSlidersParent.Find("Row1/GSlider").GetComponent<Slider>().value = System.Convert.ToInt32(hex.text.Substring(2, 2), 16) / 255.0f;
        colorSlidersParent.Find("Row2/BSlider").GetComponent<Slider>().value = System.Convert.ToInt32(hex.text.Substring(4, 2), 16) / 255.0f;
    }

    /**
	 * Called when selecting a premade color. Updates the color sliders to match
	 */
    public void UpdateColorChoice()
    {
        foreach (Toggle tog in colorPanelParent.GetChild(0).GetComponentsInChildren<Toggle>()) {
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
                customColorParent.GetComponentInChildren<TextMeshProUGUI>().text = newTextString;
            }
        }
    }

    /**
	 * Used for changing and displaying the custom color on the SectionEditorPanel
	 * Index is the index of the color (R = 0, G = 1, B = 2)
	 */
    public void EditColor(int index)
    {
        float value;
        string newTextString;
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
	 * Submits the changes from the edit section panel
	 */
    public void SubmitChanges()
    {
        //Sets the section's name to the new display name
        string sectionLinkTo = tObject.text;

        // Set conditions
        var condGroup = sectionEditPrefab.GetComponentInChildren<CondGroup>();
        //var conditions = condGroup.ConditionSerials();
        // Update conditionals
        //ds.UpdateSectionConds(sectionLinkTo, conditions);

        string newName = sectionEditPrefab.transform.Find(TitleValuePath).GetComponent<TMP_InputField>().text;
        Debug.Log("SECTIONEDITPREFAB: " + sectionEditPrefab.name);
        if (newName != null && !newName.Equals("")) {
            sectionLinkTo = newName.Replace(" ", "_") + "Section";
            //try {
            Debug.Log("tObject.name: " + tObject.name + ", text: " + tObject.text);
            Debug.Log("newName: " + newName);
            Debug.Log("sectionLinkTo: " + sectionLinkTo);
            //ds.EditSection (tObject.text, newName);
            var text = tObject.transform.parent.Find("SectionDisplayTMP").GetComponentInChildren<TextMeshProUGUI>();
            text.text = newName;
            tObject.text = sectionLinkTo;
            if (text.preferredWidth > 270) {
                text.GetComponent<LayoutElement>().preferredWidth = 270;
            } else {
                text.GetComponent<LayoutElement>().preferredWidth = -1;
            }
            tObject.transform.parent.name = sectionLinkTo + "Button";
            //} catch (Exception e) {
            //	Debug.Log (e.Message);
            //}
        }

        //Finds and sets the section's icon to the chosen one
        Sprite spr = null;
        Transform[] sectionIcons = sectionEditPrefab.transform.Find("SectionEditorPanel/Content/MainGroup/ScrollView/Viewport/Content").GetComponentsInChildren<Transform>();
        foreach (Transform t in sectionIcons) {
            Toggle tog;
            if ((tog = t.GetComponent<Toggle>()) != null && tog.isOn) {
                spr = t.Find("Icon").GetComponent<Image>().sprite;
                break;
            }
        }
        if (spr != null) {
            tObject.transform.parent.Find("Image").GetComponent<Image>().sprite = spr;
        }
    }

    /**
	 * Called by the edit section panel to remove a section
	 */
    public void removeSection()
    {
        string removedSection = tObject.text;
        Transform par = tm.SectionContentPar.transform;
        if (!tObject.text.Contains("/")) {
            Destroy(tm.SectionContentPar.transform.Find(tObject.text + "Button").gameObject);
        } else {
            for (int i = 0; i < tm.SectionContentPar.transform.childCount; i++) {
                if (tm.SectionContentPar.transform.GetChild(i).name.Equals(tObject.text + "Button")) {
                    Destroy(tm.SectionContentPar.transform.GetChild(i).gameObject);
                    break;
                }
            }
        }
        string switchTo = "";
        for (int i = 0; i < tm.SectionContentPar.transform.childCount; i++) {
            Transform child = tm.SectionContentPar.transform.GetChild(i);
            if (!child.name.Equals("Filler")) {
                switchTo = child.Find("SectionDisplayTMP").GetComponent<TextMeshProUGUI>().text.Replace(" ", "_") + "Section";
                if (!switchTo.Equals(removedSection)) {
                    break;
                }
            } else {
                switchTo = "";
            }
        }
        //Debug.Log (tm.SectionContentPar.transform.GetComponentsInChildren<Transform>()[1].name);
        //switchTo = tm.SectionContentPar.GetComponentsInChildren<Transform>()[1].Find("SectionDisplayText").GetComponent<TextMeshProUGUI>().text;
        if (switchTo.Equals("")) {
            BG.transform.Find("SectionCreatorBG").gameObject.SetActive(true);
            for (int i = 0; i < tm.TabContentPar.transform.childCount; i++) {
                Destroy(tm.TabContentPar.transform.GetChild(i).gameObject);
            }
        } else {
            tm.SwitchSection(switchTo);
        }

        //Destroy (editTabPanel);
    }

    /**
	 * Pass in the Display Text
	 */
    private void AddTab(string tabName)
    {
        string prefabName = tabName + " Tab";
        string tabCustomName = tabName;
        string xmlTabName = tabName.Replace(" ", "_") + "Tab";
        //string xml = "<data></data>";

        //Debug.Log (gData.resourcesPath + "Prefabs/Tabs/" + prefabName + "/" + prefabName.Replace (" ", string.Empty));
        GameObject test = Resources.Load(gData.resourcesPath + "/Prefabs/Tabs/" + prefabName + "/" + prefabName.Replace(" ", string.Empty)) as GameObject;
        if (test == null) {
            Debug.Log("Cannot load tab prefab");
            return;
        }
        GameObject newTab = Resources.Load(gData.resourcesPath + "/Prefabs/TabButton") as GameObject;
        TextMeshProUGUI[] children = newTab.GetComponentsInChildren<TextMeshProUGUI>();
        foreach (TextMeshProUGUI child in children) {
            if (child.name.Equals("TabButtonLinkToText")) { //Where the button links to
                child.text = xmlTabName;
            } else if (child.name.Equals("TabButtonDisplayText")) { //What the button displays
                child.text = tabCustomName;
            }
        }
        //The button's position
        newTab = Instantiate(newTab, tm.TabButtonContentPar.transform);
        newTab.name = tabCustomName.Replace(" ", "_") + "Button";
        if (tabName.Equals("Background Info")) {
            newTab.transform.SetSiblingIndex(0);
            UpdateTabPos();
        }
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
