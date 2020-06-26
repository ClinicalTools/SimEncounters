using ClinicalTools.SimEncounters;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Writer;
using ModestTree;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters.Writer
{
    public interface IMetadataUploader
    {
        void UploadMetadata(User user, EncounterMetadata metadata);
    }
}
namespace ClinicalTools.ClinicalEncounters.Writer
{
    public class CEMetadataUploader : IMetadataUploader
    {
        private readonly IServerReader serverReader;
        private readonly IUrlBuilder urlBuilder;
        public CEMetadataUploader(IServerReader serverReader, IUrlBuilder urlBuilder)
        {
            this.serverReader = serverReader;
            this.urlBuilder = urlBuilder;
        }

        private const string phpFile = "Test.php";
        public void UploadMetadata(User user, EncounterMetadata metadata)
        {
            if (user.IsGuest)
                return;

            if (!(metadata is CEEncounterMetadata ceMetadata))
                return;

            var url = urlBuilder.BuildUrl(phpFile);
            var form = CreateForm(user, ceMetadata);

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
        public WWWForm CreateForm(User user, CEEncounterMetadata metadata)
        {
            var form = new WWWForm();
            form.AddField(modeVariable, modeValue);
            form.AddField(accountIdVariable, user.AccountId);
            form.AddField(filenameVariable, GlobalData.fileName.Replace(" ", "_"));
            // Author name should be handled through linked columns in MySQL to ensure it updates
            form.AddField(authorNameVariable, user.GetName());

            form.AddField(patientFirstNameVariable, metadata.FirstName);
            form.AddField(patientLastNameVariable, metadata.LastName);
            form.AddField(recordNumberVariable, metadata.RecordNumber);
            form.AddField(difficultyVariable, metadata.Difficulty.ToString());
            form.AddField(descriptionVariable, metadata.Subtitle);
            form.AddField(summaryVariable, metadata.Description);
            form.AddField(tagsVariable, string.Join(", ", metadata.Categories));
            form.AddField(modifiedVariable, metadata.DateModified.ToString());
            form.AddField(audienceVariable, metadata.Audience);
            form.AddField(versionVariable, versionValue);
            form.AddField(caseTypeVariable, metadata.GetCaseType());

            return form;
        }

        private void ProcessResults(ServerResult serverResult)
        {
        }
    }
}