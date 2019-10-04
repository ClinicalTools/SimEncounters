using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SectionEditorScript : MonoBehaviour {

	GameObject BG;
	EditSectionScript ess;
	TMP_InputField titleValue;

	/**
	 * Script for buttons to use/call when editing a section
	 */

	// Use this for initialization
	void Start () {
		BG = GameObject.Find ("GaudyBG");
		ess = BG.GetComponent<EditSectionScript> ();
		if (transform.Find ("SectionEditorPanel/Content/Row0/TMPInputField/TitleValue")) {
			titleValue = transform.Find ("SectionEditorPanel/Content/Row0/TMPInputField/TitleValue").GetComponent<TMP_InputField> ();
			string currentSection = BG.GetComponent<TabManager> ().getCurrentSection ();
			titleValue.text = currentSection.Replace("_"," ").Remove(currentSection.Length - 7);
		}
	}

	public void OpenTabEditor(TextMeshProUGUI t) {
		ess.OpenSectionPanel (t);
	}

    public void disableSectionSelection()
    {
        Destroy(this.gameObject);
    }

    public void Submit() {
		ess.SubmitChanges ();
	}

	public void Remove() {
		ess.removeSection ();
	}

	public void ChangeColor(int i) {
		if (ess == null) {
			Start ();
		}
		ess.EditColor (i);
	}

	public void UpdateSlidersFromHex(TMP_InputField input)
	{
		ess.UpdateColorSlidersFromHex(input);
	}

	public void NewColorSelected() {
		if (ess == null) {
			Start ();
		}
		ess.UpdateColorChoice ();
	}
}
