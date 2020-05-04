using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterReaderSelector
    {
        IEncounterReader GetEncounterReader(SaveType saveType);
    }
}