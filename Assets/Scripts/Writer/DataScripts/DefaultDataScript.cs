using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This holds the default values for the Intermediary DataScript
 */
[System.Serializable]
public class DefaultDataScript {

	public string defaultSection;
	public string defaultTab;
	public string[] sectionList;
	public int screenWidth;
	public int screenHeight;
	public Color defaultColor;
	public string defaultIcon; 

	public DefaultDataScript() {
		this.defaultSection = "Patient_IntroSection";
		this.defaultTab = "Personal Info";
		string[] list = { "Patient_IntroSection" };
		this.sectionList = list;
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		defaultColor = new Color(20.0f / 255.0f, 178.0f / 255.0f, 163.0f / 255.0f, 1);
		defaultIcon = "IconPanel1"; //The person
	}
}