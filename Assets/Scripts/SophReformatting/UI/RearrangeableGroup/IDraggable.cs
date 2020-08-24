using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.UI
{
    public interface IDraggable
    {
        RectTransform RectTransform { get; }
        LayoutElement LayoutElement { get; }
        Layout.ILayoutElement LayoutElement2 { get; }

        event Action<IDraggable, Vector3> DragStarted;
        event Action<IDraggable, Vector3> DragEnded;
        event Action<IDraggable, Vector3> Dragging;

        void StartDrag(Vector3 mousePosition);
        void EndDrag(Vector3 mousePosition);
        void Drag(Vector3 mousePosition);
    }
    public interface IHasDraggable
    {
        IDraggable Draggable { get; }
    }

    public abstract class BaseDraggable : MonoBehaviour, IDraggable
    {
        public string DisplayName { get => displayName; set => displayName = value; }
        [SerializeField] private string displayName;

        public RectTransform RectTransform => (RectTransform)transform;
        public abstract LayoutElement LayoutElement { get; }

        public virtual Layout.ILayoutElement LayoutElement2 => null;

        public abstract event Action<IDraggable, Vector3> DragStarted;
        public abstract event Action<IDraggable, Vector3> DragEnded;
        public abstract event Action<IDraggable, Vector3> Dragging;

        public abstract void Drag(Vector3 mousePosition);
        public abstract void EndDrag(Vector3 mousePosition);
        public abstract void StartDrag(Vector3 mousePosition);
    }
}