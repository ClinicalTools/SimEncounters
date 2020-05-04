using ClinicalTools.SimEncounters.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterSectionToggle : MonoBehaviour
    {
        public EncounterToggleBehaviour SelectToggle { get => selectToggle; set => selectToggle = value; }
        [SerializeField] private EncounterToggleBehaviour selectToggle;
        public Image Image { get => image; set => image = value; }
        [SerializeField] private Image image;
        public Image Icon { get => icon; set => icon = value; }
        [SerializeField] private Image icon;
        public TextMeshProUGUI NameLabel { get => nameLabel; set => nameLabel = value; }
        [SerializeField] private TextMeshProUGUI nameLabel;

        public Action Selected;

        protected virtual void Awake()
        {
            SelectToggle.Selected += () => Selected?.Invoke();
            Selected += () => SetColor(true);
            SelectToggle.Unselected += () => SetColor(false);
        }

        protected Section CurrentSection { get; set; }
        public void Display(Encounter encounter, Section section)
        {
            CurrentSection = section;
            SetColor(false);
            var icons = encounter.Images.Icons;
            if (icons.ContainsKey(section.IconKey))
                Icon.sprite = icons[section.IconKey];

            NameLabel.text = section.Name;
        }

        public void Select() => SelectToggle.Select();

        public void SetToggleGroup(ToggleGroup group) => SelectToggle.SetToggleGroup(group);

        protected virtual void SetColor(bool isOn)
        {
            var color = CurrentSection.Color;
            if (isOn) {
                Image.color = color;
                Icon.color = Color.white;
                NameLabel.color = Color.white;
            } else {
                Image.color = Color.white;
                Icon.color = color;
                NameLabel.color = color;
            }
        }

    }
}