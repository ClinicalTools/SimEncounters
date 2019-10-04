using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputFieldOverrideScript : InputField {

	/**
	 * Script used to override the InputField for autocomplete tags in SaveCaseBG
	 * Uses custom methods to handle selection/deselection/cursor placement accurately
	 */

	public void UpdateCursor() {
		//OnFocus ();
		MoveTextEnd (false);
		this.selectionAnchorPosition = this.selectionFocusPosition = this.caretPosition;
	}

	public override void OnDeselect(BaseEventData eventData) {
		Transform suggestions = transform.Find ("Suggestions");
		/*if (suggestions.childCount > 0) {
			foreach (Transform t in suggestions.GetComponentsInChildren<Transform>()) {
				if (!t.name.Equals (suggestions.name))
					continue;//t.gameObject.SetActive (false);
			}
		}*/
		NextFrame.Function(delegate
		{
			if (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.transform.IsChildOf(gameObject.transform)) {
				m_CaretVisible = false;
				suggestions.gameObject.SetActive(false);
				this.DeactivateInputField();
			} else {
				EventSystem.current.SetSelectedGameObject(gameObject);
            }
		});
	}


}
