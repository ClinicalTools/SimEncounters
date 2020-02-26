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

    protected virtual bool CanDrag()
    {
        return false;
        //return GlobalData.GDS != null && GlobalData.GDS.isMobile && content.rect.height > viewport.rect.height;
    }

    public override void OnBeginDrag(PointerEventData data)
    {
        if (CanDrag()) {
            base.OnBeginDrag(data);
        }
    }

    public override void OnDrag(PointerEventData data)
    {
        if (CanDrag()) {
            base.OnDrag(data);
        }
    }

    public override void OnEndDrag(PointerEventData data)
    {
        if (CanDrag()) {
            base.OnEndDrag(data);
        }
    }
}