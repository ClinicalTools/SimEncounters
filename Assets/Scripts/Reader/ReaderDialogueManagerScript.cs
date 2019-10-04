using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Xml;
using System.Text.RegularExpressions;

public class ReaderDialogueManagerScript : MonoBehaviour
{

    public List<DialogueEntryScript> entries;   //The list of dialogue entries
    public Transform baseParent;                //The highest level parent for dialogue
    public DialogueEntryScript mostRecentEntry; //Used for adding more dialogue purposes
    public ReaderDataScript ds;
    public ReaderTabManager tm;
    public Dictionary<string, Color> charColors;//Dictionary to hold the character colors. Stores by character name
    public string UID;                          //Identifier name for the unique dialogue. Set when opening dialogue
                                                //private XmlDocument xmlDocc;
    private Transform tempButton;               //Temporarily stores the dialogue pin button if it has to run Start() first
    private bool adding;                        //A bool to prevent adding two dialogue options at almost the same time

    /**
	 * The dialogue manager works by having an entries list to store dialogue data along side having the GameObjects
	 * exist outside of the script. This makes it easier to manage/retrieve data and adjust positioning. Data retrieval
	 * is easier too. So whenever a dialogue is added, moved, or removed, the entries list has to be updated accordingly 
	 * 
	 * Every dialogue tracks its parent and it's children. They also have an index. Index increments in order from top to bottom
	 * So if A had a child B and A also had a sibling below it C, then A=1, B=2, C=3
	 */

    // Use this for initialization
    void Start()
    {
        if (entries == null) {
            entries = new List<DialogueEntryScript>();
        }

        adding = false;
        ds = GameObject.Find("GaudyBG").GetComponent<ReaderDataScript>();
        tm = ds.GetComponent<ReaderTabManager>();
        //PopulateDialogue (tempButton);
    }

    /**
	 * Removes the dialogue entry specified by entry
	 */
    public void RemoveDialogue(GameObject entry)
    {
        DialogueEntryScript temp = entry.GetComponent<DialogueEntryScript>();

        //If you're removing the last child, disable the dropdown
        if (temp.parentEntry != null) {
            if (temp.parentEntry.children.Count <= 1) {
                Toggle t = temp.parentEntry.transform.GetComponentInChildren<Toggle>();
                t.isOn = false;
            }
            temp.parentEntry.RemoveChild(temp);
        }
        mostRecentEntry = temp.parentEntry;

        //Remove the dialogue and it's children from the entries list
        entries.RemoveRange(temp.index, GetFinalChildIndex(temp) - temp.index + 1);
        //GameObject.Destroy (entry);
        GetComponentInChildren<ReaderEntryManagerScript>().RemovePanelEntry(entry);

        int i = 0;
        foreach (DialogueEntryScript des in entries) {
            entries[i].index = i;
            i++;
        }
    }

    /**
	 * For other scripts to remove entries from the list
	 */
    public void RemoveFromEntryList(DialogueEntryScript t)
    {
        entries.Remove(t);
    }

    /**
	 * Updates the indexes of the entry list
	 */
    public void UpdateEntryList()
    {
        int i = 0;
        foreach (DialogueEntryScript des in entries) {
            entries[i].index = i;
            i++;
        }
    }

    /**
	 * Returns the unique identifier for the current dialogue session
	 */
    public string GetUID()
    {
        string tempUID = "";
        string UIDToRemove = "";
        string startingUID = "";
        Transform button = tempButton;
        if (!tempButton.GetComponentInParent<ReaderEntryManagerScript>()) {
            Transform uniqueParent = tempButton;
            string path = "";
            if (uniqueParent.name.Equals("Content")) {
                path = uniqueParent.name;
                while (!uniqueParent.name.EndsWith("Tab")) {
                    uniqueParent = uniqueParent.parent;
                    path = uniqueParent.name + "/" + path;
                }
            } else {
                while (uniqueParent.parent != null && !uniqueParent.parent.name.Equals("Content")) {
                    uniqueParent = uniqueParent.parent;
                }
                path = uniqueParent.name;
                while (!uniqueParent.name.EndsWith("Tab")) {//Once you hit the Tab container
                    uniqueParent = uniqueParent.parent;
                    path = uniqueParent.name + "/" + path;
                }
            }
            path = tm.getCurrentSection() + "/" + path;
            startingUID = path;
        } else {
            while (button != null) {
                if (button.name.StartsWith("LabEntry:")) {
                    tempUID = "LabEntry: " + button.GetSiblingIndex() + tempUID;
                    UIDToRemove = button.name;
                }
                if (button.name.EndsWith("Tab")) {
                    tempUID = button.name + "/" + tempUID;
                }
                button = button.parent;
            }
            tempUID = tm.getCurrentSection() + "/" + tempUID;
            startingUID = tempUID;//.Remove(tempUID.LastIndexOf("/")) + "/" + UIDToRemove;
        }
        Debug.Log("Dialogue UID: " + startingUID);


        //ds.GetDialogues ().Remove (tempUID.Remove (tempUID.LastIndexOf (".")) + UIDToRemove);
        //Debug.Log (string.Join (",", ds.GetDialogues ().Select (x => x.Key).ToArray()));

        return startingUID;
        //return tempUID;
    }

    /**
	 * Adds a new dialogue by a character. Adds as child/sibling to the mostRecentEntry (this will change in the future)
	 */
    public DialogueEntryScript AddNewEntry(Text characterName)
    {
        if (adding) {
            throw new System.Exception("Please wait until current dialogue option is added");
        }
        adding = true;
        Transform entryParent;
        if (mostRecentEntry != null) {
            if (mostRecentEntry.characterName.Equals(characterName.text)) {
                entryParent = mostRecentEntry.transform.parent;
            } else {
                entryParent = mostRecentEntry.childrenParent;
            }
        } else {
            entryParent = baseParent;
        }
        entryParent.parent.gameObject.SetActive(true);

        GameObject entryObj;

        if (characterName.text.Equals("Patient")) {
            entryObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/DialogueEntryLeft") as GameObject;
        } else {

            entryObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/DialogueEntryRight") as GameObject;
        }

        //GameObject entryObj = Resources.Load (GlobalData.resourcePath + "/Prefabs/DialogueEntry") as GameObject;
        entryObj = Instantiate(entryObj, entryParent);
        DialogueEntryScript entry = entryObj.GetComponent<DialogueEntryScript>();
        entry.characterName = characterName.text;
        entry.index = entries.Count;
        entryObj.GetComponentInChildren<Image>().color = charColors[characterName.text];
        /*
         * Testing
        */
        entry.charColor = charColors[characterName.text];
        entries.Add(entry);
        mostRecentEntry = entry;
        //mostRecentEntry.parentEntry.transform.GetComponentInChildren<Toggle> ().isOn = true;
        Debug.Log("Entries: " + string.Join("----", entries.Select(x => x.characterName + ", " + x.index + ", " + x.transform.GetSiblingIndex()).ToArray()));
        adding = false;
        return entry;

    }

    /**
	 * Empty right now. May not use this but if I do it'll be when adding in.
	 */
    public void AddEntry(DialogueEntryScript entry)
    {
        entry.index = entries.Count;
        //print(entries.Count);
        entries.Add(entry);
    }

    /**
	 * Removes all dialogues tied to one character
	 */
    public void RemoveCharacterDialogues(string charName)
    {
        List<DialogueEntryScript> chars = GetDialoguesOfCharacter(charName);

        //I'm not sure how you want to delete the dialogues so I'll leave it to you.
        //Just know that you have to update the entries list when you're done
        //Currently I'm just going to set it up to (hopefully) remove them in reverse order to
        //avoid null references when deleting parent dialogues then trying to access children.
        chars.OrderByDescending((DialogueEntryScript arg) => arg.index);

        foreach (DialogueEntryScript des in chars) {
            des.GetComponent<RemoveEntryScript>().bypassConfirmation = true; //HA THIS DID COME IN HANDY
            RemoveDialogue(des.gameObject);
        }
    }

    /**
	 * Returns all dialogues that belong to a certain character
	 * Can be useful when updating a character's name, color, or deleting a character
	 */
    public List<DialogueEntryScript> GetDialoguesOfCharacter(string characterName)
    {
        return entries.FindAll((DialogueEntryScript obj) => obj.characterName.Equals(characterName));
    }

    /**
	 * Called when a dialogue is being loaded.
	 * Pass in the dialogue pin button that was clicked
	 */
    public void PopulateDialogue(Transform button)
    {
        tempButton = button;
        if (tm == null) {
            Start();
        }

        UID = GetUID();
        DiscardDialogue();

        GetComponentInChildren<ReaderEntryManagerScript>().PopulatePanel(button);
        return;
    }

    private float[] ParseColor(string color)
    {
        color = color.Replace("RGBA", "");
        color = color.Replace("(", "");
        color = color.Replace(")", "");
        string[] colorVals = color.Split(","[0]);

        float[] parsedColorValues = new float[4];
        for (int i = 0; i < 4; i++) {
            parsedColorValues[i] = float.Parse(colorVals[i]);
        }

        return parsedColorValues;
    }

    /**
	 * 	Advance the node to the correct next node in the XmlDoc. Think Depth first search
	 */
    /*private XmlNode AdvNode(XmlNode node)
	{
		if (node == null)
			return node;
		if (node.HasChildNodes)
			node = node.ChildNodes.Item(0);
		else if (node.NextSibling != null)
			node = node.NextSibling;
		else {
			while (node.ParentNode.NextSibling == null) {
				node = node.ParentNode;
				if (node == xmlDocc.DocumentElement || node.ParentNode == null)
					return null;
			}
			node = node.ParentNode.NextSibling;
			if (node == xmlDocc.DocumentElement.LastChild)
				return node;
		}
		return node;
	}*/

    /**
	 * Call this when the Cancel button is clicked.
	 */
    public void CancelClicked()
    {
        if (!ds.GetDialogues().ContainsKey(UID)) { //If the dialogue was never saved, remove the pin itself
            Destroy(GetComponentInChildren<ReaderEntryManagerScript>().GetPin());
        }
        DiscardDialogue();
    }

    /**
	 * When cancel button is clicked. Do not save. Destroy all dialogue and characters
	 */
    public void DiscardDialogue()
    {
        entries.RemoveRange(0, entries.Count);
        entries.Clear();
        //charColors = new Dictionary<string, Color> ();

        List<Transform> toBeDestroyed = new List<Transform>();

        for (int i = 0; i < baseParent.childCount; i++) {
            toBeDestroyed.Add(baseParent.GetChild(i));
        }
        foreach (Transform t in toBeDestroyed) {
            Destroy(t.gameObject);
        }
    }

    public void DestroyPanel(GameObject panel)
    {
        Destroy(panel);
        CanvasGroup gaudyBG = GameObject.Find("GaudyBG").GetComponent<CanvasGroup>();
        gaudyBG.alpha = 1.0f;
        gaudyBG.interactable = true;
        gaudyBG.blocksRaycasts = true;
    }

    /**
	 * Returns the dialogue at a specified index
	 */
    public DialogueEntryScript GetDialogueOfIndex(int idx)
    {
        //Debug.Log("Entries: " + string.Join("----", entries.Select(x => x.characterName + ", idx: " + x.index + ", display: " + x.dialogue).ToArray()));

        if (entries.Count > idx && entries[idx].index == idx) {
            return entries[idx];
        } else {
            foreach (DialogueEntryScript des in entries) {
                if (des.index == idx) {
                    return des;
                }
            }
            return null;
        }
    }

    /**
	 * Returns the index of the specified Dialogue's children.
	 */
    public int GetFinalChildIndex(DialogueEntryScript des)
    {
        while (des.children != null && des.children.Count > 0) {
            des = des.children[des.children.Count - 1];
        }
        return des.index;
    }



    /*
	private void OldPopulateDialogueMethod()
	{
		//This is the old method

		//Reload character options
		float[] cVals = new float[4];
		Color c = new Color();

		//Destroy any previous dialogues when re-enabling the dialogue panel
		int childCount = baseParent.childCount;
		for (int i = 0; i < childCount; i++) {
			Destroy(baseParent.GetChild(i).gameObject);
		}

		entries = new List<DialogueEntryScript>();
		DiscardDialogue();

		if (ds == null) {
			ds = GameObject.Find("GaudyBG").GetComponent<ReaderDataScript>();
			tm = ds.GetComponent<ReaderTabManager>();
		}

		//Get the UID for this dialogue session.
		UID = GetUID();

		//Get the data for this UID
		TabInfoScript tis = ds.GetData(tm.getCurrentSection()).GetCurrentTab();
		string text = "<dialogue></dialogue>";//ds.GetData (tm.getCurrentSection (), tis.customName);
		if (ds.GetDialogues().ContainsKey(UID)) {
			text = ds.GetDialogues()[UID];
		}


		//Load in the XML data
		xmlDocc = new XmlDocument();
		xmlDocc.LoadXml(text);
		XmlNode node = xmlDocc.FirstChild;

		while (node != null && !node.InnerText.Equals(UID)) {
			node = AdvNode(node);
		}
		if (node == null) {
			return;
		}
		Debug.Log(node.ParentNode.OuterXml);
		if (!node.Name.Equals("uid")) {
			Debug.Log("No existing dialogue data to load");
			return;
		}
		node = AdvNode(node); //into characters
		bool inData = false;
		List<DialogueEntryScript> parents = new List<DialogueEntryScript>();
		List<XmlNode> nodesOfParents = new List<XmlNode>();
		//parents.Add (baseParent);
		//nodesOfParents.Add (null);
		int idx = 0;
		if (charColors == null) {
			charColors = new Dictionary<string, Color>();
		}
		while (node != null) {
			if (node.Name.Equals("character")) { //load in character data
				node = AdvNode(node);
				continue;
			} else if (inData || node.Name.Equals("EntryData")) {
				inData = true;
				if (node.Name.StartsWith("Entry")) {
					if (node.PreviousSibling != null && nodesOfParents.IndexOf(node.PreviousSibling) >= 0) {//== nodesOfParents [parents.Count - 1]) {
																											//parents.RemoveAt (parents.Count - 1);
																											//nodesOfParents.RemoveAt (parents.Count - 1);

						int indexOf = nodesOfParents.IndexOf(node.PreviousSibling);
						int range = parents.Count - nodesOfParents.IndexOf(node.PreviousSibling);
						parents.RemoveRange(indexOf, range);
						nodesOfParents.RemoveRange(indexOf, range);

					}
					node = AdvNode(node);  //To character name

					GameObject dialogue;

					if (WWW.UnEscapeURL(node.InnerText).Equals("Patient")) {
						dialogue = Resources.Load(GlobalData.resourcePath + "/Prefabs/DialogueEntryLeft") as GameObject;
					} else {

						dialogue = Resources.Load(GlobalData.resourcePath + "/Prefabs/DialogueEntryRight") as GameObject;
					}

					//GameObject dialogue = Resources.Load(GlobalData.resourcePath + "/Prefabs/DialogueEntry") as GameObject;
					//if (parents.Count == 0) {
					dialogue = Instantiate(dialogue, baseParent);
					//} else {
					//	dialogue = Instantiate (dialogue, parents [parents.Count - 1].childrenParent);
					//}
					DialogueEntryScript des = dialogue.transform.GetComponent<DialogueEntryScript>();
					des.characterName = WWW.UnEscapeURL(node.InnerText);
					string charName = des.characterName;
					if (charName.Equals("Patient")) {
						charName = GlobalData.firstName;
					}
					dialogue.transform.Find("Character").GetComponent<Text>().text = charName + ":";
					des.index = idx;
					if (parents.Count > 0) {
						des.parentEntry = parents[parents.Count - 1];
						des.parentEntry.children.Add(des);
					}
					idx++;

					node = node.NextSibling;
					Debug.Log(node.InnerText);

					cVals = ParseColor(node.InnerText);
					des.charColor = new Color(cVals[0], cVals[1], cVals[2], 1f);// cVals[3])

					foreach (Image image in des.borderImages) {
						image.color = des.charColor;
					}

					node = node.NextSibling;  //To dialogue
					des.dialogue = WWW.UnEscapeURL(node.InnerText);
					dialogue.transform.Find("Image/Text").GetComponent<Text>().text = des.dialogue;
					if (node.NextSibling != null) {
						node = node.NextSibling;  //To children
						if (node.InnerText != null && !node.InnerText.Equals("")) { //if Children exist
							parents.Add(des);
							nodesOfParents.Add(node.ParentNode);
						}
					}

					entries.Add(des);
				}
			}
			node = AdvNode(node);
			if (node != null && node.PreviousSibling != null && node.PreviousSibling.Name.Equals("dialogue")) {
				break;
			}
		}
	}
	*/
}
