using System;
using System.Xml;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class DownloadEncounter
    {
        public IWebAddress WebAddress { get; }

        public event Action<XmlDocument> Completed;

        private const string downloadPhp = "Test.php";
        private const string filenameArgument = "webfilename";
        protected virtual string GetWebAddress(User user, EncounterInfo encounterInfo, string column)
        {
            WebAddress.AddArgument(filenameArgument, $"{encounterInfo.MetaGroup.Filename}.ced");
            WebAddress.AddArgument("webusername", "clinical");
            WebAddress.AddArgument("webpassword", "encounters");
            WebAddress.AddArgument("mode", "download");
            WebAddress.AddArgument("column", column);
            WebAddress.AddArgument("accountId", encounterInfo.MetaGroup.AuthorAccountId.ToString());
            return WebAddress.GetUrl(downloadPhp);
        }

        public DownloadEncounter(IWebAddress webAddress) : base()
        {
            WebAddress = webAddress;
        }

        public void GetXml(User user, EncounterInfo encounterInfo, string column)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(GetWebAddress(user, encounterInfo, column));
            var requestOperation = webRequest.SendWebRequest();
            requestOperation.completed += (asyncOperation) => ProcessWebrequest(webRequest);
        }

        protected void ProcessWebrequest(UnityWebRequest webRequest)
        {
            var text = GetWebRequestText(webRequest);
            webRequest.Dispose();

            var Results = ReadServerXml(text);
            Completed?.Invoke(Results);
        }

        protected string GetWebRequestText(UnityWebRequest webRequest)
        {
            if (!webRequest.isDone)
                return null;
            else if (webRequest.isNetworkError || webRequest.isHttpError)
                return null;
            else if (!webRequest.downloadHandler.isDone)
                return null;
            else
                return webRequest.downloadHandler.text;
        }

        protected XmlDocument ReadServerXml(string text)
        {
            text = text.Replace("â€‹", "");
            UnityEngine.Debug.Log(text);
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(text);
            return xmlDocument;
        }
    }
}