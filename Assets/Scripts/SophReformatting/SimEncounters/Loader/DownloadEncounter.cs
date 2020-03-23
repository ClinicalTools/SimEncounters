using System;
using System.Xml;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public enum XmlType
    {
        Data, Image
    }
    public class DemoEncounter : IDownloadEncounter
    {
        protected IFilePathManager FilePathManager { get; }

        public event Action<XmlDocument> Completed;

        public DemoEncounter(IFilePathManager filePathManager) : base()
        {
            FilePathManager = filePathManager;
        }

        public void GetXml(User user, EncounterInfo encounterInfo, XmlType type)
        {
            var filePath = GetFilePath(user, encounterInfo, type);
            UnityEngine.Debug.LogError("sophSomething: " + filePath);
            UnityWebRequest webRequest = UnityWebRequest.Get(filePath);
            var requestOperation = webRequest.SendWebRequest();
            requestOperation.completed += (asyncOperation) => ProcessWebrequest(webRequest);
        }

        private string GetFilePath(User user, EncounterInfo encounterInfo, XmlType type)
        {
            var filePath = FilePathManager.EncounterFilePath(user, encounterInfo);
            if (type == XmlType.Data)
                return FilePathManager.DataFilePath(filePath);
            else
                return FilePathManager.ImageFilePath(filePath);
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
    public class DownloadEncounter : IDownloadEncounter
    {
        protected IWebAddress WebAddress { get; }

        public event Action<XmlDocument> Completed;

        private const string downloadPhp = "Test.php";
        private const string filenameArgument = "webfilename";
        protected virtual string GetWebAddress(User user, EncounterInfo encounterInfo, XmlType type)
        {
            WebAddress.AddArgument(filenameArgument, $"{encounterInfo.MetaGroup.Filename}.ced");
            WebAddress.AddArgument("webusername", "clinical");
            WebAddress.AddArgument("webpassword", "encounters");
            WebAddress.AddArgument("mode", "download");
            WebAddress.AddArgument("column", GetColumn(type));
            WebAddress.AddArgument("accountId", encounterInfo.MetaGroup.AuthorAccountId.ToString());
            return WebAddress.GetUrl(downloadPhp);
        }

        private string GetColumn(XmlType type)
        {
            if (type == XmlType.Data)
                return "xmlData";
            else
                return "imgData";
        }

        public DownloadEncounter(IWebAddress webAddress) : base()
        {
            WebAddress = webAddress;
        }

        public void GetXml(User user, EncounterInfo encounterInfo, XmlType type)
        {
            var webAddress = GetWebAddress(user, encounterInfo, type);
            UnityWebRequest webRequest = UnityWebRequest.Get(webAddress);
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