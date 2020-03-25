﻿using UnityEngine;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class AudienceFilterUI : EncounterFilterBehaviour
    {
        [SerializeField] private List<LabeledToggle> audienceToggles;
        public List<LabeledToggle> AudienceToggles { get => audienceToggles; set => audienceToggles = value; }

        public override Filter<EncounterInfo> EncounterFilter => FilterAudience;
        public override event Action<Filter<EncounterInfo>> FilterChanged;

        protected List<string> FilteredAudiences { get; } = new List<string>();

        protected void Awake()
        {
            foreach (var toggle in AudienceToggles)
                toggle.Toggle.onValueChanged.AddListener((isOn) => ToggleDifficulty(isOn, toggle.Label.text));
        }

        protected void ToggleDifficulty(bool isOn, string difficulty)
        {
            if (isOn)
                FilteredAudiences.Add(difficulty);
            else
                FilteredAudiences.Remove(difficulty);

            FilterChanged?.Invoke(EncounterFilter);
        }

        protected bool FilterAudience(EncounterInfo encounter)
        {
            if (FilteredAudiences.Count == 0)
                return true;

            var audience = encounter.MetaGroup.GetLatestInfo().Audience.ToUpper();
            foreach (var filteredAudience in FilteredAudiences) {
                if (audience.Contains(filteredAudience.ToUpper()))
                    return true;
            }

            return false;
        }

        public override void Clear()
        {
            foreach (var toggle in AudienceToggles)
                toggle.Toggle.isOn = false;
        }
    }
}