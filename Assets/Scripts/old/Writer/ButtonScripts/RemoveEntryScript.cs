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

	}
}
