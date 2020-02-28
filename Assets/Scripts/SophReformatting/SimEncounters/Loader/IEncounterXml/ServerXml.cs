using ClinicalTools.SimEncounters.Data;
using System;
using System.Xml;

namespace ClinicalTools.SimEncounters.Loading
{
    public class ServerEncounter : IEncounterXml
    {
        public event Action<XmlDocument, XmlDocument> Completed;

        protected DownloadEncounter DataDownloader { get; }
        protected DownloadEncounter ImageDownloader { get; }

        public ServerEncounter(DownloadEncounter dataDownloader, DownloadEncounter imageDownloader)
        {
            DataDownloader = dataDownloader;
            ImageDownloader = imageDownloader;
        }

        public void GetEncounterXml(User user, EncounterInfoGroup encounterInfoGroup)
        {
            DataDownloader.Completed += DataDownloader_Completed;
            DataDownloader.GetXml("xmlData");
            ImageDownloader.Completed += ImageDownloader_Completed;
            ImageDownloader.GetXml("imgData");
        }

        protected bool DataDownloaded { get; set; }
        protected bool ImagesDownloaded { get; set; }
        protected XmlDocument DataXml { get; set; }
        protected XmlDocument ImagesXml { get; set; }
        private void DataDownloader_Completed(XmlDocument xmlDocument)
        {
            DataXml = xmlDocument;
            DataDownloaded = true;
            CheckIfCompleted();
        }

        private void ImageDownloader_Completed(XmlDocument xmlDocument)
        {
            ImagesXml = xmlDocument;
            ImagesDownloaded = true;
            CheckIfCompleted();
        }

        protected virtual void CheckIfCompleted()
        {
            if (DataDownloaded && ImagesDownloaded)
                Completed?.Invoke(DataXml, ImagesXml);
        }
    }
}