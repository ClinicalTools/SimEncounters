using ClinicalTools.SimEncounters.Data;
using System;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterGetter
    {
        bool IsDone { get; }
        Encounter Encounter { get; }

        event Action<Encounter> Completed;
    }
}