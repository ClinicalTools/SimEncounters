using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Loading
{
    public class EncounterXml
    {
        public virtual Task<XmlDocument> CurrentEncounterCed { get; set; }
        public virtual Task<XmlDocument> CurrentEncounterCei { get; set; }
        public virtual string LocalSavesPath => Application.persistentDataPath + "\\LocalSaves\\";
        protected virtual string DemoCase => "Chad_Wright";
        protected virtual string DemoPath => Application.streamingAssetsPath + "/DemoCases/" + DemoCase;

        public EncounterXml() { }

        public string DataFilePath(string filePath) => filePath + ".ced";
        public string ImageFilePath(string filePath) => filePath + ".cei";
        public string AutoSaveDataFilePath(string filePath) => filePath + ".auto";
        public string AutoSaveImageFilePath(string filePath) => filePath + ".iauto";

        // change to different classes
        public virtual void GetServerEncounter(string fileName)
        {
            var dataFilePath = DataFilePath(fileName);
            var imageFilePath = ImageFilePath(fileName);

            /*
            using (DownloadEncounter cedDownloader = new DownloadEncounter(dataFilePath, "xmlData"))
            using (DownloadEncounter ceiDownloader = new DownloadEncounter(imageFilePath, "imgData")) {
                CurrentEncounterCed = cedDownloader.Run();
                CurrentEncounterCei = Task.Run(ceiDownloader.Run);
            }*/
        }

        public virtual void GetFileEncounter(int accountId, string fileName)
        {
            var filePath = GetLocalFolder(accountId) + fileName;

            var dataFilePath = DataFilePath(filePath);
            var imageFilePath = ImageFilePath(filePath);

            GetEncounterByFile(dataFilePath, imageFilePath);
        }

        public virtual void GetAutosaveEncounter(int accountId, string fileName)
        {
            var filePath = GetLocalFolder(accountId) + fileName;
            var dataFilePath = AutoSaveDataFilePath(filePath);
            var imageFilePath = AutoSaveImageFilePath(filePath);
            if (!File.Exists(imageFilePath))
                imageFilePath = ImageFilePath(filePath);

            GetEncounterByFile(dataFilePath, imageFilePath);
        }

        protected virtual void GetEncounterByFile(string cedFilePath, string ceiFilePath)
        {
            var readXml = new XmlParser();
            //CurrentEncounterCed = readXml.ReadXml(cedFilePath);
            //CurrentEncounterCei = readXml.ReadXml(ceiFilePath);
        }


        /**
         * Manually entering in the variables needed to load the chad case for the reader
         */
        public virtual void GetDemoCase()
        {
            var dataFilePath = DataFilePath(DemoPath);
            var imageFilePath = ImageFilePath(DemoPath);

            GetEncounterByFile(dataFilePath, imageFilePath);
        }

        ///<summary>
        ///Returns a truncated md5 hash to represent unique folders for users. This returns only the folder
        ///</summary>
        ///<param name="accountId">User account id</param>
        public string GetLocalFolder(int accountId)
        {
            string accountStr;
            using (MD5 md5 = MD5.Create()) {
                byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(accountId.ToString()));
                StringBuilder sb = new StringBuilder();
                foreach (var b in bytes)
                    sb.Append(b.ToString("x2"));

                accountStr = sb.ToString().Substring(7, 10); //Return a random 10 digit substring of the hash to represent the folder name
            }

            return LocalSavesPath + accountStr + '\\';
        }


        private string CreateFileName(MenuCase menuCase, string encounterName)
        {
            string recordNumber = (Mathf.Floor(Random.value * 999999) + "").PadLeft(6, '0');
            menuCase.recordNumber = recordNumber;
            string fileName = "[CHECKFORDUPLICATE]" + menuCase.recordNumber + encounterName + ".ced";

            while (File.Exists(fileName)) {
                recordNumber = (Mathf.Floor(Random.value * 999999) + "").PadLeft(6, '0');
                menuCase.recordNumber = recordNumber;
                fileName = "[CHECKFORDUPLICATE]" + menuCase.recordNumber + encounterName + ".ced";
            }

            return fileName;
        }
    }
}