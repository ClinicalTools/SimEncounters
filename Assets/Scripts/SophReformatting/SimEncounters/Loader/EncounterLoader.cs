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

        public virtual EncounterData ReadEncounter(XmlDocument dataXml, XmlDocument imagesXml)
        {
            var content = GetSectionsData(dataXml);
            var images = GetImagesData(imagesXml);
            return new EncounterData(content, images);
        }

        protected virtual EncounterContent GetSectionsData(XmlDocument dataXml)
        {
            var deserializer = new XmlDeserializer(dataXml);
            return DeserializeSectionsData(deserializer);
        }
        protected virtual EncounterContent DeserializeSectionsData(XmlDeserializer deserializer) => deserializer.GetValue(ContentInfo, EncounterDataFactory);

        protected virtual EncounterImageData GetImagesData(XmlDocument imagesXml)
        {
            var deserializer = new XmlDeserializer(imagesXml);
            return DeserializeImagesData(deserializer);
        }
        protected virtual EncounterImageData DeserializeImagesData(XmlDeserializer deserializer) => deserializer.GetValue(ImagesInfo, ImageDataFactory);
    }
}