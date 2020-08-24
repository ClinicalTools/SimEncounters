using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class LegacyServerMetadatasReader : IMetadatasReader
    {
        private readonly IUrlBuilder urlBuilder;
        private readonly IServerReader serverReader;
        private readonly IParser<List<EncounterMetadata>> parser;
        public LegacyServerMetadatasReader(IUrlBuilder urlBuilder, IServerReader serverReader, IParser<List<EncounterMetadata>> parser)
        {
            this.urlBuilder = urlBuilder;
            this.serverReader = serverReader;
            this.parser = parser;
        }

        public WaitableResult<List<EncounterMetadata>> GetMetadatas(User user)
        {
            var webRequest = GetWebRequest(user);
            var serverOutput = serverReader.Begin(webRequest);
            var metadatas = new WaitableResult<List<EncounterMetadata>>();
            serverOutput.AddOnCompletedListener((result) => ProcessResults(metadatas, result));

            return metadatas;
        }

        private const string MENU_PHP = "Menu.php";
        private const string MODE_VARIABLE = "mode";
        private const string MODE_VALUE = "downloadForOneAccount";
        private const string ACCOUNT_VARIABLE = "account_id";

        private UnityWebRequest GetWebRequest(User user)
        {
            var arguments = new UrlArgument[] {
                new UrlArgument(MODE_VARIABLE, MODE_VALUE),
                new UrlArgument(ACCOUNT_VARIABLE, user.AccountId.ToString())
            };
            var url = urlBuilder.BuildUrl(MENU_PHP, arguments);
            return UnityWebRequest.Get(url);
        }


        private void ProcessResults(WaitableResult<List<EncounterMetadata>> result, WaitedResult<string> serverOutput)
        {
            if (serverOutput == null || serverOutput.IsError()) {
                result.SetError(serverOutput.Exception);
                return;
            }

            var metadatas = parser.Parse(serverOutput.Value);
            result.SetResult(metadatas);
        }
    }
}
