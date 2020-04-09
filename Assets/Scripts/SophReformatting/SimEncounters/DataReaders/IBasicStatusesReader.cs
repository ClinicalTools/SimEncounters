using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public interface IBasicStatusesReader
    {
        WaitableResult<Dictionary<int, EncounterBasicStatus>> GetBasicStatuses(User user);
    }
    public class LocalBasicStatusesReader : IBasicStatusesReader
    {
        private readonly IFileManager fileManager;
        private readonly IParser<KeyValuePair<int, EncounterBasicStatus>> parser;
        public LocalBasicStatusesReader(IFileManager fileManager, IParser<KeyValuePair<int, EncounterBasicStatus>> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableResult<Dictionary<int, EncounterBasicStatus>> GetBasicStatuses(User user)
        {
            var statuses = new WaitableResult<Dictionary<int, EncounterBasicStatus>>();

            var fileTexts = fileManager.GetFilesText(user, FileType.BasicStatus);
            fileTexts.AddOnCompletedListener((result) => ProcessResults(statuses, result));

            return statuses;
        }

        private void ProcessResults(WaitableResult<Dictionary<int, EncounterBasicStatus>> result, string[] fileTexts)
        {
            if (fileTexts == null)
            {
                result.SetError(null);
                return;
            }

            var statuses = new Dictionary<int, EncounterBasicStatus>();
            foreach (var fileText in fileTexts)
            {
                var metadata = parser.Parse(fileText);
                if (metadata.Value != null)
                    statuses.Add(metadata.Key, metadata.Value);
            }

            result.SetResult(statuses);
        }
    }
    public class ServerBasicStatusesReader : IBasicStatusesReader
    {
        private readonly IUrlBuilder urlBuilder;
        private readonly IServerReader serverReader;
        private readonly IParser<Dictionary<int, EncounterBasicStatus>> parser;
        public ServerBasicStatusesReader(IUrlBuilder urlBuilder, IServerReader serverReader, IParser<Dictionary<int, EncounterBasicStatus>> parser)
        {
            this.urlBuilder = urlBuilder;
            this.serverReader = serverReader;
            this.parser = parser;
        }

        public WaitableResult<Dictionary<int, EncounterBasicStatus>> GetBasicStatuses(User user)
        {
            var webRequest = GetWebRequest(user);
            var serverOutput = serverReader.Begin(webRequest);
            var statuses = new WaitableResult<Dictionary<int, EncounterBasicStatus>>();
            serverOutput.AddOnCompletedListener((result) => ProcessResults(statuses, result));

            return statuses;
        }

        private const string menuPhp = "Track.php";
        private const string actionVariable = "ACTION";
        private const string downloadAction = "download";
        private const string usernameVariable = "username";
        protected UnityWebRequest GetWebRequest(User user)
        {
            var url = urlBuilder.BuildUrl(menuPhp);
            var form = new WWWForm();

            form.AddField(actionVariable, downloadAction);
            form.AddField(usernameVariable, user.Username);

            return UnityWebRequest.Post(url, form);
        }
        private void ProcessResults(WaitableResult<Dictionary<int, EncounterBasicStatus>> result, ServerResult2 serverOutput)
        {
            if (serverOutput == null || serverOutput.Outcome != ServerOutcome.Success)
            {
                result.SetError(serverOutput?.Message);
                return;
            }

            var statuses = parser.Parse(serverOutput.Message);
            result.SetResult(statuses);
        }
    }
}