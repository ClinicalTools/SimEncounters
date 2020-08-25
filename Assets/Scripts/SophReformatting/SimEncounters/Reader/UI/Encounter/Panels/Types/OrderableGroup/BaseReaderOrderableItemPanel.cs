
using ClinicalTools.UI;
using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseReaderOrderableItemPanel : BaseReaderPanel, IDraggable
    {
        public UserPanel CurrentPanel { get; protected set; }
        public abstract RectTransform RectTransform { get; }
        public abstract UnityEngine.UI.LayoutElement LayoutElement { get; }

        public Layout.ILayoutElement LayoutElement2 => null;

        public abstract event Action<IDraggable, Vector3> DragStarted;
        public abstract event Action<IDraggable, Vector3> DragEnded;
        public abstract event Action<IDraggable, Vector3> Dragging;

        public abstract void Drag(Vector3 mousePosition);

        public abstract void EndDrag(Vector3 mousePosition);

        public abstract void SetColor(Color color);

        public abstract void StartDrag(Vector3 mousePosition);
    }
}