using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.XmlSerialization;
using System.Diagnostics;
using System.Xml;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterWriter
    {
        void Save(User user, Encounter encounter);
    }
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

        public void Save(User user, Encounter encounter)
        {
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

    public class DetailedStatusWriter : IDetailedStatusWriter
    {
        protected IDetailedStatusWriter ServerDetailedStatusWriter { get; }
        protected IDetailedStatusWriter FileDetailedStatusWriter { get; }
        public DetailedStatusWriter(ServerDetailedStatusWriter serverDetailedStatusWriter, FileDetailedStatusWriter fileDetailedStatusWriter)
        {
            ServerDetailedStatusWriter = serverDetailedStatusWriter;
            FileDetailedStatusWriter = fileDetailedStatusWriter;
        }

        public void WriteStatus(UserEncounter encounter)
        {
            ServerDetailedStatusWriter.WriteStatus(encounter);
            FileDetailedStatusWriter.WriteStatus(encounter);
        }
    }
}
