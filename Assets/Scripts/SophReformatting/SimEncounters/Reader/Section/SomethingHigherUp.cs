using ClinicalTools.SimEncounters.Data;
using System;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class UserEncounterDrawer : MonoBehaviour
    {
        public abstract void Display(UserEncounter userEncounter);
    }

    public class SomethingHigherUp : UserEncounterDrawer
    {
        [SerializeField] private SectionSelector sectionSelector;
        [SerializeField] private TabSelector tabSelector;
        [SerializeField] private UserSectionDrawer sectionDrawer;
        [SerializeField] private UserTabDrawer tabDrawer;
        [SerializeField] private Footer footer;
        [SerializeField] private UserEncounterDrawer generalEncounterDrawer;

        protected virtual void Awake()
        {
            sectionSelector.SectionSelected += OnSectionSelected;
            footer.SectionSelected += OnSectionSelected;
            tabSelector.TabSelected += OnTabSelected;
            footer.TabSelected += OnTabSelected;
        }

        protected UserEncounter UserEncounter { get; set; }
        public override void Display(UserEncounter userEncounter)
        {
            generalEncounterDrawer.Display(userEncounter);

            UserEncounter = userEncounter;
            footer.Display(userEncounter);
            sectionSelector.Display(userEncounter);

            var currentSection = userEncounter.Data.Content.GetCurrentSectionKey();
            SelectSection(userEncounter.GetSection(currentSection));
        }

        protected virtual UserSection CurrentSection { get; set; }
        private void OnSectionSelected(object sender, SectionSelectedEventArgs e) => SelectSection(e.SelectedSection);
        private void SelectSection(UserSection selectedSection)
        {
            if (CurrentSection == selectedSection)
                return;
            if (CurrentSection != null)
                UpdateSectionReadStatus(CurrentSection);

            CurrentSection = selectedSection;
            UserEncounter.Data.Content.SetCurrentSection(selectedSection.Data);

            sectionSelector.SelectSection(selectedSection);
            footer.SelectSection(selectedSection);

            tabSelector.Display(selectedSection);
            sectionDrawer.Display(selectedSection);

            var currentTab = selectedSection.Data.GetCurrentTabKey();
            SelectTab(selectedSection.GetTab(currentTab));
        }

        private void UpdateSectionReadStatus(UserSection section)
        {
            if (section.IsRead())
                return;

            var tabs = section.GetTabs();
            foreach (var tab in tabs) {
                if (!tab.IsRead())
                    return;
            }

            section.SetRead(true);
        }

        protected virtual UserTab CurrentTab { get; set; }
        private void OnTabSelected(object sender, TabSelectedEventArgs e) => SelectTab(e.SelectedTab);
        private void SelectTab(UserTab selectedTab)
        {
            if (CurrentTab == selectedTab)
                return;
            CurrentTab = selectedTab;

            CurrentSection.Data.SetCurrentTab(selectedTab.Data);
            tabSelector.SelectTab(selectedTab);
            footer.SelectTab(selectedTab);
            tabDrawer.Display(selectedTab);

            selectedTab.SetRead(true);
        }
    }

    public abstract class UserSectionDrawer : MonoBehaviour
    {
        public abstract void Display(UserSection userSection);
    }
    public abstract class UserTabDrawer : MonoBehaviour
    {
        public abstract void Display(UserTab userTab);
    }
    public abstract class UserPanelDrawer : MonoBehaviour
    {
        public abstract void Display(UserPanel userPanel);
    }
}