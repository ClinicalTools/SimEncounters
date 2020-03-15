using UnityEngine;
using System;
using TMPro;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class SearchFilterUI : EncounterFilterBehaviour
    {
        public override Filter<EncounterDetail> EncounterFilter => FilterSearchTerm;
        public override event Action<Filter<EncounterDetail>> FilterChanged;


        [SerializeField] private TMP_InputField searchField;
        public virtual TMP_InputField SearchField { get => searchField; set => searchField = value; }

        protected string SearchTerm { get; set; }
        protected void Awake()
        {
            SearchField.onValueChanged.AddListener(SearchTermChanged);
        }

        protected void SearchTermChanged(string searchTerm)
        {
            SearchTerm = searchTerm;
            FilterChanged?.Invoke(EncounterFilter);
        }

        protected bool FilterSearchTerm(EncounterDetail encounter)
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
                return true;

            return encounter.InfoGroup.GetLatestInfo().Title.ToLower().Contains(SearchTerm.ToLower().Trim());
        }

        public override void Clear()
        {
            SearchField.text = "";
        }
    }
}