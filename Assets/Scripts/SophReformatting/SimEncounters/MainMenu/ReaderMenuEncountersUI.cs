using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ReaderMenuEncountersUI : BaseMenuSceneDrawer
    {
        public BaseCategorySelector CategoryGroup { get => categoryGroup; set => categoryGroup = value; }
        [SerializeField] private BaseCategorySelector categoryGroup;
        public BaseEncounterSelector Category { get => category; set => category = value; }
        [SerializeField] private BaseEncounterSelector category;
        public GameObject DownloadingCases { get => downloadingCases; set => downloadingCases = value; }
        [SerializeField] private GameObject downloadingCases;
        public ChangeSidePanelScript CategoriesToggle { get => categoriesToggle; set => categoriesToggle = value; }
        [SerializeField] private ChangeSidePanelScript categoriesToggle;
        public ChangeSidePanelScript CategoryToggle { get => categoryToggle; set => categoryToggle = value; }
        [SerializeField] private ChangeSidePanelScript categoryToggle;
        public ScrollRect ScrollRect { get => scrollRect; set => scrollRect = value; }
        [SerializeField] private ScrollRect scrollRect;

        public MenuSceneInfo SceneInfo { get; set; }

        public void Initialize()
        {
            DisplayCategories();
            CategoriesToggle.Select();
            DownloadingCases.SetActive(true);
            Category.Initialize(); 
        }

        public override void Display(LoadingMenuSceneInfo loadingSceneInfo)
        {
            Initialize();
            ShowCasesLoading();
            loadingSceneInfo.Result.AddOnCompletedListener(ShowCategories);
        }

        protected virtual void ShowCasesLoading() => DownloadingCases.SetActive(true);

        protected virtual void ShowCategories(MenuSceneInfo sceneInfo)
        {
            sceneInfo.LoadingScreen.Stop();
            DownloadingCases.SetActive(false);
            CategoriesToggle.Selected += DisplayCategories;

            SceneInfo = sceneInfo;
            CategoryGroup.CategorySelected += CategorySelected;
            CategoryGroup.Display(sceneInfo, sceneInfo.MenuEncountersInfo.GetCategories());
        }

        private void DisplayCategories()
        {
            CategoryGroup.Show();
            CategoryToggle.Hide();
            Category.Hide();
            ScrollRect.content = (RectTransform)CategoryGroup.transform;
            ScrollRect.verticalNormalizedPosition = 1;
        }

        private void CategorySelected(Category category)
        {
            CategoryGroup.Hide();
            CategoryToggle.Show(category.Name);
            Category.Display(SceneInfo, category.Encounters);
        }
    }
}