using System.Threading.Tasks;
using System.Xml;

namespace ClinicalTools.SimEncounters.Loading
{
    public class ServerEncounter : IEncounterXml
    {
        public Task<XmlDocument> DataXml { get; }
        public Task<XmlDocument> ImagesXml { get; }
        protected virtual ReadEncounterXml ReadXml { get; } = new ReadEncounterXml();
        protected virtual FilePathManager FilePaths { get; } = new FilePathManager();

        public ServerEncounter(string fileName)
        {
            DataXml = Task.Run(() => GetDataXml(fileName));
            ImagesXml = Task.Run(() => GetImagesXml(fileName));
        }
        protected virtual async Task<XmlDocument> GetDataXml(string fileName)
        {
            var dataFilePath = FilePaths.DataFilePath(fileName);
            using (DownloadEncounter dataDownloader = new DownloadEncounter(dataFilePath, "xmlData"))
                return await dataDownloader.Run();
        }

        protected virtual async Task<XmlDocument> GetImagesXml(string fileName)
        {
            var imageFilePath = FilePaths.ImageFilePath(fileName);
            using (DownloadEncounter imagesDownloader = new DownloadEncounter(imageFilePath, "imgData"))
                return await imagesDownloader.Run();
        }
    }
}