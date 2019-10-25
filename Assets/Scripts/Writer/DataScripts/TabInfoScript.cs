using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class TabInfoScript
{
    public static TabInfoScript CurrentTab { get; set; }

    public int n;
    public string customName; //Display name, not formatted
    public string type;
    public string data;
    public bool persistant; //If true, cannot change
    public int position; //The local transform position of the tab
    private bool visited; //Only used in reader
    public List<string> Conditions { get; set; }

    /**
	 * Data script to hold info for every tab. Stored in SectionDataScript.Dict
	 */

    public TabInfoScript(int n, string type, string customName, string data, int position, List<string> conditions = null)
    {
        this.n = n;
        this.type = type;
        this.customName = customName;
        if (data == null) {
            this.data = "";
        } else {
            this.data = data;
        }
        this.position = position;

        Conditions = conditions;
    }

    public bool WasVisited()
    {
        return visited;
    }

    //Sets the tab as visited
    public void Visit()
    {
        visited = true;
    }

    public TabInfoScript(/*int n,*/ string name)
    {
        //this.n = n;
        this.type = name;
        this.data = "";
    }

    public string getData()
    {
        string xmlName = type.Replace(" ", "_") + "Tab";

        string conditionsXml = "";
        if (Conditions != null && Conditions.Count > 0) {
            conditionsXml = "<conditionals>";
            foreach (var condition in Conditions)
                conditionsXml += "<cond>" + condition + "</cond>";
            conditionsXml += "</conditionals>";        
        }

        if (data.Length == 0) {
            data = "<data></data>";
        }
        return "<" + xmlName + "><customTabName>" + customName + "</customTabName>" + conditionsXml  + data + "</" + xmlName + ">";
    }

    public void SetPosition(int pos)
    {
        position = pos;
    }
}
