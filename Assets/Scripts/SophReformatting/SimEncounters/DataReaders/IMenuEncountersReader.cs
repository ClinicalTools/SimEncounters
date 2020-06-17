﻿using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public interface IMenuEncountersReader
    {
        WaitableResult<List<MenuEncounter>> GetMenuEncounters(User user);
    }
    public class MenuEncountersReader : IMenuEncountersReader
    {
        private readonly IMetadataGroupsReader metadataGroupsReader;
        private readonly IBasicStatusesReader basicStatusesReader;
        public MenuEncountersReader(IMetadataGroupsReader metadataGroupsReader, IBasicStatusesReader basicStatusesReader)
        {
            this.metadataGroupsReader = metadataGroupsReader;
            this.basicStatusesReader = basicStatusesReader;
        }

        public WaitableResult<List<MenuEncounter>> GetMenuEncounters(User user)
        {
            var metadataGroups = metadataGroupsReader.GetMetadataGroups(user);
            var statuses = basicStatusesReader.GetBasicStatuses(user);

            var menuEncounters = new WaitableResult<List<MenuEncounter>>();

            void processResults() => ProcessResults(menuEncounters, metadataGroups, statuses);
            metadataGroups.AddOnCompletedListener((result) => processResults());
            statuses.AddOnCompletedListener((result) => processResults());

            return menuEncounters;
        }

        protected void ProcessResults(WaitableResult<List<MenuEncounter>> result,
            WaitableResult<Dictionary<int, Dictionary<SaveType, EncounterMetadata>>> metadataGroups,
            WaitableResult<Dictionary<int, EncounterBasicStatus>> statuses)
        {
            if (result.IsCompleted || !metadataGroups.IsCompleted || !statuses.IsCompleted)
                return;

            if (metadataGroups.Result == null)
            {
                result.SetError(null);
                return;
            }

            var menuEncounters = new List<MenuEncounter>();
            foreach (var metadataGroup in metadataGroups.Result)
                menuEncounters.Add(GetMenuEncounter(metadataGroup, statuses));
            result.SetResult(menuEncounters);
        }

        protected MenuEncounter GetMenuEncounter(KeyValuePair<int, Dictionary<SaveType, EncounterMetadata>> metadataGroup,
            WaitableResult<Dictionary<int, EncounterBasicStatus>> statuses)
        {
            EncounterBasicStatus status = null;
            if (statuses.Result?.ContainsKey(metadataGroup.Key) == true)
                status = statuses.Result[metadataGroup.Key];
            return new MenuEncounter(metadataGroup.Value, status);
        }
    }
}