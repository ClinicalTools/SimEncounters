using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Xml;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif


/**
 * This class manages Sections and Tabs
 * It also has some functions for storing/loading data
 */
public class ReaderTabManager : MonoBehaviour
{
    /**
	 * Updates all the section images to those in the imgDict
	 */
    public void ChangeSectionImages()
    {
    }

    /**
	 * Adds the section buttons' icons to the imgDict
	 */
    public void GetSectionImages()
    {
    }

    /**
	 * Adds the active tab to the dictionary
	 */
    public void AddToDictionary()
    {
        return; //Dont save data in the reader
    }

    /**
	 * Clears all data. Useful when loading new file.
	 */
    public void ClearAll()
    {

    }


    private bool AllTabsVisited()
    {
        return false;
    }

    /**
	 * Updates the tab buttons. Currently not in use
	 */
    public void updateTabButtons()
    {
    }

    /**
	 * Called when starting. Loads default data.
	 */
    public void FirstTimeLoad()
    {
    }

    /**
	 * Returns the current tab name (will maybe add the number to the end of this)
	 * Returns TabName now
	 */
    public string getCurrentTab()
    {
        //return currentTab.name;
        return null;
    }

    /**
	 * Sets the currentTab gameObject name
	 * Pass in the formatted tabName
	 */
    public void setCurrentTabName(string tabName)
    {
    }

    // Gets the tab button name 
    public string getTabName()
    {
        return null;
    }

    /**
	 * Returns the current section name
	 */
    public string getCurrentSection()
    {
        return null;
    }

    // Sets the tab button's name on tab switch
    public void setTabName(string newTabName)
    {
    }

    /**
	 * Sets the current section
	 * Pass in the LinkToName
	 */
    public void setCurrentSection(string sectionLinkToName)
    {    }

    /**
	 * Destroys the current tab object
	 */
    public void DestroyCurrentTab()
    {    }

    /**
	 * Destroys the specified tab object
	 * Pass in the LinkToText
	 */
    public void RemoveTab(string tabName)
    {
    }

    /**
	 * Sets the currentSection and currentTab variables to null
	 */
    public void RemoveCurrentSection()
    {
    }

    /// <summary>
    /// Switches the tab
    /// </summary>
    /// <param name="tabName">Formatted tab name</param>
    public void SwitchTab(string tabName)
    {
    }

    /**
	 * Switches the active section.
	 * Pass in the LinkToText
	 */
    public void SwitchSection(string sectionName, bool backwards = false)
    {
    }
}