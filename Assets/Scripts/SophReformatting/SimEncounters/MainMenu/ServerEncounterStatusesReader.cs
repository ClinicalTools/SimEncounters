﻿using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
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

        private const string phpFile = "Track2.php";
        private const string actionVariable = "ACTION";
        private const string uploadAction = "upload2";
        private const string usernameVariable = "username";

        private const string recordNumberVariable = "recordNumber";
        private const string finishedVariable = "finished";

        private const string readPanelsVariable = "readPanels";
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
            form.AddField(readPanelsVariable, string.Join(":", encounter.Status.ReadTabs));
            form.AddField(finishedVariable, encounter.Info.UserStatus.Completed ? 1 : 0);

            return form;
        }

        private void EncounterDataReader_Completed(object sender, ServerResult<string> e)
        {
        }
    }
    public class ServerEncounterStatusesReader : IEncounterStatusesReader
    {
        public event Action<Dictionary<int, EncounterBasicStatus>> Completed;
        public Dictionary<int, EncounterBasicStatus> Result { get; protected set; }
        public bool IsDone { get; protected set; }

        protected IWebAddress WebAddress { get; }
        protected ServerDataReader<Dictionary<int, EncounterBasicStatus>> EncounterDataReader { get; }
        public ServerEncounterStatusesReader(IWebAddress webAddress)
        {
            WebAddress = webAddress;
            var statusesParser = new DictionaryParser<int, EncounterBasicStatus>(new EncounterStatusParser(), new DoubleTildeStringSplitter());
            EncounterDataReader = new ServerDataReader<Dictionary<int, EncounterBasicStatus>>(statusesParser);
        }

        private const string menuPhp = "Track.php";
        private const string actionVariable = "ACTION";
        private const string downloadAction = "download";
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

        private const string menuPhp = "Track2.php";
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
