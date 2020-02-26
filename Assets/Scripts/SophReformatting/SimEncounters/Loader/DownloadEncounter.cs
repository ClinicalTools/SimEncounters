using System.Threading.Tasks;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class DownloadEncounter : SimAsyncOperation<XmlDocument>
    {
        private readonly string address = "";

        UnityWebRequest webRequest;
        public DownloadEncounter(string fileName, string column) : base()
        {
            //fileName = fileName.Replace(" ", "_");
            // Phenomenal php file name
            // TODO: rename PHP file
            //string serverURL = GlobalData.serverAddress + "Test.php";
            //string serverURL = "Test.php";
            // This is literally (at the time of writing this) a public file on github and has a username and password in plaintext very cool
            // TODO: change login and don't make login details publicly available
            //string urlParams = "?webfilename=" + fileName + "&webusername=clinical&webpassword=encounters&mode=download";
            //address = serverURL + urlParams + "&column=" + column + "&accountId=" + GlobalData.caseObj.accountId;
        }

        public async Task<XmlDocument> Run()
        {
            UpdateProgress(0, "Downloading the case.");
            webRequest = await GetStartedWebRequest(address);

            //Show progress in the slider
            while (!webRequest.isDone) {
                if (Progress != webRequest.downloadProgress)
                    UpdateProgress(webRequest.downloadProgress, "Downloading the case.");

                await Task.Delay(25);
            }

            string text = webRequest.downloadHandler.text;
            webRequest.Dispose();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(text);
            Done(xmlDoc);

            return xmlDoc;
        }

        protected async virtual Task<UnityWebRequest> GetStartedWebRequest(string address)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get("");

            var send = webRequest.SendWebRequest();
            while (!send.isDone)
                await Task.Delay(25);

            if (webRequest.error != null) {
                Debug.LogError("Error: " + webRequest.error);

                // TODO: add max attempts
                if (webRequest.error.Equals("Error: Cannot connect to destination host")) {
                    webRequest?.Dispose();
                    return await GetStartedWebRequest(address);
                }
            }

            return webRequest;
        }


        public override void Dispose()
        {
            webRequest?.Dispose();
        }
    }
}