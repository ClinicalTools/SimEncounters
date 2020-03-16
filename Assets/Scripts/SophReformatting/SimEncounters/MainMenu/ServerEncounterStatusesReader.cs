using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ServerEncounterStatusesReader : IEncounterStatusesReader
    {
        public event Action<Dictionary<int, EncounterBasicStatus>> Completed;
        public Dictionary<int, EncounterBasicStatus> Result { get; protected set; }
        public bool IsDone { get; protected set; }

        public IWebAddress WebAddress { get; }
        protected ServerDataReader<Dictionary<int, EncounterBasicStatus>> EncounterDataReader { get; }
        public ServerEncounterStatusesReader(IWebAddress webAddress)
        {
            WebAddress = webAddress;
            var statusesParser = new DictionaryParser<int, EncounterBasicStatus>(new EncounterStatusParser(), new DoubleTildeStringSplitter());
            EncounterDataReader = new ServerDataReader<Dictionary<int, EncounterBasicStatus>>(statusesParser);
        }

        private const string menuPhp = "Track.php";
        private const string actionVariable = "ACTION";
        private const string downloadAction = "download2";
        private const string usernameVariable = "username";

        private const string recordNumberVariable = "recordNumber";
        private const string recordNumber = "277231";

        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public void GetEncounterStatuses(User user)
        {
            var url = WebAddress.GetUrl(menuPhp);
            var form = new WWWForm();

            form.AddField(actionVariable, downloadAction);
            form.AddField(usernameVariable, user.Username);
            form.AddField(recordNumberVariable, recordNumber);

            var webRequest = UnityWebRequest.Post(url, form);
            EncounterDataReader.Completed += EncounterDataReader_Completed;
            EncounterDataReader.Begin(webRequest);
        }

        private void EncounterDataReader_Completed(object sender, ServerResult<Dictionary<int, EncounterBasicStatus>> e)
        {
            Result = e.Result;
            IsDone = true;
            Completed?.Invoke(Result);
        }
    }

    public class UpdateEncounterStatus
    {
        protected IWebAddress WebAddress { get; }
        protected ServerDataReader<string> EncounterDataReader { get; }
        public UpdateEncounterStatus(IWebAddress webAddress)
        {
            WebAddress = webAddress;
            EncounterDataReader = new ServerDataReader<string>(new StringParser());
        }

        private const string menuPhp = "Track.php";
        private const string actionVariable = "ACTION";
        private const string downloadAction = "download2";
        private const string usernameVariable = "username";

        private const string recordNumberVariable = "recordNumber";
        private const string recordNumber = "277231";

        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public void UPdateStatus(User user, EncounterBasicStatus encounterStatus)
        {
            var url = WebAddress.GetUrl(menuPhp);
            var form = new WWWForm();

            form.AddField(actionVariable, downloadAction);
            form.AddField(usernameVariable, user.Username);
            form.AddField(recordNumberVariable, recordNumber);

            var webRequest = UnityWebRequest.Post(url, form);
            EncounterDataReader.Completed += EncounterDataReader_Completed;
            EncounterDataReader.Begin(webRequest);
        }

        private void EncounterDataReader_Completed(object sender, ServerResult<string> e)
        {
        }
    }
}
