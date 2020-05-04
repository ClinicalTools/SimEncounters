using ClinicalTools.SimEncounters.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseReaderOrderableItemPanel : BaseReaderPanel, IDraggable
    {
        public UserPanel CurrentPanel { get; protected set; }
        public abstract RectTransform RectTransform { get; }
        public abstract LayoutElement LayoutElement { get; }

        public abstract event Action<IDraggable, Vector3> DragStarted;
        public abstract event Action<IDraggable, Vector3> DragEnded;
        public abstract event Action<IDraggable, Vector3> Dragging;

        public abstract void Drag(Vector3 mousePosition);

        public abstract void EndDrag(Vector3 mousePosition);

        public abstract void SetColor(Color color);

        public abstract void StartDrag(Vector3 mousePosition);
    }
}