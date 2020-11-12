﻿using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFieldScrollOver : TMP_InputField
{
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

        if (scrollView != null)
            scrollView.OnScroll(eventData);
    }

    public override void OnSelect(BaseEventData eventData)
    {
        richText = false;
        base.OnSelect(eventData);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        richText = true;
        base.OnDeselect(eventData);
    }
}
