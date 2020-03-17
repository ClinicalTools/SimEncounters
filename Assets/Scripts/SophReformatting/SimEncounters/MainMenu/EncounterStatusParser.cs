using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class EncounterStatusParser : IParser<KeyValuePair<int, EncounterBasicStatus>>
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
        private const int filenameIndex = 1;
        private const int authorNameIndex = 2;
        private const int titleIndex = 3;
        private const int difficultyIndex = 5;
        private const int descriptionIndex = 7;
        private const int subtitleIndex = 6;
        private const int tagsIndex = 8;
        private const int dateModifiedIndex = 9;
        private const int audienceIndex = 10;
        private const int editorVersionIndex = 11;
        private const int ratingIndex = 12;
        private const int caseTypeIndex = 13;
        private const string filenameExtension = ".ced";

        protected virtual string GetFilename(string filename)
        {
            if (filename.EndsWith(filenameExtension, StringComparison.OrdinalIgnoreCase))
                filename = filename.Substring(0, filename.Length - filenameExtension.Length);
            return filename;
        }

        protected KeyValuePair<int, EncounterBasicStatus> GetEncounterStatus(string[] parsedItem)
        {
            if (parsedItem == null || parsedItem.Length < encounterParts)
                return new KeyValuePair<int, EncounterBasicStatus>();

            var encounterInfo = new EncounterBasicStatus();

            if (!int.TryParse(parsedItem[recordNumberIndex], out var recordNumber))
                return new KeyValuePair<int, EncounterBasicStatus>();

            return new KeyValuePair<int, EncounterBasicStatus>(recordNumber, encounterInfo);
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
        private const int panelsIndex = 0;
        protected EncounterDetailedStatus GetEncounterStatus(string[] parsedItem)
        {
            if (parsedItem == null || parsedItem.Length < encounterParts)
                return null;

            var encounterInfo = new EncounterDetailedStatus();
            

            return encounterInfo;
        }

        protected virtual void AddPanels(List<string> readPanels, string panelText)
        {
            var parsedPanels = panelText.Split(':');
            foreach (var panel in parsedPanels)
                readPanels.Add(panel);
        }
    }
}