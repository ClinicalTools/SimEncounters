using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


namespace ClinicalTools.SimEncounters.MainMenu
{
    public class DemoCasesInfoReader : IEncountersInfoReader
    {
        public event Action<List<EncounterInfo>> Completed;
        public List<EncounterInfo> Result { get; protected set; }
        public bool IsDone { get; protected set; }

        public IFilePathManager FilePathManager { get; }
        protected ServerDataReader<List<EncounterInfo>> EncounterDataReader { get; }
        public DemoCasesInfoReader(IFilePathManager filePathManager)
        {
            FilePathManager = filePathManager;
            var encounterDetailParser = new EncounterDetailParser(new EncounterDemoInfoSetter());
            var listParser = new ListParser<EncounterInfo>(encounterDetailParser, new DoubleColonStringSplitter());
            EncounterDataReader = new ServerDataReader<List<EncounterInfo>>(listParser);
        }

        public void GetEncounterInfos(User user)
        {
            var filePath = FilePathManager.GetLocalSavesFolder(user);
            filePath = Path.Combine(filePath, "encounters.txt");
            Debug.LogError("SophPATH: " + filePath);
            var webRequest = UnityWebRequest.Get(filePath);
            EncounterDataReader.Completed += EncounterDataReader_Completed;
            EncounterDataReader.Begin(webRequest);
        }

        private void EncounterDataReader_Completed(object sender, ServerResult<List<EncounterInfo>> e)
        {
            if (e.Outcome == ServerOutcome.HttpError)
                Debug.LogError("SophOUTCOME: " + e.Outcome);
            Result = e.Result;
            IsDone = true;
            Completed?.Invoke(Result);
        }
    }

    public class ServerDetailedStatusReader : IDetailedStatusReader
    {
        public event Action<EncounterDetailedStatus> Completed;
        public EncounterDetailedStatus DetailedStatus { get; protected set; }
        public bool IsDone { get; protected set; }

        public IWebAddress WebAddress { get; }
        protected ServerDataReader<EncounterDetailedStatus> StatusReader { get; }
        public ServerDetailedStatusReader(IWebAddress webAddress)
        {
            WebAddress = webAddress;
            var statusParser = new DetailedStatusParser();
            StatusReader = new ServerDataReader<EncounterDetailedStatus>(statusParser);
        }

        private const string menuPhp = "Track.php";
        private const string actionVariable = "ACTION";
        private const string downloadAction = "downloadDetails";
        private const string usernameVariable = "username";

        private const string recordNumberVariable = "recordNumber";

        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public void DoStuff(User user, EncounterInfo encounterInfo)
        {
            var url = WebAddress.GetUrl(menuPhp);
            var form = new WWWForm();

            form.AddField(actionVariable, downloadAction);
            form.AddField(usernameVariable, user.Username);
            form.AddField(recordNumberVariable, encounterInfo.MetaGroup.RecordNumber);

            var webRequest = UnityWebRequest.Post(url, form);
            StatusReader.Completed += EncounterDataReader_Completed;
            StatusReader.Begin(webRequest);
        }

        private void EncounterDataReader_Completed(object sender, ServerResult<EncounterDetailedStatus> e)
        {
            if (e.Result != null)
                DetailedStatus = e.Result;
            else
                DetailedStatus = new EncounterDetailedStatus();
            IsDone = true;
            Completed?.Invoke(DetailedStatus);
        }
    }
}
