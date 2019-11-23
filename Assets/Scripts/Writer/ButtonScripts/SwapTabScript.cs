using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * Script only to be called when using Tab buttons
 * The name of the Tab that the button is switching to must be stored in a Text child componnent named "Description"
 */
public class SwapTabScript : MonoBehaviour
{

    private TabManager BG;
    public TextMeshProUGUI tabType;
    public TextMeshProUGUI customTabName;
    public string TabKey { get; set; }

    // Use this for initialization
    void Start()
    {
        BG = GameObject.Find("GaudyBG").GetComponentInChildren<TabManager>();
    }

    public void ChangeTab()
    {
        if (TabKey == null) {
            //BG.setTabName(customTabName.text);
            BG.setTabName(customTabName.text);//.Replace(" ", "_") + "Tab");
                                              //BG.SwitchTab (tabType.text);
            BG.SwitchTab(customTabName.text);//.Replace(" ", "_") + "Tab");
        } else {
            BG.setTabName(TabKey);//.Replace(" ", "_") + "Tab");
                                              //BG.SwitchTab (tabType.text);
            BG.SwitchTab(TabKey);//.Replace(" ", "_") + "Tab");

        }
    }

    public void SetName(string name)
    {
        TabKey = name;
        customTabName.text = name;
    }
}
