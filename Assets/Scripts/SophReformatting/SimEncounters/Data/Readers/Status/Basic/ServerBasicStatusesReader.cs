using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
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