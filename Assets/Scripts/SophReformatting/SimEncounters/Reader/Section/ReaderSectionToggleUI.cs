using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderSectionToggleUI : MonoBehaviour
    {
        [SerializeField] private EncounterToggleBehaviour selectToggle;
        public EncounterToggleBehaviour SelectToggle { get => selectToggle; set => selectToggle = value; }

        [SerializeField] private Image image;
        public Image Image { get => image; set => image = value; }

        [SerializeField] private Image icon;
        public Image Icon { get => icon; set => icon = value; }

        [SerializeField] private TextMeshProUGUI nameLabel;
        public TextMeshProUGUI NameLabel { get => nameLabel; set => nameLabel = value; }

        [SerializeField] private GameObject visited;
        public GameObject Visited { get => visited; set => visited = value; }

        public Action Selected;
        public Action Unselected;

        protected virtual void Awake()
        {
            SelectToggle.Selected += () => Selected?.Invoke();
            Selected += () => SetColor(true);
            SelectToggle.Unselected += () => Unselected?.Invoke();
            Unselected += () => SetColor(false);
        }

        protected UserSection UserSection { get; set; }
        public void Display(UserSection userSection)
        {
            if (UserSection != null)
                userSection.StatusChanged -= StatusChanged;

            UserSection = userSection;
            var section = userSection.Data;
            SetColor(false);
            var icons = userSection.Encounter.Data.Images.Icons;
            if (icons.ContainsKey(section.IconKey))
                Icon.sprite = icons[section.IconKey];

            NameLabel.text = userSection.Data.Name;
            Visited.SetActive(userSection.IsRead());
            userSection.StatusChanged += StatusChanged;
        }

        private void StatusChanged() => Visited.SetActive(UserSection.IsRead());
        public void Select()
        {
            SelectToggle.Select();
        }

        public void SetToggleGroup(ToggleGroup group) => SelectToggle.SetToggleGroup(group);

        protected virtual void SetColor(bool isOn)
        {
            var color = UserSection.Data.Color;
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
