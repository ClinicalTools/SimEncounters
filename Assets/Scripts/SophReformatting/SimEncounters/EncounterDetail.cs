using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public class EncounterDetail
    {
        public int EncounterNumber { get; }

        public UserEncounterStatus UserStatus { get; }

        public EncounterInfoGroup EncounterInfoGroup { get; }
    }
}