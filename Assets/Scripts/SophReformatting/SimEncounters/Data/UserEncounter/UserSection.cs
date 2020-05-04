﻿using ClinicalTools.SimEncounters.Collections;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Data
{
    public class UserSection
    {
        public UserEncounter Encounter { get; }
        public Section Data { get; }
        protected SectionStatus Status { get; }
        public event Action StatusChanged;

        public UserSection(UserEncounter encounter, Section data, SectionStatus status)
        {
            Encounter = encounter;
            Data = data;
            Status = status;

            foreach (var tab in data.Tabs) {
                var userTab = new UserTab(encounter, tab.Value, status.GetTabStatus(tab.Key));
                userTab.StatusChanged += UpdateIsRead;
                Tabs.Add(tab.Key, userTab);
            }
        }

        protected virtual void UpdateIsRead()
        {
            if (Status.Read)
                return;

            foreach (var tab in Tabs.Values) {
                if (!tab.IsRead())
                    return;
            }

            SetRead(true);
        }

        protected virtual OrderedCollection<UserTab> Tabs { get; } = new OrderedCollection<UserTab>();
        public virtual IEnumerable<UserTab> GetTabs() => Tabs.Values;
        public virtual UserTab GetTab(string key) => Tabs[key];

        public bool IsRead() => Status.Read;
        protected virtual void SetRead(bool read)
        {
            if (Status.Read == read)
                return;
            Status.Read = read;
            StatusChanged?.Invoke();
        }
    }
}