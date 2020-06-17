using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerDetailedStatusReader : IDetailedStatusReader
    {
        private readonly IUrlBuilder urlBuilder;
        private readonly IServerReader serverReader;
        private readonly IParser<EncounterContentStatus> parser;
        public ServerDetailedStatusReader(IUrlBuilder urlBuilder, IServerReader serverReader, IParser<EncounterContentStatus> parser)
        {
            this.urlBuilder = urlBuilder;
            this.serverReader = serverReader;
            this.parser = parser;
        }

        public WaitableResult<EncounterStatus> GetDetailedStatus(User user,
            EncounterMetadata metadata, EncounterBasicStatus basicStatus)
        {
            if (user.IsGuest)
                return new WaitableResult<EncounterStatus>(
                    new EncounterStatus(new EncounterBasicStatus(), new EncounterContentStatus()));

            var webRequest = GetWebRequest(user, metadata);
            var serverOutput = serverReader.Begin(webRequest);
            var detailedStatus = new WaitableResult<EncounterStatus>();
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

        private void ProcessResults(WaitableResult<EncounterStatus> result,
            ServerResult serverOutput, EncounterBasicStatus basicStatus)
        {
            if (serverOutput == null || serverOutput.Outcome != ServerOutcome.Success)
            {
                result.SetError(serverOutput?.Message);
                return;
            }

            var contentStatus = parser.Parse(serverOutput.Message);
            var status = new EncounterStatus(basicStatus, contentStatus);
            result.SetResult(status);
        }
    }
}