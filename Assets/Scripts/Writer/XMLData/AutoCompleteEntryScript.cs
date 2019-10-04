using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AutoCompleteEntryScript : MonoBehaviour {

	private SaveCaseScript parent;
	public int pos;
	public bool selected;

	/**
	 * This script is used to hold data for the auto complete entries currently used when applying tags when saving
	 */

	// Use this for initialization
	void Start () {
		parent = transform.parent.GetComponentInParent<SaveCaseScript> ();
		Color c = transform.GetComponent<Image> ().color;
		c.r = 1f;
		transform.GetComponent<Image> ().color = c;
	}

	/**
	 * Initiates the autocomplete suggestion with it's position and if it's selected or not
	 */
	public void Initiate(int pos, bool select) {
		this.pos = pos;
		this.selected = select;
	}

	/**
	 * Updates the image of the suggestion. Will be teal/blue if it's selected and white if it's not
	 */
	public void UpdateImage() {
		if (selected) {
			Color c = transform.GetComponent<Image> ().color;
			c.r = .8f;
			transform.GetComponent<Image> ().color = c;
		} else {
			Color c = transform.GetComponent<Image> ().color;
			c.r = 1f;
			transform.GetComponent<Image> ().color = c;
		}
	}

	/**
	 * Called when the suggestion is chosen/accepted
	 */
	public void SubmitChoiceToParent() {
        if (transform.GetComponentInParent<InputFieldOverrideScript>())
        {
            InputFieldOverrideScript parentInput = transform.GetComponentInParent<InputFieldOverrideScript>();

            parent.AddTag(transform.Find("Text").GetComponent<Text>().text);
            //parent.AddTag(transform.GetComponent<Dropdown>().captionText.text);
            parentInput.Select();
            parentInput.OnSelect(null);
            parentInput.UpdateCursor();

        } else if (transform.GetComponentInParent<TMPInputFieldOverrideScript>()) {
			TMPInputFieldOverrideScript parentInput = transform.GetComponentInParent<TMPInputFieldOverrideScript>();

			transform.parent.GetComponentInParent<AutofillTMP>().AddTag(transform.Find("Text").GetComponent<TextMeshProUGUI>().text);
			//parent.AddTag(transform.GetComponent<Dropdown>().captionText.text);
			parentInput.Select();
			parentInput.OnSelect(null);
			parentInput.UpdateCursor();

		} else
        {
            Transform tfPath = transform;

            string path = transform.name;
            while (transform.parent != null)
            {
                tfPath = tfPath.parent;
                path = tfPath.name + "/" + path;
            }
            Debug.Log("There is no overridescript for: " + path);
        }        
	}
}
