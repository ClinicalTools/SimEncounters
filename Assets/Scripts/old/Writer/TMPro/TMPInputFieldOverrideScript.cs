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
    }
}
