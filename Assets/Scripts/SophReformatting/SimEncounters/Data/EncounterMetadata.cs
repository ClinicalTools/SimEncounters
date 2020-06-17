using System.Collections.Generic;

namespace ClinicalTools.ClinicalEncounters
{
}

namespace ClinicalTools.SimEncounters.Data
{
    public enum Difficulty
    {
        Beginner, Intermediate, Advanced
    }
    public class EncounterMetadata
    {
        public virtual float Rating { get; set; } = -1;
        public virtual int RecordNumber { get; set; }
        public virtual string Filename { get; set; }
        public virtual string AuthorName { get; set; }
        public virtual int AuthorAccountId { get; set; }
        public virtual string Title { get; set; }
        public virtual long DateModified { get; set; }
        public virtual string Subtitle { get; set; }
        public virtual string Description { get; set; }
        public virtual List<string> Categories { get; } = new List<string>();
        public virtual string Audience { get; set; }
        public virtual Difficulty Difficulty { get; set; }
        public virtual string EditorVersion { get; set; } = "0";
        public virtual bool IsTemplate { get; set; }
        public virtual bool IsPublic { get; set; }

        public EncounterMetadata() { }

        public EncounterMetadata(EncounterMetadata baseEncounterInfo)
        {
            Categories.AddRange(baseEncounterInfo.Categories);
            Subtitle = baseEncounterInfo.Subtitle;
            Description = baseEncounterInfo.Description;
            Audience = baseEncounterInfo.Audience;
            Difficulty = baseEncounterInfo.Difficulty;
            EditorVersion = baseEncounterInfo.EditorVersion;
        }
    }
}