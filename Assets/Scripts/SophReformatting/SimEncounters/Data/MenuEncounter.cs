using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Data
{
    public enum SaveType
    {
        Default, Autosave, Demo, Local, Server
    }
    public class MenuEncounter
    {
        public Dictionary<SaveType, IEncounterMetadata> Metadata { get; }
        public EncounterBasicStatus Status { get; set; }

        public MenuEncounter(Dictionary<SaveType, IEncounterMetadata> metadata, EncounterBasicStatus status)
        {
            Metadata = metadata;
            Status = status;
        }

        public KeyValuePair<SaveType, IEncounterMetadata> GetLatestTypedMetada()
        {
            var latest = new KeyValuePair<SaveType, IEncounterMetadata>();
            foreach (var metaData in Metadata)
            {
                if (latest.Value == null || latest.Value.DateModified < metaData.Value.DateModified)
                    latest = metaData;
            }

            return latest;
        }
        public SaveType GetLatestType() => GetLatestTypedMetada().Key;
        public IEncounterMetadata GetLatestMetadata() => GetLatestTypedMetada().Value;

    }
}