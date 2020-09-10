using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileSidebar : MonoBehaviour, IUserEncounterDrawer, IUserSectionSelector
    {
        public virtual event UserSectionSelectedHandler SectionSelected;

        public List<MonoBehaviour> SidebarObjects { get => sidebarObjects; }
        [SerializeField] private List<MonoBehaviour> sidebarObjects = new List<MonoBehaviour>();

        protected List<IUserEncounterDrawer> EncounterDrawers { get; } = new List<IUserEncounterDrawer>();
        protected List<IUserSectionSelector> SectionSelectors { get; } = new List<IUserSectionSelector>();

        protected virtual void Awake() => Initialize();


        private bool initialized = false;
        protected virtual void Initialize()
        {
            if (initialized)
                return;
            initialized = true;

            foreach (var sidebarObject in SidebarObjects)
                AddSidebarObject(sidebarObject);

            AddListeners();
        }

        protected virtual void AddSidebarObject(MonoBehaviour sidebarObject)
        {
            if (sidebarObject is IUserEncounterDrawer encounterDrawer)
                EncounterDrawers.Add(encounterDrawer);

            if (sidebarObject is IUserSectionSelector sectionSelector)
                SectionSelectors.Add(sectionSelector);
        }

        protected virtual void AddListeners()
        {
            foreach (var sectionSelector in SectionSelectors)
                sectionSelector.SectionSelected += OnSectionSelected;
        }

        public virtual void Display(UserEncounter userEncounter)
        {
            Initialize();
            foreach (var encounterDrawer in EncounterDrawers)
                encounterDrawer.Display(userEncounter);
        }

        public virtual void OnSectionSelected(object sender, UserSectionSelectedEventArgs e)
            => SectionSelected?.Invoke(sender, e);
        public void SelectSection(UserSection userSection)
        {
            foreach (var sectionSelector in SectionSelectors)
                sectionSelector.SelectSection(userSection);
        }
    }
}
