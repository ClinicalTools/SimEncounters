using ClinicalTools.SimEncounters.Data;
using System;
using static MenuCase;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class EncounterDetailParser : IParser<EncounterInfo>
    {
        protected IEncounterInfoSetter EncounterInfoSetter { get; }

        public EncounterDetailParser(IEncounterInfoSetter encounterInfoSetter)
        {
            EncounterInfoSetter = encounterInfoSetter;
        }

        public virtual EncounterInfo Parse(string text)
        {
            var parsedItem = GetParsedEncounterText(text);
            var infoGroup = GetInfoGroup(parsedItem);
            if (infoGroup == null)
                return null;

            EncounterInfoSetter.SetEncounterInfo(infoGroup, GetInfo(parsedItem));
            
            var recordNumber = GetRecordNumber(parsedItem);
            infoGroup.RecordNumber = recordNumber;
            return new EncounterInfo(recordNumber, infoGroup);
        }

        private const int recordNumberIndex = 4;
        private int GetRecordNumber(string[] parsedItem)
        {
            if (parsedItem?.Length > recordNumberIndex)
                if (int.TryParse(parsedItem[recordNumberIndex], out var recordNumber))
                    return recordNumber;

            return 0;
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
        public EncounterMetaGroup GetInfoGroup(string[] parsedItem)
        {
            if (parsedItem == null || parsedItem.Length < encounterParts)
                return null;

            var encounterInfoGroup = new EncounterMetaGroup {
                Filename = GetFilename(parsedItem[filenameIndex]),
                AuthorAccountId = int.Parse(parsedItem[authorAccountIdIndex]),
                AuthorName = parsedItem[authorNameIndex],
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
        public EncounterMetadata GetInfo(string[] parsedItem)
        {
            if (parsedItem.Length < encounterParts)
                return null;

            var encounterInfo = new EncounterMetadata() {
                Title = parsedItem[titleIndex].Replace('_', ' '),
                Difficulty = GetDifficulty(parsedItem[difficultyIndex]),
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
            if (difficulty == "Intermediate")
                return Difficulty.Intermediate;
            else if (difficulty == "Beginner")
                return Difficulty.Beginner;
            else if (difficulty == "Advanced")
                return Difficulty.Advanced;
            return Difficulty.None;
        }
    }
}