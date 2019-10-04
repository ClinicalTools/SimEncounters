using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptButtonFixScript : MonoBehaviour {

	private bool isSection;

	/**
	 * This is used to fix the LayoutGroup messing up when changing the size of a section or tab
	 * Call FixTab() after doing anything to change the width of a tab/section
	 */

	// Use this for initialization
	void Start () {
		//InvokeRepeating ("Reenable", 0.02f, 5.0f);

        if (this.transform.parent.name == "SectionButtonContent")
        {
            this.GetComponent<HorizontalLayoutGroup>().childControlWidth = false;
            this.GetComponent<HorizontalLayoutGroup>().childControlWidth = true;
        }

        else if (this.transform.parent.name == "TabButtonContent")
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.parent.GetComponent<RectTransform>());
        }
    }

	/**
	 * Fixes the issue with LayoutGroups not resizing correctly after adjusting the size of a tab or section
	 */
	public void FixTab() {
		Start ();
    }

    private void Reenable() {
		GetComponent<HorizontalLayoutGroup> ().enabled = false;
		GetComponent<HorizontalLayoutGroup> ().enabled = true;
		CancelInvoke ();
	}

	public void FixPlaceholder() {
		isSection = transform.name.EndsWith ("SectionButton");
		LayoutRebuilder.ForceRebuildLayoutImmediate (transform.parent.GetComponent<RectTransform> ());
		transform.parent.GetComponent<HorizontalLayoutGroup> ().childControlWidth = !isSection;
		InvokeRepeating ("AdjustWidth", 0.02f, 5.0f);
	}

	private void AdjustWidth() {

		transform.parent.GetComponent<HorizontalLayoutGroup> ().childControlWidth = isSection;
		LayoutRebuilder.ForceRebuildLayoutImmediate (transform.parent.GetComponent<RectTransform> ());
		CancelInvoke ();
	}
}
