using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public interface IMetadataGroupsReader
    {
        WaitableResult<Dictionary<int, Dictionary<SaveType, IEncounterMetadata>>> GetMetadataGroups(User user);
    }

    public class MetadataGroupsReader : IMetadataGroupsReader
    {
        private readonly Dictionary<SaveType, IMetadatasReader> metadatasReaders = new Dictionary<SaveType, IMetadatasReader>();
        public MetadataGroupsReader(
            [Inject(Id = SaveType.Autosave)] IMetadatasReader autosaveMetadataReader,
            [Inject(Id = SaveType.Demo)] IMetadatasReader demoMetadatasReader,
            [Inject(Id = SaveType.Local)] IMetadatasReader localMetadataReader,
            [Inject(Id = SaveType.Server)] IMetadatasReader serverMetadatasReader)
        {
            if (autosaveMetadataReader != null)
                metadatasReaders.Add(SaveType.Autosave, autosaveMetadataReader);
            if (localMetadataReader != null)
                metadatasReaders.Add(SaveType.Local, localMetadataReader);
            if (serverMetadatasReader != null)
                metadatasReaders.Add(SaveType.Server, serverMetadatasReader);
            if (demoMetadatasReader != null)
                metadatasReaders.Add(SaveType.Demo, demoMetadatasReader);
        }

        public WaitableResult<Dictionary<int, Dictionary<SaveType, IEncounterMetadata>>> GetMetadataGroups(User user)
        {
            var metadatasResults = new Dictionary<SaveType, WaitableResult<List<IEncounterMetadata>>>();
            foreach (var metadatasReader in metadatasReaders)
                metadatasResults.Add(metadatasReader.Key, metadatasReader.Value.GetMetadatas(user));

            var metadataGroups = new WaitableResult<Dictionary<int, Dictionary<SaveType, IEncounterMetadata>>>();

            foreach (var metadatasResult in metadatasResults)
                metadatasResult.Value.AddOnCompletedListener((result) => ProcessResult(metadataGroups, metadatasResults));

            return metadataGroups;
        }

        private void ProcessResult(WaitableResult<Dictionary<int, Dictionary<SaveType, IEncounterMetadata>>> result,
            Dictionary<SaveType, WaitableResult<List<IEncounterMetadata>>> metadatasResults)
        {
            if (result.IsCompleted)
                return;

            foreach (var metadatasResult in metadatasResults)
            {
                if (!metadatasResult.Value.IsCompleted)
                    return;
            }

            var metadataGroups = new Dictionary<int, Dictionary<SaveType, IEncounterMetadata>>();
            foreach (var metadatasResult in metadatasResults)
            {
                if (metadatasResult.Value.Result == null)
                    continue;

                foreach (var metadata in metadatasResult.Value.Result)
                    AddMetadata(metadataGroups, metadatasResult.Key, metadata);
            }

            result.SetResult(metadataGroups);
        }

        private void AddMetadata(Dictionary<int, Dictionary<SaveType, IEncounterMetadata>> metadataGroups, SaveType saveType, IEncounterMetadata metadata)
        {
            Dictionary<SaveType, IEncounterMetadata> metadataGroup;
            if (metadataGroups.ContainsKey(metadata.RecordNumber))
            {
                metadataGroup = metadataGroups[metadata.RecordNumber];
            }
            else
            {
                metadataGroup = new Dictionary<SaveType, IEncounterMetadata>();
                metadataGroups.Add(metadata.RecordNumber, metadataGroup);
            }
            
            if (!metadataGroup.ContainsKey(saveType))
                metadataGroup.Add(saveType, metadata);
        }
    }
}