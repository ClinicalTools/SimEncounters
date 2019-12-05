using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class EncounterXml
    {
        public virtual Task<XmlDocument> CurrentEncounterCed { get; set; }
        public virtual Task<XmlDocument> CurrentEncounterCei { get; set; }
        public virtual string LocalSavesPath => Application.persistentDataPath + "\\LocalSaves\\";
        protected virtual string DemoCase => "Chad_Wright";
        protected virtual string DemoPath => Application.streamingAssetsPath + "/DemoCases/Cases/" + DemoCase;

        public EncounterXml() { }

        public async virtual Task GetServerEncounter(string fileName)
        {
            var cedFileName = fileName + ".ced";
            var ceiFileName = fileName + ".cei";

            using (DownloadEncounter cedDownloader = new DownloadEncounter(fileName, "xmlData"))
            using (DownloadEncounter ceiDownloader = new DownloadEncounter(fileName, "imgData")) {
                CurrentEncounterCed = cedDownloader.Run();
                CurrentEncounterCei = ceiDownloader.Run();

                await CurrentEncounterCed;
                await CurrentEncounterCei;
            }
        }

        public async virtual Task GetFileEncounter(int accountId, string fileName)
        {
            var filePath = GetLocalFolder(accountId) + fileName;

            var cedFilePath = filePath + ".ced";
            var ceiFilePath = filePath + ".cei";

            await GetEncounterByFile(cedFilePath, ceiFilePath);
        }

        public async Task GetAutosaveEncounter(int accountId, string fileName)
        {
            var filePath = GetLocalFolder(accountId) + fileName;
            var cedFilePath = filePath + ".auto";
            var ceiFilePath = filePath + ".iauto";
            if (!File.Exists(ceiFilePath))
                ceiFilePath = filePath + ".cei";

            await GetEncounterByFile(cedFilePath, ceiFilePath);
        }

        protected async virtual Task GetEncounterByFile(string cedFilePath, string ceiFilePath)
        {
            CurrentEncounterCed = ReadEncounterXml.ReadXml(cedFilePath);
            CurrentEncounterCei = ReadEncounterXml.ReadXml(ceiFilePath);

            await CurrentEncounterCed;
            await CurrentEncounterCei;
        }


        /**
         * Manually entering in the variables needed to load the chad case for the reader
         */
        public async virtual Task GetDemoCase()
        {
            var cedFilePath = DemoPath + ".ced";
            var ceiFilePath = DemoPath + ".cei";

            await GetEncounterByFile(cedFilePath, ceiFilePath);
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