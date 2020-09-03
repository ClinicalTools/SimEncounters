using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class KeyedEncounterStatusDeserializer : IStringDeserializer<KeyValuePair<int, EncounterBasicStatus>>
    {
        private const string CaseInfoDivider = "|";

        public KeyValuePair<int, EncounterBasicStatus> Deserialize(string text)
        {
            var parsedText = GetParsedEncounterText(text);

            return GetEncounterStatus(parsedText);
        }

        protected virtual string[] GetParsedEncounterText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            //Split each data string of the current MenuCase, each string divided by "--"
            return text.Split(new string[] { CaseInfoDivider }, StringSplitOptions.None);
        }
        private const int EncounterParts = 5;

        private const int RecordNumberIndex = 0;
        private const int ModifiedIndex = 1;
        private const int CompletedIndex = 2;
        private const int UserRatingIndex = 3;
        private const int TotalRatingIndex = 4;

        protected KeyValuePair<int, EncounterBasicStatus> GetEncounterStatus(string[] parsedItem)
        {
            if (parsedItem == null || parsedItem.Length < EncounterParts)
                return new KeyValuePair<int, EncounterBasicStatus>();

            var encounterStatus = new EncounterBasicStatus();

            if (!int.TryParse(parsedItem[RecordNumberIndex], out var recordNumber))
                return new KeyValuePair<int, EncounterBasicStatus>();

            encounterStatus.Completed = parsedItem[CompletedIndex] == "1";
            if (parsedItem.Length > UserRatingIndex && int.TryParse(parsedItem[UserRatingIndex], out var rating))
                encounterStatus.Rating = rating;
            if (parsedItem.Length > TotalRatingIndex && int.TryParse(parsedItem[TotalRatingIndex], out var rating2))
                encounterStatus.Rating = rating2;

            return new KeyValuePair<int, EncounterBasicStatus>(recordNumber, encounterStatus);
        }
    }
}