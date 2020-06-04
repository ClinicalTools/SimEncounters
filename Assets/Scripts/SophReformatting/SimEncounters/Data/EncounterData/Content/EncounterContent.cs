using ClinicalTools.SimEncounters.Collections;
using System;
using System.Collections.Generic;
using Zenject;

namespace ClinicalTools.SimEncounters.Data
{
    public class EncounterDataReaderSelector : IEncounterDataReaderSelector
    {
        protected virtual Dictionary<SaveType, IEncounterDataReader> EncounterReaders { get; } = new Dictionary<SaveType, IEncounterDataReader>();
        public EncounterDataReaderSelector(
            [Inject(Id = SaveType.Default)] IEncounterDataReader defaultReader,
            [Inject(Id = SaveType.Autosave)] IEncounterDataReader autosaveReader,
            [Inject(Id = SaveType.Demo)] IEncounterDataReader demoReader,
            [Inject(Id = SaveType.Local)] IEncounterDataReader localReader,
            [Inject(Id = SaveType.Server)] IEncounterDataReader serverReader)
        {
            EncounterReaders.Add(SaveType.Default, defaultReader);
            EncounterReaders.Add(SaveType.Autosave, autosaveReader);
            EncounterReaders.Add(SaveType.Demo, demoReader);
            EncounterReaders.Add(SaveType.Local, localReader);
            EncounterReaders.Add(SaveType.Server, serverReader);
        }

        public IEncounterDataReader GetEncounterDataReader(SaveType saveType) => EncounterReaders[saveType];
    }

    public interface IEncounterDataReaderSelector
    {
        IEncounterDataReader GetEncounterDataReader(SaveType saveType);
    }
    public interface IEncounterDataReader
    {
        WaitableResult<EncounterData> GetEncounterData(User user, EncounterMetadata metadata);
    }
    public class EncounterDataReader : IEncounterDataReader
    {
        public virtual WaitableResult<EncounterData> GetEncounterData(User user, EncounterMetadata metadata)
        {
            return null;
        }
    }
    public class EncounterData
    {
        public EncounterContent Content { get; }
        public EncounterImageData ImageData { get; }

        public EncounterData(EncounterContent content, EncounterImageData imageData)
        {
            Content = content;
            ImageData = imageData;
        }
    }

    public class EncounterContent
    {
        public virtual int CurrentSectionIndex { get; set; }
        public virtual string GetCurrentSectionKey() => Sections[CurrentSectionIndex].Key;
        public virtual void SetCurrentSection(Section section)
        {
            if (!Sections.Contains(section))
                throw new Exception($"Passed section is not contained in the collection of sections.");
            CurrentSectionIndex = Sections.IndexOf(section);
        }

        public virtual OrderedCollection<Section> Sections { get; } = new OrderedCollection<Section>();
        public virtual VariableData Variables { get; }

        public EncounterContent()
        {
            Variables = new VariableData();
        }

        public EncounterContent(VariableData variables)
        {
            Variables = variables;
        }

        public int MoveToNextSection()
        {
            if (CurrentSectionIndex >= Sections.Count - 1)
                return CurrentSectionIndex;

            CurrentSectionIndex++;
            var section = Sections[CurrentSectionIndex].Value;
            section.CurrentTabIndex = 0;

            return CurrentSectionIndex;
        }
        public int MoveToPreviousSection()
        {
            if (CurrentSectionIndex <= 0)
                return CurrentSectionIndex;

            CurrentSectionIndex--;
            var section = Sections[CurrentSectionIndex].Value;
            section.CurrentTabIndex = section.Tabs.Count - 1;

            return CurrentSectionIndex;
        }
    }
}