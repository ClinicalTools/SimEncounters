using ClinicalTools.ClinicalEncounters;
using ClinicalTools.SimEncounters.Extensions;
using ClinicalTools.SimEncounters.XmlSerialization;
using System;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters.Writer
{
    public class EncounterUploader : IEncounterWriter
    {
        protected IServerReader ServerReader { get; }
        protected IUrlBuilder UrlBuilder { get; }
        protected ISerializationFactory<EncounterImageContent> ImageDataSerializer { get; }
        protected ISerializationFactory<EncounterNonImageContent> EncounterContentSerializer { get; }
        public EncounterUploader(IServerReader serverReader, IUrlBuilder urlBuilder,
            ISerializationFactory<EncounterImageContent> imageDataSerializer,
            ISerializationFactory<EncounterNonImageContent> encounterContentSerializer)
        {
            ServerReader = serverReader;
            UrlBuilder = urlBuilder;
            ImageDataSerializer = imageDataSerializer;
            EncounterContentSerializer = encounterContentSerializer;
        }

        private const string PHP_FILE = "UploadEncounter.php";
        public WaitableResult Save(User user, Encounter encounter)
        {
            if (user.IsGuest)
                return new WaitableResult(true);

            var url = UrlBuilder.BuildUrl(PHP_FILE);
            var form = CreateForm(user, encounter);

            var webRequest = UnityWebRequest.Post(url, form);
            var serverResults = ServerReader.Begin(webRequest);

            var result = new WaitableResult();
            serverResults.AddOnCompletedListener((serverResult) => ProcessResults(result, serverResult, encounter.Metadata));
            return result;
        }


        private const string AccountIdVariable = "accountId";
        protected virtual WWWForm CreateForm(User user, Encounter encounter)
        {
            var form = new WWWForm();

            if (!(encounter.Metadata is CEEncounterMetadata metadata))
                return null;

            form.AddField(AccountIdVariable, user.AccountId);
            AddFormModeFields(form, metadata);
            AddFormContentFields(form, encounter.Content.NonImageContent);
            AddFormImageDataFields(form, encounter.Content.ImageContent);
            AddMetadataFields(form, metadata);

            return form;
        }

        private const string ModeVariable = "mode";
        private const string UploadModeValue = "upload";
        private const string UpdateModeValue = "update";
        private const string RecordNumberVariable = "recordNumber";
        private void AddFormModeFields(WWWForm form, EncounterMetadata metadata)
        {
            string mode;
            if (metadata.RecordNumber >= 0 && metadata.RecordNumber < 1000) {
                form.AddField(RecordNumberVariable, metadata.RecordNumber);
                mode = UpdateModeValue;
            } else {
                mode = UploadModeValue;
            }

            form.AddField(ModeVariable, mode);
        }

        private const string XmlDataVariable = "xmlData";
        private const string XmlDataFilename = "xmlData";
        private const string XmlMimeType = "text/xml";
        private void AddFormContentFields(WWWForm form, EncounterNonImageContent content)
        {
            var contentDoc = new XmlDocument();
            var contentSerializer = new XmlSerializer(contentDoc);
            EncounterContentSerializer.Serialize(contentSerializer, content);
            var fileBytes = GetFileAsByteArray(contentDoc.OuterXml);
            form.AddBinaryData(XmlDataVariable, fileBytes, XmlDataFilename, XmlMimeType);
        }
        private const string ImageDataVariable = "imgData";
        private const string ImageDataFilename = "imgData";
        private const int MaxAllowedPacketSize = 10000000;
        private void AddFormImageDataFields(WWWForm form, EncounterImageContent imageData)
        {
            var imagesDoc = new XmlDocument();
            var imagesSerializer = new XmlSerializer(imagesDoc);
            ImageDataSerializer.Serialize(imagesSerializer, imageData);
            byte[] fileBytesImg = GetFileAsByteArray(imagesDoc.OuterXml);
            Debug.Log("Image file size (in bytes): " + fileBytesImg.Length);
            // Eventually an approach to get around the size limit may be needed
            if (fileBytesImg.Length <= MaxAllowedPacketSize)
                form.AddBinaryData(ImageDataVariable, fileBytesImg, ImageDataFilename, XmlMimeType);
            else
                Debug.LogError("Error: Images exceed upload size limit");
        }

        private const string FirstNameVariable = "firstName";
        private const string LastNameVariable = "lastName";
        private const string DifficultyVariable = "difficulty";
        //private const string SubtitleVariable = "description";
        //private const string DescriptionVariable = "summary";
        private const string SubtitleVariable = "subtitle";
        private const string DescriptionVariable = "description";
        private const string TagsVariable = "tags";
        private const string DateModifiedVariable = "modified";
        private const string AudienceVariable = "audience";
        private const string VersionVariable = "version";
        private const string VersionValue = "0.1";
        private const string UrlVariable = "url";
        private const string CompletionCodeVariable = "urlkey";
        private const string PublicVariable = "public";
        private const string TemplateVariable = "template";
        private void AddMetadataFields(WWWForm form, CEEncounterMetadata metadata)
        {
            form.AddField(FirstNameVariable, metadata.Name.FirstName);
            form.AddField(LastNameVariable, metadata.Name.LastName);
            form.AddField(DifficultyVariable, metadata.Difficulty.ToString());
            form.AddField(SubtitleVariable, metadata.Subtitle);
            form.AddField(DescriptionVariable, metadata.Description);
            form.AddField(TagsVariable, string.Join(";", metadata.Categories));
            metadata.ResetDateModified();
            form.AddField(DateModifiedVariable, metadata.DateModified.ToString());
            form.AddField(AudienceVariable, metadata.Audience);
            form.AddField(VersionVariable, VersionValue);
            form.AddField(UrlVariable, metadata.Url);
            form.AddField(CompletionCodeVariable, metadata.CompletionCode);
            form.AddField(PublicVariable, metadata.IsPublic);
            form.AddField(TemplateVariable, metadata.IsTemplate);
        }


        /**
         * Returns the passed in string as a byte array. Makes code easier to read
         */
        private byte[] GetFileAsByteArray(string data) => Encoding.UTF8.GetBytes(data);

        private void ProcessResults(WaitableResult actionResult, WaitedResult<string> serverResult, EncounterMetadata metadata)
        {
            if (serverResult.IsError()) {
                actionResult.SetError(serverResult.Exception);
                return;
            }

            Debug.Log("Returned text from PHP: \n" + serverResult.Value);
            if (string.IsNullOrWhiteSpace(serverResult.Value)) {
                actionResult.SetError(new Exception("No text returned from the server."));
                return;
            }

            var splitStr = serverResult.Value.Split('|');
            if (int.TryParse(splitStr[0], out var recordNumber))
                metadata.RecordNumber = recordNumber;

            actionResult.SetCompleted();
        }
    }
}