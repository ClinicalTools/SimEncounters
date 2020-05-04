using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using UnityEngine.Networking;


namespace ClinicalTools.SimEncounters
{

    public class ServerDetailedStatusWriter : IDetailedStatusWriter
    {
        protected IUrlBuilder WebAddress { get; }
        protected IServerReader ServerReader { get; }
        protected EncounterStatusSerializer StatusSerializer { get; }
        public ServerDetailedStatusWriter(IUrlBuilder webAddress, IServerReader serverReader, 
            EncounterStatusSerializer statusSerializer)
        {
            WebAddress = webAddress;
            ServerReader = serverReader;
            StatusSerializer = statusSerializer;
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
        public void WriteStatus(UserEncounter encounter)
        {
            if (encounter.User.IsGuest)
                return;

            var url = WebAddress.BuildUrl(phpFile);
            var form = CreateForm(encounter.User, encounter);

            var webRequest = UnityWebRequest.Post(url, form);
            var serverResults = ServerReader.Begin(webRequest);
            serverResults.AddOnCompletedListener(ProcessResults);
        }

        public WWWForm CreateForm(User user, UserEncounter encounter)
        {
            var form = new WWWForm();

            form.AddField(actionVariable, uploadAction);
            form.AddField(usernameVariable, user.Username);
            form.AddField(recordNumberVariable, encounter.Metadata.RecordNumber);
            var statusString = StatusSerializer.Serialize(encounter.Status.ContentStatus);
            form.AddField(readTabsVariable, statusString);
            form.AddField(finishedVariable, encounter.Status.ContentStatus.Read ? 1 : 0);
            form.AddField(ratingVariable, encounter.Status.BasicStatus.Rating);

            return form;
        }

        private void ProcessResults(ServerResult serverResult)
        {
        }
    }
}
