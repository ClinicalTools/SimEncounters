using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class DemoPathManager : IFilePathManager
    {
        protected virtual string DemoDirectory => Application.streamingAssetsPath + "/DemoCases/";

        public virtual string DataFilePath(string filePath) => filePath + ".ced";
        public virtual string ImageFilePath(string filePath) => filePath + ".cei";
        public virtual string AutoSaveDataFilePath(string filePath) => null;
        public virtual string AutoSaveImageFilePath(string filePath) => null;
        public virtual string StatusFilePath(string filePath) => null;
        public virtual string DetailedStatusFilePath(string filePath) => null;
        public virtual string EncounterFilePath(User user, EncounterInfo encounterInfo) => GetLocalSavesFolder(user) + encounterInfo.MetaGroup.Filename.Replace('_', ' ');

        ///<summary>
        ///Returns a truncated md5 hash to represent unique folders for users. This returns only the folder
        ///</summary>
        ///<param name="accountId">User account id</param>
        public virtual string GetLocalSavesFolder(User user) => DemoDirectory;
    }
    public class FilePathManager : IFilePathManager
    {
        protected virtual string LocalSavesPath => Application.persistentDataPath + "/LocalSaves/";

        public virtual string DataFilePath(string filePath) => filePath + ".ced";
        public virtual string ImageFilePath(string filePath) => filePath + ".cei";
        public virtual string AutoSaveDataFilePath(string filePath) => filePath + ".auto";
        public virtual string AutoSaveImageFilePath(string filePath) => filePath + ".iauto";
        public virtual string StatusFilePath(string filePath) => filePath + ".ces";
        public virtual string DetailedStatusFilePath(string filePath) => filePath + ".cesd";
        public virtual string EncounterFilePath(User user, EncounterInfo encounterInfo) => GetLocalSavesFolder(user) + encounterInfo.MetaGroup.Filename.Replace('_', ' ');


        ///<summary>
        ///Returns a truncated md5 hash to represent unique folders for users. This returns only the folder
        ///</summary>
        ///<param name="accountId">User account id</param>
        public virtual string GetLocalSavesFolder(User user)
        {
            string accountStr;
            using (MD5 md5 = MD5.Create()) {
                byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(user.AccountId.ToString()));
                StringBuilder sb = new StringBuilder();
                foreach (var b in bytes)
                    sb.Append(b.ToString("x2"));

                accountStr = sb.ToString().Substring(7, 10); //Return a random 10 digit substring of the hash to represent the folder name
            }

            var path = $"{LocalSavesPath}{accountStr}/";
            //path = $"{LocalSavesPath}abc\\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }
    }
}