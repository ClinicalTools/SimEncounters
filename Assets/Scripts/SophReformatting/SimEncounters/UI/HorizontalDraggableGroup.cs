using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class HorizontalDraggableGroup : DraggableGroup
    {
        protected override void DragStarted(IDraggable draggable, Vector3 mousePosition)
        {
            base.DragStarted(draggable, mousePosition);

            (Placeholder.GetComponent<Layout.LayoutElement>()).WidthValues.Min = draggable.RectTransform.sizeDelta.x;
        }

        protected override void SetPosition(IDraggable draggable, Vector3 mousePosition)
        {
            var position = draggable.RectTransform.position;

            var worldCorners = new Vector3[4];
            ChildrenParent.GetWorldCorners(worldCorners);
            position.x = Mathf.Clamp(mousePosition.x, worldCorners[0].x, worldCorners[2].x) - Offset;

            draggable.RectTransform.position = position;
        }

        protected override bool BeforeNeighbor(IDraggable neighbor, Vector3 mousePosition)
            => DistanceFromMouse(neighbor.RectTransform, mousePosition) - Offset < 0;
        protected override bool AfterNeighbor(IDraggable neighbor, Vector3 mousePosition)
            => DistanceFromMouse(neighbor.RectTransform, mousePosition) - Offset > 0;
        protected override float DistanceFromMouse(RectTransform rectTransform, Vector3 mousePosition)
        {
            var worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners); 
            return mousePosition.x - worldCorners[0].x;
        }
    }
}