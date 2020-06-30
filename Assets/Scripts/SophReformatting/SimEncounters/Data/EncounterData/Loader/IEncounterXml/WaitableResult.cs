using System;

namespace ClinicalTools.SimEncounters
{
    public class WaitedResult<T>
    {
        public T Value { get; protected set; }
        public Exception Exception { get; }

        public WaitedResult(T value) => Value = value;
        public WaitedResult(Exception exception) => Exception = exception;

        public bool IsError() => Exception != null;
    }

    public class WaitableResult<T>
    {
        public WaitedResult<T> Result { get; protected set; }
        public bool IsCompleted() => Result != null;

        private event Action<WaitedResult<T>> Completed;

        public WaitableResult() { }
        public WaitableResult(T value) => Result = new WaitedResult<T>(value);
        public WaitableResult(Exception exception) => Result = new WaitedResult<T>(exception);

        public void SetResult(T value)
        {
            if (IsCompleted())
                throw new Exception("Result already set.");

            Result = new WaitedResult<T>(value);
            Complete();
        }
        public void SetError(Exception exception)
        {
            if (IsCompleted())
                throw new Exception("Result already set.");

            Result = new WaitedResult<T>(exception);
            Complete();
        }

        protected void Complete()
        {
            if (Completed == null)
                return;

            Completed(Result);
            foreach (Delegate d in Completed.GetInvocationList())
                Completed -= (Action<WaitedResult<T>>)d;
        }

        public void AddOnCompletedListener(Action<WaitedResult<T>> action)
        {
            if (IsCompleted())
                action.Invoke(Result);
            else
                Completed += action;
        }
    }
}