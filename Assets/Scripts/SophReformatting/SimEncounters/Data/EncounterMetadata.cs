using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Data
{
    public enum Difficulty
    {
        Beginner, Intermediate, Advanced
    }

    public class EncounterMetadata
    {
        public float Rating { get; set; } = -1;
        public int RecordNumber { get; set; }
        public string Filename { get; set; }
        public string AuthorName { get; set; }
        public int AuthorAccountId { get; set; }
        public string Title { get; set; }
        public long DateModified { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public List<string> Categories { get; } = new List<string>();
        public string Audience { get; set; }
        public Difficulty Difficulty { get; set; }
        public string EditorVersion { get; set; } = "0";
        public bool IsTemplate { get; set; }
        public bool IsPublic { get; set; }

        public EncounterMetadata() { }

        public EncounterMetadata(EncounterMetadata baseEncounterInfo)
        {
            Title = baseEncounterInfo.Title;
            Categories.AddRange(baseEncounterInfo.Categories);
            DateModified = baseEncounterInfo.DateModified;
            Subtitle = baseEncounterInfo.Subtitle;
            Description = baseEncounterInfo.Description;
            Audience = baseEncounterInfo.Audience;
            Difficulty = baseEncounterInfo.Difficulty;
            EditorVersion = baseEncounterInfo.EditorVersion;
            IsTemplate = baseEncounterInfo.IsTemplate;
            IsPublic = baseEncounterInfo.IsPublic;
        }
    }
}