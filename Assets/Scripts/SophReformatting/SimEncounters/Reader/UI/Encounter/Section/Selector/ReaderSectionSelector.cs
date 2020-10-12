﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderSectionSelector : BaseUserSectionSelector
    {
        public virtual bool StartSectionAtFirstTab { get => startSectionAtFirstTab; set => startSectionAtFirstTab = value; }
        [SerializeField] private bool startSectionAtFirstTab;

        public virtual Transform SectionButtonsParent { get => sectionButtonsParent; set => sectionButtonsParent = value; }
        [SerializeField] private Transform sectionButtonsParent;
        public virtual ReaderSectionToggle SectionButtonPrefab { get => sectionButtonPrefab; set => sectionButtonPrefab = value; }
        [SerializeField] private ReaderSectionToggle sectionButtonPrefab;
        public virtual ToggleGroup SectionsToggleGroup { get => sectionsToggleGroup; set => sectionsToggleGroup = value; }
        [SerializeField] private ToggleGroup sectionsToggleGroup;

        public override event UserSectionSelectedHandler SectionSelected;

        protected UserEncounter UserEncounter { get; set; }
        protected Dictionary<UserSection, ReaderSectionToggle> SectionButtons { get; } = new Dictionary<UserSection, ReaderSectionToggle>();
        public override void Display(UserEncounter encounter)
        {
            foreach (var sectionButton in SectionButtons)
                Destroy(sectionButton.Value.gameObject);
            SectionButtons.Clear();

            UserEncounter = encounter;
            foreach (var section in encounter.Data.Content.NonImageContent.Sections) {
                var userSection = encounter.GetSection(section.Key);
                AddButton(userSection);
            }
        }

        protected void AddButton(UserSection section)
        {
            var sectionButton = Instantiate(SectionButtonPrefab, SectionButtonsParent);
            sectionButton.SetToggleGroup(SectionsToggleGroup);
            sectionButton.Display(section);
            sectionButton.Selected += () => OnSelected(section);
            SectionButtons.Add(section, sectionButton);
        }

        protected UserSection CurrentSection { get; set; }
        protected void OnSelected(UserSection section)
        {
            if (CurrentSection == section)
                return;

            var selectedArgs = new UserSectionSelectedEventArgs(section);
            if (StartSectionAtFirstTab)
                section.Data.CurrentTabIndex = 0;
            CurrentSection = section;
            SectionSelected?.Invoke(this, selectedArgs);
        }


        public override void Display(UserSection section)
        {
            if (CurrentSection == section)
                return;

            CurrentSection = section;
            SectionButtons[section].Select();
            foreach (var sectionButton in SectionButtons) {
                if (sectionButton.Key == section)
                    continue;
                sectionButton.Value.SelectToggle.Toggle.isOn = false;
            }
        }
    }
}