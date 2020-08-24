﻿using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class ServerContentDataReader : IEncounterContentReader
    {
        private readonly IServerReader serverReader;
        private readonly IUrlBuilder urlBuilder;
        private readonly IParser<EncounterNonImageContent> parser;
        public ServerContentDataReader(IServerReader serverReader, IUrlBuilder urlBuilder, IParser<EncounterNonImageContent> parser)
        {
            this.serverReader = serverReader;
            this.urlBuilder = urlBuilder;
            this.parser = parser;
        }

        public WaitableResult<EncounterNonImageContent> GetEncounterContent(User user, EncounterMetadata metadata)
        {
            var contentData = new WaitableResult<EncounterNonImageContent>();

            var webRequest = GetWebRequest(user, metadata);
            var serverResult = serverReader.Begin(webRequest);
            serverResult.AddOnCompletedListener((result) => ProcessResults(contentData, result));

            return contentData;
        }

        private const string DownloadPhp = "DownloadEncounters.php";
        private const string ModeVariable = "mode";
        private const string ModeValue = "downloadCase";
        private const string RecordNumberVariable = "recordNumber";
        private const string ColumnVariable = "column";
        private const string ColumnValue = "xmlData";
        private const string AccountIdVariable = "accountId";
        private UnityWebRequest GetWebRequest(User user, EncounterMetadata metadata)
        {
            var arguments = new UrlArgument[] {
                new UrlArgument(AccountIdVariable, user.AccountId.ToString()),
                new UrlArgument(ModeVariable, ModeValue),
                new UrlArgument(RecordNumberVariable, metadata.RecordNumber.ToString()),
                new UrlArgument(ColumnVariable, ColumnValue)
            };
            var url = urlBuilder.BuildUrl(DownloadPhp, arguments);
            return UnityWebRequest.Get(url);
        }

        private void ProcessResults(WaitableResult<EncounterNonImageContent> result, WaitedResult<string> serverResult)
        {
            result.SetResult(parser.Parse(serverResult.Value));
        }
    }
}