using ClinicalTools.SimEncounters;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Writer;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters.Writer
{
    public interface IMetadataWriter
    {
        void Save(User user, EncounterMetadata metadata);
    }
}
namespace ClinicalTools.ClinicalEncounters.Writer
{
    public class CEMetadataUploader : IMetadataWriter
    {
        protected virtual IServerReader ServerReader { get; }
        protected virtual IUrlBuilder UrlBuilder { get; }
        public CEMetadataUploader(IServerReader serverReader, IUrlBuilder urlBuilder)
        {
            ServerReader = serverReader;
            UrlBuilder = urlBuilder;
        }

        private const string PHP_FILE = "Menu.php";
        public void Save(User user, EncounterMetadata metadata)
        {
            if (user.IsGuest)
                return;

            if (!(metadata is CEEncounterMetadata ceMetadata))
                return;

            var url = UrlBuilder.BuildUrl(PHP_FILE);
            var form = CreateForm(user, ceMetadata);

            var webRequest = UnityWebRequest.Post(url, form);
            var serverResults = ServerReader.Begin(webRequest);
            serverResults.AddOnCompletedListener(ProcessResults);
        }

        private const string MODE_VARIABLE = "mode";
        private const string MODE_VALUE = "upload2";
        private const string ACCOUNT_ID_VARIABLE = "account_id";
        private const string FILENAME_VARIABLE = "filename";
        private const string AUTHOR_NAME_VARIABLE = "authorName";
        private const string PATIENT_FIRST_NAME_VARIABLE = "patientFirstName";
        private const string PATIENT_LAST_NAME_VARIABLE = "patientLastName";
        private const string RECORD_NUMBER_VARIABLE = "recordNumber";
        private const string DIFFICULTY_VARIABLE = "difficulty";
        private const string DESCRIPTION_VARIABLE = "description";
        private const string SUMMARY_VARIABLE = "summary";
        private const string TAGS_VARIABLE = "tags";
        private const string MODIFIED_VARIABLE = "modified";
        private const string AUDIENCE_VARIABLE = "audience";
        private const string VERSION_VARIABLE = "version";
        private const string VERSION_VALUE = "0.1";
        private const string CASE_TYPE_VARIABLE = "caseType";
        protected virtual WWWForm CreateForm(User user, CEEncounterMetadata metadata)
        {
            var form = new WWWForm();
            form.AddField(MODE_VARIABLE, MODE_VALUE);
            form.AddField(ACCOUNT_ID_VARIABLE, user.AccountId);
            form.AddField(FILENAME_VARIABLE, metadata.Filename.Replace(" ", "_") + ".ced");
            // Author name should be handled through linked columns in MySQL to ensure it updates
            form.AddField(AUTHOR_NAME_VARIABLE, user.GetName());

            form.AddField(PATIENT_FIRST_NAME_VARIABLE, metadata.FirstName);
            form.AddField(PATIENT_LAST_NAME_VARIABLE, metadata.LastName);
            if (metadata.RecordNumber < 0)
                metadata.RecordNumber *= -1;
            form.AddField(RECORD_NUMBER_VARIABLE, metadata.RecordNumber);
            form.AddField(DIFFICULTY_VARIABLE, metadata.Difficulty.ToString());
            form.AddField(DESCRIPTION_VARIABLE, metadata.Subtitle);
            form.AddField(SUMMARY_VARIABLE, metadata.Description);
            form.AddField(TAGS_VARIABLE, string.Join(", ", metadata.Categories));
            form.AddField(MODIFIED_VARIABLE, metadata.DateModified.ToString());
            form.AddField(AUDIENCE_VARIABLE, metadata.Audience);
            form.AddField(VERSION_VARIABLE, VERSION_VALUE);
            form.AddField(CASE_TYPE_VARIABLE, metadata.GetCaseType());

            return form;
        }

        private void ProcessResults(WaitedResult<ServerResult> serverResult)
        {
            Debug.Log("Returned text from metadata PHP: \n" + serverResult.Value.Message);
        }
    }
}