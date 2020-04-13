using ClinicalTools.SimEncounters.Data;
using System;

namespace ClinicalTools.SimEncounters
{
    public enum ServerOutcome
    {
        Success, WebRequestNotDone, NetworkError, HttpError, DownloadNotDone, ParsingError
    }
    public class ServerResult : EventArgs
    {
        public string Message { get; }
        public ServerOutcome Outcome { get; }

        public ServerResult(ServerOutcome outcome, string message)
        {
            Message = message;
            Outcome = outcome;
        }
    }
}