using UnityEngine;

public class ControlDropdownBorders : MonoBehaviour
{
    [SerializeField] private RectTransform controlledRect = null;
    private CanvasGroup controlledGroup;
    [SerializeField] private GameObject[] hide = new GameObject[0];

    public CanvasGroup ControlledGroup
    {
        get {
            if (!controlledGroup) controlledGroup = controlledRect.GetComponent<CanvasGroup>();
            return controlledGroup;
        }
    }

    private RectTransform rectTrans;
    public RectTransform RectTrans
    {
        get {
            if (!rectTrans) rectTrans = (RectTransform)transform;
            return rectTrans;
        }
    }

    private RectTransform parentTrans;
    public RectTransform ParentTrans
    {
        get {
            if (!parentTrans) parentTrans = (RectTransform)controlledRect.parent;
            return parentTrans;
        }
    }

    private CanvasGroup group;
    public CanvasGroup Group
    {
        get {
            if (!group) group = GetComponent<CanvasGroup>();
            return group;
        }
    }

    private void OnRectTransformDimensionsChange()
    {
        controlledRect.pivot = RectTrans.pivot;
        controlledRect.anchorMax = RectTrans.anchorMax;
        controlledRect.anchorMin = RectTrans.anchorMin;

        var height = Mathf.Min(RectTrans.rect.height, ParentTrans.rect.height);
        controlledRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

        var alpha = (controlledRect.rect.height < 10) ? 0 : 1;
        Group.alpha = alpha;
        ControlledGroup.alpha = alpha;
    }

    private void Update()
    {
        var shouldHide = Group.alpha >= .5f;
        for (int i = 0; i < hide.Length; i++)
            hide[i].SetActive(shouldHide);
    }
}
