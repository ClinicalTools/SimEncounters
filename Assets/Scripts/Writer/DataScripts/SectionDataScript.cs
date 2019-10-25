using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

/**
 * This holds all of the data for each Section. This includes the XML strings,
 * Current Tab, the section's custom name, and a list of all tab names
 */
public class SectionDataScript
{
    private readonly DataScript ds;

    private Dictionary<string, TabInfoScript> Dict;     //Dictionary holding each Tab's information. Key=TabName. Value=TabData.
    private TabInfoScript currentTab;                   //Current/last tab the user was on in this section
    private string displayName;         //Custom name set for the section
    private List<string> tabList;       //List of the names of tabs
    private int count;                  //Number of all tabs ever created (NOT THE NUMBER OF CURRENT TABS)
    private int position;
    public List<string> Conditions { get; set; }
    
    //private bool visited = false; //Only used in the reader

    // Use this for initialization
    public SectionDataScript()
    {
        Dict = new Dictionary<string, TabInfoScript>();
        tabList = new List<string>();
        count = 0;
        this.position = 0;


        var bg = GameObject.Find("GaudyBG");
        if (bg != null)
            ds = bg.GetComponent<DataScript>();
    }
    /**
	 * Initiates the script variables (to make sure)
	 */
    public void Initiate()
    {
        Dict = new Dictionary<string, TabInfoScript>();
        tabList = new List<string>();
    }

    /**
	 * Returns the data of a certain tab
	 */
    public string GetData(string key)
    {
        if (Dict.ContainsKey(key)) {
            return Dict[key].data;
        } else {
            return null;
        }
    }

    /**
	 * Adds a tab name to the list of tabs in the section (does not add tab data)
	 * Pass tabName
	 */
    public void AddTabToList(string tabName)
    {
        tabList.Add(tabName);
        //count++;
    }

    /**
	 * Returns the list of tabs in this section
	 */
    public List<string> GetTabList()
    {
        //Debug.Log ("Dictionary List: " + string.Join (",", tabList.OrderBy ((string arg) => Dict [arg].position).Select (x => x + ": " + Dict [x].position).ToArray ()));
        /*Debug.Log ("Tab listing: " + string.Join (",", tabList.ToArray ()) + ".");
		Debug.Log ("Dictionary listing: " + string.Join (",", Dict.Keys.ToArray ()));*/
        return tabList.OrderBy((string arg) => Dict[arg].position).ToList();
    }

    /**
	 * Returns all data in this section as one string
	 */
    public string GetAllData()
    {
        //Debug.Log(string.Join("", Dict.Values.OrderBy((TabInfoScript arg) => arg.position).Select(x => x.getData()).ToArray ()));
        string data = "";

        if (Conditions != null && Conditions.Count > 0) {
            data += "<conditionals>";
            foreach (var condition in Conditions)
                data += "<cond>" + condition + "</cond>";
            data += "</conditionals>";
        }

        //data += string.Join("", Dict.Select(x => x.Value.getData()).ToArray());
        data += string.Join("", Dict.Values.OrderBy((TabInfoScript arg) => arg.position).Select(x => x.getData()).ToArray());

        return data;
    }

    /**
	 * Adds a certain Tab's information to the dictionary whose name cannot be changed
	 * Pass in tabType as key, nonformatted
	 */
    public void AddPersistingData(string key, string data)
    {
        //string key2 = Regex.Split (key, "[0-9]*$")[0];
        if (Dict.ContainsKey(key)) {
            Dict[key].data = data;
        } else {
            Dict.Add(key, new TabInfoScript(count, key, key, data, Dict.Count));
            count++;
            AddTabToList(key);
            Dict[key].persistant = true;
        }
    }

    /**
	 * Adds a certain Tab's information to the dictionary whose name cannot be changed
	 * Pass in tabType as key, nonformatted
	 */
    public void AddPersistingData(string key, string customName, string data)
    {
        //string key2 = Regex.Split (key, "[0-9]*$")[0];
        if (Dict.ContainsKey(key)) {
            Dict[key].data = data;
        } else {
            Dict.Add(customName, new TabInfoScript(count, key, customName, data, Dict.Count));
            count++;
            AddTabToList(customName);
            Dict[customName].persistant = true;
        }
    }

    /**
	 * Returns whether or not the tab in question is persistant/Non changeable
	 * True if it is persistant
	 */
    public bool IsPersistant(string key)
    {
        if (Dict.ContainsKey(key))
            return Dict[key].persistant;
        return false;
    }

    /**
	 * Adds a certain Tab's information to the dictionary
	 * Pass in tabType as key, nonformatted
	 */
    public void AddData(string key, string data)
    {
        if (Dict.ContainsKey(key)) {
            Dict[key].data = data;
        } else {
            //Debug.Log ("I'M USING REGULAR ADDDATA, ADDING " + key);
            Dict.Add(key, new TabInfoScript(count, key, key, data, Dict.Count));
            count++;
            AddTabToList(key);
        }
    }

    /**
	 * Adds a certain Tab's information to the dictionary
	 * Pass in tabType as key, nonformatted
	 */
    //I replaced key with custom name below
    public void AddData(string key, string customName, string data, List<string> conditions = null)
    {
        if (Dict.ContainsKey(customName)) {
            var tabInfo = Dict[customName];
            tabInfo.data = data;
            tabInfo.Conditions = conditions;
        } else {
            //Dict.Add(key, new TabInfoScript(count, key, customName, data, Dict.Count));
            //Debug.Log("ADDING " + customName + " TO DICTIONARY AS " + key);
            Dict.Add(customName, new TabInfoScript(count, key, customName, data, Dict.Count, conditions));
            count++;
            AddTabToList(customName);
        }
    }

    public bool AllTabsVisited()
    {
        foreach (string tab in Dict.Keys) {
            if (!Dict[tab].WasVisited()) {
                return false;
            }
        }
        return true;
    }

    public void VisitAllTabs()
    {
        foreach (string tab in Dict.Keys) {
            Dict[tab].Visit();
        }
    }

    /**
	 * True if Tab exists. False otherwise.
	 */
    public bool ContainsKey(string key)
    {
        return Dict.ContainsKey(key);
    }

    /**
	 * Updates the current tab
	 */
    public void SetCurrentTab(string tabName)
    {
        if (Dict.ContainsKey(tabName)) {
            currentTab = Dict[tabName];
            TabInfoScript.CurrentTab = currentTab;
        }
        //if (Dict [tabName] != null) {
        //currentTab = Dict [tabName];
        //} 
        else {
            Debug.Log("Could not find " + tabName + " in Dict");
        }
    }

    /**
	 * Returns the current tab
	 */
    public TabInfoScript GetCurrentTab()
    {
        return currentTab;
    }

    /**
	 * Updates the section's custom name
	 */
    public void SetSectionDisplayName(string name)
    {
        Debug.Log("DISPLAY NAME: " + name);
        displayName = name;
    }

    /**
	 * Returns the section's custom name
	 */
    public string GetSectionDisplayName()
    {
        return displayName;
    }

    /**
	 * Replaces a tab's custom name
	 * oldName = tab's LinkToText
	 * newName = new custom name
	 */
    public void Replace(string oldName, string newName)
    {
        if (Dict.ContainsKey(oldName)) {
            //TabInfoScript temp = Dict [oldName];
            try {
                Dict.Add(newName, Dict[oldName]);
                var tab = Dict[newName];
                tab.customName = newName;
                Dict.Remove(oldName);
                tabList[tabList.FindIndex(listIdxname => listIdxname == oldName)] = newName;
            } catch (System.Exception) {
                ds.ShowMessage("Cannot have two tabs with matching names!", true);
                throw new System.Exception("Cannot have two tabs with matching names!");
            }
        }
    }

    /**
	 * Returns the TabInfoScript of the provided tab
	 * Pass in the unformatted tabName
	 */
    public TabInfoScript GetTabInfo(string tab)
    {
        if (Dict == null) {
            return null;
        }
        if (Dict.ContainsKey(tab)) {
            return Dict[tab];
        }
        Debug.Log("No tab info found for tab: " + tab);
        Debug.Log("Current dictionary entries: " + string.Join(",", Dict.Keys.ToArray()));
        return null;
    }

    /**
	 * Removes a tab from the section
	 */
    public void Remove(string tab)
    {
        Dict.Remove(tab);
        tabList.Remove(tab);
        Debug.Log(GetAllData());
    }

    /**
	 * Returns the number of all tabs which have ever been added to this section
	 */
    public int GetCount()
    {
        return count;
    }

    public int GetPosition()
    {
        return position;
    }

    public void SetPosition(int pos)
    {
        position = pos;
    }

    public string GetAllPositions()
    {
        return string.Join(",", Dict.Values.OrderBy((TabInfoScript arg) => arg.position).Select(x => x.type + ": " + x.position).ToArray());
    }
}
