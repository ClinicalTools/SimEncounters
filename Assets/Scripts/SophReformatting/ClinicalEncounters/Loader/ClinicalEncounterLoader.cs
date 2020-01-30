using ClinicalTools.ClinicalEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Loader;
using ClinicalTools.SimEncounters.SerializationFactories;
using ClinicalTools.SimEncounters.XmlSerialization;

namespace ClinicalTools.ClinicalEncounters.Loader
{
    public class ClinicalEncounterLoader : EncounterLoader
    {
        protected override EncounterDataFactory EncounterDataFactory => new ClinicalEncounterDataFactory();
        protected override ImageDataFactory ImageDataFactory => new ClinicalImageDataFactory();

        protected NodeInfo LegacyContentInfo { get; } = NodeInfo.RootValue;
        protected NodeInfo LegacyImagesInfo { get; } = NodeInfo.RootValue;

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