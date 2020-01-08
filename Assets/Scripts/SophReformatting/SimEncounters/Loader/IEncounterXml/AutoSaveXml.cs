using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace ClinicalTools.SimEncounters.Loading
{
    public class AutoSaveXml : FileXml
    {

        public AutoSaveXml(int accountId, string fileName) : base(accountId, fileName) { }

        protected override async Task<XmlDocument> GetDataXml(string filePath)
        {
            var dataFilePath = FilePaths.AutoSaveDataFilePath(filePath);

            return await ReadXml.ReadXml(dataFilePath);
        }
        protected override async Task<XmlDocument> GetImagesXml(string filePath)
        {
            var imageFilePath = FilePaths.AutoSaveImageFilePath(filePath);
            if (!File.Exists(imageFilePath))
                imageFilePath = FilePaths.ImageFilePath(filePath);

            return await ReadXml.ReadXml(imageFilePath);
        }
    }
}
