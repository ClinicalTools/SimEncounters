using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileSidebar : MonoBehaviour, IUserEncounterDrawer, IUserSectionSelector, IReaderSceneDrawer
    {
        public virtual event UserSectionSelectedHandler SectionSelected;

        public List<MonoBehaviour> SidebarObjects { get => sidebarObjects; }
        [SerializeField] private List<MonoBehaviour> sidebarObjects = new List<MonoBehaviour>();

        protected List<IUserEncounterDrawer> EncounterDrawers { get; } = new List<IUserEncounterDrawer>();
        protected List<IUserSectionSelector> SectionSelectors { get; } = new List<IUserSectionSelector>();
        protected List<IReaderSceneDrawer> SceneDrawers { get; } = new List<IReaderSceneDrawer>();

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
            if (sidebarObject is IReaderSceneDrawer sceneDrawer)
                SceneDrawers.Add(sceneDrawer);
        }

        protected virtual void AddListeners()
        {
            foreach (var sectionSelector in SectionSelectors)
                sectionSelector.SectionSelected += OnSectionSelected;
        }

        public void Display(LoadingReaderSceneInfo sceneInfo)
        {
            Initialize();
            foreach (var sceneDrawer in SceneDrawers)
                sceneDrawer.Display(sceneInfo);
        }

        public virtual void Display(UserEncounter userEncounter)
        {
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
