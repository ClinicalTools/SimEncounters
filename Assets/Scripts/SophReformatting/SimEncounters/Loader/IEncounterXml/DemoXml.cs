using System;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Loading
{
    public class DemoXml : IEncounterXml
    {
        public event Action<XmlDocument, XmlDocument> Completed;
        public Task<XmlDocument> DataXml { get; }
        public Task<XmlDocument> ImagesXml { get; }
        public XmlDocument DataXmlSync { get; }
        public XmlDocument ImagesXmlSync { get; }
        protected virtual ReadEncounterXml ReadXml { get; } = new ReadEncounterXml();
        protected virtual FilePathManager FilePaths { get; } = new FilePathManager();

        protected virtual string DemoCase => "Chad_Wright";
        protected virtual string DemoPath => Application.streamingAssetsPath + "/DemoCases/" + DemoCase;

        public DemoXml()
        {
            //DataXml = Task.Run(() => GetDataXmlSync(DemoPath));
            //ImagesXml = Task.Run(() => GetImagesXmlSync(DemoPath));
            DataXmlSync = GetDataXmlSync(DemoPath);
            ImagesXmlSync = GetImagesXmlSync(DemoPath);

        }

        public void GetEncounterXml()
        {
            var dataXmlSync = GetDataXmlSync(DemoPath);
            var imagesXmlSync = GetImagesXmlSync(DemoPath);
            Completed?.Invoke(dataXmlSync, imagesXmlSync);
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
        protected virtual XmlDocument GetDataXmlSync(string filePath)
        {
            var dataFilePath = FilePaths.DataFilePath(filePath);
            return ReadXml.ReadXmlSync(dataFilePath);
        }

        protected virtual XmlDocument GetImagesXmlSync(string filePath)
        {
            var imageFilePath = FilePaths.ImageFilePath(filePath);
            return ReadXml.ReadXmlSync(imageFilePath);
        }
    }
}