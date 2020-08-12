using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public interface IBasicStatusesReader
    {
        WaitableResult<Dictionary<int, EncounterBasicStatus>> GetBasicStatuses(User user);
    }

    public class BasicStatusesReader : IBasicStatusesReader
    {
        private readonly List<IBasicStatusesReader> basicStatusesReaders;
        public BasicStatusesReader(List<IBasicStatusesReader> basicStatusesReaders)
        {
            this.basicStatusesReaders = basicStatusesReaders;
        }

        public WaitableResult<Dictionary<int, EncounterBasicStatus>> GetBasicStatuses(User user)
        {
            var statuses = new WaitableResult<Dictionary<int, EncounterBasicStatus>>();

            var results = new List<WaitableResult<Dictionary<int, EncounterBasicStatus>>>();
            foreach (var basicStatusesReader in basicStatusesReaders)
                results.Add(basicStatusesReader.GetBasicStatuses(user));

            foreach (var result in results)
                result.AddOnCompletedListener((statusesResult) => ProcessResults(statuses, results));

            return statuses;
        }

        private void ProcessResults(WaitableResult<Dictionary<int, EncounterBasicStatus>> result,
            List<WaitableResult<Dictionary<int, EncounterBasicStatus>>> listResults)
        {
            if (result.IsCompleted())
                return;
            foreach (var statusesResult in listResults) {
                if (!statusesResult.IsCompleted())
                    return;
            }

            var statuses = listResults[0].Result.Value;
            for (int i = 1; i < listResults.Count; i++)
                statuses = CombineStatuses(statuses, listResults[i].Result.Value);

            result.SetResult(statuses);
        }

        private Dictionary<int, EncounterBasicStatus> CombineStatuses(
            Dictionary<int, EncounterBasicStatus> statuses1, Dictionary<int, EncounterBasicStatus> statuses2)
        {
            if (statuses1 == null)
                return statuses2;
            else if (statuses2 == null)
                return statuses1;

            foreach (var status in statuses2) {
                if (!statuses1.ContainsKey(status.Key)) {
                    statuses1.Add(status.Key, status.Value);
                } else if (statuses1[status.Key].Timestamp < status.Value.Timestamp){
                    statuses1[status.Key] = status.Value;
                }
            }
            return statuses1;
        }
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

        private void ProcessResults(WaitableResult<Dictionary<int, EncounterBasicStatus>> result, WaitedResult<string[]> fileTexts)
        {
            if (fileTexts == null) {
                result.SetError(null);
                return;
            }

            var statuses = new Dictionary<int, EncounterBasicStatus>();
            foreach (var fileText in fileTexts.Value) {
                var metadata = parser.Parse(fileText);
                if (statuses.ContainsKey(metadata.Key)) {
                    Debug.LogError($"Duplicate saved status for key {metadata.Key}");
                    continue;
                }
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
            if (user.IsGuest)
                return new WaitableResult<Dictionary<int, EncounterBasicStatus>>(new Exception("Guest user has no server statuses."));

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
        private void ProcessResults(WaitableResult<Dictionary<int, EncounterBasicStatus>> result, WaitedResult<string> serverOutput)
        {
            if (serverOutput == null || serverOutput.IsError()) {
                result.SetError(serverOutput.Exception);
                return;
            }

            var statuses = parser.Parse(serverOutput.Value);
            result.SetResult(statuses);
        }
    }
}