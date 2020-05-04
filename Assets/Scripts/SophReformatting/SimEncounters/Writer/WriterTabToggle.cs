using ClinicalTools.SimEncounters.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterTabToggle : MonoBehaviour
    {
        public EncounterToggleBehaviour SelectToggle { get => selectToggle; set => selectToggle = value; }
        [SerializeField] private EncounterToggleBehaviour selectToggle;
        public TextMeshProUGUI NameLabel { get => nameLabel; set => nameLabel = value; }
        [SerializeField] private TextMeshProUGUI nameLabel;

        public event Action Selected;
        protected virtual void Awake() => SelectToggle.Selected += () => Selected?.Invoke();

        protected Tab CurrentTab { get; set; }
        public void Display(Encounter encounter, Tab tab)
        {
            CurrentTab = tab;
            NameLabel.text = tab.Name;
        }

        public void SetToggleGroup(ToggleGroup group) => SelectToggle.SetToggleGroup(group);

        public virtual void Select() => SelectToggle.Select();
    }
}