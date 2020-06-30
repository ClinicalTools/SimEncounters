using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Writer;
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Xml;

namespace ClinicalTools.SimEncounters
{
    public class LocalEncounterWriter : IEncounterWriter
    {
        protected IFileManager FileManager { get; }
        protected ISerializationFactory<EncounterImageData> ImageDataSerializer { get; }
        protected ISerializationFactory<EncounterContent> EncounterContentSerializer { get; }
        public LocalEncounterWriter(IFileManager fileManager, 
            ISerializationFactory<EncounterImageData> imageDataSerializer, 
            ISerializationFactory<EncounterContent> encounterContentSerializer)
        {
            FileManager = fileManager;
            ImageDataSerializer = imageDataSerializer;
            EncounterContentSerializer = encounterContentSerializer;
        }

        public void Save(User user, Encounter encounter)
        {
            var contentDoc = new XmlDocument();
            var contentSerializer = new XmlSerializer(contentDoc);
            EncounterContentSerializer.Serialize(contentSerializer, encounter.Content);
            FileManager.SetFileText(user, FileType.Data, encounter.Metadata, contentDoc.OuterXml);

            var imagesDoc = new XmlDocument();
            var imagesSerializer = new XmlSerializer(imagesDoc);
            ImageDataSerializer.Serialize(imagesSerializer, encounter.Images);
            FileManager.SetFileText(user, FileType.Image, encounter.Metadata, imagesDoc.OuterXml);
        }
    }

    public class LocalMetadataWriter : IMetadataWriter
    {
        protected IFileManager FileManager { get; }
        protected IStringSerializer<EncounterMetadata> MetadataSerializer { get; }
        public LocalMetadataWriter(IFileManager fileManager, IStringSerializer<EncounterMetadata> metadataSerializer)
        {
            FileManager = fileManager;
            MetadataSerializer = metadataSerializer;
        }

        public void Save(User user, EncounterMetadata metadata)
        {
            var metadataText = MetadataSerializer.Serialize(metadata);
            FileManager.SetFileText(user, FileType.Metadata, metadata, metadataText);
        }
    }
}
