namespace ClinicalTools.SimEncounters.Data
{
    public class EncounterMetaGroup
    {
        public float Rating { get; set; } = -1;
        public string Filename { get; set; }

        public EncounterMetadata CurrentInfo { get; set; }
        public EncounterMetadata AutosaveInfo { get; set; }
        public EncounterMetadata LocalInfo { get; set; }
        public EncounterMetadata ServerInfo { get; set; }
        public EncounterMetadata GetLatestInfo()
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
}