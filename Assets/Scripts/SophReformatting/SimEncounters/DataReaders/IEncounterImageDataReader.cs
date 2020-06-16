using ClinicalTools.SimEncounters.Data;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public interface IImageDataReader
    {
        WaitableResult<EncounterImageData> GetImageData(User user, IEncounterMetadata metadata);
    }
    public class LocalImageDataReader : IImageDataReader
    {
        private readonly IFileManager fileManager;
        private readonly IParser<EncounterImageData> parser;
        public LocalImageDataReader(IFileManager fileManager, IParser<EncounterImageData> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableResult<EncounterImageData> GetImageData(User user, IEncounterMetadata metadata)
        {
            var imageData = new WaitableResult<EncounterImageData>();

            var fileText = fileManager.GetFileText(user, FileType.Image, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(imageData, result));

            return imageData;
        }

        private void ProcessResults(WaitableResult<EncounterImageData> result, string fileText)
        {
            result.SetResult(parser.Parse(fileText));
        }
    }
    public class ServerImageDataReader : IImageDataReader
    {
        private readonly IServerReader serverReader;
        private readonly IUrlBuilder urlBuilder;
        private readonly IParser<EncounterImageData> parser;
        public ServerImageDataReader(IServerReader serverReader, IUrlBuilder urlBuilder, IParser<EncounterImageData> parser)
        {
            this.serverReader = serverReader;
            this.urlBuilder = urlBuilder;
            this.parser = parser;
        }

        public WaitableResult<EncounterImageData> GetImageData(User user, IEncounterMetadata metadata)
        {
            var imageData = new WaitableResult<EncounterImageData>();

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
        private UnityWebRequest GetWebRequest(User user, IEncounterMetadata metadata)
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

        private void ProcessResults(WaitableResult<EncounterImageData> result, ServerResult serverResult)
        {
            result.SetResult(parser.Parse(serverResult.Message));
        }
    }
}