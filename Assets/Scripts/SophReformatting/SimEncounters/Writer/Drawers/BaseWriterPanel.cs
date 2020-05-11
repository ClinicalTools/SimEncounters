using ClinicalTools.SimEncounters.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseAddableWriterPanel : BaseWriterPanel, IDraggable
    {
        public RectTransform RectTransform => (RectTransform)transform;
        public abstract LayoutElement LayoutElement { get; }

        public abstract event Action<IDraggable, Vector3> DragStarted;
        public abstract event Action<IDraggable, Vector3> DragEnded;
        public abstract event Action<IDraggable, Vector3> Dragging;

        public abstract void Drag(Vector3 mousePosition);
        public abstract void EndDrag(Vector3 mousePosition);
        public abstract void StartDrag(Vector3 mousePosition);
    }

    public abstract class BaseWriterPanel : MonoBehaviour
    {
        public virtual string Type { get => type; set => type = value; }
        [SerializeField] private string type;

        public abstract void Display(Encounter encounter);
        public abstract void Display(Encounter encounter, Panel panel);

        public abstract Panel Serialize();
    }
}