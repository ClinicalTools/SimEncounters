using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveDialogueScript : MonoBehaviour {
    private GameObject BG;

    private void Start()
    {
        BG = GameObject.Find("GaudyBG");
    }

    public bool bypassConfirmation; //Used to bypass the confirmation screen and directly remove something

	/**
	 * Called by the buttons that are trying to remove the obj. Opens the confirmation panel if not bypassing it
	 */
	public void RemoveEntry(GameObject obj) {
		Debug.Log ("OPOPOP");
        string panelName = "ConfirmActionBG";
        GameObject pinPanelPrefab = Instantiate(Resources.Load("Writer/Prefabs/Panels/" + panelName)) as GameObject;
        pinPanelPrefab.transform.SetParent(BG.transform, false);
        ConfirmationScript cs = pinPanelPrefab.GetComponent<ConfirmationScript>(); //GameObject.Find ("GaudyBG").transform.Find ("ConfirmActionBG").GetComponent<ConfirmationScript> ();
		cs.gameObject.SetActive (true);
		cs.obj = obj;
		cs.actionText.text = "Are you sure you want to remove this dialogue?";
		cs.MethodToConfirm = ApprovedRemove;
	}

	/**
	 * Removes the specified object
	 */
	public void ApprovedRemove(GameObject obj) {
		DialogueManagerScript dms = null;
		Transform t = transform;
		while (t != null && t.parent.GetComponent<DialogueManagerScript> () == null) {
			t = t.parent;
		}
		if (t == null) {
			return;
		} else {
			dms = t.parent.GetComponent<DialogueManagerScript> ();
		}

		dms.RemoveDialogue (obj);
	}
}
