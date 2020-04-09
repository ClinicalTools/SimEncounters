using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public enum ServerOutcome
    {
        Success, WebRequestNotDone, NetworkError, HttpError, DownloadNotDone, ParsingError
    }
    public class ServerResult<T> : EventArgs
    {
        public string Message { get; }
        public ServerOutcome Outcome { get; }
        public T Result { get; }

        /// <summary>
        /// Creates failed reader results.
        /// </summary>
        /// <param name="message">Error message</param>
        public ServerResult(ServerOutcome outcome, string message)
        {
            Message = message;
            Outcome = outcome;
        }
        /// <summary>
        /// Creates successful reader results.
        /// </summary>
        /// <param name="result">Resulting value</param>
        public ServerResult(T result)
        {
            Result = result;
            Outcome = ServerOutcome.Success;
        }
    }
    public class ServerResult2 : EventArgs
    {
        public string Message { get; }
        public ServerOutcome Outcome { get; }

        public ServerResult2(ServerOutcome outcome, string message)
        {
            Message = message;
            Outcome = outcome;
        }
    }
}