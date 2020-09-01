using System;

namespace ClinicalTools.SimEncounters
{
    public class WaitableResult
    {
        public static WaitableResult CompletedResult = new WaitableResult(true);

        public WaitedResult Result { get; protected set; }
        public bool IsCompleted() => Result != null;
        private event Action<WaitedResult> Completed;

        public WaitableResult(bool completed = false)
        {
            if (completed)
                Result = new WaitedResult();
        }
        public WaitableResult(Exception exception) => Result = new WaitedResult(exception);

        public void SetCompleted()
        {
            if (IsCompleted())
                throw new Exception("Result already completed.");

            Result = new WaitedResult();
            Complete();
        }
        public void SetError(Exception exception)
        {
            if (IsCompleted())
                throw new Exception("Result already set.");

            Result = new WaitedResult(exception);
            Complete();
        }

        protected void Complete()
        {
            if (Completed == null)
                return;

            Completed(Result);
            foreach (Delegate d in Completed.GetInvocationList())
                Completed -= (Action<WaitedResult>)d;
        }

        public void AddOnCompletedListener(Action<WaitedResult> action)
        {
            if (IsCompleted())
                action.Invoke(Result);
            else
                Completed += action;
        }
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
            RemoveListeners();
        }

        public void AddOnCompletedListener(Action<WaitedResult<T>> action)
        {
            if (IsCompleted())
                action.Invoke(Result);
            else
                Completed += action;
        }

        public void RemoveListeners()
        {
            if (Completed == null)
                return;

            foreach (Delegate d in Completed.GetInvocationList())
                Completed -= (Action<WaitedResult<T>>)d;
        }

        public void CopyValueWhenCompleted(WaitableResult<T> destination)
            => AddOnCompletedListener((source) => CopyValue(source, destination));

        private void CopyValue(WaitedResult<T> source, WaitableResult<T> destination)
        {
            if (source.IsError())
                destination.SetError(source.Exception);
            else
                destination.SetResult(source.Value);
        }
    }
}