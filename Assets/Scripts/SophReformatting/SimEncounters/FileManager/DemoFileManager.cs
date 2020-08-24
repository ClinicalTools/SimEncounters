
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public class DemoFileManager : IFileManager
    {
        protected string DemoDirectory => Application.streamingAssetsPath + "/DemoCases/";
        protected string EncountersListFilename => "list.txt";

        private readonly IFileExtensionManager fileExtensionManager;
        private readonly IServerReader serverReader;
        public DemoFileManager(IFileExtensionManager fileExtensionManager, IServerReader serverReader)
        {
            this.fileExtensionManager = fileExtensionManager;
            this.serverReader = serverReader;
        }

        public void SetFileText(User user, FileType fileType, EncounterMetadata metadata, string contents)
            => throw new Exception("Cannot write to demo files");

        public WaitableResult<string> GetFileText(User user, FileType fileType, EncounterMetadata metadata)
        {
            var fileText = new WaitableResult<string>();

            var filePath = GetFile(fileType, metadata.Filename);
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
        {
            var filesText = new WaitableResult<string[]>();
            var demoEncounters = GetDemoEncounters();
            demoEncounters.AddOnCompletedListener((result) => ReadFiles(result, filesText, fileType));

            return filesText;
        }

        protected void ReadFiles(WaitedResult<string[]> demoEncounters, WaitableResult<string[]> result, FileType fileType) {
            if (demoEncounters == null)
                return;

            var serverResults = new WaitableResult<string>[demoEncounters.Value.Length];
            for (int i = 0; i <demoEncounters.Value.Length; i++) {
                var filePath = GetFile(fileType, demoEncounters.Value[i]);
                var webRequest = UnityWebRequest.Get(filePath);
                serverResults[i] = serverReader.Begin(webRequest);
                serverResults[i].AddOnCompletedListener((serverResult) => SetFilesResults(result, serverResults));
            }
        }

        protected void SetFilesResults(WaitableResult<string[]> result, WaitableResult<string>[] serverResults) { 
            foreach (var serverResult in serverResults) {
                if (serverResult == null || !serverResult.IsCompleted())
                    return;
            }

            var filesTexts = new List<string>();
            foreach (var serverResult in serverResults) {
                if (!serverResult.Result.IsError())
                    filesTexts.Add(serverResult.Result.Value);
            }

            result.SetResult(filesTexts.ToArray());
        }

        protected string GetFile(FileType fileType, string filename)
        {
            var path = Path.Combine(DemoDirectory, filename);
            var extension = fileExtensionManager.GetExtension(fileType);
            return $"{path}.{extension}";
        }

        private WaitableResult<string[]> demoEncounters;
        protected WaitableResult<string[]> GetDemoEncounters()
        {
            if (demoEncounters != null)
                return demoEncounters;

            demoEncounters = new WaitableResult<string[]>();
            var demoEncountersPath = Path.Combine(DemoDirectory, EncountersListFilename);
            var webRequest = UnityWebRequest.Get(demoEncountersPath);
            var serverResult = serverReader.Begin(webRequest);
            serverResult.AddOnCompletedListener(SetEncounters);

            return demoEncounters;
        }

        protected void SetEncounters(WaitedResult<string> serverResult)
        {
            if (demoEncounters == null || demoEncounters.IsCompleted())
                return;

            if (serverResult.IsError())
                demoEncounters.SetError(new Exception("Could not get demo encounters from file."));

            var splitChars = new char[] { '\n', '\r' };
            var encounters = serverResult.Value.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
            demoEncounters.SetResult(encounters);
        }

        public void UpdateFilename(User user, EncounterMetadata metadata)
            => throw new Exception("Cannot update names of demo files");
        public void DeleteFiles(User user, EncounterMetadata metadata)
            => throw new Exception("Cannot delete demo files");
    }
}