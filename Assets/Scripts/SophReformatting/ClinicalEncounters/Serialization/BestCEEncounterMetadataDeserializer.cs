using ClinicalTools.SimEncounters;
using System;

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
        private const int SubtitleIndex = 5;
        private const int DescriptionIndex = 6;
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
                    Subtitle = parsedItem[SubtitleIndex],
                    Description = parsedItem[DescriptionIndex],
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
}