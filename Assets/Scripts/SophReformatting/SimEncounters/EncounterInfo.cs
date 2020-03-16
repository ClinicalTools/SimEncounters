using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public class EncounterInfo
    {
        public int RecordNumber { get; }

        public EncounterMetaGroup MetaGroup { get; }

        public EncounterBasicStatus UserStatus { get; set; }

        public EncounterInfo(int recordNumber, EncounterMetaGroup infoGroup)
        {
            RecordNumber = recordNumber;
            MetaGroup = infoGroup;
        }
    }
}