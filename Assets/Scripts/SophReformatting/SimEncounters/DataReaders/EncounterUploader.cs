using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters.Writer
{
    public interface IEncounterUploader
    {
        void UploadEncounter(User user, Encounter metadata);
    }

    public class EncounterUploader : IEncounterUploader
    {
        private readonly IEncounterUploader mainDataUploader;
        private readonly IMetadataUploader metadataUploader;
        public EncounterUploader(IEncounterUploader mainDataUploader, IMetadataUploader metadataUploader)
        {
            this.mainDataUploader = mainDataUploader;
            this.metadataUploader = metadataUploader;
        }

        public void UploadEncounter(User user, Encounter encounter)
        {
            mainDataUploader.UploadEncounter(user, encounter);
            metadataUploader.UploadMetadata(user, encounter.Metadata);
        }
    }

    public class EncounterMainDataUploader : IEncounterUploader
    {
        private readonly IServerReader serverReader;
        private readonly IUrlBuilder urlBuilder;
        public EncounterMainDataUploader(IServerReader serverReader, IUrlBuilder urlBuilder)
        {
            this.serverReader = serverReader;
            this.urlBuilder = urlBuilder;
        }

        private const string phpFile = "Test.php";
        public void UploadEncounter(User user, Encounter encounter)
        {
            if (user.IsGuest)
                return;

            var url = urlBuilder.BuildUrl(phpFile);
            var form = CreateForm(user, encounter);

            var webRequest = UnityWebRequest.Post(url, form);
            var serverResults = serverReader.Begin(webRequest);
            serverResults.AddOnCompletedListener(ProcessResults);
        }

        private const string modeVariable = "mode";
        private const string modeValue = "upload2";
        private const string accountIdVariable = "accountId";
        private const string filenameVariable = "filename";
        private const string authorNameVariable = "authorName";
        private const string patientFirstNameVariable = "patientFirstName";
        private const string patientLastNameVariable = "patientLastName";
        private const string recordNumberVariable = "recordNumber";
        private const string difficultyVariable = "difficulty";
        private const string descriptionVariable = "description";
        private const string summaryVariable = "summary";
        private const string tagsVariable = "tags";
        private const string modifiedVariable = "modified";
        private const string audienceVariable = "audience";
        private const string versionVariable = "version";
        private const string versionValue = "0.1";
        private const string caseTypeVariable = "caseType";
        public WWWForm CreateForm(User user, Encounter encounter)
        {
            var form = new WWWForm();
            form.AddField(modeVariable, modeValue);
            form.AddField(accountIdVariable, user.AccountId);
            form.AddField(filenameVariable, GlobalData.fileName.Replace(" ", "_"));
            // Author name should be handled through linked columns in MySQL to ensure it updates
            form.AddField(authorNameVariable, user.GetName());

            return form;
        }

        private void ProcessResults(ServerResult serverResult)
        {
        }
    }
}