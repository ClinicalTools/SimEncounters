using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public class EncounterDetail
    {
        public int RecordNumber { get; }

        public EncounterInfoGroup InfoGroup { get; }

        public UserEncounterStatus UserStatus { get; set; }

        public EncounterDetail(int recordNumber, EncounterInfoGroup infoGroup)
        {
            RecordNumber = recordNumber;
            InfoGroup = infoGroup;
        }
    }
}