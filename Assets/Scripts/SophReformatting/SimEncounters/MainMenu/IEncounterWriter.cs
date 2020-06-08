using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterWriter
    {
        void Save(User user, Encounter encounter);
    }
}
