using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Loading
{
    public class DemoXml : IEncounterXml
    {
        public Task<XmlDocument> DataXml { get; }
        public Task<XmlDocument> ImagesXml { get; }
        protected virtual ReadEncounterXml ReadXml { get; } = new ReadEncounterXml();
        protected virtual FilePathManager FilePaths { get; } = new FilePathManager();

        protected virtual string DemoCase => "Chad_Wright";
        protected virtual string DemoPath => Application.streamingAssetsPath + "/DemoCases/" + DemoCase;

        public DemoXml()
        {
            DataXml = Task.Run(() => GetDataXml(DemoPath));
            ImagesXml = Task.Run(() => GetImagesXml(DemoPath));
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