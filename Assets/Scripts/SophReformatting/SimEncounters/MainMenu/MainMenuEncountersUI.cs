using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuEncountersUI : MonoBehaviour
    {
        [SerializeField] private MainMenuCategoryGroupUI categoryGroup;
        public MainMenuCategoryGroupUI CategoryGroup { get => categoryGroup; set => categoryGroup = value; }

        [SerializeField] private MainMenuCategoryUI category;
        public MainMenuCategoryUI Category { get => category; set => category = value; }
                
        [SerializeField] private GameObject downloadingCases;
        public GameObject DownloadingCasesObject { get => downloadingCases; set => downloadingCases = value; }

        [SerializeField] private ChangeSidePanelScript categoriesToggle;
        public ChangeSidePanelScript CategoriesToggle { get => categoriesToggle; set => categoriesToggle = value; }

        [SerializeField] private ChangeSidePanelScript categoryToggle;
        public ChangeSidePanelScript CategoryToggle { get => categoryToggle; set => categoryToggle = value; }

        [SerializeField] private ScrollRect scrollRect;
        public ScrollRect ScrollRect { get => scrollRect; set => scrollRect = value; }

        protected InfoNeededForMainMenuToHappen CurrentData { get; set; }

        public void Initialize()
        {
            CategoryGroup.Clear();
            CategoryToggle.Hide();
            CategoriesToggle.Select();
            DownloadingCasesObject.SetActive(true);
            Category.Initialize();
        }

        public virtual void Display(InfoNeededForMainMenuToHappen data)
        {
            Initialize();
            if (data.IsDone)
                ShowCategories(data);
            else
                data.CategoriesLoaded += (categories) => ShowCategories(data);
        }

        protected virtual void ShowCategories(InfoNeededForMainMenuToHappen data)
        {
            DownloadingCasesObject.SetActive(false);
            CategoriesToggle.Selected += CategoriesToggle_Selected;

            CurrentData = data;
            CategoryGroup.CategorySelected += CategorySelected;
            CategoryGroup.Display(data.Categories.Keys);
        }

        private void CategoriesToggle_Selected()
        {
            CategoryGroup.Show();
            CategoryToggle.Hide();
            Category.Hide();
            ScrollRect.content = (RectTransform)CategoryGroup.transform;
        }

        private void CategorySelected(string category)
        {
            CategoryGroup.Hide();
            CategoryToggle.Show(category);
            Category.Display(CurrentData, CurrentData.Categories[category]);
        }
    }
}