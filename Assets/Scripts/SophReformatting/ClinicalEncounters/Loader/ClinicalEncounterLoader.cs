using ClinicalTools.ClinicalEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Loader;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Xml;

namespace ClinicalTools.ClinicalEncounters.Loader
{
    public class ClinicalEncounterLoader : EncounterLoader
    {
        private EncounterDataFactory encounterDataFactory;
        protected override EncounterDataFactory EncounterDataFactory => encounterDataFactory;
        protected override ImageDataFactory ImageDataFactory => new ClinicalImageDataFactory();

        protected NodeInfo LegacyContentInfo { get; } = NodeInfo.RootValue;
        protected NodeInfo LegacyImagesInfo { get; } = NodeInfo.RootValue;

        public override Encounter ReadEncounter(EncounterInfo info, XmlDocument dataXml, XmlDocument imagesXml)
        {
            var images = GetImagesData(imagesXml);
            encounterDataFactory = new ClinicalEncounterDataFactory(images);
            var content = GetSectionsData(dataXml);
            return new Encounter(info, content, images);
        }

        protected override SectionsData DeserializeSectionsData(XmlDeserializer deserializer)
        {
            var sectionsData = base.DeserializeSectionsData(deserializer);
            if (sectionsData != null)
                return sectionsData;

            return deserializer.GetValue(LegacyContentInfo, EncounterDataFactory);
        }

        protected override ImagesData DeserializeImagesData(XmlDeserializer deserializer)
        {
            var imagesData = base.DeserializeImagesData(deserializer);
            if (imagesData != null)
                return imagesData;

            return deserializer.GetValue(LegacyImagesInfo, ImageDataFactory);
        }
    }
}