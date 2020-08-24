using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class LocalBasicStatusesReader : IBasicStatusesReader
    {
        private readonly IFileManager fileManager;
        private readonly IParser<KeyValuePair<int, EncounterBasicStatus>> parser;
        public LocalBasicStatusesReader(IFileManager fileManager, IParser<KeyValuePair<int, EncounterBasicStatus>> parser)
        {
            this.fileManager = fileManager;
            this.parser = parser;
        }

        public WaitableResult<Dictionary<int, EncounterBasicStatus>> GetBasicStatuses(User user)
        {
            var statuses = new WaitableResult<Dictionary<int, EncounterBasicStatus>>();

            var fileTexts = fileManager.GetFilesText(user, FileType.BasicStatus);
            fileTexts.AddOnCompletedListener((result) => ProcessResults(statuses, result));

            return statuses;
        }

        private void ProcessResults(WaitableResult<Dictionary<int, EncounterBasicStatus>> result, WaitedResult<string[]> fileTexts)
        {
            if (fileTexts == null) {
                result.SetError(null);
                return;
            }

            var statuses = new Dictionary<int, EncounterBasicStatus>();
            foreach (var fileText in fileTexts.Value) {
                var metadata = parser.Parse(fileText);
                if (statuses.ContainsKey(metadata.Key)) {
                    Debug.LogError($"Duplicate saved status for key {metadata.Key}");
                    continue;
                }
                if (metadata.Value != null)
                    statuses.Add(metadata.Key, metadata.Value);
            }

            result.SetResult(statuses);
        }
    }
}