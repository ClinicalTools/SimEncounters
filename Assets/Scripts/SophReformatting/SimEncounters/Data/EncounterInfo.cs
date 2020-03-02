using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Data
{
    public enum Difficulty
    {
        None, Beginner, Intermediate, Advanced
    }
    public class EncounterInfoGroup
    {
        public string RecordNumber { get; set; }
        public float Rating { get; set; } = -1;
        public string Filename { get; set; }

        public EncounterInfo CurrentInfo { get; set; }
        public EncounterInfo AutosaveInfo { get; set; }
        public EncounterInfo LocalInfo { get; set; }
        public EncounterInfo ServerInfo { get; set; }
        public EncounterInfo GetLatestInfo()
        {
            var autosaveDate = AutosaveInfo == null ? long.MinValue : AutosaveInfo.DateModified + 1;
            var localDate = LocalInfo == null ? long.MinValue : LocalInfo.DateModified + 1;
            var serverDate = ServerInfo == null ? long.MinValue : ServerInfo.DateModified + 1;
            if (serverDate > localDate && serverDate > autosaveDate)
                return ServerInfo;
            else if (localDate > autosaveDate)
                return LocalInfo;
            else
                return AutosaveInfo;
        }
    }

    public class EncounterInfo
    {
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public int AuthorAccountId { get; set; }
        public long DateModified { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public List<string> Categories { get; } = new List<string>();
        public string Audience { get; set; }
        public Difficulty Difficulty { get; set; }
        public string EditorVersion { get; set; }
        public bool IsTemplate { get; set; }
        public bool IsPublic { get; set; }

        public EncounterInfo() { }

        public EncounterInfo(EncounterInfo baseEncounterInfo)
        {
            Title = baseEncounterInfo.Title;
            AuthorName = baseEncounterInfo.AuthorName;
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