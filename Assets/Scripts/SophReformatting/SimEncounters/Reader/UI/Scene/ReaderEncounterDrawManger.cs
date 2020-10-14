using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderEncounterDrawManger
    {
        protected List<IUserEncounterDrawer> EncounterDrawers { get; } = new List<IUserEncounterDrawer>();
        protected List<IUserSectionDrawer> SectionDrawers { get; } = new List<IUserSectionDrawer>();
        protected List<IUserTabDrawer> TabDrawers { get; } = new List<IUserTabDrawer>();

        protected List<IUserSectionSelector> SectionSelectors { get; } = new List<IUserSectionSelector>();
        protected List<IUserTabSelector> TabSelectors { get; } = new List<IUserTabSelector>();

        protected UserEncounter UserEncounter { get; set; }
        public virtual void DrawEncounter(IEnumerable<MonoBehaviour> readerObjects, UserEncounter userEncounter)
        {
            UserEncounter = userEncounter;

            foreach (var readerObject in readerObjects)
                AddReaderObject(readerObject);

            AddListeners();

            foreach (var encounterDrawer in EncounterDrawers)
                encounterDrawer.Display(userEncounter);

            SelectSection(userEncounter.GetCurrentSection());
        }

        protected virtual void AddReaderObject(MonoBehaviour readerObject)
        {
            if (readerObject is IUserEncounterDrawer encounterDrawer)
                EncounterDrawers.Add(encounterDrawer);
            if (readerObject is IUserSectionDrawer sectionDrawer)
                SectionDrawers.Add(sectionDrawer);
            if (readerObject is IUserTabDrawer tabDrawer)
                TabDrawers.Add(tabDrawer);

            if (readerObject is IUserSectionSelector sectionSelector)
                SectionSelectors.Add(sectionSelector);
            if (readerObject is IUserTabSelector tabSelector)
                TabSelectors.Add(tabSelector);
        }

        protected virtual void AddListeners()
        {
            foreach (var sectionSelector in SectionSelectors)
                sectionSelector.SectionSelected += OnSectionSelected;

            foreach (var tabSelector in TabSelectors)
                tabSelector.TabSelected += OnTabSelected;
        }

        protected virtual UserSection CurrentSection { get; set; }
        private void OnSectionSelected(object sender, UserSectionSelectedEventArgs e) => SelectSection(e.SelectedSection);
        private void SelectSection(UserSection selectedSection)
        {
            if (CurrentSection == selectedSection)
                return;
            CurrentSection = selectedSection;
            UserEncounter.Data.Content.NonImageContent.SetCurrentSection(selectedSection.Data);

            foreach (var sectionDrawer in SectionDrawers)
                sectionDrawer.Display(CurrentSection);

            SelectTab(selectedSection.GetCurrentTab());
        }

        protected virtual UserTab CurrentTab { get; set; }
        private void OnTabSelected(object sender, UserTabSelectedEventArgs e) => SelectTab(e.SelectedTab);
        private void SelectTab(UserTab selectedTab)
        {
            if (CurrentTab == selectedTab)
                return;

            CurrentTab = selectedTab;
            CurrentSection.Data.SetCurrentTab(selectedTab.Data);

            SetPanelsAsRead(selectedTab.GetPanels());

            foreach (var tabDrawer in TabDrawers)
                tabDrawer.Display(CurrentTab);
        }

        protected virtual void SetPanelsAsRead(IEnumerable<UserPanel> panels)
        {
            foreach (var panel in panels) {
                var childPanels = new List<UserPanel>(panel.GetChildPanels());
                if (childPanels.Count > 0)
                    SetPanelsAsRead(childPanels);
                else
                    panel.SetRead(true);
            }
        }
    }
}