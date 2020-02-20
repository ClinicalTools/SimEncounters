using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ServerCasesInfoReader : ICasesInfoReader
    {
        public event Action<List<EncounterInfoGroup>> Completed;
        public List<EncounterInfoGroup> Results { get; protected set; }
        public bool IsDone { get; protected set; }

        public EncounterInfoParser EncounterInfoParser { get; }
        public ServerCasesInfoReader()
        {
            EncounterInfoParser = new EncounterInfoParser();
        }

        private const string serverAddress = "https://takecontrolgame.com/docs/games/CECreator/PHP/";
        private const string menuPhp = "Menu.php";
        private const string modeVariable = "mode";
        private const string getMenuCasesMode = "downloadForOneAccount";
        private const string accountVariable = "account_id";

        protected string GetMenuCasesUrl(string accountId) => MenuModeUrl(getMenuCasesMode, accountId);
        protected string MenuModeUrl(string mode, string accountId)
            => $"{serverAddress}{menuPhp}?{UrlVarString(modeVariable, mode)}{UrlVarString(accountVariable, accountId)}";
        protected string UrlVarString(string varName, string varValue) => $"&{varName}={varValue}";


        /**
         * Downloads all available and applicable menu files to display on the main manu.
         * Returns them as a MenuCase item
         */
        public void GetEncounterInfos(User user)
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

            Results = ReadServerEncounters(text);
            IsDone = true;
            Completed?.Invoke(Results);
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
        protected List<EncounterInfoGroup> ReadServerEncounters(string text)
        {
            var items = text.Split(new string[] { encounterDivider }, StringSplitOptions.None);
            List<EncounterInfoGroup> encounters = new List<EncounterInfoGroup>(items.Length);
            foreach (string item in items) {
                var encounterInfo = EncounterInfoParser.GetServerEncounter(item);
                if (encounterInfo != null)
                    encounters.Add(encounterInfo);
            }

            return encounters;
        }

    }
}