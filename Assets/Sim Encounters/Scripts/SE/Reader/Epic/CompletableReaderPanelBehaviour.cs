using ClinicalTools.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public abstract class CompletableReaderPanelBehaviour : BaseReaderPanelBehaviour, IPanelCompletedHandler
    {
        public bool IsCompleted { get; protected set; } = false;
        public virtual event Action Completed;
        public virtual void SetCompleted()
        {
            CurrentPanel.SetChildPanelsRead(true);
            IsCompleted = true;
            Completed?.Invoke();
        }
    }
    public abstract class ReaderOptionPanelBehaviour : BaseReaderPanelBehaviour
    {
        public virtual void GetFeedback()
        {
            CurrentPanel.SetChildPanelsRead(true);
        }
    }
    public abstract class ReaderExclusiveOptionPanelBehaviour : ReaderOptionPanelBehaviour
    {
        public abstract void SetToggleGroup(ToggleGroup toggleGroup);
        public abstract void SetFeedbackParent(Transform feedbackParent);
    }

    public delegate void ReaderPanelsReordered(object sender, ReaderPanelsReorderedEventArgs e);
    public class ReaderPanelsReorderedEventArgs : EventArgs
    {
        public int LastPosition { get; }
        public int NewPosition { get; }

        public ReaderPanelsReorderedEventArgs(int lastPosition, int newPosition)
        {
            LastPosition = lastPosition;
            NewPosition = newPosition;
        }
    }
    public abstract class ReaderOrderablePanelBehaviour : BaseReaderPanelBehaviour, IDraggable
    {
        public abstract RectTransform RectTransform { get; }
        public abstract LayoutElement LayoutElement { get; }

        public abstract event ReaderPanelsReordered ReaderPanelsReordered;
        public abstract event Action<IDraggable, Vector3> DragStarted;
        public abstract event Action<IDraggable, Vector3> DragEnded;
        public abstract event Action<IDraggable, Vector3> Dragging;

        public abstract void Drag(Vector3 mousePosition);
        public abstract void EndDrag(Vector3 mousePosition);
        public abstract void StartDrag(Vector3 mousePosition);

        public abstract void SetColor(Color color);
    }
}