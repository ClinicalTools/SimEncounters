using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IUserEncounterReaderSelector
    {
        IUserEncounterReader GetUserEncounterReader(SaveType saveType);
    }
}