using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class DialogueEntryScript : MonoBehaviour
{

    public string dialogue;
    public string characterName;
    public Color charColor;
    public Transform childrenParent;
    public DialogueEntryScript parentEntry;
    public List<DialogueEntryScript> children;
    public int index;
    public List<Image> borderImages;
    private int minSpacerWidth = 200;

    public void UpdateDialogue(TMP_InputField t)
    {
        dialogue = t.text;
    }

    void Awake()
    {
        childrenParent = transform.Find("ChildrenDialogue/Entries");
        if (transform.Find("Spacer")) {
            transform.Find("Spacer").GetComponent<LayoutElement>().minWidth = minSpacerWidth;
        }
    }

    // Use this for initialization
    void Start()
    {
        if (!transform.parent.name.Equals("Entries")) {
            parentEntry = null;
        } else {
            parentEntry = transform.parent.GetComponentInParent<DialogueEntryScript>();
        }
        children = new List<DialogueEntryScript>();
        if (parentEntry != null) {
            parentEntry.GetComponentInChildren<Toggle>().isOn = true;
            parentEntry.children.Add(this);
            parentEntry.UpdateChildren();
        }
    }

    /**
	 * Refreshes/updates the children. Used to update children list of this dialogue, not childrens' indexes.
	 */
    public void UpdateChildren()
    {
        children = new List<DialogueEntryScript>();
        for (int i = 0; i < childrenParent.childCount; i++) {
            DialogueDraggingScript childDrag = childrenParent.GetChild(i).GetComponentInChildren<DialogueDraggingScript>();
            if (childDrag != null) {
                children.Add(childDrag.GetComponentInParent<DialogueEntryScript>());
            }
        }
    }

    /**
	 * Removes a child from the list of children (does not delete the game object)
	 */
    public void RemoveChild(DialogueEntryScript des)
    {
        children.Remove(des);
    }

    public void SetWriterColor(string characterName)
    {
        DialogueManagerScript manager = GetComponentInParent<DialogueManagerScript>();
        Transform name;
        print(characterName);
        for (int i = 0; i < manager.characterParent.childCount; i++) {
            if ((name = manager.characterParent.GetChild(i).Find("CharacterNameValue")) && name.GetComponent<TextMeshProUGUI>().text.Equals(characterName)) {
                charColor = manager.characterParent.GetChild(i).Find("CharacterImage").GetComponent<Image>().color;
                GetComponentInChildren<Image>().color = charColor;
                print(charColor.ToString());
                break;
            }
        }
    }

    public void SetReaderColor(Color c)
    {
        charColor = c;
        borderImages[0].color = charColor;
        //print(charColor.ToString());
    }

    /**
	 * Returns the dialogue's XML data
	 */
    public string GetData()
    {
        string xml = "";
        //xml += "<dialogueEntry>";
        xml += "<characterName>" + UnityWebRequest.EscapeURL(characterName) + "</characterName>";
        xml += "<charColor>" + charColor + "</charColor>";
        xml += "<dialogueText>" + UnityWebRequest.EscapeURL(dialogue) + "</dialogueText>";
        /*xml += "<children>";
		foreach (DialogueEntryScript d in children) {
			xml += d.GetData ();
		}
		xml += "</children>";*/
        //xml += "</dialogueEntry>";
        return xml;
    }
}
