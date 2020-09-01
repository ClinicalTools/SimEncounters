
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

        private const string PHP_FILE = "Track.php";
        private const string ACTION_VARIABLE = "ACTION";
        private const string ACTION_VALUE = "upload2";
        private const string USERNAME_VARIABLE = "username";

        private const string RECORD_NUMBER_VARIABLE = "recordNumber";
        private const string FINISHED_VARIABLE = "finished";
        private const string RATING_VARIABLE = "rating";

        private const string READ_TABS_VARIABLE = "readTabs";

        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public void WriteStatus(UserEncounter encounter)
        {
            if (encounter.User.IsGuest)
                return;

            var url = WebAddress.BuildUrl(PHP_FILE);
            var form = CreateForm(encounter.User, encounter);

            var webRequest = UnityWebRequest.Post(url, form);
            var serverResults = ServerReader.Begin(webRequest);
            serverResults.AddOnCompletedListener(ProcessResults);
        }

        public WWWForm CreateForm(User user, UserEncounter encounter)
        {
            var form = new WWWForm();

            form.AddField(ACTION_VARIABLE, ACTION_VALUE);
            form.AddField(USERNAME_VARIABLE, user.Username);
            form.AddField(RECORD_NUMBER_VARIABLE, encounter.Data.Metadata.RecordNumber);
            var statusString = StatusSerializer.Serialize(encounter.Status.ContentStatus);
            form.AddField(READ_TABS_VARIABLE, statusString);
            form.AddField(FINISHED_VARIABLE, encounter.Status.ContentStatus.Read ? 1 : 0);
            form.AddField(RATING_VARIABLE, encounter.Status.BasicStatus.Rating);

            return form;
        }

        private void ProcessResults(WaitedResult<string> serverResult)
        {
        }
    }
}
