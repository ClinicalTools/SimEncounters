using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using static MenuCase;

namespace ClinicalTools.SimEncounters
{
    public interface IStringSerializer<T>
    {
        string Serialize(T value);
    }

    public class CEEncounterMetadataDeserializer : IParser<IEncounterMetadata>
    {
        private readonly IParser<IEncounterMetadata> legacyParser;
        public CEEncounterMetadataDeserializer(IParser<IEncounterMetadata> legacyParser)
            => this.legacyParser = legacyParser;

        private const char caseInfoDivider = '|';
        private const char categoryDivider = ';';
        private const int encounterParts = 15;
        private const int authorAccountIdIndex = 0;
        private const int filenameIndex = 1;
        private const int authorNameIndex = 2;
        private const int firstNameIndex = 3;
        private const int lastNameIndex = 4;
        private const int recordNumberIndex = 5;
        private const int difficultyIndex = 6;
        private const int descriptionIndex = 8;
        private const int subtitleIndex = 7;
        private const int categoriesIndex = 9;
        private const int dateModifiedIndex = 10;
        private const int audienceIndex = 11;
        private const int editorVersionIndex = 12;
        private const int ratingIndex = 13;
        private const int isPublicIndex = 14;
        private const int isTemplateIndex = 15;

        public IEncounterMetadata Parse(string text)
        {
            try {
                var parsedItem = text.Split(caseInfoDivider);
                if (parsedItem == null || parsedItem.Length < encounterParts)
                    return legacyParser.Parse(text);

                var metadata = new CEEncounterMetadata() {
                    RecordNumber = int.Parse(parsedItem[recordNumberIndex]),
                    Filename = parsedItem[filenameIndex],
                    AuthorAccountId = int.Parse(parsedItem[authorAccountIdIndex]),
                    AuthorName = parsedItem[authorNameIndex],
                    Rating = float.Parse(parsedItem[ratingIndex]),
                    FirstName = parsedItem[firstNameIndex],
                    LastName = parsedItem[lastNameIndex],
                    Difficulty = GetDifficulty(parsedItem[difficultyIndex]),
                    Description = parsedItem[descriptionIndex],
                    Subtitle = parsedItem[subtitleIndex],
                    DateModified = long.Parse(parsedItem[dateModifiedIndex]),
                    Audience = parsedItem[audienceIndex],
                    EditorVersion = parsedItem[editorVersionIndex],
                    IsPublic = GetBoolValue(parsedItem[isPublicIndex]),
                    IsTemplate = GetBoolValue(parsedItem[isTemplateIndex])
                };
                if (float.TryParse(parsedItem[ratingIndex], out var rating))
                    metadata.Rating = rating;
                metadata.Categories.AddRange(parsedItem[categoriesIndex].Split(categoryDivider));

                return metadata;
            } catch (Exception) {
                return null;
            }
        }

        protected Difficulty GetDifficulty(string difficulty)
        {
            difficulty = difficulty.ToLower();
            if (difficulty == "intermediate")
                return Difficulty.Intermediate;
            else if (difficulty == "beginner")
                return Difficulty.Beginner;
            else if (difficulty == "advanced")
                return Difficulty.Advanced;
            return Difficulty.Beginner;
        }

        protected bool GetBoolValue(string value) => value == "1";
    }

    public class EncounterMetadataSerializer : IStringSerializer<IEncounterMetadata>
    {
        public virtual string Serialize(IEncounterMetadata metadata)
        {
            string firstName, lastName;
            if (metadata is CEEncounterMetadata ceMetadata) {
                firstName = ceMetadata.FirstName;
                lastName = ceMetadata.LastName;
            } else {
                firstName = "";
                lastName = "";
            }
            var str = "" + metadata.AuthorAccountId;
            str += AppendValue(metadata.Filename);
            str += AppendValue(metadata.AuthorName);
            str += AppendValue(firstName);
            str += AppendValue(lastName);
            str += AppendValue(metadata.RecordNumber.ToString());
            str += AppendValue(metadata.Difficulty.ToString());
            str += AppendValue(metadata.Description);
            str += AppendValue(metadata.Subtitle);
            str += AppendValue(string.Join(categoryDivider, metadata.Categories));
            str += AppendValue(metadata.DateModified.ToString());
            str += AppendValue(metadata.Audience);
            str += AppendValue(metadata.EditorVersion);
            str += AppendValue(metadata.Rating.ToString());
            str += AppendValue(metadata.IsPublic);
            str += AppendValue(metadata.IsTemplate);

            return str;
        }

        private const string caseInfoDivider = "|";
        private const string categoryDivider = ";";
        protected virtual string AppendValue(bool value) => caseInfoDivider + (value ? "1" : "0");
        protected virtual string AppendValue(string value) => caseInfoDivider + value;
    }
    public class LegacyCEEncounterMetadataParser : IParser<IEncounterMetadata>
    {
        public LegacyCEEncounterMetadataParser() { }

        public virtual IEncounterMetadata Parse(string text)
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
        public IEncounterMetadata GetMetadata(string[] parsedItem)
        {
            if (parsedItem == null || parsedItem.Length < encounterParts)
                return null;

            var name = parsedItem[titleIndex].Split('_');
            var metadata = new CEEncounterMetadata() {
                RecordNumber = int.Parse(parsedItem[recordNumberIndex]),
                Filename = GetFilename(parsedItem[filenameIndex]),
                AuthorAccountId = int.Parse(parsedItem[authorAccountIdIndex]),
                AuthorName = parsedItem[authorNameIndex],
                FirstName = name[0],
                LastName = name[1],
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