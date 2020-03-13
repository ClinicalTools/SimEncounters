using UnityEngine;
using System;
using TMPro;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class SearchFilterUI : MonoBehaviour, IEncounterFilter
    {
        public Filter<EncounterDetail> EncounterFilter => FilterSearchTerm;
        public event Action<Filter<EncounterDetail>> FilterChanged;


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

            return encounter.InfoGroup.GetLatestInfo().Title.Contains(SearchTerm);
        }
    }
}