using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ServerEncounterStatusesReader0 : IEncounterStatusesReader
    {
        public event Action<Dictionary<string, UserEncounterStatus>> Completed;
        public Dictionary<string, UserEncounterStatus> Result { get; protected set; }
        public bool IsDone { get; protected set; }

        public EncounterStatusParser EncounterStatusParser { get; }
        public IWebAddress WebAddress { get; }
        protected ServerDataReader<Dictionary<string, UserEncounterStatus>> EncounterDataReader { get; }
        public ServerEncounterStatusesReader0(IWebAddress webAddress)
        {
            WebAddress = webAddress;
            EncounterStatusParser = new EncounterStatusParser();
        }

        private const string menuPhp = "Menu.php";
        private const string modeVariable = "mode";
        private const string getMenuCasesMode = "downloadForOneAccount";
        private const string accountVariable = "account_id";

        protected string GetMenuCasesUrl(string accountId) => MenuModeUrl(getMenuCasesMode, accountId);
        protected string MenuModeUrl(string mode, string accountId)
        {
            WebAddress.AddArgument(modeVariable, mode);
            WebAddress.AddArgument(accountVariable, accountId);
            return WebAddress.GetUrl(menuPhp);
        }

        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public void GetEncounterStatuses(User user)
        {
            var url = GetMenuCasesUrl(user.AccountId.ToString());

            var webRequest = UnityWebRequest.Get(url);
            var requestOperation = webRequest.SendWebRequest();
            requestOperation.completed += (asyncOperation) => ProcessWebrequest(webRequest);

            Debug.LogError(url);
        }

        protected void ProcessWebrequest(UnityWebRequest webRequest)
        {
            var text = GetWebRequestText(webRequest);
            webRequest.Dispose();

            Result = ReadServerEncounters(text);
            IsDone = true;
            Completed?.Invoke(Result);
        }

        protected string GetWebRequestText(UnityWebRequest webRequest)
        {
            if (!webRequest.isDone)
                return null;
            else if (webRequest.isNetworkError || webRequest.isHttpError)
                return null;
            else if (!webRequest.downloadHandler.isDone)
                return null;
            else
                return webRequest.downloadHandler.text;
        }

        private const string encounterDivider = "::";
        protected Dictionary<string, UserEncounterStatus> ReadServerEncounters(string text)
        {
            if (text == null)
                return null;
            var items = text.Split(new string[] { encounterDivider }, StringSplitOptions.None);
            Dictionary<string, UserEncounterStatus> encounters = new Dictionary<string, UserEncounterStatus>(items.Length);
            foreach (string item in items)
            {
                var encounterStatusPair = EncounterStatusParser.Parse(item);
                if (encounterStatusPair.Value != null && !encounters.ContainsKey(encounterStatusPair.Key))
                    encounters.Add(encounterStatusPair.Key, encounterStatusPair.Value);
            }

            return encounters;
        }
    }
}
