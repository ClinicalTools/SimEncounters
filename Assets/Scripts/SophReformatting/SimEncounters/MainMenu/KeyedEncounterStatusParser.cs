using ModestTree.Util;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
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
        private const int ratingIndex = 2;
        private const int ratingIndex2 = 3;

        protected KeyValuePair<int, EncounterBasicStatus> GetEncounterStatus(string[] parsedItem)
        {
            if (parsedItem == null || parsedItem.Length < encounterParts)
                return new KeyValuePair<int, EncounterBasicStatus>();

            var encounterStatus = new EncounterBasicStatus();

            if (!int.TryParse(parsedItem[recordNumberIndex], out var recordNumber))
                return new KeyValuePair<int, EncounterBasicStatus>();

            encounterStatus.Completed = parsedItem[completedIndex] == "1";
            if (parsedItem.Length > ratingIndex && int.TryParse(parsedItem[ratingIndex], out var rating))
                encounterStatus.Rating = rating;
            if (parsedItem.Length > ratingIndex2 && int.TryParse(parsedItem[ratingIndex2], out var rating2))
                encounterStatus.Rating = rating2;

            return new KeyValuePair<int, EncounterBasicStatus>(recordNumber, encounterStatus);
        }
    }
}