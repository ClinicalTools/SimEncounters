using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Data
{
    public enum SaveType
    {
        Autosave, Local, Demo, Server
    }
    public class MenuEncounter
    {
        public Dictionary<SaveType, EncounterMetadata> Metadata { get; }
        public EncounterBasicStatus Status { get; set; }

        public MenuEncounter(Dictionary<SaveType, EncounterMetadata> metadata, EncounterBasicStatus status)
        {
            Metadata = metadata;
            Status = status;
        }

        public KeyValuePair<SaveType, EncounterMetadata> GetLatestTypedMetada()
        {
            var latest = new KeyValuePair<SaveType, EncounterMetadata>();
            foreach (var metaData in Metadata)
            {
                if (latest.Value == null || latest.Value.DateModified < metaData.Value.DateModified)
                    latest = metaData;
            }

            return latest;
        }
        public SaveType GetLatestType() => GetLatestTypedMetada().Key;
        public EncounterMetadata GetLatestMetadata() => GetLatestTypedMetada().Value;

    }
}