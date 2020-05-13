using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class DraggableGroupUI : MonoBehaviour
    {
        public GameObject Placeholder { get => placeholder; set => placeholder = value; }
        [SerializeField] private GameObject placeholder;
        public virtual RectTransform ChildrenParent => (RectTransform)transform;

        public event Action<List<IDraggable>> OrderChanged;
        protected List<IDraggable> DraggableObjects { get; } = new List<IDraggable>();

        public virtual T Add2<T>(T draggablePrefab)
            where T : MonoBehaviour, IDraggable
        {
            var draggableElement = Instantiate(draggablePrefab, ChildrenParent);
            Add(draggableElement);
            return draggableElement;
        }

        public virtual void Add(IDraggable draggable)
        {
            DraggableObjects.Add(draggable);
            draggable.DragStarted += DragStarted;
            draggable.DragEnded += DragEnded;
            draggable.Dragging += Dragging;

            // would be optimal if this was only done at the end of adding a number of items
            // moving it to a start method and doing it here if not started would minimize the issue
            // checking for it at the beginning of update could also work, but I generally prefer to minimize work in Update
            Placeholder.transform.SetAsLastSibling();
        }

        public virtual void Remove(IDraggable draggable) => DraggableObjects.Remove(draggable);


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
            draggable.RectTransform.SetAsLastSibling();

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

            Placeholder.transform.SetAsLastSibling();
        }
    }
}