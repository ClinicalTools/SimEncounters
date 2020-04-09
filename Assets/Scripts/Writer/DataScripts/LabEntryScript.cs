using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

public class LabEntryScript
{
    private int position;               //The entry's position compared to it's siblings
    public GameObject gObject;          //The actual game object this entry is used for
    public string panelType;            //The name of the prefab that represents this entry
 
    /**
	 * An entry (panel) used in HistoryFieldManagerScript
	 */

    public LabEntryScript(int pos, GameObject obj, string panelType)
    {
        position = pos;
        gObject = obj;
        this.panelType = panelType;
    }

    /**
	 * Returns the position
	 */
    public int GetPosition()
    {
        return position;
    }

    /**
	 * Sets the position
	 */
    public void SetPosition(int pos)
    {
        position = pos;
    }

    /**
	 * Returnst he name of the type of prefab this entry is
	 */
    public string GetPanelType()
    {
        return panelType;
    }

    /**
	 * Returns the data of this entry in an XML formated string
	 */
    public string getData()
    {
        return "";
    }
}
