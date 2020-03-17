﻿using ClinicalTools.SimEncounters.Data;
using System.Xml;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Loading
{
    public class DemoXml : IEncounterXmlReader
    {
        public event EncounterXmlRetrievedEventHandler Completed;

        protected virtual IXmlReader XmlReader { get; }
        protected virtual IFilePathManager FilePaths { get; }
        protected virtual string DemoDirectory => Application.streamingAssetsPath + "/DemoCases/";

        public DemoXml(IFilePathManager filePaths, IXmlReader xmlReader)
        {
            FilePaths = filePaths;
            XmlReader = xmlReader;
        }

        public virtual void GetEncounterXml(User user, EncounterInfo encounterInfo)
        {
            var filePath = DemoDirectory + encounterInfo.MetaGroup.Filename;
            var dataXml = GetDataXml(filePath);
            var imagesXml = GetImagesXml(filePath);
            Completed?.Invoke(this, new EncounterXmlRetrievedEventArgs(dataXml, imagesXml));
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