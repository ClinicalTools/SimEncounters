using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class LegacyServerNonImageContentReader : INonImageContentReader
    {
        private readonly IServerReader serverReader;
        private readonly IUrlBuilder urlBuilder;
        private readonly IStringDeserializer<EncounterNonImageContent> parser;
        public LegacyServerNonImageContentReader(IServerReader serverReader, IUrlBuilder urlBuilder, IStringDeserializer<EncounterNonImageContent> parser)
        {
            this.serverReader = serverReader;
            this.urlBuilder = urlBuilder;
            this.parser = parser;
        }

        public WaitableTask<EncounterNonImageContent> GetNonImageContent(User user, EncounterMetadata metadata)
        {
            var contentData = new WaitableTask<EncounterNonImageContent>();

            var webRequest = GetWebRequest(user, metadata);
            var serverResult = serverReader.Begin(webRequest);
            serverResult.AddOnCompletedListener((result) => ProcessResults(contentData, result));

            return contentData;
        }

        private const string downloadPhp = "Test.php";
        private const string filenameArgument = "webfilename";
        private const string usernameVariable = "webusername";
        private const string passwordVariable = "webpassword";
        private const string modeVariable = "mode";
        private const string modeValue = "download";
        private const string columnVariable = "column";
        private const string columnValue = "xmlData";
        private const string accountIdVariable = "accountId";
        private UnityWebRequest GetWebRequest(User user, EncounterMetadata metadata)
        {
            var arguments = new UrlArgument[] {
                new UrlArgument(filenameArgument, $"{metadata.Filename}.ced"),
                new UrlArgument(usernameVariable, "clinical"),
                new UrlArgument(passwordVariable, "encounters"),
                new UrlArgument(modeVariable, modeValue),
                new UrlArgument(columnVariable, columnValue),
                new UrlArgument(accountIdVariable, metadata.AuthorAccountId.ToString()),
            };
            var url = urlBuilder.BuildUrl(downloadPhp, arguments);
            return UnityWebRequest.Get(url);
        }

        private void ProcessResults(WaitableTask<EncounterNonImageContent> result, TaskResult<string> serverResult)
        {
            result.SetResult(parser.Deserialize(serverResult.Value));
        }
    }
}