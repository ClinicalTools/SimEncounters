using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Xml;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.Networking;
using ClinicalTools.SimEncounters;

public class DialogueManagerScript : MonoBehaviour
{

    public List<DialogueEntryScript> entries;   //The list of dialogue entries
    public Transform baseParent;                //The highest level parent for dialogue
    public DialogueEntryScript mostRecentEntry; //Used for adding more dialogue purposes
    public WriterHandler ds;
    public TabManager tm;
    public Dictionary<string, Color> charColors;//Dictionary to hold the character colors. Stores by character name
    public Transform characterParent;           //The parent of the character buttons
    public string UID;                          //Identifier name for the unique dialogue. Set when opening dialogue
                                                //private XmlDocument xmlDocc;
                                                //private bool hasStarted = false;
    private Transform tempButton;               //Temporarily stores the dialogue pin button if it has to run Start() first
    public Transform characterPanel;            //Reference to the panel for adding a character
    private List<string> defaultCharacters;     //The list of default characters to be reloaded for every dialogue
    private Dropdown characterPanelDropdown;    //The dropdown holding the list of characters that can be added
    private List<string> currentCharacters;     //The list of current active characters
    private bool adding;                        //A bool to prevent adding two dialogue options at almost the same time

    //Do a search for "//NESTED" to find where to revert back to nested dialogues
    //Also, set the CollapseContentScript to not auto collapse for nested dialogue

    /**
	 * The dialogue manager works by having an entries list to store dialogue data along side having the GameObjects
	 * exist outside of the script. This makes it easier to manage/retrieve data and adjust positioning. Data retrieval
	 * is easier too. So whenever a dialogue is added, moved, or removed, the entries list has to be updated accordingly 
	 * 
	 * Every dialogue tracks its parent and it's children. They also have an index. Index increments in order from top to bottom
	 * So if A had a sibling B below it and A also had a child C, then A=1, C=2, B=3
	 */

    // Use this for initialization
    void Start()
    {
        if (entries == null) {
            entries = new List<DialogueEntryScript>();
        }
        List<Transform> startingChildren = new List<Transform>();
        charColors = new Dictionary<string, Color>();
        for (int i = 0; i < characterParent.childCount; i++) {
            startingChildren.Add(characterParent.GetChild(i));
        }

        foreach (Transform t in startingChildren) {
            if ((t.name.StartsWith("CharacterEntry") || t.name.StartsWith("PlayerEntry") || t.name.StartsWith("InstructorEntry")) && t.gameObject.activeInHierarchy) {
                //AddCharacter (t.GetComponentInChildren<Text> (), t.Find("CharacterImage").GetComponentInChildren<Image> ());
                if (!charColors.ContainsKey(t.GetComponentInChildren<TextMeshProUGUI>().text)) {
                    Debug.Log("Adding color for " + t.GetComponentInChildren<TextMeshProUGUI>().text);
                    charColors.Add(t.GetComponentInChildren<TextMeshProUGUI>().text, t.Find("CharacterImage").GetComponentInChildren<Image>().color);
                }
            }
        }

        //Store the default characters
        defaultCharacters = new List<string>();
        characterPanelDropdown = characterPanel.GetComponentInChildren<Dropdown>();
        defaultCharacters.AddRange(characterPanelDropdown.options.Select(x => x.text));

        currentCharacters = new List<string>();
        adding = false;
        //hasStarted = true;
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
        //if (temp.index > 0 && entries.Count > (temp.index + GetFinalChildIndex(temp) - temp.index + 1)) {
        print(temp.index + ", " + entries.Count + ", " + (temp.index + GetFinalChildIndex(temp) - temp.index + 1));
        entries.RemoveRange(temp.index, GetFinalChildIndex(temp) - temp.index + 1);
        //}
        //GameObject.Destroy (entry); //This is handled in the below line
        GetComponentInChildren<HistoryFieldManagerScript>().RemovePanelEntry(entry);

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
        string startingUID;
        Transform button = tempButton;
        if (!tempButton.GetComponentInParent<HistoryFieldManagerScript>()) {
            Transform uniqueParent = tempButton;
            string path;
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
            path = tm.GetCurrentSectionKey() + "/" + path;
            startingUID = path;
        } else {
            while (button != null) {
                if (button.name.StartsWith("LabEntry:")) {
                    tempUID = button.name + tempUID;
                }
                if (button.name.EndsWith("Tab")) {
                    tempUID = button.name + "/" + tempUID;
                }
                button = button.parent;
            }
            tempUID = tm.GetCurrentSectionKey() + "/" + tempUID;
            startingUID = tempUID;//.Remove(tempUID.LastIndexOf("/")) + "/" + UIDToRemove;
        }
        Debug.Log("Dialogue UID: " + startingUID);


        //ds.GetDialogues ().Remove (tempUID.Remove (tempUID.LastIndexOf (".")) + UIDToRemove);
        //Debug.Log (string.Join (",", ds.GetDialogues ().Select (x => x.Key).ToArray()));

        return startingUID;
        //return tempUID;
    }

    /**
	 * Returns the unique identifier for a dialogue session based off a given button
	 */
    public static string GetUID(string section, Transform button)
    {
        string tempUID = "";
        string startingUID;

        while (button != null) {
            if (button.name.StartsWith("LabEntry:")) {
                tempUID = "LabEntry: " + button.GetSiblingIndex() + tempUID;
            }
            if (button.name.EndsWith("Tab")) {
                tempUID = button.name + "/" + tempUID;
            }
            button = button.parent;
        }
        tempUID = section + "/" + tempUID;
        startingUID = tempUID;//.Remove(tempUID.LastIndexOf("/")) + "/" + UIDToRemove;

        //ds.GetDialogues ().Remove (tempUID.Remove (tempUID.LastIndexOf (".")) + UIDToRemove);
        //Debug.Log (string.Join (",", ds.GetDialogues ().Select (x => x.Key).ToArray()));

        return startingUID;
        //return tempUID;
    }

    /**
	 * Returns a string of all XML data
	 */
    public string GetData()
    {
        string xml = "";
        xml += "<dialogue>";
        //Add in identifiers for this particular dialogue here


        xml += "<uid>" + GetUID() + "</uid>";

        xml += "<characters>";
        for (int i = 0; i < characterParent.childCount; i++) {
            Transform ch;
            if ((ch = characterParent.GetChild(i)).name.StartsWith("Character")) {
                xml += "<character><name>" + UnityWebRequest.EscapeURL(ch.GetComponentInChildren<TextMeshProUGUI>().text) + "</name><charColor>";
                Color col = ch.Find("CharacterImage").GetComponent<Image>().color;
                string[] colors = new string[3] { col.r + "", col.g + "", col.b + "" };
                xml += string.Join(",", colors) + "</charColor></character>";
            }
        }
        xml += "</characters>";

        xml += baseParent.GetComponent<HistoryFieldManagerScript>().getData();
        /*xml += "<data>";
		for (int i = 0; i < baseParent.childCount; i++) {	
			xml += baseParent.GetChild (i).GetComponent<DialogueEntryScript> ().GetData ();
		}
		xml += "</data>";*/
        xml += "</dialogue>";
        Debug.Log(xml);
        return xml;
    }

    public void AddNewChoice()
    {
        if (adding) {
            throw new System.Exception("Please wait until current dialogue option is added");
        }
        adding = true;
        Transform entryParent = baseParent;
        entryParent.parent.gameObject.SetActive(true);
        GameObject entryObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/DialogueChoiceEntry") as GameObject;

        //NESTED swap the commented lines below to restore nesting
        //entryObj = Instantiate (entryobj, entryParent);
        entryObj = Instantiate(entryObj, baseParent);

        //mostRecentEntry.parentEntry.transform.GetComponentInChildren<Toggle> ().isOn = true;
        Debug.Log("Entries: " + string.Join("----", entries.Select(x => x.characterName + ", " + x.index + ", " + x.transform.GetSiblingIndex()).ToArray()));
        adding = false;
    }

    /**
	 * Adds a new dialogue by a character. Adds as child/sibling to the mostRecentEntry (this will change in the future)
	 */
    public DialogueEntryScript AddNewEntry(TextMeshProUGUI characterName)
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
        GameObject entryObj = Resources.Load(GlobalData.resourcePath + "/Prefabs/DialogueEntry") as GameObject;

        //NESTED swap the commented lines below to restore nesting
        //entryObj = Instantiate (entryobj, entryParent);
        entryObj = Instantiate(entryObj, baseParent);
        DialogueEntryScript entry = entryObj.GetComponent<DialogueEntryScript>();
        entry.characterName = characterName.text;
        entry.index = entries.Count;
        entryObj.GetComponentInChildren<Image>().color = charColors[characterName.text];
        /*
			* Testing
		*/
        entry.charColor = charColors[characterName.text];
        print(entries.Count);
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
        print(entries.Count);
        entries.Add(entry);
        //mostRecentEntry = entry;
    }

    /**
	 * Changes a character's name and updates all references to that name
	 */
    public void ChangeCharacterName(string oldName, string newName)
    {
        //If you want duplicate names possible, then find another identifier for each character button
        if (currentCharacters.Contains(newName)) { //Dont add duplicate names
            return;
        }
        //Update the new name everywhere
        if (charColors.ContainsKey(oldName)) {
            Color c = charColors[oldName];
            charColors.Remove(oldName);
            charColors.Add(newName, c);
        }
        //There's probably a better way to do this, but I think it'll work. Maybe just remove old and add new, but that's boring
        currentCharacters[currentCharacters.FindIndex((string obj) => obj.Equals(oldName))] = newName;
        defaultCharacters[defaultCharacters.FindIndex((string obj) => obj.Equals(oldName))] = newName;

        List<DialogueEntryScript> chars = GetDialoguesOfCharacter(oldName);
        foreach (DialogueEntryScript des in chars) {
            des.characterName = newName;
        }
    }

    /**
	 * Changes the color of a character and updates all references to that color
	 */
    public void ChangeCharacterColor(string charName, Color c)
    {
        //If you want duplicate names possible, then find another identifier for each character button
        if (currentCharacters.Contains(charName)) { //Dont add duplicate names
            return;
        }

        //Update color change in the dictionary
        if (charColors.ContainsKey(charName)) {
            charColors[charName] = c;
        } else {
            charColors.Add(charName, c);
        }

        List<DialogueEntryScript> chars = GetDialoguesOfCharacter(charName);
        foreach (DialogueEntryScript des in chars) {
            des.GetComponentInChildren<Image>().color = charColors[des.characterName];
        }
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
        characterPanel.transform.parent.GetComponent<Button>().interactable = true;
        tempButton = button;

        UID = GetUID();
        DiscardDialogue();

        GetComponentInChildren<HistoryFieldManagerScript>().PopulateQuiz(button);
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
	 * Save the dialogue to DataScript (when apply button is clicked)
	 */
    public void SaveDialogue()
    {
        //Before: Debug.Log (string.Join ("-----", ds.GetDialogues ().Select (x => x.Key + "DATA::::" + x.Value).ToArray()));
        ds.AddDialogue(UID, GetData());
        Transform pin = GetComponentInChildren<HistoryFieldManagerScript>().GetPin().transform;
        if (pin.Find("Item Background Off")) {
            pin.Find("Item Background Off").gameObject.SetActive(false);
            pin.Find("Item Background On").gameObject.SetActive(true);
        }
        //After: Debug.Log (string.Join ("-----", ds.GetDialogues ().Select (x => x.Key + "DATA::::" + x.Value).ToArray()));
    }

    /**
	 * Call this when the Cancel button is clicked.
	 */
    public void CancelClicked()
    {
        if (!ds.GetDialogues().ContainsKey(UID)) { //If the dialogue was never saved, remove the pin itself
            GameObject pin = GetComponentInChildren<HistoryFieldManagerScript>().GetPin();
            if (pin.transform.Find("Item Background Off")) {
                pin.transform.Find("Item Background Off").gameObject.SetActive(true);
                pin.transform.Find("Item Background On").gameObject.SetActive(false);
            } else {
                Destroy(pin);
            }
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
        for (int i = 0; i < characterParent.childCount; i++) {
            if (characterParent.GetChild(i).name.StartsWith("CharacterEntry") && !characterParent.GetChild(i).name.EndsWith("Default")) {
                toBeDestroyed.Add(characterParent.GetChild(i));
            }
        }
        for (int i = 0; i < baseParent.childCount; i++) {
            toBeDestroyed.Add(baseParent.GetChild(i));
        }
        foreach (Transform t in toBeDestroyed) {
            Destroy(t.gameObject);
        }
    }

    /**
	 * When adding a character from the AddCharacterPanel
	 */
    public void AddCharacterFromPanel(Transform panel)
    {
        Dropdown charNames = panel.GetComponentInChildren<Dropdown>();
        Text charName = charNames.captionText;
        Toggle[] colors = panel.GetComponentsInChildren<Toggle>();
        Image img = null;
        foreach (Toggle c in colors) {
            if (c.isOn) {
                img = c.transform.GetComponent<Image>();
                c.gameObject.SetActive(false);
            }
        }

        //Add the character and remove them from the list
        AddCharacter(charName.text, img.color);
        charNames.options.RemoveAt(charNames.value);

        //If you added the last available character, disable the AddCharacterButton
        if (characterPanelDropdown.options.Count == 0) {
            characterPanel.transform.parent.GetComponent<Button>().interactable = false;
            return;
        }

        //Set the default character in the dropdown to the first remaining character in the list
        charNames.value = 0;
        charNames.captionText.text = charNames.options[0].text;
        if (charNames.captionImage != null) {
            charNames.captionImage.sprite = charNames.options[0].image;
        }
    }

    /**
	 * Add a character not from the panel (aka from loading XML)
	 */
    public void AddCharacter(string name, Color img)
    {
        if (!charColors.ContainsKey(name)) {
            charColors.Add(name, img);
            GameObject charEntry = Resources.Load(GlobalData.resourcePath + "/Prefabs/CharacterEntry") as GameObject;
            charEntry = Instantiate(charEntry, characterParent);
            charEntry.transform.SetSiblingIndex(characterParent.childCount - 2);
            charEntry.GetComponentInChildren<TextMeshProUGUI>().text = name;
            charEntry.GetComponentsInChildren<Image>()[1].color = img;
            currentCharacters.Add(charEntry.transform.GetComponentInChildren<TextMeshProUGUI>().text);
            charEntry.name = charEntry.transform.GetComponentInChildren<TextMeshProUGUI>().text + " Panel";
        }
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

    /**
	 * Moves a dialogue from one position to another
	 * pos = index the dialogue is moving to
	 * beforePos = the index the dialogue started at
	 */
    public void MoveChildTo(GameObject dialogue, int pos, int beforePos)
    {
        DialogueEntryScript movingDES = dialogue.GetComponent<DialogueEntryScript>();
        Debug.Log("Index we're moving: " + movingDES.index + ", newpos:" + pos + ", beforepos:" + beforePos);
        Debug.Log("Entries: " + string.Join("----", entries.Select(x => x.characterName + ", idx: " + x.index + ", display: " + x.dialogue).ToArray()));


        int movingUp = 0;
        DialogueEntryScript newParent;
        if (pos == 0) {
            newParent = null;
        } else {
            if (pos <= movingDES.index) {
                movingUp = 1;
            }
            if (!GetDialogueOfIndex(pos - movingUp).characterName.Equals(movingDES.characterName)) { // && GetDialogueOfIndex (pos).children.Count > 0) {
                newParent = GetDialogueOfIndex(pos - movingUp);
                GetDialogueOfIndex(pos - movingUp).transform.GetComponentInChildren<Toggle>().isOn = true;
                //Debug.Log ("Becoming child");
            } else {
                newParent = GetDialogueOfIndex(pos - movingUp).parentEntry;
                //pos = pos + GetFinalChildIndex (movingDES) - movingDES.index;
            }
        }

        //NESTED remove the following line to restore nesting
        newParent = null;

        //newParent = GetDialogueOfIndex (pos).parentEntry;

        DialogueEntryScript oldParent = movingDES.parentEntry;


        List<DialogueEntryScript> rangeToMove = entries.GetRange(beforePos, GetFinalChildIndex(entries[beforePos]) - beforePos + 1);
        //Debug.Log("RangeToMove: " + string.Join("----", rangeToMove.Select(x => x.characterName + ", idx: " + x.index + ", display: " + x.dialogue).ToArray()));
        //Debug.Log ("Pos: " + pos);

        entries.RemoveRange(beforePos, rangeToMove.Count);
        int tempPos = pos;
        if (pos > 0) {
            if (newParent == null) {
                Debug.Log(GetFinalChildIndex(GetDialogueOfIndex(pos - movingUp)));
                Debug.Log(GetDialogueOfIndex(pos - movingUp).index);
                pos += GetFinalChildIndex(GetDialogueOfIndex(pos - movingUp)) - GetDialogueOfIndex(pos - movingUp).index;
            } else if (GetDialogueOfIndex(pos - movingUp).characterName.Equals(movingDES.characterName)) { // && GetDialogueOfIndex (pos).children.Count > 0) {
                pos += GetFinalChildIndex(GetDialogueOfIndex(pos - movingUp)) - GetDialogueOfIndex(pos - movingUp).index;
            }
            if (oldParent != null && oldParent.transform.IsChildOf(GetDialogueOfIndex(tempPos - movingUp).transform)) {
                if (newParent == null || !newParent.transform.IsChildOf(GetDialogueOfIndex(tempPos - movingUp).transform)) {
                    pos--;
                }
            }
        }
        if (pos > movingDES.index) { //If moving down, adjust position to account for dialogue's children
            pos = pos - rangeToMove.Count + 1;
        }

        Debug.Log("Pos: " + pos);
        entries.InsertRange(pos, rangeToMove);
        //entries.Insert (pos, temp);

        int i = 0;
        foreach (DialogueEntryScript des in entries) {
            entries[i].index = i;
            i++;
        }
        Debug.Log("Entries: " + string.Join("----", entries.Select(x => x.characterName + ", idx: " + x.index + ", display: " + x.dialogue).ToArray()));

        movingDES.parentEntry = newParent;
        if (oldParent != null) {
            oldParent.children.Remove(movingDES);
            //oldParent.UpdateChildren ();
        }
        if (newParent != null) {
            dialogue.transform.SetParent(newParent.childrenParent);

            //newParent.UpdateChildren ();
        } else {
            dialogue.transform.SetParent(baseParent);
        }

        //Set sibling transform index accordingly with reference to it's children
        int position = 0;
        for (i = 0; i < dialogue.transform.parent.childCount; i++) {
            Debug.Log(dialogue.transform.parent.GetChild(i).name);

            if (dialogue.transform.parent.GetChild(i).name.Equals("placeholder")) {
                continue;
            }
            if (movingDES.index > dialogue.transform.parent.GetChild(i).GetComponent<DialogueEntryScript>().index) {
                position++;
            }
        }
        if (newParent != null) {
            newParent.children.Insert(position, movingDES);
        }
        dialogue.transform.SetSiblingIndex(position);

        //If the moved dialogue was placed as a sibling above those of another character 
        //As in, character foo's dialogue being placed as a sibling above character bar's dialogue
        //so therefore bar's dialogue should become children of foo's dialogue
        DialogueEntryScript temp = null;
        int siblingPosition = position + 1;
        while (movingDES.transform.parent.childCount > siblingPosition) {
            if (movingDES.transform.parent.GetChild(siblingPosition) != null) {
                temp = movingDES.transform.parent.GetChild(siblingPosition).GetComponent<DialogueEntryScript>();
            }
            if (temp != null && !temp.characterName.Equals(movingDES.characterName)) {
                movingDES.GetComponentInChildren<Toggle>().isOn = true;
                temp.parentEntry = movingDES;
                temp.parentEntry.children.Remove(temp);
                temp.transform.SetParent(movingDES.childrenParent);
                temp.transform.SetAsLastSibling();
                siblingPosition--;
            }
            temp = null;
            siblingPosition++;
        }
        movingDES.UpdateChildren();
    }
}
