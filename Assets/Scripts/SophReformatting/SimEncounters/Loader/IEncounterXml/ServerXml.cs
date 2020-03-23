using System.Xml;

namespace ClinicalTools.SimEncounters.Loading
{
    public class ServerXml : IEncounterXmlReader
    {
        public event EncounterXmlRetrievedEventHandler Completed;

        protected IDownloadEncounter DataDownloader { get; }
        protected IDownloadEncounter ImageDownloader { get; }

        public ServerXml(IDownloadEncounter dataDownloader, IDownloadEncounter imageDownloader)
        {
            DataDownloader = dataDownloader;
            ImageDownloader = imageDownloader;
        }

        public void GetEncounterXml(User user, EncounterInfo encounterInfo)
        {
            DataDownloader.Completed += DataDownloader_Completed;
            DataDownloader.GetXml(user, encounterInfo, XmlType.Data);
            ImageDownloader.Completed += ImageDownloader_Completed;
            ImageDownloader.GetXml(user, encounterInfo, XmlType.Image);
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
                Completed?.Invoke(this, new EncounterXmlRetrievedEventArgs(DataXml, ImagesXml));
        }
    }
}