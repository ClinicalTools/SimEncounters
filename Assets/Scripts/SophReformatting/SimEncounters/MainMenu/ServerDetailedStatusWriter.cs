using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using UnityEngine.Networking;


namespace ClinicalTools.SimEncounters.MainMenu
{

    public class ServerDetailedStatusWriter : IDetailedStatusWriter
    {
        protected IUrlBuilder WebAddress { get; }
        protected ServerDataReader<string> EncounterDataReader { get; }
        public ServerDetailedStatusWriter(IUrlBuilder webAddress)
        {
            WebAddress = webAddress;
            var statusesParser = new StringParser();
            EncounterDataReader = new ServerDataReader<string>(statusesParser);
        }

        private const string phpFile = "Track.php";
        private const string actionVariable = "ACTION";
        private const string uploadAction = "upload2";
        private const string usernameVariable = "username";

        private const string recordNumberVariable = "recordNumber";
        private const string finishedVariable = "finished";
        private const string ratingVariable = "rating";

        private const string readTabsVariable = "readTabs";
        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public void DoStuff(User user, FullEncounter encounter)
        {
            if (user.IsGuest)
                return;

            var url = WebAddress.BuildUrl(phpFile);
            var form = CreateForm(user, encounter);

            var webRequest = UnityWebRequest.Post(url, form);
            EncounterDataReader.Completed += EncounterDataReader_Completed;
            EncounterDataReader.Begin(webRequest);
        }

        public WWWForm CreateForm(User user, FullEncounter encounter)
        {
            var form = new WWWForm();

            form.AddField(actionVariable, uploadAction);
            form.AddField(usernameVariable, user.Username);
            form.AddField(recordNumberVariable, encounter.Metadata.RecordNumber);
            form.AddField(readTabsVariable, string.Join(":", encounter.Status.ReadTabs));
            form.AddField(finishedVariable, encounter.Status.BasicStatus.Completed ? 1 : 0);
            form.AddField(ratingVariable, encounter.Status.BasicStatus.Rating);

            return form;
        }

        private void EncounterDataReader_Completed(object sender, ServerResult<string> e)
        {
        }
    }
}
