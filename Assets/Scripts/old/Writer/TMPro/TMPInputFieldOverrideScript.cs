using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TMPInputFieldOverrideScript : TMP_InputField {
    // TextMeshPro glitches for fields added with data filled in them
    // Enabling and disabling the text component of it or a parent of it fixes the issue
    protected override void Awake()
    {
        base.Awake();

        textComponent.enabled = false;
    }

    protected override void Start()
    {
        base.Start();

        textComponent.enabled = true;
    }

    // Allows the scroll view to scroll, even if this object is selected
    public override void OnScroll(PointerEventData eventData)
    {
        var scrollView = GetComponentInParent<ScrollRect>();

        if (scrollView != null && !transform.Find("Suggestions").gameObject.activeInHierarchy)
            scrollView.OnScroll(eventData);
    }

    /**
	 * Script used to override the InputField for autocomplete tags in SaveCaseBG
	 * Uses custom methods to handle selection/deselection/cursor placement accurately
	 */

    public void UpdateCursor()
    {
        //OnFocus ();
        MoveTextEnd(false);
        selectionAnchorPosition = selectionFocusPosition = caretPosition;
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        return;
        Transform suggestions = transform.Find("Suggestions");
        Transform closeSuggestions = transform.Find("CloseSuggestions");
        /*if (suggestions.childCount > 0) {
			foreach (Transform t in suggestions.GetComponentsInChildren<Transform>()) {
Search





Avatar image

				if (!t.name.Equals (suggestions.name))
					continue;//t.gameObject.SetActive (false);
			}
		}*/
        //image.sprite = spriteState.disabledSprite;

        NextFrame.Function(delegate {
            if (EventSystem.current.currentSelectedGameObject == null || !EventSystem.current.currentSelectedGameObject.transform.IsChildOf(gameObject.transform)) {
                m_CaretVisible = false;
                suggestions.gameObject.SetActive(false);
                closeSuggestions?.gameObject.SetActive(false);
                GetComponentInParent<AutofillTMP>().ResetSelected();
                DeactivateInputField();
            } else {
                EventSystem.current.SetSelectedGameObject(gameObject);
            }
        });

        base.OnDeselect(eventData);
    }
}
