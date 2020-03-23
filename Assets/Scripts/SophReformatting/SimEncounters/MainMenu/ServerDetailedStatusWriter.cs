using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using UnityEngine.Networking;


namespace ClinicalTools.SimEncounters.MainMenu
{

    public class ServerDetailedStatusWriter : IDetailedStatusWriter
    {
        protected IWebAddress WebAddress { get; }
        protected ServerDataReader<string> EncounterDataReader { get; }
        public ServerDetailedStatusWriter(IWebAddress webAddress)
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
        public void DoStuff(User user, Encounter encounter)
        {
            if (user.IsGuest)
                return;

            var url = WebAddress.GetUrl(phpFile);
            var form = CreateForm(user, encounter);

            var webRequest = UnityWebRequest.Post(url, form);
            EncounterDataReader.Completed += EncounterDataReader_Completed;
            EncounterDataReader.Begin(webRequest);
        }

        public WWWForm CreateForm(User user, Encounter encounter)
        {
            var form = new WWWForm();

            form.AddField(actionVariable, uploadAction);
            form.AddField(usernameVariable, user.Username);
            form.AddField(recordNumberVariable, encounter.Info.MetaGroup.RecordNumber);
            form.AddField(readTabsVariable, string.Join(":", encounter.Status.ReadTabs));
            form.AddField(finishedVariable, encounter.Info.UserStatus.Completed ? 1 : 0);
            form.AddField(ratingVariable, encounter.Info.UserStatus.Rating);

            return form;
        }

        private void EncounterDataReader_Completed(object sender, ServerResult<string> e)
        {
        }
    }
}
