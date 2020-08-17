using ClinicalTools.SimEncounters;
using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using static MenuCase;

namespace ClinicalTools.SimEncounters
{
    public interface IStringSerializer<T>
    {
        string Serialize(T value);
    }
}

namespace ClinicalTools.ClinicalEncounters
{
    public class BestCEEncounterMetadataDeserializer : IParser<EncounterMetadata>
    {
        private readonly IParser<EncounterMetadata> legacyParser;
        public BestCEEncounterMetadataDeserializer()
            => legacyParser = new CEEncounterMetadataDeserializer(new LegacyCEEncounterMetadataParser());

        // Ideally I'd use JSON objects, and I've set the PHP to allow them.
        // Unfortunately, it would drastically increase the size of the menu information the server gives
        // It would also cause more work to support it, so for now, I'll stick with more simplistic storage methods
        private const char CaseInfoDivider = '|';
        private const char CategoryDivider = ';';
        private const int EncounterParts = 16;
        private const int RecordNumberIndex = 0;
        private const int AuthorAccountIdIndex = 1;
        private const int AuthorNameIndex = 2;
        private const int TitleIndex = 3;
        private const int DifficultyIndex = 4;
        private const int DescriptionIndex = 5;
        private const int SummaryIndex = 6;
        private const int TagsIndex = 7;
        private const int ModifiedIndex = 8;
        private const int AudienceIndex = 9;
        private const int VersionIndex = 10;
        private const int IsPublicIndex = 11;
        private const int IsTemplateIndex = 12;
        private const int RatingIndex = 13;
        private const int UrlIndex = 14;
        private const int CompletionCodeIndex = 15;


        public EncounterMetadata Parse(string text)
        {
            try {
                var parsedItem = text.Split(CaseInfoDivider);
                if (parsedItem == null || parsedItem.Length != EncounterParts)
                    return legacyParser.Parse(text);

                var metadata = new CEEncounterMetadata() {
                    RecordNumber = int.Parse(parsedItem[RecordNumberIndex]),
                    AuthorAccountId = int.Parse(parsedItem[AuthorAccountIdIndex]),
                    AuthorName = GetName(parsedItem[AuthorNameIndex]),
                    Name = GetName(parsedItem[TitleIndex]),
                    Difficulty = GetDifficulty(parsedItem[DifficultyIndex]),
                    Description = parsedItem[DescriptionIndex],
                    Subtitle = parsedItem[SummaryIndex],
                    DateModified = long.Parse(parsedItem[ModifiedIndex]),
                    Audience = parsedItem[AudienceIndex],
                    EditorVersion = parsedItem[VersionIndex],
                    IsPublic = GetBoolValue(parsedItem[IsPublicIndex]),
                    IsTemplate = GetBoolValue(parsedItem[IsTemplateIndex]),
                    Rating = float.Parse(parsedItem[RatingIndex]),
                    Url = parsedItem[UrlIndex].Trim(),
                    CompletionCode = parsedItem[CompletionCodeIndex].Trim()
                };
                if (float.TryParse(parsedItem[RatingIndex], out var rating))
                    metadata.Rating = rating;
                var categories = parsedItem[TagsIndex].Split(CategoryDivider);
                if (categories.Length == 1)
                    categories = parsedItem[TagsIndex].Split(',');
                foreach (var category in categories)
                    metadata.Categories.Add(category.Trim());

                metadata.Filename = metadata.GetDesiredFilename();

                return metadata;
            } catch (Exception) {
                return null;
            }
        }

        protected Name GetName(string name)
        {
            var nameParts = name.Split(CategoryDivider);
            switch (nameParts.Length) {
                case 0:
                    return new Name();
                case 1:
                    return new Name(nameParts[0]);
                case 2:
                    return new Name(nameParts[0], nameParts[1]);
                default:
                    return new Name(nameParts[0], nameParts[1], nameParts[2]);
            }
        }

        protected Difficulty GetDifficulty(string difficulty)
        {
            if (difficulty.Equals("intermediate", StringComparison.InvariantCultureIgnoreCase))
                return Difficulty.Intermediate;
            else if (difficulty.Equals("beginner", StringComparison.InvariantCultureIgnoreCase))
                return Difficulty.Beginner;
            else if (difficulty.Equals("advanced", StringComparison.InvariantCultureIgnoreCase))
                return Difficulty.Advanced;
            return Difficulty.Beginner;
        }

        protected bool GetBoolValue(string value) => value == "1";
    }
    public class CEEncounterMetadataDeserializer : IParser<EncounterMetadata>
    {
        private readonly IParser<EncounterMetadata> legacyParser;
        public CEEncounterMetadataDeserializer(IParser<EncounterMetadata> legacyParser)
            => this.legacyParser = legacyParser;

        private const char caseInfoDivider = '|';
        private const char categoryDivider = ';';
        private const int encounterParts = 18;
        private const int authorAccountIdIndex = 0;
        private const int filenameIndex = 1;
        private const int authorNameIndex = 2;
        private const int firstNameIndex = 3;
        private const int lastNameIndex = 4;
        private const int recordNumberIndex = 5;
        private const int difficultyIndex = 6;
        private const int descriptionIndex = 7;
        private const int subtitleIndex = 8;
        private const int categoriesIndex = 9;
        private const int dateModifiedIndex = 10;
        private const int audienceIndex = 11;
        private const int editorVersionIndex = 12;
        private const int ratingIndex = 13;
        private const int isPublicIndex = 14;
        private const int isTemplateIndex = 15;
        private const int urlIndex = 16;
        private const int completionCodeIndex = 17;

        public EncounterMetadata Parse(string text)
        {
            try {
                var parsedItem = text.Split(caseInfoDivider);
                if (parsedItem == null || parsedItem.Length < encounterParts)
                    return legacyParser.Parse(text);

                var metadata = new CEEncounterMetadata() {
                    RecordNumber = int.Parse(parsedItem[recordNumberIndex]),
                    Filename = parsedItem[filenameIndex],
                    AuthorAccountId = int.Parse(parsedItem[authorAccountIdIndex]),
                    AuthorName = new Name(parsedItem[authorNameIndex]),
                    Rating = float.Parse(parsedItem[ratingIndex]),
                    Name = new Name(parsedItem[firstNameIndex], parsedItem[lastNameIndex]),
                    Difficulty = GetDifficulty(parsedItem[difficultyIndex]),
                    Description = parsedItem[descriptionIndex],
                    Subtitle = parsedItem[subtitleIndex],
                    DateModified = long.Parse(parsedItem[dateModifiedIndex]),
                    Audience = parsedItem[audienceIndex],
                    EditorVersion = parsedItem[editorVersionIndex],
                    IsPublic = GetBoolValue(parsedItem[isPublicIndex]),
                    IsTemplate = GetBoolValue(parsedItem[isTemplateIndex]),
                    Url = parsedItem[urlIndex].Trim(),
                    CompletionCode = parsedItem[completionCodeIndex].Trim()
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
    public class LegacyCEEncounterMetadataParser : IParser<EncounterMetadata>
    {
        public LegacyCEEncounterMetadataParser() { }

        public virtual EncounterMetadata Parse(string text)
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