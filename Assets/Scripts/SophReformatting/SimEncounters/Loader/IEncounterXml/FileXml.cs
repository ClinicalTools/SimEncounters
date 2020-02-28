using ClinicalTools.SimEncounters.Data;
using System;
using System.Xml;

namespace ClinicalTools.SimEncounters.Loading
{
    public class FileXml : IEncounterXml
    {
        public event Action<XmlDocument, XmlDocument> Completed;

        protected virtual IXmlReader XmlReader { get; }
        protected virtual IFilePathManager FilePaths { get; }

        public FileXml(IFilePathManager filePaths, IXmlReader xmlReader)
        {
            FilePaths = filePaths;
            XmlReader = xmlReader;
        }

        public virtual void GetEncounterXml(User user, EncounterInfoGroup encounterInfoGroup)
        {
            var filePath = FilePaths.GetLocalFolder(user) + encounterInfoGroup.Filename;
            var dataXml = GetDataXml(filePath);
            var imagesXml = GetImagesXml(filePath);
            Completed?.Invoke(dataXml, imagesXml);
        }


        protected virtual XmlDocument GetDataXml(string filePath)
        {
            var dataFilePath = FilePaths.DataFilePath(filePath);
            return XmlReader.ReadXml(dataFilePath);
        }

        protected virtual XmlDocument GetImagesXml(string filePath)
        {
            var imageFilePath = FilePaths.ImageFilePath(filePath);
            return XmlReader.ReadXml(imageFilePath);
        }
    }
}