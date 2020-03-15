using UnityEngine;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class AudienceFilterUI : EncounterFilterBehaviour
    {
        [SerializeField] private List<LabeledToggle> audienceToggles;
        public List<LabeledToggle> AudienceToggles { get => audienceToggles; set => audienceToggles = value; }

        public override Filter<EncounterDetail> EncounterFilter => FilterAudience;
        public override event Action<Filter<EncounterDetail>> FilterChanged;

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

        protected bool FilterAudience(EncounterDetail encounter)
        {
            if (FilteredAudiences.Count == 0)
                return true;

            return FilteredAudiences.Contains(encounter.InfoGroup.GetLatestInfo().Audience);
        }

        public override void Clear()
        {
            foreach (var toggle in AudienceToggles)
                toggle.Toggle.isOn = false;
        }
    }
}