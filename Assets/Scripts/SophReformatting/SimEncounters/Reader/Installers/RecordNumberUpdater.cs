using ClinicalTools.SimEncounters.Data;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters.Writer
{
    public class RecordNumberUpdater
    {
        protected virtual IServerReader ServerReader { get; }
        protected virtual IUrlBuilder UrlBuilder { get; }
        public RecordNumberUpdater(IServerReader serverReader, IUrlBuilder urlBuilder)
        {
            ServerReader = serverReader;
            UrlBuilder = urlBuilder;
        }

        public WaitableResult<EncounterMetadata> GetRecordNumber(User user, EncounterMetadata metadata)
        {
            if (user.IsGuest)
                return new WaitableResult<EncounterMetadata>();

            var result = new WaitableResult<EncounterMetadata>();
            AaaRecordNumber(metadata, result);
            return result;
        }

        private const string PHP_FILE = "Menu.php";
        protected virtual void AaaRecordNumber(EncounterMetadata metadata, WaitableResult<EncounterMetadata> result, int attempt = 0)
        {
            var url = UrlBuilder.BuildUrl(PHP_FILE);
            var form = CreateForm(metadata);

            var webRequest = UnityWebRequest.Post(url, form);
            var serverResults = ServerReader.Begin(webRequest);
            serverResults.AddOnCompletedListener((serverResult) => ProcessResults(serverResult, metadata, result, attempt));
        }

        private const string MODE_VARIABLE = "mode";
        private const string MODE_VALUE = "checkFields";
        private const string FILENAME_VARIABLE = "filename";
        private const string RECORD_NUMBER_VARIABLE = "recordNumber";
        protected virtual WWWForm CreateForm(EncounterMetadata metadata)
        {
            var form = new WWWForm();
            form.AddField(MODE_VARIABLE, MODE_VALUE);
            form.AddField(FILENAME_VARIABLE, metadata.Filename);
            form.AddField(RECORD_NUMBER_VARIABLE, metadata.GetRecordNumberString());

            return form;
        }


        protected virtual void ProcessResults(WaitedResult<ServerResult> serverResult, EncounterMetadata metadata, 
            WaitableResult<EncounterMetadata> result, int attempt)
        {
            Debug.Log("Returned text from metadata PHP: \n" + serverResult.Value.Message);
            if (string.Equals(serverResult.Value.Message, "duplicate", StringComparison.InvariantCultureIgnoreCase)) {

            }
        }
    }
}