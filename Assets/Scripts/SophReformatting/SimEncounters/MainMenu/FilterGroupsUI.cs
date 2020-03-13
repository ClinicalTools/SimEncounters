using UnityEngine;
using System;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class FilterGroupsUI : MonoBehaviour, IEncounterFilter
    {
        public Filter<EncounterDetail> EncounterFilter => FilterGroups;

        public event Action<Filter<EncounterDetail>> FilterChanged;

        [SerializeField] private IEncounterFilter encounterFiltersaaaa;

        [SerializeField] private List<IEncounterFilter> encounterFilters;
        public List<IEncounterFilter> EncounterFilters { get => encounterFilters; set => encounterFilters = value; }

        protected void Awake()
        {
            foreach (var encounterFilter in EncounterFilters)
                encounterFilter.FilterChanged += (filter) => FilterChanged?.Invoke(EncounterFilter);
        }

        protected bool FilterGroups(EncounterDetail encounter)
        {
            foreach (var encounterFilter in EncounterFilters) {
                if (!encounterFilter.EncounterFilter(encounter))
                    return false;
            }

            return true;
        }
    }
}