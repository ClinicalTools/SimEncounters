using System;

namespace ClinicalTools.SimEncounters
{
    public class WaitedResult
    {
        public Exception Exception { get; }
        public bool IsError() => Exception != null;
        public WaitedResult() { }
        public WaitedResult(Exception exception) => Exception = exception;
    }
    public class WaitedResult<T>
    {
        private readonly T value = default;
        public T Value {
            get {
                if (Exception != null)
                    throw Exception;
                return value;
            }
        }
        public Exception Exception { get; }

        public WaitedResult(T value) => this.value = value;
        public WaitedResult(Exception exception) => Exception = exception;

        public bool IsError() => Exception != null;
        public bool HasValue() => Exception == null && Value != null;
    }
}