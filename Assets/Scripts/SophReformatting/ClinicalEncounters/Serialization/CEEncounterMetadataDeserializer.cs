using ClinicalTools.SimEncounters;
using System;

namespace ClinicalTools.ClinicalEncounters
{
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
}