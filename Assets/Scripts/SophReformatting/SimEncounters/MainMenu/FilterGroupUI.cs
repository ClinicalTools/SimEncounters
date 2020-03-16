using UnityEngine;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class FilterGroupUI : EncounterFilterBehaviour
    {
        public override Filter<EncounterInfo> EncounterFilter => FilterGroups;

        public override event Action<Filter<EncounterInfo>> FilterChanged;

        [SerializeField] private List<EncounterFilterBehaviour> encounterFilters;
        public List<EncounterFilterBehaviour> EncounterFilters { get => encounterFilters; set => encounterFilters = value; }

        protected void Awake()
        {
            foreach (var encounterFilter in EncounterFilters)
                encounterFilter.FilterChanged += (filter) => FilterChanged?.Invoke(EncounterFilter);
        }

        protected bool FilterGroups(EncounterInfo encounter)
        {
            foreach (var encounterFilter in EncounterFilters) {
                if (!encounterFilter.EncounterFilter(encounter))
                    return false;
            }

            return true;
        }

        public override void Clear()
        {
            foreach (var encounterFilter in EncounterFilters)
                encounterFilter.Clear();
        }
    }
}