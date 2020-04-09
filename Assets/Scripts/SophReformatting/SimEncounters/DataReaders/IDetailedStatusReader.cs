using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public interface IDetailedStatusReader
    {
        WaitableResult<EncounterDetailedStatus> GetDetailedStatus(User user, EncounterMetadata metadata, EncounterBasicStatus basicStatus);
    }

    public interface IDetailedStatusParser
    {
        EncounterDetailedStatus Parse(string text, EncounterBasicStatus basicStatus);
    }

    public class LocalDetailedStatusReader : IDetailedStatusReader
    {
        private readonly IFileManager fileManager;
        private readonly IDetailedStatusParser parser;
        public LocalDetailedStatusReader(IFileManager fileManager, IDetailedStatusParser parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableResult<EncounterDetailedStatus> GetDetailedStatus(User user, 
            EncounterMetadata metadata, EncounterBasicStatus basicStatus)
        {
            var detailedStatus = new WaitableResult<EncounterDetailedStatus>();

            var fileText = fileManager.GetFileText(user, FileType.DetailedStatus, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(detailedStatus, result, basicStatus));

            return detailedStatus;
        }

        private void ProcessResults(WaitableResult<EncounterDetailedStatus> result, 
            string fileText, EncounterBasicStatus basicStatus)
        {
            var detailedStatus = parser.Parse(fileText, basicStatus);
            result.SetResult(detailedStatus);
        }
    }


    public class ServerDetailedStatusReader : IDetailedStatusReader
    {
        private readonly IUrlBuilder urlBuilder;
        private readonly IServerReader serverReader;
        private readonly IDetailedStatusParser parser;
        public ServerDetailedStatusReader(IUrlBuilder urlBuilder, IServerReader serverReader, IDetailedStatusParser parser)
        {
            this.urlBuilder = urlBuilder;
            this.serverReader = serverReader;
            this.parser = parser;
        }

        public WaitableResult<EncounterDetailedStatus> GetDetailedStatus(User user,
            EncounterMetadata metadata, EncounterBasicStatus basicStatus)
        {

            var webRequest = GetWebRequest(user, metadata);
            var serverOutput = serverReader.Begin(webRequest);
            var detailedStatus = new WaitableResult<EncounterDetailedStatus>();
            serverOutput.AddOnCompletedListener((result) => ProcessResults(detailedStatus, result, basicStatus));

            return detailedStatus;
        }

        private const string menuPhp = "Track.php";
        private const string actionVariable = "ACTION";
        private const string downloadAction = "download";
        private const string usernameVariable = "username";
        private const string recordNumberVariable = "recordNumber";
        protected UnityWebRequest GetWebRequest(User user, EncounterMetadata metadata)
        {
            var url = urlBuilder.BuildUrl(menuPhp);
            var form = new WWWForm();

            form.AddField(actionVariable, downloadAction);
            form.AddField(usernameVariable, user.Username);
            form.AddField(recordNumberVariable, metadata.RecordNumber);

            return UnityWebRequest.Post(url, form);
        }

        private void ProcessResults(WaitableResult<EncounterDetailedStatus> result,
            ServerResult2 serverOutput, EncounterBasicStatus basicStatus)
        {
            if (serverOutput == null || serverOutput.Outcome != ServerOutcome.Success)
            {
                result.SetError(serverOutput?.Message);
                return;
            }

            var detailedStatus = parser.Parse(serverOutput.Message, basicStatus);
            result.SetResult(detailedStatus);
        }
    }
}