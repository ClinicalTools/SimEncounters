using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class DraggableGroupUI : MonoBehaviour
    {
        [SerializeField] private GameObject placeholder;
        public GameObject Placeholder { get => placeholder; set => placeholder = value; }

        [SerializeField] private RectTransform childrenParent;
        public virtual RectTransform ChildrenParent { get => childrenParent; set => childrenParent = value; }


        public event Action<List<IDraggable>> OrderChanged;
        protected List<IDraggable> DraggableObjects { get; } = new List<IDraggable>();

        public virtual void Add(IDraggable draggable)
        {
            DraggableObjects.Add(draggable);
            draggable.DragStarted += DragStarted;
            draggable.DragEnded += DragEnded;
            draggable.Dragging += Dragging;
        }

        protected virtual float Offset { get; set; }
        protected virtual int InitialIndex { get; set; }
        protected virtual int Index { get; set; }

        public void DragStarted(IDraggable draggable, Vector3 mousePosition)
        {
            Index = DraggableObjects.IndexOf(draggable);
            InitialIndex = Index;
            DraggableObjects.RemoveAt(Index);

            draggable.LayoutElement.ignoreLayout = true;
            draggable.LayoutElement.layoutPriority = 10000;

            Placeholder.SetActive(true);
            SetPlaceholderIndex(draggable);
            draggable.RectTransform.SetSiblingIndex(ChildrenParent.childCount - 1);

            Offset = DistanceFromMouse(draggable.RectTransform, mousePosition);
        }

        protected virtual float DistanceFromMouse(RectTransform rectTransform, Vector3 mousePosition)
        {
            var worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);
            return mousePosition.y - worldCorners[0].y;
        }

        protected virtual void SetPlaceholderIndex(IDraggable draggable)
            => Placeholder.transform.SetSiblingIndex(draggable.RectTransform.GetSiblingIndex());

        protected virtual void Dragging(IDraggable draggable, Vector3 mousePosition)
        {
            SetPosition(draggable, mousePosition);

            if (Index > 0) {
                var upperNeighbor = DraggableObjects[Index - 1];
                if (AboveUpperNeighbor(upperNeighbor, mousePosition)) {
                    SetPlaceholderIndex(upperNeighbor);
                    Index--;
                    return;
                }
            }
            if (Index < DraggableObjects.Count) {
                var lowerNeighbor = DraggableObjects[Index];
                if (BelowLowerNeighbor(lowerNeighbor, mousePosition)) {
                    SetPlaceholderIndex(lowerNeighbor);
                    Index++;
                }
            }
        }

        private void SetPosition(IDraggable draggable, Vector3 mousePosition)
        {
            var position = draggable.RectTransform.position;

            var worldCorners = new Vector3[4];
            ChildrenParent.GetWorldCorners(worldCorners);
            position.y = Mathf.Clamp(mousePosition.y, worldCorners[0].y, worldCorners[1].y);

            draggable.RectTransform.position = position;
        }

        protected virtual bool AboveUpperNeighbor(IDraggable neighbor, Vector3 mousePosition)
            => DistanceFromMouse(neighbor.RectTransform, mousePosition) - Offset > 0;

        protected virtual bool BelowLowerNeighbor(IDraggable neighbor, Vector3 mousePosition)
            => DistanceFromMouse(neighbor.RectTransform, mousePosition) - Offset < 0;

        protected virtual void DragEnded(IDraggable draggable, Vector3 mousePosition)
        {
            draggable.RectTransform.SetSiblingIndex(Placeholder.transform.GetSiblingIndex());
            Placeholder.SetActive(false);
            draggable.LayoutElement.ignoreLayout = false;
            draggable.LayoutElement.layoutPriority = 1;
            DraggableObjects.Insert(Index, draggable);
            if (InitialIndex != Index)
                OrderChanged?.Invoke(DraggableObjects);
        }
    }
}