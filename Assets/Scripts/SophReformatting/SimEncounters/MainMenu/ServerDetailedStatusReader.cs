using System;
using UnityEngine;
using UnityEngine.Networking;


namespace ClinicalTools.SimEncounters.MainMenu
{

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
