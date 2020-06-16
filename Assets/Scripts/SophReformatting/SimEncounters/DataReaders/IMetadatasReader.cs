using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public interface IMetadatasReader
    {
        WaitableResult<List<IEncounterMetadata>> GetMetadatas(User user);
    }
    public class LocalMetadatasReader : IMetadatasReader
    {
        private readonly IFileManager fileManager;
        private readonly IParser<IEncounterMetadata> parser;
        public LocalMetadatasReader(IFileManager fileManager, IParser<IEncounterMetadata> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableResult<List<IEncounterMetadata>> GetMetadatas(User user)
        {
            var metadatas = new WaitableResult<List<IEncounterMetadata>>();

            var fileTexts = fileManager.GetFilesText(user, FileType.Metadata);
            fileTexts.AddOnCompletedListener((result) => ProcessResults(metadatas, result));

            return metadatas;
        }

        private void ProcessResults(WaitableResult<List<IEncounterMetadata>> result, string[] fileTexts)
        {
            if (fileTexts == null)
            {
                result.SetError(null);
                return;
            }

            var metadatas = new List<IEncounterMetadata>();
            foreach (var fileText in fileTexts)
            {
                var metadata = parser.Parse(fileText);
                if (metadata != null)
                    metadatas.Add(metadata);
            }

            result.SetResult(metadatas);
        }
    }
    public class ServerMetadatasReader : IMetadatasReader
    {
        private readonly IUrlBuilder urlBuilder;
        private readonly IServerReader serverReader;
        private readonly IParser<List<IEncounterMetadata>> parser;
        public ServerMetadatasReader(IUrlBuilder urlBuilder, IServerReader serverReader, IParser<List<IEncounterMetadata>> parser)
        {
            this.urlBuilder = urlBuilder;
            this.serverReader = serverReader;
            this.parser = parser;
        }

        public WaitableResult<List<IEncounterMetadata>> GetMetadatas(User user)
        {
            var webRequest = GetWebRequest(user);
            var serverOutput = serverReader.Begin(webRequest);
            var metadatas = new WaitableResult<List<IEncounterMetadata>>();
            serverOutput.AddOnCompletedListener((result) => ProcessResults(metadatas, result));

            return metadatas;
        }

        private const string menuPhp = "Menu.php";
        private const string modeVariable = "mode";
        private const string modeValue = "downloadForOneAccount";
        private const string accountVariable = "account_id";

        private UnityWebRequest GetWebRequest(User user)
        {
            var arguments = new UrlArgument[] {
                new UrlArgument(modeVariable, modeValue),
                new UrlArgument(accountVariable, user.AccountId.ToString())
            };
            var url = urlBuilder.BuildUrl(menuPhp, arguments);
            return UnityWebRequest.Get(url);
        }


        private void ProcessResults(WaitableResult<List<IEncounterMetadata>> result, ServerResult serverOutput)
        {
            if (serverOutput == null || serverOutput.Outcome != ServerOutcome.Success)
            {
                result.SetError(serverOutput?.Message);
                return;
            }

            var metadatas = parser.Parse(serverOutput.Message);
            result.SetResult(metadatas);
        }
    }
}
 