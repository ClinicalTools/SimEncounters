using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Data
{
    public enum Difficulty
    {
        Beginner, Intermediate, Advanced
    }

    public class CEEncounterMetadata : EncounterMetadata
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public override string Title { 
            get => $"{FirstName} {LastName}"; 
            set { throw new Exception("Cannot set title directly for CE Encounter."); } }


        public CEEncounterMetadata() : base() { }
        public CEEncounterMetadata(IEncounterMetadata baseEncounterInfo) : base()
        {
            if (baseEncounterInfo is CEEncounterMetadata clinicalMetadata) {
                FirstName = clinicalMetadata.FirstName;
                LastName = clinicalMetadata.LastName;
            }
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
    public class EncounterMetadata : IEncounterMetadata
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

        public EncounterMetadata(IEncounterMetadata baseEncounterInfo)
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