using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderFullEncounterHandler : ReaderBehaviour, IUserEncounterDrawer, IUserSectionDrawer, IUserTabDrawer, IUserSectionSelector, IUserTabSelector
    {
        public List<MonoBehaviour> ReaderObjects { get => readerObjects; }
        [SerializeField] private List<MonoBehaviour> readerObjects = new List<MonoBehaviour>();

        protected List<IUserEncounterDrawer> EncounterDrawers { get; } = new List<IUserEncounterDrawer>();
        protected List<IUserSectionDrawer> SectionDrawers { get; } = new List<IUserSectionDrawer>();
        protected List<IUserTabDrawer> TabDrawers { get; } = new List<IUserTabDrawer>();

        protected List<IUserSectionSelector> SectionSelectors { get; } = new List<IUserSectionSelector>();
        protected List<IUserTabSelector> TabSelectors { get; } = new List<IUserTabSelector>();

        public virtual event UserSectionSelectedHandler SectionSelected;
        public virtual event UserTabSelectedHandler TabSelected;

        protected virtual void Awake()
        {
            foreach (var readerObject in ReaderObjects)
                AddReaderObject(readerObject);

            AddListeners();
        }

        protected virtual void AddListeners()
        {
            foreach (var sectionSelector in SectionSelectors)
                sectionSelector.SectionSelected += OnSectionSelected;

            foreach (var tabSelector in TabSelectors)
                tabSelector.TabSelected += OnTabSelected;
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

        public void Display(UserEncounter userEncounter)
        {
            foreach (var encounterDrawer in EncounterDrawers)
                encounterDrawer.Display(userEncounter);
        }
        public void Display(UserSection userSection)
        {
            foreach (var sectionDrawer in SectionDrawers)
                sectionDrawer.Display(userSection);
        }
        public void Display(UserTab userTab)
        {
            foreach (var tabDrawer in TabDrawers)
                tabDrawer.Display(userTab);
        }

        protected virtual UserSection CurrentSection { get; set; }
        private void OnSectionSelected(object sender, UserSectionSelectedEventArgs e)
            => SectionSelected?.Invoke(sender, e);
        public void SelectSection(UserSection userSection)
        {
            foreach (var sectionSelector in SectionSelectors)
                sectionSelector.SelectSection(userSection);
        }

        protected virtual UserTab CurrentTab { get; set; }
        private void OnTabSelected(object sender, UserTabSelectedEventArgs e)
            => TabSelected?.Invoke(sender, e);
        public virtual void SelectTab(UserTab userTab)
        {
            foreach (var tabSelector in TabSelectors)
                tabSelector.SelectTab(userTab);
        }
    }
}
