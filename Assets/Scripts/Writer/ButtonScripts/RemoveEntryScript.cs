using SimEncounters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoveEntryScript : MonoBehaviour {

	public bool bypassConfirmation; //Used to bypass the confirmation screen and directly remove something

	/**
	 * Called by the buttons that are trying to remove the obj. Opens the confirmation panel if not bypassing it
	 */
	public void RemoveEntry(GameObject obj) {
		if (bypassConfirmation) {
			ApprovedRemove (obj);
			return;
		}

		GameObject panel = Instantiate(Resources.Load("Writer/Prefabs/Panels/ConfirmActionBG"),GameObject.Find("GaudyBG").transform) as GameObject;
		panel.name = "ConfirmActionBG";
		ConfirmationScript cs = panel.GetComponent<ConfirmationScript> ();

		cs.obj = obj;
		cs.actionText.text = "Are you sure you want to remove this entry?";
		cs.MethodToConfirm = ApprovedRemove;
	}

	/**
	 * Removes the specified object
	 */
	public void ApprovedRemove(GameObject obj) {
		HistoryFieldManagerScript hm = null;
		Transform t = transform;
		while (t != null && t.parent.GetComponent<HistoryFieldManagerScript> () == null) {
			t = t.parent;
		}
		if (t == null) {
			return;
		} else {
			hm = t.parent.GetComponent<HistoryFieldManagerScript> ();
		}

		hm.RemovePanelEntry (obj);
	}

	/**
	 * Called by the buttons that are trying to remove the obj. Opens the confirmation panel if not bypassing it
	 * Can also pass in the HistoryFieldManagerScript that uses the pin
	 */
	public void RemovePin(GameObject pin) {
		if (pin.GetComponent<HistoryFieldManagerScript> ()) {
            pin = pin.GetComponent<HistoryFieldManagerScript> ().GetPin();
		}
		if (bypassConfirmation) {
			ApprovedRemove (pin);
			return;
		}
		GameObject panel = Instantiate(Resources.Load("Writer/Prefabs/Panels/ConfirmActionBG"),GameObject.Find("GaudyBG").transform) as GameObject;
		panel.name = "ConfirmActionBG";
		ConfirmationScript cs = panel.GetComponent<ConfirmationScript> ();

		cs.obj = pin;
		cs.actionText.text = "Are you sure you want to remove this pin?";
        cs.MethodToConfirm = ApprovedPinRemove;
        //ApprovedPinRemove(pin);
	}

	public void ApprovedPinRemove(GameObject pin) {
		//Dropdown pDrop = pin.transform.parent.parent.Find ("AddPinButton").GetComponentInChildren<Dropdown> ();
		//pDrop.AddOptions (new List<Dropdown.OptionData>(){new Dropdown.OptionData ("Quiz", pin.transform.Find ("ItemIcon").GetComponent<Image> ().sprite)});
		//pDrop.GetComponent<PinTabScript>().myOptions.Add (pin.name.Remove (pin.name.Length - "pin".Length));

		WriterHandler ds = WriterHandler.WriterInstance;
		string uniquePath = "";
        Debug.Log(pin.transform.name);
		Transform tempPin = pin.transform;

		if (!tempPin.GetComponentInParent<HistoryFieldManagerScript> ()) {
			Transform uniqueParent = tempPin;
			uniquePath = "";
			while (uniqueParent.parent != null && !uniqueParent.parent.name.Equals ("Content")) {
				if (uniqueParent.parent.GetComponent<HistoryFieldManagerScript> () != null) {
					break;
				}
				uniqueParent = uniqueParent.parent;
			}
			uniquePath = uniqueParent.name;

			while (!uniqueParent.name.Equals (ds.GetComponent<TabManager>().getCurrentTab() + "Tab")) {
				uniqueParent = uniqueParent.parent;
				if (uniqueParent == null) {
					break;
				}
				uniquePath = uniqueParent.name + "/" + uniquePath;
			}
			uniquePath = ds.GetComponent<TabManager>().getCurrentSection () + "/" + uniquePath;
		} else {
			while (tempPin != null) {
				if (tempPin.name.StartsWith ("LabEntry:")) {
					//uniquePath = "LabEntry: " + tempPin.GetSiblingIndex() + uniquePath;
					uniquePath = tempPin.name + uniquePath;
				}
				if (tempPin.name.EndsWith ("Tab")) {
					uniquePath = tempPin.name + "/" + uniquePath;
				}
				tempPin = tempPin.parent;
			}
			uniquePath = ds.GetComponent<TabManager>().getCurrentSection () + "/" + uniquePath;
		}
		Debug.Log ("Removing: " + uniquePath);
        switch (pin.transform.name)
        {
			case "DialoguePin":
				ds.GetDialogues ().Remove (uniquePath);
				break;
			case "QuizPin":
				Dictionary<string, string> quizes = ds.GetQuizes ();
				string quizData = "";
				if (quizes.ContainsKey (uniquePath)) {
					quizData = quizes [uniquePath];
				} else {
					break;
				}
				ds.GetQuizes ().Remove (uniquePath);
				List<string> imagesToRemove = new List<string> ();
				foreach (string key in ds.EncounterData.Images.Keys) {
					if (quizData.Contains ("<Image>" + key + "</Image>")) {
						Debug.Log ("Removing image: " + key);
						imagesToRemove.Add (key);
					}
				}

				foreach (string s in imagesToRemove) {
					quizes.Remove (s);
				}
				break;
			case "FlagPin":
				ds.GetFlags ().Remove (uniquePath);
				break;
			case "EventPin":
	            // Wipe event input
				break;
        }

		if (pin.transform.Find("Item Background Off")) {
			pin.transform.Find("Item Background Off").gameObject.SetActive(true);
			pin.transform.Find("Item Background On").gameObject.SetActive(false);
		} else {
			Destroy(pin.gameObject);
		}
	}
}
