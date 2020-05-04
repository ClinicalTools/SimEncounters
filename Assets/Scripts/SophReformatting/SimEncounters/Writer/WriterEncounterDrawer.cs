﻿using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterEncounterDrawer : BaseEncounterDrawer
    {
        public BaseSectionSelector SectionSelector { get => sectionSelector; set => sectionSelector = value; }
        [SerializeField] private BaseSectionSelector sectionSelector;
        public BaseTabSelector TabSelector { get => tabSelector; set => tabSelector = value; }
        [SerializeField] private BaseTabSelector tabSelector;
        public BaseSectionDrawer SectionDrawer { get => sectionDrawer; set => sectionDrawer = value; }
        [SerializeField] private BaseSectionDrawer sectionDrawer;
        public BaseTabDrawer TabDrawer { get => tabDrawer; set => tabDrawer = value; }
        [SerializeField] private BaseTabDrawer tabDrawer;

        protected virtual void Awake()
        {
            SectionSelector.SectionSelected += OnSectionSelected;
            TabSelector.TabSelected += OnTabSelected;
        }

        protected Encounter Encounter { get; set; }
        public override void Display(Encounter encounter)
        {
            Encounter = encounter;

            SectionSelector.Display(encounter);
            SelectSection(encounter.Content.Sections[0].Value);
        }
        protected virtual Section CurrentSection { get; set; }
        private void OnSectionSelected(object sender, SectionSelectedEventArgs e) => SelectSection(e.SelectedSection);
        private void SelectSection(Section selectedSection)
        {
            if (CurrentSection == selectedSection)
                return;

            CurrentSection = selectedSection;
            Encounter.Content.SetCurrentSection(selectedSection);

            SectionSelector.SelectSection(selectedSection);

            TabSelector.Display(Encounter, selectedSection);
            SectionDrawer.Display(Encounter, selectedSection);

            var currentTab = selectedSection.GetCurrentTabKey();
            SelectTab(selectedSection.Tabs[currentTab]);
        }

        protected virtual Tab CurrentTab { get; set; }
        private void OnTabSelected(object sender, TabSelectedEventArgs e) => SelectTab(e.SelectedTab);
        private void SelectTab(Tab selectedTab)
        {
            if (CurrentTab == selectedTab)
                return;
            CurrentTab = selectedTab;

            CurrentSection.SetCurrentTab(selectedTab);
            TabSelector.SelectTab(selectedTab);
            TabDrawer.Display(Encounter, selectedTab);
        }
    }
}