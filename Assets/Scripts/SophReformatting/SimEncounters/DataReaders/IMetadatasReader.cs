using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public interface IMetadataReader
    {
        WaitableResult<EncounterMetadata> GetMetadata(User user, EncounterMetadata metadata);
    }
    public class LocalMetadataReader : IMetadataReader
    {
        private readonly IFileManager fileManager;
        private readonly IParser<EncounterMetadata> parser;
        public LocalMetadataReader(IFileManager fileManager, IParser<EncounterMetadata> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableResult<EncounterMetadata> GetMetadata(User user, EncounterMetadata metadata)
        {
            var metadataResult = new WaitableResult<EncounterMetadata>();

            var fileText = fileManager.GetFileText(user, FileType.Metadata, metadata);
            fileText.AddOnCompletedListener((result) => ProcessResults(metadataResult, result));

            return metadataResult;
        }

        private void ProcessResults(WaitableResult<EncounterMetadata> result, WaitedResult<string> fileText)
        {
            if (fileText.Value == null) {
                result.SetError(null);
                return;
            }

            var metadata = parser.Parse(fileText.Value);
            result.SetResult(metadata);
        }
    }

    public interface IMetadatasReader
    {
        WaitableResult<List<EncounterMetadata>> GetMetadatas(User user);
    }
    public class LocalMetadatasReader : IMetadatasReader
    {
        private readonly IFileManager fileManager;
        private readonly IParser<EncounterMetadata> parser;
        public LocalMetadatasReader(IFileManager fileManager, IParser<EncounterMetadata> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableResult<List<EncounterMetadata>> GetMetadatas(User user)
        {
            var metadatas = new WaitableResult<List<EncounterMetadata>>();

            var fileTexts = fileManager.GetFilesText(user, FileType.Metadata);
            fileTexts.AddOnCompletedListener((result) => ProcessResults(metadatas, result));

            return metadatas;
        }

        private void ProcessResults(WaitableResult<List<EncounterMetadata>> result, WaitedResult<string[]> fileTexts)
        {
            if (fileTexts == null) {
                result.SetError(null);
                return;
            }

            var metadatas = new List<EncounterMetadata>();
            foreach (var fileText in fileTexts.Value) {
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
        private readonly IParser<List<EncounterMetadata>> parser;
        public ServerMetadatasReader(IUrlBuilder urlBuilder, IServerReader serverReader, IParser<List<EncounterMetadata>> parser)
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


        private void ProcessResults(WaitableResult<List<EncounterMetadata>> result, WaitedResult<ServerResult> serverOutput)
        {
            if (serverOutput == null || serverOutput.Value.Outcome != ServerOutcome.Success) {
                result.SetError(new Exception(serverOutput?.Value.Message));
                return;
            }

            var metadatas = parser.Parse(serverOutput.Value.Message);
            result.SetResult(metadatas);
        }
    }
}
