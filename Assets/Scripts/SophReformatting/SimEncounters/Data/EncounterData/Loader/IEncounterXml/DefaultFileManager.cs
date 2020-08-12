using ClinicalTools.SimEncounters.Data;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class DefaultFileManager : IFileManager
    {
        protected string DefaultDirectory => Application.streamingAssetsPath + "/Default/";
        protected string EncounterFilename => "encounter";

        private readonly IFileExtensionManager fileExtensionManager;
        private readonly IServerReader serverReader;
        public DefaultFileManager(IFileExtensionManager fileExtensionManager, IServerReader serverReader)
        {
            this.fileExtensionManager = fileExtensionManager;
            this.serverReader = serverReader;
        }

        public void SetFileText(User user, FileType fileType, EncounterMetadata metadata, string contents)
            => throw new Exception("Cannot write to default files");

        public WaitableResult<string> GetFileText(User user, FileType fileType, EncounterMetadata metadata)
        {
            var fileText = new WaitableResult<string>();

            var filePath = GetFile(fileType);
            var webRequest = UnityWebRequest.Get(filePath);
            var serverResult = serverReader.Begin(webRequest);
            serverResult.AddOnCompletedListener((result) => SetFileResult(result, fileText));
            return fileText;
        }

        protected virtual void SetFileResult(WaitedResult<string> serverResult, WaitableResult<string> fileText)
        {
            if (serverResult.IsError())
                fileText.SetError(serverResult.Exception);
            else
                fileText.SetResult(serverResult.Value);
        }

        public WaitableResult<string[]> GetFilesText(User user, FileType fileType)
            => new WaitableResult<string[]>(new Exception());

        protected string GetFile(FileType fileType)
        {
            var path = Path.Combine(DefaultDirectory, EncounterFilename);
            var extension = fileExtensionManager.GetExtension(fileType);
            return $"{path}.{extension}";
        }

        public void UpdateFilename(User user, EncounterMetadata metadata)
            => throw new Exception("Cannot update names of default files");
        public void DeleteFiles(User user, EncounterMetadata metadata)
            => throw new Exception("Cannot delete default files");
    }
}