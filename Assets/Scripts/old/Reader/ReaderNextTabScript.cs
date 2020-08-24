using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReaderNextTabScript : MonoBehaviour {

	private ReaderDataScript ds;
	private ReaderTabManager tm;

	// Use this for initialization
	void Start () {
		ds = GameObject.Find("GaudyBG").GetComponent<ReaderDataScript>();
		tm = ds.GetComponent<ReaderTabManager>();
	}
	
	public void NextTab()
	{
		TabInfoScript currentTab = ds.GetData(tm.getCurrentSection()).GetTabInfo(tm.getCurrentTab());

		//This is the name of the next tab.
		string newTabName = ds.GetData(tm.getCurrentSection()).GetTabList().ToArray()[currentTab.position + 1];
		tm.setTabName(newTabName);
		tm.SwitchTab(newTabName);
	}

	public void NextSection()
	{
		string nextSection = ds.GetSectionsList().ToArray()[ds.GetSectionsList().FindIndex((string obj) => obj.Equals(tm.getCurrentSection())) + 1];
		tm.SwitchSection(nextSection);
	}
}
