using System;

namespace ClinicalTools.SimEncounters
{
    public class CompletableReaderPanelBehaviour : ReaderPanelBehaviour, IPanelCompletedHandler
    {
        public bool IsCompleted { get; protected set; } = false;
        public virtual event Action Completed;
        public void SetCompleted()
        {
            IsCompleted = true;
            Completed?.Invoke();
        }
    }
}