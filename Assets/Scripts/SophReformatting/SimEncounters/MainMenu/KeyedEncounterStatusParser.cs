using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class KeyedEncounterStatusParser : IParser<KeyValuePair<int, EncounterBasicStatus>>
    {
        private const string caseInfoDivider = "::";

        public KeyValuePair<int, EncounterBasicStatus> Parse(string text)
        {
            var parsedText = GetParsedEncounterText(text);

            return GetEncounterStatus(parsedText);
        }

        protected virtual string[] GetParsedEncounterText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            //Split each data string of the current MenuCase, each string divided by "--"
            return text.Split(new string[] { caseInfoDivider }, StringSplitOptions.None);
        }
        private const int encounterParts = 2;

        private const int recordNumberIndex = 0;
        private const int completedIndex = 1;

        protected KeyValuePair<int, EncounterBasicStatus> GetEncounterStatus(string[] parsedItem)
        {
            if (parsedItem == null || parsedItem.Length < encounterParts)
                return new KeyValuePair<int, EncounterBasicStatus>();

            var encounterStatus = new EncounterBasicStatus();

            if (!int.TryParse(parsedItem[recordNumberIndex], out var recordNumber))
                return new KeyValuePair<int, EncounterBasicStatus>();

            encounterStatus.Completed = parsedItem[completedIndex] == "1";

            return new KeyValuePair<int, EncounterBasicStatus>(recordNumber, encounterStatus);
        }
    }
    public class DetailedStatusParser : IParser<EncounterDetailedStatus>
    {
        private const string caseInfoDivider = "::";

        public EncounterDetailedStatus Parse(string text)
        {
            var parsedText = GetParsedEncounterText(text);

            return GetEncounterStatus(parsedText);
        }

        protected virtual string[] GetParsedEncounterText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            //Split each data string of the current MenuCase, each string divided by "--"
            return text.Split(new string[] { caseInfoDivider }, StringSplitOptions.None);
        }
        private const int encounterParts = 1;
        private const int readTabsIndex = 0;
        protected EncounterDetailedStatus GetEncounterStatus(string[] parsedItem)
        {
            if (parsedItem == null || parsedItem.Length < encounterParts || parsedItem[0].Length > 100)
                return null;

            var encounterInfo = new EncounterDetailedStatus();
            AddReadTabs(encounterInfo.ReadTabs, parsedItem[readTabsIndex]);
                
            return encounterInfo;
        }

        protected virtual void AddReadTabs(HashSet<string> readTabs, string tabText)
        {
            var parsedTabs = tabText.Split(':');
            foreach (var tab in parsedTabs) {
                if (tab.Length > 20)
                    continue;
                if (!readTabs.Contains(tab))
                    readTabs.Add(tab);
            }
        }
    }
}