﻿using System;
using System.Collections.Generic;

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
        public virtual Name AuthorName { get; set; }
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
        public virtual string GetDesiredFilename() => $"{RecordNumber}_{Title}";


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

        public virtual string GetRecordNumberString() => RecordNumber.ToString("D6");

        private readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        public virtual long ResetDateModified() => DateModified = (long)(DateTime.UtcNow - unixEpoch).TotalSeconds;
    }
}