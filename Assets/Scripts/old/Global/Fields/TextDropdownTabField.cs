using UnityEngine;

public class TextDropdownTabField : TabField
{
    private GameObject suggestions;
    private RectTransform content;

    protected override void Awake()
    {
        suggestions = transform.Find("Suggestions")?.gameObject;
        content = (RectTransform)transform.Find("Suggestions/Viewport/Content");

        base.Awake();
    }

    protected override void Update()
    {
        if (suggestions && suggestions.activeSelf && content.rect.height > 0)
            WasSelected = false;

        base.Update();

        if (suggestions && suggestions.activeSelf && content.rect.height > 0)
            WasSelected = false;
    }
}
