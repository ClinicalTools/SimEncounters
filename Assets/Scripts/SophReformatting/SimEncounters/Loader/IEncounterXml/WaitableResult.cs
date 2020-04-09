using System;

namespace ClinicalTools.SimEncounters
{
    public class WaitableResult<T>
    {
        public T Result { get; protected set; }
        public bool IsCompleted { get; protected set; }
        public bool IsError { get; protected set; }
        public string Message { get; protected set; }

        private Action<T> Completed;

        public WaitableResult() { }

        public WaitableResult(T result, string message = null, bool isError = false)
        {
            Result = result;
            IsCompleted = true;
            IsError = isError;
            Message = message;
        }

        public void SetResult(T result, string message = null)
        {
            if (IsCompleted)
                throw new Exception("Result already set.");

            Message = message;
            Result = result;
            IsCompleted = true;
            Completed?.Invoke(Result);
        }
        public void SetError(string message = null)
        {
            if (IsCompleted)
                throw new Exception("Result already set.");

            Message = message;
            IsCompleted = true;
            IsError = true;
            Completed?.Invoke(Result);
        }

        public void AddOnCompletedListener(Action<T> action)
        {
            if (IsCompleted)
                action.Invoke(Result);
            else
                Completed += action;
        }
    }
}