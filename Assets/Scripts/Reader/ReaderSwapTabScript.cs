using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * Script only to be called when using Tab buttons
 * The name of the Tab that the button is switching to must be stored in a Text child componnent named "Description"
 */
public class ReaderSwapTabScript : MonoBehaviour {

	private ReaderTabManager BG;
	public TextMeshProUGUI tabType;
    public TextMeshProUGUI customTabName;

	// Use this for initialization
	void Start () {
		BG = GameObject.Find ("GaudyBG").GetComponentInChildren<ReaderTabManager> ();
	}

	public void ChangeTab() {
		//BG.setTabName(customTabName.text);
		BG.setTabName(customTabName.text);//.Replace(" ", "_") + "Tab");
		//BG.SwitchTab (tabType.text);
		BG.SwitchTab (customTabName.text);//.Replace(" ", "_") + "Tab");
	}
}
