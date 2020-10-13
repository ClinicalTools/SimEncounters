using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public interface ICloseSidebar
    {
        event Action CloseSidebar;
    }

    public interface IOpenSidebar
    {
        event Action OpenSidebar;
    }

    public class ReaderMobileSidebar : MonoBehaviour, IUserEncounterDrawer, IUserSectionSelector, IReaderSceneDrawer, ICloseSidebar, IOpenSidebar
    {
        public virtual event UserSectionSelectedHandler SectionSelected;

        public List<MonoBehaviour> SidebarObjects { get => sidebarObjects; }
        [SerializeField] private List<MonoBehaviour> sidebarObjects = new List<MonoBehaviour>();
        public List<Button> CloseButtons { get => closeButtons; }
        [SerializeField] private List<Button> closeButtons = new List<Button>();

        protected List<IUserEncounterDrawer> EncounterDrawers { get; } = new List<IUserEncounterDrawer>();
        protected List<IUserSectionSelector> SectionSelectors { get; } = new List<IUserSectionSelector>();
        protected List<IReaderSceneDrawer> SceneDrawers { get; } = new List<IReaderSceneDrawer>();

        public event Action CloseSidebar;
        public event Action OpenSidebar;


        protected virtual void Awake() => Initialize();


        private bool initialized = false;
        protected virtual void Initialize()
        {
            if (initialized)
                return;
            initialized = true;

            foreach (var sidebarObject in SidebarObjects)
                AddSidebarObject(sidebarObject);
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(() => CloseSidebar?.Invoke());

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

        protected virtual void OnDisable()
        {
            foreach (var sectionSelector in SectionSelectors)
                sectionSelector.SectionSelected += OnSectionSelected;
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
            StartCoroutine(CloseAfterSecond());
            foreach (var encounterDrawer in EncounterDrawers)
                encounterDrawer.Display(userEncounter);
        }

        protected UserSection CurrentSection { get; set; }

        public virtual void OnSectionSelected(object sender, UserSectionSelectedEventArgs e)
        {
            CurrentSection = e.SelectedSection;
            CloseSidebar?.Invoke();
            SectionSelected?.Invoke(sender, e);
        }

        public void Display(UserSection userSection)
        {
            foreach (var sectionSelector in SectionSelectors)
                sectionSelector.Display(userSection);

            if (CurrentSection == userSection)
                return;

            CurrentSection = userSection;
            //OpenSidebar?.Invoke();
            //StartCoroutine(CloseAfterSecond());
        }

        protected IEnumerator CloseAfterSecond()
        {
            yield return new WaitForSeconds(2f);
            CloseSidebar?.Invoke();
        }
    }
}
