using System.Xml;

namespace ClinicalTools.SimEncounters.Loading
{
    public class AutoSaveXml : FileXml
    {
        public AutoSaveXml(IFilePathManager filePaths, IXmlReader xmlReader) : base(filePaths, xmlReader) { }

        protected override XmlDocument GetDataXml(string filePath)
        {
            var dataFilePath = FilePaths.AutoSaveDataFilePath(filePath);
            return XmlReader.ReadXml(dataFilePath);
        }

        protected override XmlDocument GetImagesXml(string filePath)
        {
            var imageFilePath = FilePaths.AutoSaveImageFilePath(filePath);
            return XmlReader.ReadXml(imageFilePath);
        }
    }
}
