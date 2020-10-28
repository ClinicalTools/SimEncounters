using System;

namespace ClinicalTools.SimEncounters
{
    public class CompletableReaderPanelBehaviour : ReaderPanelBehaviour, IPanelCompletedHandler
    {
        public bool IsCompleted { get; protected set; } = false;
        public virtual event Action Completed;
        public virtual void SetCompleted()
        {
            IsCompleted = true;
            Completed?.Invoke();

            CurrentPanel.SetChildPanelsRead(true);
        }
    }
}