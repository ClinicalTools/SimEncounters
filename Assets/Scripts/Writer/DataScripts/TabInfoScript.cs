using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class TabInfoScript {

	public int n;
	public string customName; //Display name, not formatted
	public string type;
	public string data;
	public bool persistant; //If true, cannot change
	public int position; //The local transform position of the tab
	private bool visited; //Only used in reader

	/**
	 * Data script to hold info for every tab. Stored in SectionDataScript.Dict
	 */

	public TabInfoScript(int n, string type, string customName,  string data, int position) {
		this.n = n;
		this.type = type;
		this.customName = customName;
		if (data == null) {
			this.data = "";
		} else {
			this.data = data;
		}
		this.position = position;
	}

	public bool WasVisited()
	{
		return visited;
	}

	//Sets the tab as visited
	public void Visit() {
		visited = true;
	}

	public TabInfoScript(/*int n,*/ string name) {
		//this.n = n;
		this.type = name;
		this.data = "";
	}

	public string getData() {
		string xmlName = type.Replace (" ", "_") + "Tab";
		if (data.Length == 0) {
			data = "<data></data>";
		}
		return "<" + xmlName + "><customTabName>" + customName + "</customTabName>" + data + "</" + xmlName + ">";
	}

	public void SetPosition(int pos) {
		position = pos;
	}
}
