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
        public Button EditButton { get => editButton; set => editButton = value; }
        [SerializeField] private Button editButton;
        public Layout.LayoutElement LayoutElement { get => layoutElement; set => layoutElement = value; }
        [SerializeField] private Layout.LayoutElement layoutElement;

        public event Action Selected;
        protected virtual void Awake()
        {
            SelectToggle.Selected += OnSelected;
            SelectToggle.Unselected += OnUnselected;
        }

        protected Tab CurrentTab { get; set; }
        public void Display(Encounter encounter, Tab tab)
        {
            CurrentTab = tab;
            NameLabel.text = tab.Name;

            LayoutElement.WidthValues.Min = 150;
        }

        public void SetToggleGroup(ToggleGroup group) => SelectToggle.SetToggleGroup(group);

        public virtual void Select() => SelectToggle.Select();

        protected virtual void OnSelected()
        {
            Selected?.Invoke();
            EditButton.gameObject.SetActive(true);
            LayoutElement.WidthValues.Min = 210;
        }
        protected virtual void OnUnselected()
        {
            EditButton.gameObject.SetActive(false);
            LayoutElement.WidthValues.Min = 150;
        }
    }
}