using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ReaderMenuEncountersUI : BaseMenuSceneDrawer
    {
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

        public GameObject DownloadingCases { get => downloadingCases; set => downloadingCases = value; }
        [SerializeField] private GameObject downloadingCases;


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
            DownloadingCases.SetActive(true);
            ShowCategoriesToggle.Select();
            DisplayCategories();

            loadingSceneInfo.Result.AddOnCompletedListener(SceneInfoLoaded);
        }

        protected virtual void SceneInfoLoaded(MenuSceneInfo sceneInfo)
        {
            sceneInfo.LoadingScreen.Stop();
            DownloadingCases.SetActive(false);

            SceneInfo = sceneInfo;
            CategorySelector.Display(sceneInfo, sceneInfo.MenuEncountersInfo.GetCategories());
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