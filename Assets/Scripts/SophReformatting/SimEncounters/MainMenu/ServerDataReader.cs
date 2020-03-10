using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ServerDataReader<T>
    {
        public delegate void CompletedEventHandler(object sender, ServerResult<T> e);
        public event CompletedEventHandler Completed;
        public ServerResult<T> Result { get; protected set; }
        public bool IsDone { get; protected set; }

        public IParser<T> Parser { get; }
        public ServerDataReader(IParser<T> parser)
        {
            Parser = parser;
        }

        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public void Begin(UnityWebRequest webRequest)
        {
            var requestOperation = webRequest.SendWebRequest();
            requestOperation.completed += (asyncOperation) => ProcessWebrequest(webRequest);
        }

        protected void ProcessWebrequest(UnityWebRequest webRequest)
        {
            var text = GetResults(webRequest);
            webRequest.Dispose();

            Result = text;
            IsDone = true;
            Completed?.Invoke(this, Result);
        }

        protected ServerResult<T> GetResults(UnityWebRequest webRequest)
        {
            if (!webRequest.isDone)
                return new ServerResult<T>(ServerOutcome.DownloadNotDone, webRequest.error);
            else if (webRequest.isNetworkError)
                return new ServerResult<T>(ServerOutcome.NetworkError, webRequest.error);
            else if (webRequest.isHttpError)
                return new ServerResult<T>(ServerOutcome.HttpError, webRequest.error);
            else if (!webRequest.downloadHandler.isDone)
                return new ServerResult<T>(ServerOutcome.DownloadNotDone, webRequest.error);

            var text = webRequest.downloadHandler.text;
            text = text.Replace("â€™", "'");
            var results = Parser.Parse(text);
            Debug.LogError(text);
            if (results == null)
                return new ServerResult<T>(ServerOutcome.ParsingError, webRequest.downloadHandler.text);

            return new ServerResult<T>(results);
        }
    }
}