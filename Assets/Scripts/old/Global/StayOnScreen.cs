using UnityEngine;

public class StayOnScreen : MonoBehaviour {
    [SerializeField] private RectTransform anchor;
    [SerializeField] private float xPadding = 0;
    [SerializeField] private float yPadding = 0;
    [SerializeField] private float xOffset = 0;
    [SerializeField] private float yOffset = 0;
    private RectTransform rectTransform;
    private Vector3 defaultPos;
    private RectTransform parent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parent = transform.parent.GetComponent<RectTransform>();
        defaultPos = rectTransform.localPosition;

        if (!anchor) {
            anchor = (RectTransform)transform.parent.Find("CursorContainer/Cursor");
        }
    }

    private void Update()
    {
        rectTransform.position = new Vector2(anchor.position.x + xOffset, anchor.position.y + yOffset);
        var width = rectTransform.rect.width;
        var height = rectTransform.rect.height;

        var halfParentWidth = parent.rect.width / 2 - xPadding;
        if (rectTransform.localPosition.x < -halfParentWidth) {
            rectTransform.localPosition = new Vector2(-halfParentWidth, rectTransform.localPosition.y);
        } else if (rectTransform.localPosition.x > halfParentWidth - width) {
            rectTransform.localPosition = new Vector2(halfParentWidth - width, rectTransform.localPosition.y);
        }

        var halfParentHeight = parent.rect.height / 2 - yPadding;
        if (rectTransform.localPosition.y < height - halfParentHeight) {
            rectTransform.localPosition = new Vector2(rectTransform.localPosition.x, height - halfParentHeight);
        } else if (rectTransform.localPosition.y > halfParentHeight) {
            rectTransform.localPosition = new Vector2(rectTransform.localPosition.x, halfParentHeight);
        }
    }
}
