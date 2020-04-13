using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerReader : IServerReader
    {
        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public WaitableResult<ServerResult> Begin(UnityWebRequest webRequest)
        {
            var result = new WaitableResult<ServerResult>();
            var requestOperation = webRequest.SendWebRequest();
            requestOperation.completed += (asyncOperation) => ProcessWebrequest(webRequest, result);
            return result;
        }

        protected void ProcessWebrequest(UnityWebRequest webRequest, WaitableResult<ServerResult> result)
        {
            var serverResult = GetResults(webRequest);
            webRequest.Dispose();

            result.SetResult(serverResult);
        }

        protected ServerResult GetResults(UnityWebRequest webRequest)
        {
            if (!webRequest.isDone)
                return new ServerResult(ServerOutcome.DownloadNotDone, webRequest.error);
            else if (webRequest.isNetworkError)
                return new ServerResult(ServerOutcome.NetworkError, webRequest.error);
            else if (webRequest.isHttpError)
                return new ServerResult(ServerOutcome.HttpError, webRequest.error);
            else if (!webRequest.downloadHandler.isDone)
                return new ServerResult(ServerOutcome.DownloadNotDone, webRequest.error);

            var text = webRequest.downloadHandler.text;
            text = text.Replace("â€™", "'");
            text = text.Replace("â€‹", "");

            return new ServerResult(ServerOutcome.Success, text);
        }
    }
}