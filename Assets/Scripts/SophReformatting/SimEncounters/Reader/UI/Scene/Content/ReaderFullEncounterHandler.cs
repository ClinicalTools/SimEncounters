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
                Register(readerObject);
        }

        protected virtual void Register(MonoBehaviour readerObject)
        {
            if (readerObject is IUserEncounterDrawer encounterDrawer)
                EncounterDrawers.Add(encounterDrawer);
            if (readerObject is IUserSectionDrawer sectionDrawer)
                SectionDrawers.Add(sectionDrawer);
            if (readerObject is IUserTabDrawer tabDrawer)
                TabDrawers.Add(tabDrawer);

            if (readerObject is IUserSectionSelector sectionSelector) {
                SectionSelectors.Add(sectionSelector);
                sectionSelector.SectionSelected += OnSectionSelected;
            }
            if (readerObject is IUserTabSelector tabSelector) {
                TabSelectors.Add(tabSelector);
                tabSelector.TabSelected += OnTabSelected;
            }
        }

        protected virtual void Deregister(MonoBehaviour readerObject)
        {
            if (readerObject is IUserEncounterDrawer encounterDrawer)
                EncounterDrawers.Remove(encounterDrawer);
            if (readerObject is IUserSectionDrawer sectionDrawer)
                SectionDrawers.Remove(sectionDrawer);
            if (readerObject is IUserTabDrawer tabDrawer)
                TabDrawers.Remove(tabDrawer);

            if (readerObject is IUserSectionSelector sectionSelector) {
                SectionSelectors.Remove(sectionSelector);
                sectionSelector.SectionSelected -= OnSectionSelected;
            }
            if (readerObject is IUserTabSelector tabSelector) {
                TabSelectors.Remove(tabSelector);
                tabSelector.TabSelected -= OnTabSelected;
            }
        }

        public virtual void Display(UserEncounter userEncounter)
        {
            foreach (var encounterDrawer in EncounterDrawers)
                encounterDrawer.Display(userEncounter);
        }
        public virtual void Display(UserSection userSection)
        {
            foreach (var sectionDrawer in SectionDrawers)
                sectionDrawer.Display(userSection);
        }
        public virtual void Display(UserTab userTab)
        {
            foreach (var tabDrawer in TabDrawers)
                tabDrawer.Display(userTab);
        }

        protected virtual void OnSectionSelected(object sender, UserSectionSelectedEventArgs e)
            => SectionSelected?.Invoke(sender, e);
        public virtual void SelectSection(UserSection userSection)
        {
            foreach (var sectionSelector in SectionSelectors)
                sectionSelector.SelectSection(userSection);
        }

        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs e)
            => TabSelected?.Invoke(sender, e);
        public virtual void SelectTab(UserTab userTab)
        {
            foreach (var tabSelector in TabSelectors)
                tabSelector.SelectTab(userTab);
        }
    }
}
