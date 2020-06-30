using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.XmlSerialization;
using System;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters.Writer
{
    public class EncounterWriter : IEncounterWriter
    {
        protected IEncounterWriter MainDataWriter { get; }
        protected IMetadataWriter MetadataWriter { get; }
        public EncounterWriter(IEncounterWriter mainDataWriter, IMetadataWriter metadataWriter)
        {
            MainDataWriter = mainDataWriter;
            MetadataWriter = metadataWriter;
        }

        private readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public void Save(User user, Encounter encounter)
        {
            encounter.Metadata.DateModified = (long)(DateTime.UtcNow - unixEpoch).TotalSeconds;
            encounter.Metadata.AuthorName = user.GetName();
            encounter.Metadata.AuthorAccountId = user.AccountId;
            MainDataWriter.Save(user, encounter);
            MetadataWriter.Save(user, encounter.Metadata);
        }
    }

    public class EncounterMainDataUploader : IEncounterWriter
    {
        protected IServerReader ServerReader { get; }
        protected IUrlBuilder UrlBuilder { get; }
        protected ISerializationFactory<EncounterImageData> ImageDataSerializer { get; }
        protected ISerializationFactory<EncounterContent> EncounterContentSerializer { get; }
        public EncounterMainDataUploader(IServerReader serverReader, IUrlBuilder urlBuilder,
            ISerializationFactory<EncounterImageData> imageDataSerializer,
            ISerializationFactory<EncounterContent> encounterContentSerializer)
        {
            ServerReader = serverReader;
            UrlBuilder = urlBuilder;
            ImageDataSerializer = imageDataSerializer;
            EncounterContentSerializer = encounterContentSerializer;
        }

        private const string PHP_FILE = "Test.php";
        public void Save(User user, Encounter encounter)
        {
            if (user.IsGuest)
                return;

            var url = UrlBuilder.BuildUrl(PHP_FILE);
            var form = CreateForm(user, encounter);

            var webRequest = UnityWebRequest.Post(url, form);
            var serverResults = ServerReader.Begin(webRequest);
            serverResults.AddOnCompletedListener(ProcessResults);
        }

        private const string MODE_VARIABLE = "mode";
        private const string MODE_VALUE = "upload";
        private const string ACCOUNT_ID_VARIABLE = "account_id";
        private const string FILENAME_VARIABLE = "fileN";
        private const string INDEX_VARIABLE = "index";
        private const int INDEX_VALUE = -1;
        private const int MAX_ALLOWED_PACKET_SIZE = 10000000;
        private const string XML_MIME_TYPE = "text/xml";
        private const string XML_DATA_VARIABLE = "xmlData";
        private const string IMAGE_DATA_VARIABLE = "imgData";

        protected virtual WWWForm CreateForm(User user, Encounter encounter)
        {
            var filename = encounter.Metadata.Filename.Replace(" ", "_") + ".ced";

            var form = new WWWForm();
            form.AddField(MODE_VARIABLE, MODE_VALUE);
            form.AddField(ACCOUNT_ID_VARIABLE, user.AccountId);
            form.AddField(FILENAME_VARIABLE, filename);
            
            // Index was originally used to segmenting image uploads, but is no longer used
            // Kept at the initial value of -1 to avoid breaking anything
            form.AddField(INDEX_VARIABLE, INDEX_VALUE);

            var contentDoc = new XmlDocument();
            var contentSerializer = new XmlSerializer(contentDoc);
            EncounterContentSerializer.Serialize(contentSerializer, encounter.Content);
            var fileBytes = GetFileAsByteArray(contentDoc.OuterXml);
            form.AddBinaryData(XML_DATA_VARIABLE, fileBytes, filename, XML_MIME_TYPE);

            var imagesDoc = new XmlDocument();
            var imagesSerializer = new XmlSerializer(imagesDoc);
            ImageDataSerializer.Serialize(imagesSerializer, encounter.Images);
            byte[] fileBytesImg = GetFileAsByteArray(imagesDoc.OuterXml);
            Debug.Log("Image file size (in bytes): " + fileBytesImg.Length);
            // Eventually an approach to get around the size limit may be needed
            if (fileBytesImg.Length <= MAX_ALLOWED_PACKET_SIZE)
                form.AddBinaryData(IMAGE_DATA_VARIABLE, fileBytesImg, filename, XML_MIME_TYPE);
            else
                Debug.LogError("Error: Images exceed upload size limit");

            return form;
        }
        /**
         * Returns the passed in string as a byte array. Makes code easier to read
         */
        private byte[] GetFileAsByteArray(string data) => Encoding.UTF8.GetBytes(data);

        private void ProcessResults(ServerResult serverResult)
        {
            Debug.Log("Returned text from PHP: \n" + serverResult.Message);
        }
    }
}