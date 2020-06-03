using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class CaseCreator
    {
        public void Something() { }
    }

    public class EncounterReaderSelector : IEncounterReaderSelector
    {
        protected virtual Dictionary<SaveType, IEncounterReader> EncounterReaders { get; } = new Dictionary<SaveType, IEncounterReader>();
        public EncounterReaderSelector(
            [Inject(Id = SaveType.Autosave)] IEncounterReader autosaveReader,
            [Inject(Id = SaveType.Demo)] IEncounterReader demoReader,
            [Inject(Id = SaveType.Local)] IEncounterReader localReader,
            [Inject(Id = SaveType.Server)] IEncounterReader serverReader)
        {
            EncounterReaders.Add(SaveType.Autosave, autosaveReader);
            EncounterReaders.Add(SaveType.Demo, demoReader);
            EncounterReaders.Add(SaveType.Local, localReader);
            EncounterReaders.Add(SaveType.Server, serverReader);
        }

        public IEncounterReader GetEncounterReader(SaveType saveType) => EncounterReaders[saveType];
    }
}