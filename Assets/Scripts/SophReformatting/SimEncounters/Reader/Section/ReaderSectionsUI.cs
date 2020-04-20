using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ClinicalTools.SimEncounters.Collections;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderSectionsUI : SectionSelector
    {
        [SerializeField] private Transform sectionButtonsParent;
        public virtual Transform SectionButtonsParent { get => sectionButtonsParent; set => sectionButtonsParent = value; }

        [SerializeField] private ReaderSectionToggleUI sectionButtonPrefab;
        public virtual ReaderSectionToggleUI SectionButtonPrefab { get => sectionButtonPrefab; set => sectionButtonPrefab = value; }

        [SerializeField] private ToggleGroup sectionsToggleGroup;
        public virtual ToggleGroup SectionsToggleGroup { get => sectionsToggleGroup; set => sectionsToggleGroup = value; }


        public override event SectionSelectedHandler SectionSelected;

        protected UserEncounter UserEncounter { get; set; }
        protected Dictionary<UserSection, ReaderSectionToggleUI> SectionButtons { get; } = new Dictionary<UserSection, ReaderSectionToggleUI>();
        public override void Display(UserEncounter userEncounter)
        {
            foreach (var sectionButton in SectionButtons)
                Destroy(sectionButton.Value.gameObject);
            SectionButtons.Clear();

            UserEncounter = userEncounter;
            foreach (var section in userEncounter.Data.Content.Sections) {
                var userSection = userEncounter.GetSection(section.Key);
                AddButton(userSection);
            }
        }

        protected void AddButton(UserSection userSection)
        {
            var sectionButton = Instantiate(SectionButtonPrefab, SectionButtonsParent);
            sectionButton.SetToggleGroup(SectionsToggleGroup);
            sectionButton.Display(userSection);
            sectionButton.Selected += () => OnSelected(userSection);
            SectionButtons.Add(userSection, sectionButton);
        }

        protected UserSection CurrentSection { get; set; }
        protected void OnSelected(UserSection userSection)
        {
            var section = userSection.Data;
            var selectedArgs = new SectionSelectedEventArgs(userSection);
            CurrentSection = userSection;
            SectionSelected?.Invoke(this, selectedArgs);
        }


        public override void SelectSection(UserSection userSection)
        {
            if (CurrentSection == userSection)
                return;

            SectionButtons[userSection].Select();
        }
    }
}