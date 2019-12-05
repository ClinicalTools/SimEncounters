using System;

namespace ClinicalTools.SimEncounters
{
    /// <summary>
    /// Allows a function that returns a value to be ran as async and provide access to its progress.
    /// </summary>
    /// <typeparam name="T">Type of the result from the async operation.</typeparam>
    public abstract class SimAsyncOperation<T> : IDisposable
    {
        public delegate void ProgressEventHandler(float progress, string message);
        public delegate void CompletedEventHandler(T result);

        public virtual event ProgressEventHandler ProgressChanged;
        public virtual event CompletedEventHandler Completed;

        protected string ProgressMessage { get; set; }
        public virtual float Progress { get; protected set; }
        public virtual bool IsDone { get; protected set; }

        public virtual T Result { get; protected set; }

        public SimAsyncOperation() { }
        
        public abstract void Dispose();

        protected virtual void UpdateProgress(float progress, string message = null)
        {
            Progress = progress;
            if (message != null)
                ProgressMessage = message;

            ProgressChanged?.Invoke(Progress, ProgressMessage);
        }

        protected virtual void Done(T result)
        {
            Result = result;
            IsDone = true;
            Completed?.Invoke(Result);
        }
    }
}