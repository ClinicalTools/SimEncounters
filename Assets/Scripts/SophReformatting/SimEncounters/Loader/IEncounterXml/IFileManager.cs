using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters
{
    public interface IFileManager
    {
        //string GetFile(User user, FileType fileType, EncounterMetadata metadata);
        //WaitableResult<string[]> GetFile(User user, FileType fileType);
        void SetFileText(User user, FileType fileType, EncounterMetadata metadata, string contents);

        WaitableResult<string> GetFileText(User user, FileType fileType, EncounterMetadata metadata);
        WaitableResult<string[]> GetFilesText(User user, FileType fileType);
    }
    public class UserFileManager : IFileManager
    {
        protected virtual string LocalSavesPath => Application.persistentDataPath + "/LocalSaves/";

        private readonly IFileExtensionManager fileExtensionManager;
        public UserFileManager(IFileExtensionManager fileExtensionManager)
        {
            this.fileExtensionManager = fileExtensionManager;
        }

        public void SetFileText(User user, FileType fileType, EncounterMetadata metadata, string contents)
        {
            var filepath = GetFilepath(user, fileType, metadata);
            File.WriteAllText(filepath, contents);
        }
        public WaitableResult<string> GetFileText(User user, FileType fileType, EncounterMetadata metadata)
        {
            var filepath = GetFilepath(user, fileType, metadata);
            var text = File.ReadAllText(filepath);
            return new WaitableResult<string>(text);
        }

        public WaitableResult<string[]> GetFilesText(User user, FileType fileType)
        {
            var filepaths = GetFilepaths(user, fileType);
            var texts = new string[filepaths.Length];
            for (int i = 0; i < filepaths.Length; i++)
                texts[i] = File.ReadAllText(filepaths[i]);

            return new WaitableResult<string[]>(texts);
        }

        protected string GetFilepath(User user, FileType fileType, EncounterMetadata metadata)
        {
            var folder = GetFolder(user);
            var path = Path.Combine(folder, metadata.Filename);
            var extension = fileExtensionManager.GetExtension(fileType);
            return $"{path}.{extension}";
        }

        protected string[] GetFilepaths(User user, FileType fileType)
        {
            var folder = GetFolder(user);
            var extension = fileExtensionManager.GetExtension(fileType);
            var filepaths = Directory.GetFiles(folder, $"*.{extension}");

            return filepaths;
        }

        ///<summary>
        ///Returns a truncated md5 hash to represent unique folders for users. This returns only the folder
        ///</summary>
        ///<param name="accountId">User account id</param>
        protected string GetFolder(User user)
        {
            string accountStr;
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(user.AccountId.ToString()));
                StringBuilder sb = new StringBuilder();
                foreach (var b in bytes)
                    sb.Append(b.ToString("x2"));

                accountStr = sb.ToString().Substring(7, 10); //Return a random 10 digit substring of the hash to represent the folder name
            }

            var path = Path.Combine(LocalSavesPath, accountStr);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }
    }
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
        {
            throw new Exception("Cannot write to demo files");
        }

        public WaitableResult<string> GetFileText(User user, FileType fileType, EncounterMetadata metadata)
        {
            return new WaitableResult<string>("");
        }

        public WaitableResult<string[]> GetFilesText(User user, FileType fileType)
        {
            var filesText = new WaitableResult<string[]>();
            var demoEncounters = GetDemoEncounters();
            demoEncounters.AddOnCompletedListener((result) => ReadFiles(result, filesText, fileType));

            return filesText;
        }

        protected void ReadFiles(string[] demoEncounters, WaitableResult<string[]> result, FileType fileType) {
            if (demoEncounters == null)
                return;

            var serverResults = new WaitableResult<ServerResult>[demoEncounters.Length];
            for (int i = 0; i <demoEncounters.Length; i++) {
                var filePath = GetFile(fileType, demoEncounters[i]);
                var webRequest = new UnityWebRequest(filePath);
                serverResults[i] = serverReader.Begin(webRequest);
                serverResults[i].AddOnCompletedListener((serverResult) => SetFileResults(result, serverResults));
            }
        }

        protected void SetFileResults(WaitableResult<string[]> result, WaitableResult<ServerResult>[] serverResults) { 
            foreach (var serverResult in serverResults) {
                if (serverResult == null || !serverResult.IsCompleted)
                    return;
            }

            var filesTexts = new List<string>();
            foreach (var serverResult in serverResults) {
                if (serverResult.IsError || serverResult.Result.Outcome != ServerOutcome.Success)
                    continue;

                filesTexts.Add(serverResult.Result.Message);
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
            var webRequest = new UnityWebRequest(demoEncountersPath);
            var serverResult = serverReader.Begin(webRequest);
            serverResult.AddOnCompletedListener(SetEncounters);

            return demoEncounters;
        }

        protected void SetEncounters(ServerResult serverResult)
        {
            if (demoEncounters == null || demoEncounters.IsCompleted)
                return;

            if (serverResult.Outcome != ServerOutcome.Success)
                demoEncounters.SetError("Could not get demo encounters from file.");

            demoEncounters.SetResult(serverResult.Message.Split('\n'));
        }
    }
}