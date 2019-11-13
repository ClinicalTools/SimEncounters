using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using SimEncounters;

public class LabEntryScript
{

    private int position;               //The entry's position compared to it's siblings
    public GameObject gObject;          //The actual game object this entry is used for
    public string panelType;            //The name of the prefab that represents this entry
    private WriterHandler ds;              //Variable to hold a reference to DataScript

    /**
	 * An entry (panel) used in HistoryFieldManagerScript
	 */

    public LabEntryScript(int pos, GameObject obj, string panelType)
    {
        position = pos;
        gObject = obj;
        this.panelType = panelType;
        ds = WriterHandler.WriterInstance;
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

        if (gObject.GetComponent<DialogueEntryScript>()) {
            return gObject.GetComponent<DialogueEntryScript>().GetData();
        }

        string xml = "";
        Transform nextChild = null; //Used to skip sections of XML
        Transform[] allChildren = gObject.GetComponentsInChildren<Transform>();

		/*EntryValue.EntryValue[] values = gObject.GetComponentsInChildren<EntryValue.EntryValue>(true);
		foreach(EntryValue.EntryValue value in values) {
			Debug.Log(value + ", " + value.GetValueWithXML());
			xml += value.GetValueWithXML();
			//Nested hfms
			//Pins
		}
		Debug.Log("ENTRY VALUE XML: " + xml);
		*/
		xml = "";
        foreach (Transform child in allChildren) {
            if (child != null) {
                string startingUID = "";
                string newUID = "";
                if (child.GetComponent<HistoryFieldManagerScript>() != null) { //There's a nested entry. We don't worry about that entry's data
                    xml += child.GetComponent<HistoryFieldManagerScript>().getData();
                    Transform tempChild = child;
                    while (nextChild == null) {
                        if (tempChild.GetSiblingIndex() + 1 == tempChild.parent.childCount)
                            tempChild = tempChild.parent;
                        else
                            nextChild = tempChild.parent.GetChild(tempChild.GetSiblingIndex() + 1);
                    }
                } else if (child.gameObject.GetComponent<EntryValue.EntryValue>() != null) {
					//Value is escaped if appropriate by the EnvryValue class
					xml += child.gameObject.GetComponent<EntryValue.EntryValue>().GetValueWithXML();
				} else if ((child.name.ToLower().EndsWith("value") || child.tag.Equals("Value") || child.name.ToLower().EndsWith("toggle")) && nextChild == null) { //Input object
					if (child.gameObject.GetComponent<Toggle>() != null && child.gameObject.GetComponent<Toggle>().isOn) {
                        xml += "<" + child.name + ">";
                        xml += child.gameObject.GetComponent<Toggle>().isOn;
                        xml += "</" + child.name + ">";
                    } else if (child.name.ToLower().EndsWith("toggle")) {
                        continue;
                    } else {

                        xml += "<" + child.name + ">";

                        //Handle reading the child
                        if (child.gameObject.GetComponent<InputField>() != null) {
                            xml += UnityWebRequest.EscapeURL(child.gameObject.GetComponent<InputField>().text);
                        } else if (child.gameObject.GetComponent<Dropdown>() != null) {
                            xml += UnityWebRequest.EscapeURL(child.gameObject.GetComponent<Dropdown>().captionText.text);
                        } else if (child.gameObject.GetComponent<Text>() != null) {
                            xml += UnityWebRequest.EscapeURL(child.gameObject.GetComponent<Text>().text);
                        } else if (child.gameObject.GetComponent<TMPro.TMP_InputField>() != null) {
                            xml += UnityWebRequest.EscapeURL(child.gameObject.GetComponent<TMPro.TMP_InputField>().text);
                        } else if (child.gameObject.GetComponent<TMPro.TextMeshProUGUI>() != null) {
                            xml += UnityWebRequest.EscapeURL(child.gameObject.GetComponent<TMPro.TextMeshProUGUI>().text);
                        } else if (child.gameObject.GetComponent<TMPro.TMP_Dropdown>() != null) {
                            xml += UnityWebRequest.EscapeURL(child.gameObject.GetComponent<TMPro.TMP_Dropdown>().captionText.text);
                        } else if (child.name.Equals("DialoguePin") || child.name.Equals("QuizPin") || child.name.Equals("FlagPin") || child.name.Equals("EventPin")) {
                            newUID = "";
                            startingUID = "";
                            Transform tempChild = child;
                            while (tempChild != null) {
                                if (tempChild.name.StartsWith("LabEntry:")) {
                                    startingUID = tempChild.name + startingUID;
                                    newUID = "LabEntry: " + tempChild.GetSiblingIndex();
                                }
                                if (tempChild.name.EndsWith("Tab")) {
                                    newUID = tempChild.name + "/" + newUID;
                                }
                                tempChild = tempChild.parent;
                            }
                            newUID = ds.transform.GetComponent<TabManager>().getCurrentSection() + "/" + newUID;

                            startingUID = newUID.Remove(newUID.LastIndexOf("/") + 1) + startingUID;
                            //Debug.Log ("starting: " + startingUID + ", new: " + newUID);

                            //If the dialogue entry has been moved, adjust the dictionary accordingly
                            if (child.name.Equals("DialoguePin")) {
                                if (ds.GetDialogues().ContainsKey(startingUID)) {
                                    string dialogueXML = ds.GetDialogues()[startingUID]; //Dialogue data
                                    if (Regex.Matches(startingUID, "LabEntry").Count != Regex.Matches(newUID, "LabEntry").Count) {
                                        var labEntry = child;
                                        while (!labEntry.name.Contains("LabEntry")) {
                                            if (labEntry.parent)
                                                labEntry = labEntry.parent;
                                            else
                                                break;
                                        }

                                        newUID += "LabEntry: " + labEntry.GetSiblingIndex();
                                    }

                                    //If there was no dialogue where this dialogue was moved to
                                    if (!ds.GetDialogues().ContainsKey(newUID)) {
                                        ds.GetDialogues().Remove(startingUID);
                                        //ds.AddDialogue (newUID, dialogueXML.Replace (startingUID, newUID));
                                    }
                                   // if (ds.correctlyOrderedDialogues.ContainsKey(newUID)) {
                                        ds.CorrectlyOrderedDialogues.Add(newUID, dialogueXML.Replace(startingUID, newUID));
                                    //}

                                    xml += dialogueXML.Replace(startingUID, newUID);
                                } else {
                                    xml = xml.Substring(0, xml.Length - ("<" + child.name + ">").Length);
                                    continue;
                                }
                            }

                            //If the quiz entry has been moved, adjust the dictionary accordingly
                            if (child.name.Equals("QuizPin")) {
                                if (ds.GetQuizes().ContainsKey(startingUID)) {
                                    string quizXML = ds.GetQuizes()[startingUID]; //Quiz data
                                    if (Regex.Matches(startingUID, "LabEntry").Count != Regex.Matches(newUID, "LabEntry").Count) {
                                        var labEntry = child;
                                        while (!labEntry.name.Contains("LabEntry")) {
                                            if (labEntry.parent)
                                                labEntry = labEntry.parent;
                                            else
                                                break;
                                        }

                                        newUID += "LabEntry: " + labEntry.GetSiblingIndex();
                                    }

                                    //If there was no quiz where this quiz was moved to
                                    if (!ds.GetQuizes().ContainsKey(newUID)) {
                                        ds.GetQuizes().Remove(startingUID);
                                        //ds.AddQuiz (newUID, quizXML.Replace (startingUID, newUID));
                                    }
                                    if (ds.CorrectlyOrderedQuizes.ContainsKey(newUID)) {
                                        ds.CorrectlyOrderedQuizes.Add(newUID, quizXML.Replace(startingUID, newUID));
                                    }
                                    xml += quizXML.Replace(startingUID, newUID);
                                } else {
                                    xml = xml.Substring(0, xml.Length - ("<" + child.name + ">").Length);
                                    continue;
                                }
                            }
                            //Debug.Log (string.Join ("-----", ds.GetDialogues ().Select (x => x.Key + "DATA::::" + x.Value).ToArray()));
                        }

                        xml += "</" + child.name + ">";
                    }
                } else if (child.tag.Equals("Image") && child.GetComponent<OpenImageUploadPanelScript>()) {
                    startingUID = child.GetComponent<OpenImageUploadPanelScript>().GetGuid();
                    xml += "<Image>" + startingUID + "</Image>";
                }
                if (child == nextChild) {
                    nextChild = null;
                }
            }
        }
        Debug.Log("Current section's XML: " + xml);

        return xml;
    }
}
