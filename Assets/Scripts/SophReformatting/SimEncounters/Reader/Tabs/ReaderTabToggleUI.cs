using ClinicalTools.SimEncounters.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabToggleUI : MonoBehaviour
    {
        [SerializeField] private EncounterToggleBehaviour selectToggle;
        public EncounterToggleBehaviour SelectToggle { get => selectToggle; set => selectToggle = value; }

        [SerializeField] private TextMeshProUGUI nameLabel;
        public TextMeshProUGUI NameLabel { get => nameLabel; set => nameLabel = value; }

        [SerializeField] private GameObject visited;
        public GameObject Visited { get => visited; set => visited = value; }


        public event Action Selected;

        protected virtual void Awake()
        {
            SelectToggle.Selected += () => Selected?.Invoke();
        }

        protected UserTab CurrentTab { get; set; }
        public void Display(UserTab tab)
        {
            CurrentTab = tab;
            NameLabel.text = tab.Data.Name;

            StatusChanged();
            tab.StatusChanged += StatusChanged;
        }

        public void SetToggleGroup(ToggleGroup group) => SelectToggle.SetToggleGroup(group);

        protected virtual void StatusChanged()
        {
            Visited.SetActive(CurrentTab.IsRead());
        }
        public virtual void Select() => SelectToggle.Select();

        protected virtual void OnDestroy()
        {
            CurrentTab.StatusChanged -= StatusChanged;
        }
    }
}