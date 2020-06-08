using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.XmlSerialization;
using System;
using System.Xml;

namespace ClinicalTools.SimEncounters
{
    public class EncounterWriter : IEncounterWriter
    {
        protected IFileManager FileManager { get; }
        protected ISerializationFactory<EncounterImageData> ImageDataSerializer { get; }
        protected ISerializationFactory<EncounterContent> EncounterContentSerializer { get; }
        protected IStringSerializer<EncounterMetadata> MetadataSerializer { get; }
        public EncounterWriter(IFileManager fileManager,  
            ISerializationFactory<EncounterImageData> imageDataSerializer, 
            ISerializationFactory<EncounterContent> encounterContentSerializer, 
            IStringSerializer<EncounterMetadata> metadataSerializer)
        {
            FileManager = fileManager;
            ImageDataSerializer = imageDataSerializer;
            EncounterContentSerializer = encounterContentSerializer;
            MetadataSerializer = metadataSerializer;
        }


        private readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public void Save(User user, Encounter encounter)
        {
            encounter.Metadata.DateModified = (long)(DateTime.UtcNow - unixEpoch).TotalSeconds;
            var xmlDoc = new XmlDocument();
            var serializer = new XmlSerializer(xmlDoc);
            EncounterContentSerializer.Serialize(serializer, encounter.Content);
            FileManager.SetFileText(user, FileType.Data, encounter.Metadata, xmlDoc.OuterXml);

            var xmlDoc2 = new XmlDocument();
            var serializer2 = new XmlSerializer(xmlDoc2);
            ImageDataSerializer.Serialize(serializer2, encounter.Images);
            FileManager.SetFileText(user, FileType.Image, encounter.Metadata, xmlDoc2.OuterXml);

            var metadataText = MetadataSerializer.Serialize(encounter.Metadata);
            FileManager.SetFileText(user, FileType.Metadata, encounter.Metadata, metadataText);
        }
    }
}
