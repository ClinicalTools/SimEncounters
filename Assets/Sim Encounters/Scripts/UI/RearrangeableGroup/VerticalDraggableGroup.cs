using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public class VerticalDraggableGroup : DraggableGroup
    {
        protected override void DragStarted(IDraggable draggable, Vector3 mousePosition)
        {
            base.DragStarted(draggable, mousePosition);

            (Placeholder.GetComponent<LayoutElement>()).minHeight = draggable.RectTransform.sizeDelta.y;
        }

        protected override void SetPosition(IDraggable draggable, Vector3 mousePosition)
        {
            var position = draggable.RectTransform.position;

            var worldCorners = new Vector3[4];
            ChildrenParent.GetWorldCorners(worldCorners);
            position.y = Mathf.Clamp(mousePosition.y, worldCorners[0].y, worldCorners[1].y);

            draggable.RectTransform.position = position;
        }

        protected override bool BeforeNeighbor(IDraggable neighbor, Vector3 mousePosition)
            => DistanceFromMouse(neighbor.RectTransform, mousePosition) - Offset > 0;
        protected override bool AfterNeighbor(IDraggable neighbor, Vector3 mousePosition)
            => DistanceFromMouse(neighbor.RectTransform, mousePosition) - Offset < 0;
        protected override float DistanceFromMouse(RectTransform rectTransform, Vector3 mousePosition)
        {
            var worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);
            return mousePosition.y - worldCorners[0].y;
        }
    }
}