using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * Script to be called when using the section buttons
 */
public class SwapSectionScript : MonoBehaviour {

	private TabManager BG;
	public TextMeshProUGUI sectionName;	//Provided section name

	// Use this for initialization
	void Start () {
		BG = GameObject.Find ("GaudyBG").GetComponentInChildren<TabManager> ();
	}

	public void ChangeSection() {
		BG.SwitchSection(sectionName.text);
        //GetComponent<Button>().interactable = false;
	}
}
