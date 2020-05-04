using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserEncounterReaderSelector : IUserEncounterReaderSelector
    {
        protected virtual Dictionary<SaveType, IUserEncounterReader> EncounterReaders { get; } = new Dictionary<SaveType, IUserEncounterReader>();
        public UserEncounterReaderSelector(
            [Inject(Id = SaveType.Autosave)] IUserEncounterReader autosaveReader,
            [Inject(Id = SaveType.Demo)] IUserEncounterReader demoReader,
            [Inject(Id = SaveType.Local)] IUserEncounterReader localReader,
            [Inject(Id = SaveType.Server)] IUserEncounterReader serverReader)
        {
            EncounterReaders.Add(SaveType.Autosave, autosaveReader);
            EncounterReaders.Add(SaveType.Demo, demoReader);
            EncounterReaders.Add(SaveType.Local, localReader);
            EncounterReaders.Add(SaveType.Server, serverReader);
        }

        public IUserEncounterReader GetUserEncounterReader(SaveType saveType) => EncounterReaders[saveType];
    }
}