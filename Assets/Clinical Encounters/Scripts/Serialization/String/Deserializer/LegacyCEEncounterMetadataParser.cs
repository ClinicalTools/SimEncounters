using ClinicalTools.SimEncounters;
using System;
using System.Collections.Generic;
using static MenuCase;

namespace ClinicalTools.ClinicalEncounters
{
    public class LegacyCEEncounterMetadataParser : IStringDeserializer<EncounterMetadata>
    {
        public LegacyCEEncounterMetadataParser() { }

        public virtual EncounterMetadata Deserialize(string text)
        {
            var parsedItem = GetParsedEncounterText(text);
            return GetMetadata(parsedItem);
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

        private const string categoryDivider = ", ";
        public EncounterMetadata GetMetadata(string[] parsedItem)
        {
            if (parsedItem == null || parsedItem.Length < encounterParts)
                return null;

            var name = parsedItem[titleIndex].Split('_');
            var metadata = new CEEncounterMetadata() {
                RecordNumber = int.Parse(parsedItem[recordNumberIndex]),
                Filename = GetFilename(parsedItem[filenameIndex]),
                AuthorAccountId = int.Parse(parsedItem[authorAccountIdIndex]),
                AuthorName = new Name(parsedItem[authorNameIndex]),
                Name = new Name(name[0], name[1]),
                Difficulty = GetDifficulty(parsedItem[difficultyIndex]),
                Description = parsedItem[descriptionIndex],
                Subtitle = parsedItem[subtitleIndex],
                DateModified = long.Parse(parsedItem[dateModifiedIndex]),
                Audience = parsedItem[audienceIndex],
                EditorVersion = parsedItem[editorVersionIndex]
            };

            if (metadata.EditorVersion == "-")
                metadata.EditorVersion = "0";

            if (float.TryParse(parsedItem[ratingIndex], out var rating))
                metadata.Rating = rating;
            AddCategories(metadata.Categories, parsedItem[tagsIndex]);

            var caseType = (CaseType)int.Parse(parsedItem[caseTypeIndex]);
            metadata.IsPublic = caseType == CaseType.publicCase || caseType == CaseType.publicTemplate;
            metadata.IsTemplate = caseType == CaseType.publicTemplate || caseType == CaseType.privateTemplate;

            return metadata;
        }

        protected void AddCategories(List<string> categories, string categoriesString)
        {
            var categoriesToAdd = categoriesString.Split(new string[] { categoryDivider }, StringSplitOptions.None);
            foreach (var category in categoriesToAdd) {
                if (category == "Buprenorphine")
                    categories.Add("Opioids");
                else
                    categories.Add(category);
            }
        }

        protected Difficulty GetDifficulty(string difficulty)
        {
            if (difficulty == "Intermediate")
                return Difficulty.Intermediate;
            else if (difficulty == "Beginner")
                return Difficulty.Beginner;
            else if (difficulty == "Advanced")
                return Difficulty.Advanced;
            return Difficulty.Beginner;
        }
    }
}