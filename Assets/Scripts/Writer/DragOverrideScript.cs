using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragOverrideScript : CEScrollRect
{
    protected override void Start()
    {
        if (GlobalData.GDS != null && GlobalData.GDS.isMobile) {
            inertia = true;
            decelerationRate = .05f;
        }

        base.Start();
    }

    public override void OnBeginDrag(PointerEventData data)
    {
        if (GlobalData.GDS != null && GlobalData.GDS.isMobile && content.rect.height > viewport.rect.height) {
            base.OnBeginDrag(data);
        }
    }

    public override void OnDrag(PointerEventData data)
    {
        if (GlobalData.GDS != null && GlobalData.GDS.isMobile && content.rect.height > viewport.rect.height) {
            base.OnDrag(data);
        }
    }

    public override void OnEndDrag(PointerEventData data)
    {
        if (GlobalData.GDS != null && GlobalData.GDS.isMobile && content.rect.height > viewport.rect.height) {
            base.OnEndDrag(data);
        }
    }
}