using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class DoubleTildeStringSplitter : IStringSplitter
    {
        private const string divider = "~~";
        public string[] Split(string str) => str.Split(new string[] { divider }, StringSplitOptions.None);
    }
    public class DoubleColonStringSplitter : IStringSplitter
    {
        private const string divider = "::";
        public string[] Split(string str) => str.Split(new string[] { divider }, StringSplitOptions.None);
    }

    public class ServerCasesInfoReader : IEncountersInfoReader
    {
        public event Action<List<EncounterDetail>> Completed;
        public List<EncounterDetail> Result { get; protected set; }
        public bool IsDone { get; protected set; }

        protected ServerDataReader<List<EncounterDetail>> EncounterDataReader { get; }
        protected IWebAddress WebAddress { get; }
        public ServerCasesInfoReader(IWebAddress webAddress)
        {
            WebAddress = webAddress;
            var encounterDetailParser = new EncounterDetailParser(new EncounterServerInfoSetter());
            var listParser = new ListParser<EncounterDetail>(encounterDetailParser, new DoubleColonStringSplitter());
            EncounterDataReader = new ServerDataReader<List<EncounterDetail>>(listParser);
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
        public void GetEncounterInfos(User user)
        {
            var url = GetMenuCasesUrl(user.AccountId.ToString());

            var webRequest = UnityWebRequest.Get(url);
            EncounterDataReader.Completed += EncounterDataReader_Completed;
            EncounterDataReader.Begin(webRequest);

            Debug.LogError(url);
        }

        private void EncounterDataReader_Completed(object sender, ServerResult<List<EncounterDetail>> e)
        {
            Result = e.Result;
            IsDone = true;
            Completed?.Invoke(Result);
        }
    }
}