using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GlobalDataScript : MonoBehaviour {

	public string resourcesPath = "";
    public string gender;
    public string firstName;
    public string lastName;
	public CanvasGroup toolTip;
    public CanvasGroup contentTip;
    public Color correctColor; //Green (0, 191, 9)
    public Color partialCorrectColor; //Orange (225, 132, 0)
    public Color incorrectColor; //Red (255, 26, 26)
    public Color debugColor;
	public int autosaveRate;
	public Color defaultGreen = new Color(20.0f / 255.0f, 178.0f / 255.0f, 163.0f / 255.0f, 1);
	public GameObject developerWindow;
	public bool developer;
	public bool chooseFileLoadLocation;
	public bool demoMode;
	public string args;
	public bool isMobile;

	private void Awake()
    {
		GlobalData.GDS = this;
    }

    // Use this for initialization
    void Start () {
		GlobalData.resourcePath = resourcesPath;
		GlobalData.toolTip = toolTip;
        GlobalData.contentTip = contentTip;
		gender = "Female";
		if (GlobalData.autosaveRate == 0) {
			GlobalData.enableAutoSave = false;
			if (autosaveRate > 0) {
				GlobalData.enableAutoSave = true;
				GlobalData.autosaveRate = autosaveRate;
			}
		}
	}

	public void setCharacter(string gender) {
		if (this.GetComponent<CharacterManagerScript>()) {
			//this.GetComponent<CharacterManagerScript>().setCharacter(gender);
		}
		this.gender = gender;
	}
}
