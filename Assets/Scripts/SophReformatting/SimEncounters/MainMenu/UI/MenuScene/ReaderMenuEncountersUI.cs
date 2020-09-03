using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMenuEncountersUI : BaseMenuSceneDrawer
    {
        public bool SelectCategory { get => selectCategory; set => selectCategory = value; }
        [SerializeField] private bool selectCategory;

        public BaseCategorySelector CategorySelector { get => categorySelector; set => categorySelector = value; }
        [SerializeField] private BaseCategorySelector categorySelector;
        public BaseEncounterSelector EncounterSelector { get => encounterSelector; set => encounterSelector = value; }
        [SerializeField] private BaseEncounterSelector encounterSelector;
        public ChangeSidePanelScript ShowCategoriesToggle { get => showCategoriesToggle; set => showCategoriesToggle = value; }
        [SerializeField] private ChangeSidePanelScript showCategoriesToggle;
        public ChangeSidePanelScript ShowEncountersToggle { get => showEncountersToggle; set => showEncountersToggle = value; }
        [SerializeField] private ChangeSidePanelScript showEncountersToggle;

        public OverviewUI Overview { get => overview; set => overview = value; }
        [SerializeField] private OverviewUI overview;

        public List<GameObject> DownloadingMessageObjects { get => downloadingMessageObjects; set => downloadingMessageObjects = value; }
        [SerializeField] private List<GameObject> downloadingMessageObjects;


        public MenuSceneInfo SceneInfo { get; set; }

        protected virtual bool IsOn { get; set; }
        protected virtual void AddListeners()
        {
            if (IsOn)
                return;
            IsOn = true;

            ShowCategoriesToggle.Selected += DisplayCategories;
            CategorySelector.CategorySelected += CategorySelected;
            EncounterSelector.EncounterSelected += EncounterSelected;
        }
        protected virtual void RemoveListeners()
        {
            if (!IsOn)
                return;
            IsOn = false;

            ShowCategoriesToggle.Selected -= DisplayCategories;
            CategorySelector.CategorySelected -= CategorySelected;
            EncounterSelector.EncounterSelected -= EncounterSelected;
        }

        public override void Display(LoadingMenuSceneInfo loadingSceneInfo)
        {
            AddListeners();

            EncounterSelector.Initialize();
            foreach (var downloadingMessageObject in DownloadingMessageObjects)
                downloadingMessageObject.SetActive(true);

            if (SelectCategory) {
                ShowCategoriesToggle.Display();
                ShowCategoriesToggle.Select();
                DisplayCategories();
            } else {
                ShowCategoriesToggle.Hide();
                ShowEncountersToggle.Hide();
                CategorySelector.Hide();
            }

            loadingSceneInfo.Result.AddOnCompletedListener(SceneInfoLoaded);
        }

        protected virtual void SceneInfoLoaded(TaskResult<MenuSceneInfo> sceneInfo)
        {
            sceneInfo.Value.LoadingScreen?.Stop();
            foreach (var downloadingMessageObject in DownloadingMessageObjects)
                downloadingMessageObject.SetActive(false);

            SceneInfo = sceneInfo.Value;


            if (SelectCategory)
                CategorySelector.Display(sceneInfo.Value, sceneInfo.Value.MenuEncountersInfo.GetCategories());
            else
                EncounterSelector.DisplayForRead(SceneInfo, SceneInfo.MenuEncountersInfo.GetEncounters());
        }

        private void DisplayCategories()
        {
            ShowEncountersToggle.Hide();
            EncounterSelector.Hide();
            Overview.Hide();

            CategorySelector.Show();
        }

        protected virtual void CategorySelected(Category category)
        {
            CategorySelector.Hide();

            ShowEncountersToggle.Show(category.Name);
            EncounterSelector.DisplayForRead(SceneInfo, category.Encounters);
        }

        protected virtual void EncounterSelected(MenuEncounter menuEncounter) => Overview.DisplayForRead(SceneInfo, menuEncounter);
        public override void Hide()
        {
            RemoveListeners();

            CategorySelector.Hide();
            EncounterSelector.Hide();
            ShowCategoriesToggle.Hide();
            ShowEncountersToggle.Hide();
            Overview.Hide();
        }
    }
}