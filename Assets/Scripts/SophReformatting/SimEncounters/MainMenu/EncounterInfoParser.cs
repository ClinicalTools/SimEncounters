using ClinicalTools.SimEncounters.Data;
using System;
using static MenuCase;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class EncounterInfoParser
    {
        public virtual EncounterInfoGroup GetAutosaveEncounter(string text)
        {
            var parsedItem = GetParsedEncounterText(text);
            var encounter = GetEncounterInfoGroup(parsedItem);
            if (encounter != null)
                encounter.AutosaveInfo = GetEncounterInfo(parsedItem);
            return encounter;
        }

        public virtual EncounterInfoGroup GetLocalEncounter(string text)
        {
            var parsedItem = GetParsedEncounterText(text);
            var encounter = GetEncounterInfoGroup(parsedItem);
            if (encounter != null)
                encounter.LocalInfo = GetEncounterInfo(parsedItem);
            return encounter;
        }

        public virtual EncounterInfoGroup GetServerEncounter(string text)
        {
            var parsedItem = GetParsedEncounterText(text);
            var encounter = GetEncounterInfoGroup(parsedItem);
            if (encounter != null)
                encounter.ServerInfo = GetEncounterInfo(parsedItem);
            return encounter;
        }

        private const string caseInfoDivider = "--";
        protected virtual string[] GetParsedEncounterText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return null;

            //Split each data string of the current MenuCase, each string divided by "--"
            return text.Split(new string[] { caseInfoDivider }, StringSplitOptions.None);
        }
        private const int encounterParts = 14;

        private const int authorAccountIdIndex = 0;
        private const int filenameIndex = 1;
        private const int authorNameIndex = 2;
        private const int titleIndex = 3;
        private const int recordNumberIndex = 4;
        private const int difficultyIndex = 5;
        private const int descriptionIndex = 6;
        private const int subtitleIndex = 7;
        private const int tagsIndex = 8;
        private const int dateModifiedIndex = 9;
        private const int audienceIndex = 10;
        private const int editorVersionIndex = 11;
        private const int ratingIndex = 12;
        private const int caseTypeIndex = 13;
        private const string filenameExtension = ".ced";
        public EncounterInfoGroup GetEncounterInfoGroup(string[] parsedItem)
        {
            if (parsedItem == null || parsedItem.Length < encounterParts)
                return null;

            var encounterInfoGroup = new EncounterInfoGroup {
                RecordNumber = parsedItem[recordNumberIndex],
                Filename = GetFilename(parsedItem[filenameIndex])
            };

            if (float.TryParse(parsedItem[ratingIndex], out var rating))
                encounterInfoGroup.Rating = rating;

            return encounterInfoGroup;
        }

        protected virtual string GetFilename(string filename)
        {
            if (filename.EndsWith(filenameExtension, StringComparison.OrdinalIgnoreCase))
                filename = filename.Substring(0, filename.Length - filenameExtension.Length);
            return filename;
        }

        private const string categoryDivider = ", ";
        public EncounterInfo GetEncounterInfo(string[] parsedItem)
        {
            if (parsedItem.Length < encounterParts)
                return null;

            var encounterInfo = new EncounterInfo() {
                AuthorAccountId = int.Parse(parsedItem[authorAccountIdIndex]),
                Title = parsedItem[titleIndex],
                Difficulty = GetDifficulty(parsedItem[difficultyIndex]),
                AuthorName = parsedItem[authorNameIndex],
                Description = parsedItem[descriptionIndex],
                Subtitle = parsedItem[subtitleIndex],
                DateModified = long.Parse(parsedItem[dateModifiedIndex]),
                Audience = parsedItem[audienceIndex],
                EditorVersion = parsedItem[editorVersionIndex]
            };

            encounterInfo.Categories.AddRange(parsedItem[tagsIndex]
                .Split(new string[] { categoryDivider }, StringSplitOptions.None));

            var caseType = (CaseType)int.Parse(parsedItem[caseTypeIndex]);
            encounterInfo.IsPublic = caseType == CaseType.publicCase || caseType == CaseType.publicTemplate;
            encounterInfo.IsTemplate = caseType == CaseType.publicTemplate || caseType == CaseType.privateTemplate;

            return encounterInfo;
        }

        protected Difficulty GetDifficulty(string difficulty)
        {
            return Difficulty.None;
        }
    }
}