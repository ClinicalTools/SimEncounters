using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Xml;

namespace ClinicalTools.SimEncounters.Loader
{
    public class EncounterLoader
    {
        protected virtual NodeInfo ContentInfo { get; } = new NodeInfo("content");
        protected virtual NodeInfo ImagesInfo { get; } = new NodeInfo("images");

        protected virtual EncounterDataFactory EncounterDataFactory => new EncounterDataFactory();
        protected virtual ImageDataFactory ImageDataFactory => new ImageDataFactory();

        public virtual Encounter ReadEncounter(EncounterInfo info, XmlDocument dataXml, XmlDocument imagesXml)
        {
            var content = GetSectionsData(dataXml);
            var images = GetImagesData(imagesXml);
            return new Encounter(info, content, images);
        }

        protected virtual SectionsData GetSectionsData(XmlDocument dataXml)
        {
            var deserializer = new XmlDeserializer(dataXml);
            return DeserializeSectionsData(deserializer);
        }
        protected virtual SectionsData DeserializeSectionsData(XmlDeserializer deserializer) => deserializer.GetValue(ContentInfo, EncounterDataFactory);

        protected virtual ImagesData GetImagesData(XmlDocument imagesXml)
        {
            var deserializer = new XmlDeserializer(imagesXml);
            return DeserializeImagesData(deserializer);
        }
        protected virtual ImagesData DeserializeImagesData(XmlDeserializer deserializer) => deserializer.GetValue(ImagesInfo, ImageDataFactory);
    }
}