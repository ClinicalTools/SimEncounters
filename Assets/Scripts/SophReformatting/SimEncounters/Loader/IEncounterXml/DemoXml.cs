using ClinicalTools.SimEncounters.Data;
using System;
using System.Xml;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Loading
{
    public class DemoXml : IEncounterXml
    {
        public event Action<XmlDocument, XmlDocument> Completed;

        protected virtual IXmlReader XmlReader { get; }
        protected virtual IFilePathManager FilePaths { get; }
        protected virtual string DemoDirectory => Application.streamingAssetsPath + "/DemoCases/";

        public DemoXml(IFilePathManager filePaths, IXmlReader xmlReader)
        {
            FilePaths = filePaths;
            XmlReader = xmlReader;
        }

        public virtual void GetEncounterXml(User user, EncounterInfoGroup encounterInfoGroup)
        {
            var filePath = DemoDirectory + encounterInfoGroup.Filename;
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