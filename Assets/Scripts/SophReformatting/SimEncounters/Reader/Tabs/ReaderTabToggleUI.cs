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

        protected UserTab UserTab { get; set; }

        protected virtual void Awake()
        {
            SelectToggle.Selected += () => Selected?.Invoke();
        }

        public void Display(UserTab userTab)
        {
            UserTab = userTab;
            NameLabel.text = userTab.Data.Name;

            StatusChanged();
            userTab.StatusChanged += StatusChanged;
        }

        public void SetToggleGroup(ToggleGroup group) => SelectToggle.SetToggleGroup(group);

        protected virtual void StatusChanged()
        {
            Visited.SetActive(UserTab.IsRead());
        }
        public virtual void Select() => SelectToggle.Select();
    }
}