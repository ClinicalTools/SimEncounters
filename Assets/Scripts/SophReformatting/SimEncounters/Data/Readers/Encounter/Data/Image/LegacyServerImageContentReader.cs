using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class LegacyServerImageContentReader : IImageContentReader
    {
        private readonly IServerReader serverReader;
        private readonly IUrlBuilder urlBuilder;
        private readonly IStringDeserializer<EncounterImageContent> parser;
        public LegacyServerImageContentReader(IServerReader serverReader, IUrlBuilder urlBuilder, IStringDeserializer<EncounterImageContent> parser)
        {
            this.serverReader = serverReader;
            this.urlBuilder = urlBuilder;
            this.parser = parser;
        }

        public WaitableTask<EncounterImageContent> GetImageData(User user, EncounterMetadata metadata)
        {
            var imageData = new WaitableTask<EncounterImageContent>();

            var webRequest = GetWebRequest(user, metadata);
            var serverResult = serverReader.Begin(webRequest);
            serverResult.AddOnCompletedListener((result) => ProcessResults(imageData, result));

            return imageData;
        }

        private const string downloadPhp = "Test.php";
        private const string filenameArgument = "webfilename";
        private const string usernameVariable = "webusername";
        private const string passwordVariable = "webpassword";
        private const string modeVariable = "mode";
        private const string modeValue = "download";
        private const string columnVariable = "column";
        private const string columnValue = "imgData";
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

        private void ProcessResults(WaitableTask<EncounterImageContent> result, TaskResult<string> serverResult)
        {
            result.SetResult(parser.Deserialize(serverResult.Value));
        }
    }
}