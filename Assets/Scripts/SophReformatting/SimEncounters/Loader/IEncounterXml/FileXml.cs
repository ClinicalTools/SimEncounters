using System.Threading.Tasks;
using System.Xml;

namespace ClinicalTools.SimEncounters.Loading
{
    public class FileXml : IEncounterXml
    {
        public Task<XmlDocument> DataXml { get; }

        public Task<XmlDocument> ImagesXml { get; }
        protected virtual ReadEncounterXml ReadXml { get; } = new ReadEncounterXml();
        protected virtual FilePathManager FilePaths { get; } = new FilePathManager();

        public FileXml(int accountId, string fileName)
        {
            var filePath = FilePaths.GetLocalFolder(accountId) + fileName;

            DataXml = Task.Run(() => GetDataXml(filePath));
            ImagesXml = Task.Run(() => GetImagesXml(filePath));
        }

        protected virtual async Task<XmlDocument> GetDataXml(string filePath)
        {
            var dataFilePath = FilePaths.DataFilePath(filePath);
            return await ReadXml.ReadXml(dataFilePath);
        }
        protected virtual async Task<XmlDocument> GetImagesXml(string filePath)
        {
            var imageFilePath = FilePaths.ImageFilePath(filePath);
            return await ReadXml.ReadXml(imageFilePath);
        }
    }
}