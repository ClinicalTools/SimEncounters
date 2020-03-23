using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Data
{
    public enum SaveType
    {
        Autosave, Local, Demo, Info
    }

    public class EncounterMetaGroup
    {
        public float Rating { get; set; } = -1;
        public int RecordNumber { get; set; }
        public string Filename { get; set; }
        public string AuthorName { get; set; }
        public int AuthorAccountId { get; set; }

        public Dictionary<SaveType, EncounterMetadata> Metadata { get; } = new Dictionary<SaveType, EncounterMetadata>();
        public EncounterMetadata AutosaveInfo { get; set; }
        public EncounterMetadata DemoInfo { get; set; }
        public EncounterMetadata LocalInfo { get; set; }
        public EncounterMetadata ServerInfo { get; set; }
        public EncounterMetadata GetLatestInfo()
        {
            if (AutosaveInfo != null)
                return AutosaveInfo;
            else if (ServerInfo != null)
                return ServerInfo;
            else if (LocalInfo != null)
                return LocalInfo;
            else if (DemoInfo != null)
                return DemoInfo;
            else 
                return null;
        }
    }
}