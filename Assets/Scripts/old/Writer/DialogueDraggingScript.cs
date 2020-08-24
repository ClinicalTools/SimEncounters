using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Linq;
using TMPro;

public class DialogueDraggingScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

	public Sprite cursorPicture;
	public Sprite movePicture;
	private DialogueManagerScript manager;
	private Image cursor;
	private bool hover;
	private bool drag;
	private int transformIndex;
	private GameObject placeholder;
	private RectTransform scrollRectTransform;
	private DragOverrideScript scrollScrollRect;
	private RectTransform rt;
	private float dMPos;
	private List<DialogueEntryScript> entries;
	private DialogueEntryScript dialogue;
	private bool isCharacterButton;

	/**
	 * To reenable branching, do a Ctrl+F for //DISABLE BRANCHING
	 * In the DialogueEntry prefab, reenable the ReorderBar children and the Parent Dialogue's ReorderBar
	 */

	void Awake() {
		hover = false;
		cursor = GameObject.Find("CursorContainer").transform.Find("Cursor").GetComponent<Image> ();
	}


	// Use this for initialization
	void Start () {
		if (transform.name.EndsWith ("Default") || transform.name.StartsWith("Character")) {
			isCharacterButton = true;
		}
		if (manager == null) {
			if (isCharacterButton) {
				manager = transform.parent.parent.GetComponentInChildren<DialogueManagerScript> ();
			} else {
				manager = transform.GetComponentInParent<DialogueManagerScript> ();
			}
		}
		scrollRectTransform = manager.transform.parent.GetComponent<RectTransform>();
		scrollScrollRect = scrollRectTransform.transform.GetComponent<DragOverrideScript> ();
		dialogue = transform.GetComponentInParent<DialogueEntryScript> ();
		rt = dialogue.transform.GetComponent<RectTransform>();
		dMPos = 0;
		//entries = new List<DialogueEntryScript> ();
		entries = manager.entries;
		UpdateEntryList ();
        drag = false;
	}

	/**
	 * Returns the manager if anything else needed it for some reason
	 */
	public DialogueManagerScript GetManager() {
		return manager;
	}

	/**
	 * Changes the cursor to the moveable icon
	 */
	public void OnPointerEnter(PointerEventData data) {
		//DISABLE BRANCHING
		if (isCharacterButton && manager.mostRecentEntry != null && manager.mostRecentEntry.characterName.Equals (transform.Find ("CharacterNameValue").GetComponent<TextMeshProUGUI> ().text)) {
			return;
		}
		if (!drag) {
			cursor.sprite = movePicture;
		}
		hover = true;
	}

	/**
	 * Changes the cursor icon back to normal
	 */
	public void OnPointerExit(PointerEventData data) {
        if (!drag)
        {
            cursor.sprite = cursorPicture;
            //dialogue.transform.GetComponent<LayoutElement> ().ignoreLayout = false;
        }

		hover = false;

	}

	/**
	 * If clicked
	 */
	public void OnPointerDown(PointerEventData data) {
		if (data.button.Equals (PointerEventData.InputButton.Right) || data.button.Equals (PointerEventData.InputButton.Middle)) {
			return;
		}
		if (hover) {
			drag = true;
			if (isCharacterButton) {
				dMPos = Input.mousePosition.y - rt.position.y;
				dialogue = (Instantiate (transform.gameObject, manager.baseParent) as GameObject).GetComponent<DialogueEntryScript> ();
				rt = dialogue.transform.GetComponent<RectTransform>();
				dialogue.parentEntry = null;
				dialogue.characterName = transform.Find ("CharacterNameValue").GetComponent<TextMeshProUGUI> ().text;
				dialogue.index = manager.entries.Count;
				transformIndex = dialogue.transform.GetSiblingIndex ();
				UpdateEntryList ();
				if (!dialogue.transform.GetComponent<LayoutElement> ()) {
					dialogue.gameObject.AddComponent<LayoutElement> ();
				}
				if (!dialogue.transform.GetComponent<CanvasGroup> ()) {
					dialogue.gameObject.AddComponent<CanvasGroup> ();
				}
				dialogue.transform.GetComponent<CanvasGroup> ().alpha = 1.0f;
			} else {
				transformIndex = dialogue.transform.GetSiblingIndex ();
                Debug.Log(transformIndex);
				dialogue.transform.GetComponent<CanvasGroup> ().alpha = .5f;
				dMPos = Input.mousePosition.y - rt.position.y;
			}
			//Comment this line out to grab the objects by their center.


			//Spawn placeholder
			placeholder = Instantiate (Resources.Load (GlobalData.resourcePath + "/Prefabs/Placeholder") as GameObject, dialogue.transform.parent);
			placeholder.transform.SetSiblingIndex (dialogue.transform.GetSiblingIndex ());
			placeholder.name = "placeholder";

			dialogue.transform.SetParent (manager.baseParent);
			dialogue.transform.SetAsLastSibling ();
			dialogue.transform.GetComponent<LayoutElement> ().ignoreLayout = true;
		}
	}

	/**
	 * Updates the list of entries the dialogue uses for position calculation
	 */
	public void UpdateEntryList() {
		entries = manager.entries;
	}

	/**
	 * Get's the lowest index of any children the placeholder is a sibling to
	 * Used for adjusting the local transform sibling index
	 */
	private int GetNewPlaceholderPos() { 
		DialogueEntryScript[] siblingDialogues = placeholder.transform.parent.GetComponentsInChildren<DialogueEntryScript> ();
		//Debug.Log ("Dialogue index: " + dialogue.index + ", siblings: " + string.Join ("::", siblingDialogues.Select (x => x.dialogue + "," + x.index).ToArray()));

		int isChild = 0;
		if (placeholder.transform.parent != manager.baseParent && placeholder.transform.parent != dialogue.transform.parent && placeholder.GetComponentInParent<DialogueEntryScript>().index > dialogue.index) {
			isChild = 1;
		}
		if (siblingDialogues.Length == 0) {
			Debug.Log ("THIS SHOULD NOT HAPPEN");
			return placeholder.transform.parent.GetComponent<DialogueEntryScript> ().index;
		}
		int min = siblingDialogues [0].index;
		foreach (DialogueEntryScript des in siblingDialogues) {
			if (des.index < min) {
				min = des.index;
			}
		}
		return min - isChild;



		//Not needed code but i'm keeping it here just in case
		/*
		if (placeholder.transform.parent.GetComponentInParent<DialogueEntryScript> () == null) { //No DES to take up the first array spot
			if (dialogue.transform.parent == placeholder.transform.parent && dialogue.index < siblingDialogues[0].index) {
				return dialogue.index;
			}
			return siblingDialogues [0].index + isChild;
		}
			
		if (false && siblingDialogues.Length > 1) {
			
			if (siblingDialogues [1].transform == placeholder.transform.parent) {// && siblingDialogues[1].transform != dialogue.transform) {
				if (dialogue.transform.parent == placeholder.transform.parent && dialogue.index < siblingDialogues[1].index) {
					return dialogue.index; //-isChild;
				}
				return siblingDialogues [1].index + isChild;
			}

		}
		return siblingDialogues [0].index - isChild;
		*/
	}

	/**
	 * Runs every frame to determine position of dragged dialogue and updating placeholder position
	 */
	void Update() {
		if (drag) {
			if (rt) {
               rt.position = new Vector3 (Input.mousePosition.x, Input.mousePosition.y - dMPos, 0f);
            }
			//Count the number of entries this dialogue is below

			rt.transform.GetComponent<LayoutElement> ().preferredWidth = placeholder.transform.GetComponent<RectTransform> ().rect.width;
			float vPos = 0f;
		    vPos = transform.position.y;

			int pos = 0;

			foreach (DialogueEntryScript entry in entries) {
				if (!entry.transform.Equals (dialogue) && entry.transform.Find ("ParentDialogue").position.y > vPos) {
					if (!entry.transform.IsChildOf (dialogue.transform))
						pos++;
				}
			}

			Debug.Log ("Pos:" + pos + ", Dialogue idx: " + dialogue.index);
			DialogueEntryScript desBelow;

			//If it's the only child, disable the parent's dropdown while moving
			if (dialogue.parentEntry != null && dialogue.parentEntry.children.Count == 1) {
				//dialogue.transform.SetParent (dialogue.parentEntry.transform.parent);
				dialogue.transform.SetParent (manager.baseParent);
				dialogue.transform.SetAsLastSibling ();
				dialogue.parentEntry.transform.GetComponentInChildren<Toggle> ().isOn = false;
			}

			//If the entry is at the bottom of the list
			if (pos == entries.Count - manager.GetFinalChildIndex(dialogue)+dialogue.index-1) {
				Debug.Log("Setting as last child");

				//adjust pos for the number of children.
				if (pos > dialogue.index) {
					pos += manager.GetFinalChildIndex (dialogue) - dialogue.index;
				}
				desBelow = manager.GetDialogueOfIndex (pos);
				if (desBelow.characterName.Equals (dialogue.characterName)) {
					placeholder.transform.SetParent (desBelow.transform.parent);
					if (desBelow != dialogue) {
						desBelow.transform.GetComponentInChildren<Toggle> ().isOn = false;
					}
				} else {
					placeholder.transform.SetParent (desBelow.transform.parent);
				}

				placeholder.transform.SetAsLastSibling ();
				if (placeholder.transform.parent == dialogue.transform.parent) {
					placeholder.transform.SetSiblingIndex (placeholder.transform.GetSiblingIndex () - 1);
				}
				//placeholder.transform.SetSiblingIndex (pos - GetNewPlaceholderPos ());
			} else { //entry within list
				
				//If you're moving it downward, adjust pos for the number of children.
				if (pos > dialogue.index) {
					pos += manager.GetFinalChildIndex (dialogue) - dialogue.index;
				}
				int movingUp = 0;
				//Assign the desBelow accurately
				if (pos >= dialogue.index) {//&& dialogue.parentEntry != manager.GetDialogueOfIndex (pos + 1).parentEntry) {
					desBelow = manager.GetDialogueOfIndex (pos + 1);
				} else {
					desBelow = manager.GetDialogueOfIndex (pos);
					movingUp = 1;
				}
				Debug.Log (pos);
				int tempPos = pos;
				Debug.Log("POS: " + tempPos + ", " + movingUp);

				/*
				List<DialogueEntryScript> siblings = entries.FindAll ((DialogueEntryScript obj) => obj.transform.parent == placeholder.transform.parent);
				int accountForChildren = 0;
				foreach (DialogueEntryScript Des in siblings) {
					if (Des.index < pos) {
						int finalChildIndex;
						if (pos > (finalChildIndex = manager.GetFinalChildIndex (Des))) { //make sure pos is actually above children too
							accountForChildren += (finalChildIndex - Des.index);
							if (dialogue.parentEntry != null && dialogue.parentEntry.transform.IsChildOf (Des.transform)) {
								accountForChildren--;
							}
						}
					}
				}
				pos -= accountForChildren;*/

				//Debug.Log ("desbelow: " + desBelow.dialogue + ",Index: " + desBelow.index);

				//Check if the parent of desBelow is of the same character as dialogue. If so, set dialogue as sibling to parent
				if (tempPos - movingUp == dialogue.index) {
					Debug.Log ("Option 0");
					if (dialogue.parentEntry == null) {
						placeholder.transform.SetParent (manager.baseParent);
					} else if (dialogue.parentEntry.children.Count > 1) {
						placeholder.transform.SetParent (dialogue.parentEntry.childrenParent);
					} else {
						placeholder.transform.SetParent (dialogue.parentEntry.transform.parent);
					}
                    
				} else if (tempPos > 0 && !manager.GetDialogueOfIndex (tempPos - movingUp).characterName.Equals (dialogue.characterName)) {
					Debug.Log ("Option 1");
					int isChildOf = 0;
					if (dialogue.parentEntry == manager.GetDialogueOfIndex (tempPos - movingUp)) {
						isChildOf = 1;
					}
					if (manager.GetDialogueOfIndex (tempPos - movingUp).children.Count >= 1 + isChildOf) {
						placeholder.transform.SetParent (manager.GetDialogueOfIndex (tempPos - movingUp).childrenParent);
					} else {
						placeholder.transform.SetParent (manager.GetDialogueOfIndex (tempPos - movingUp).transform.parent);
					}
				} else if (desBelow.parentEntry != null && desBelow.parentEntry.characterName.Equals(dialogue.characterName) && tempPos == desBelow.parentEntry.index + movingUp) {
					Debug.Log ("Option 2");
					placeholder.transform.SetParent (desBelow.parentEntry.transform.parent);
				} else {
					Debug.Log ("Option 3");
					placeholder.transform.SetParent (desBelow.transform.parent);
				}

				int localPosition = 0;
				Debug.Log ("movingUP: " + movingUp);
				for (int i = 0; i < placeholder.transform.parent.childCount; i++) {
					Transform child = placeholder.transform.parent.GetChild (i);
					if (child.name.Equals ("placeholder") || child == dialogue.transform) {
						continue;
					}
					if (pos - movingUp >= child.GetComponent<DialogueEntryScript>().index) {
						localPosition++;
					}
				}
				Debug.Log (localPosition);
				placeholder.transform.SetSiblingIndex (localPosition);
				/*
				int index = manager.GetDialogueOfIndex (pos).transform.GetSiblingIndex ();
				if (placeholder.transform.IsChildOf (manager.GetDialogueOfIndex (pos).transform.parent) && placeholder.transform.GetSiblingIndex () <= index) {
					pos = index;
					pos--;
				} else {
					pos = index;
				}*/
				//Debug.Log ("POS: " + pos + ",NEW placeholder POS: " + GetNewPlaceholderPos ());

				//Set local transform index
				//placeholder.transform.SetSiblingIndex (pos - GetNewPlaceholderPos ());
			}


			//Used for scrolling. Hold dragable near top/bottom to scroll
			Vector3[] corners = new Vector3[4];
			scrollRectTransform.GetWorldCorners (corners);
			if (vPos > corners[1].y - 40 && scrollScrollRect.verticalNormalizedPosition < 1.0f) { //Scrolling up
				scrollScrollRect.verticalNormalizedPosition += .02f;
			} else if (vPos < corners[0].y + 40 && scrollScrollRect.verticalNormalizedPosition > 0.0f) { //Scrolling down
				scrollScrollRect.verticalNormalizedPosition -= .02f;
			}

			//When let go
			if (!Input.GetMouseButton(0)) { //&& false) {
				drag = false;
                
				if (isCharacterButton) {
					if (pos > 0) {
						manager.mostRecentEntry = manager.GetDialogueOfIndex (pos - 1);
					}
					Destroy (dialogue.gameObject);
					/*try {
						dialogue = manager.AddNewEntry (transform.Find ("CharacterNameValue").GetComponent<TextMeshProUGUI> ());
					} catch (Exception e) {
						Debug.Log (e.Message);
					}*/
					scrollScrollRect.verticalNormalizedPosition = 0.0f;
					dialogue = manager.AddNewEntry (transform.Find ("CharacterNameValue").GetComponent<TextMeshProUGUI> ());
					scrollScrollRect.verticalNormalizedPosition = 0.0f;

					//DISABLE BRANCHING
					hover = false;

				}
                
				dialogue.transform.GetComponent<CanvasGroup> ().alpha = 1.0f;
				dialogue.transform.GetComponent<LayoutElement> ().ignoreLayout = false;

				pos = 0;
				//if (isCharacterButton) { //Use the mouse position, not the dialogue entry position, for comparisons
					//foreach (DialogueEntryScript entry in entries) {
						//if (!entry.transform.Equals (dialogue) && entry.transform.Find ("ParentDialogue").position.y > Input.mousePosition.y) {
							//if (!entry.transform.IsChildOf (dialogue.transform))
								//pos++;
						//}
					//}
				//} else { //Use the dialogue entry position for comparisons
					foreach (DialogueEntryScript entry in entries) {
						if (!entry.transform.Equals (dialogue) && entry.transform.Find ("ParentDialogue").position.y > transform.position.y) {
							if (!entry.transform.IsChildOf (dialogue.transform))
								pos++;
						}
					}
				//}
				if (pos > dialogue.index) {
					pos += manager.GetFinalChildIndex (dialogue) - dialogue.index;
				}
				Destroy(placeholder);
				placeholder = null;

				manager.MoveChildTo (dialogue.transform.gameObject, pos, dialogue.index);

				if (!EventSystem.current.IsPointerOverGameObject ()) {
					//OnPointerExit (null);
				}
				if (!hover) {
					cursor.sprite = cursorPicture;
				}
				dialogue = transform.GetComponentInParent<DialogueEntryScript> ();
                rt = dialogue.gameObject.GetComponent<RectTransform> ();
            }
		}
	}
}