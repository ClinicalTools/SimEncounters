using ClinicalTools.SimEncounters.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterEncounterDrawer : BaseEncounterDrawer
    {
        public BaseWriterSectionsHandler SectionSelector { get => sectionSelector; set => sectionSelector = value; }
        [SerializeField] private BaseWriterSectionsHandler sectionSelector;
        public BaseTabSelector TabSelector { get => tabSelector; set => tabSelector = value; }
        [SerializeField] private BaseTabSelector tabSelector;
        public BaseSectionDrawer SectionDrawer { get => sectionDrawer; set => sectionDrawer = value; }
        [SerializeField] private BaseSectionDrawer sectionDrawer;
        public BaseTabDrawer TabDrawer { get => tabDrawer; set => tabDrawer = value; }
        [SerializeField] private BaseTabDrawer tabDrawer;

        protected virtual void Awake()
        {
            SectionSelector.SectionSelected += OnSectionSelected;
            SectionSelector.SectionEdited += OnSectionEdited;
            TabSelector.TabSelected += OnTabSelected;
        }

        protected Encounter Encounter { get; set; }
        public override void Display(Encounter encounter)
        {
            Encounter = encounter;

            SectionSelector.Display(encounter);
            SelectSection(encounter.Content.Sections[encounter.Content.CurrentSectionIndex].Value);
        }

        public override void Serialize() => TabDrawer.Serialize();

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
            if (currentTab != null)
                SelectTab(selectedSection.Tabs[currentTab]);
            else
                UnselectTab();
        }
        private void OnSectionEdited(Section section)
        {
            if (section == CurrentSection)
                SectionDrawer.Display(Encounter, section);
        }

        protected virtual Tab CurrentTab { get; set; }
        private void OnTabSelected(object sender, TabSelectedEventArgs e) => SelectTab(e.SelectedTab);
        private void SelectTab(Tab selectedTab)
        {
            if (CurrentTab == selectedTab)
                return;
            if (CurrentTab != null)
                TabDrawer.Serialize();

            CurrentTab = selectedTab;

            CurrentSection.SetCurrentTab(selectedTab);
            TabSelector.SelectTab(selectedTab);
            TabDrawer.Display(Encounter, selectedTab);
        }
        private void UnselectTab()
        {
            if (CurrentTab != null)
                TabDrawer.Serialize();
            CurrentTab = null;
            TabDrawer.Display(Encounter, null);
        }
    }
}