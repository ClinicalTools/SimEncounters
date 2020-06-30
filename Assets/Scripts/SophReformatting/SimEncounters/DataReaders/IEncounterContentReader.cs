using ClinicalTools.SimEncounters.Data;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterContentReader
    {
        WaitableResult<EncounterContent> GetEncounterContent(User user, EncounterMetadata metadata);
    }
    public class LocalContentDataReader : IEncounterContentReader
    {
        private readonly IFileManager fileManager;
        private readonly IParser<EncounterContent> parser;
        public LocalContentDataReader(IFileManager fileManager, IParser<EncounterContent> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableResult<EncounterContent> GetEncounterContent(User user, EncounterMetadata metadata)
        {
            var content = new WaitableResult<EncounterContent>();

            var fileText = fileManager.GetFileText(user, FileType.Data, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(content, result));

            return content;
        }

        private void ProcessResults(WaitableResult<EncounterContent> result, WaitedResult<string> fileText)
        {
            result.SetResult(parser.Parse(fileText.Value));
        }
    }
    public class ServerContentDataReader : IEncounterContentReader
    {
        private readonly IServerReader serverReader;
        private readonly IUrlBuilder urlBuilder;
        private readonly IParser<EncounterContent> parser;
        public ServerContentDataReader(IServerReader serverReader, IUrlBuilder urlBuilder, IParser<EncounterContent> parser)
        {
            this.serverReader = serverReader;
            this.urlBuilder = urlBuilder;
            this.parser = parser;
        }

        public WaitableResult<EncounterContent> GetEncounterContent(User user, EncounterMetadata metadata)
        {
            var contentData = new WaitableResult<EncounterContent>();

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

        private void ProcessResults(WaitableResult<EncounterContent> result, WaitedResult<ServerResult> serverResult)
        {
            result.SetResult(parser.Parse(serverResult.Value.Message));
        }
    }
}