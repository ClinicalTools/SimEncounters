﻿using ClinicalTools.SimEncounters.Collections;
using System;

namespace ClinicalTools.SimEncounters
{
    public class UserEncounter
    {
        public User User { get; }
        public EncounterStatus Status { get; }
        public event Action StatusChanged;
        public Encounter Data { get; }

        public UserEncounter(User user, Encounter data, EncounterStatus status)
        {
            User = user;
            Data = data;
            Status = status;

            foreach (var section in data.Content.NonImageContent.Sections) {
                var sectionStatus = status.ContentStatus.GetSectionStatus(section.Key);
                var userSection = new UserSection(this, section.Value, sectionStatus);
                userSection.StatusChanged += UpdateIsRead;
                Sections.Add(section.Key, userSection);
            }
        }

        protected virtual void UpdateIsRead()
        {
            if (Status.ContentStatus.Read)
                return;

            foreach (var section in Sections.Values) {
                if (!section.IsRead())
                    return;
            }

            SetRead(true);
        }

        protected virtual OrderedCollection<UserSection> Sections { get; } = new OrderedCollection<UserSection>();
        public virtual UserSection GetCurrentSection() 
            => GetSection(Data.Content.NonImageContent.GetCurrentSectionKey());
        public virtual UserSection GetSection(string key) => Sections[key];

        public bool IsRead() => Status.ContentStatus.Read;
        protected virtual void SetRead(bool read)
        {
            if (Status.ContentStatus.Read == read)
                return;
            Status.ContentStatus.Read = read;
            Status.BasicStatus.Completed = true;
            StatusChanged?.Invoke();
        }
    }
}