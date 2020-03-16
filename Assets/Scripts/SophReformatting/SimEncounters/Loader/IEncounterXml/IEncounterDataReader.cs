using ClinicalTools.SimEncounters.Data;
using System;

namespace ClinicalTools.SimEncounters
{
    public interface IEncounterDataReader
    {
        bool IsDone { get; }
        EncounterData EncounterData { get; }

        event Action<EncounterData> Completed;

        void DoStuff(User user, EncounterInfo info);
    }
    public interface IEncounterReader
    {
        bool IsDone { get; }
        Encounter Encounter { get; }

        event Action<Encounter> Completed;

        void DoStuff(User user, EncounterInfo info, EncounterMetadata metadata);
    }
}