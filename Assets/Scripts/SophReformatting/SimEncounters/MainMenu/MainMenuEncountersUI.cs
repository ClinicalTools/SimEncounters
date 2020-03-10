using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuEncountersUI : MonoBehaviour
    {
        [SerializeField] private ToggleViewButtonUI toggleViewButton;
        public ToggleViewButtonUI ToggleViewButton { get => toggleViewButton; set => toggleViewButton = value; }

        [SerializeField] private MainMenuCategoryGroupUI categoryGroup;
        public MainMenuCategoryGroupUI CategoryGroup { get => categoryGroup; set => categoryGroup = value; }

        int currentViewIndex = 0;
        [SerializeField] private List<MainMenuEncountersViewUI> encounterViews;
        public List<MainMenuEncountersViewUI> EncounterViews { get => encounterViews; set => encounterViews = value; }




        [SerializeField] private SidebarUI sidebar;
        public SidebarUI Sidebar { get => sidebar; set => sidebar = value; }

        [SerializeField] private OverviewUI overview;
        public OverviewUI Overview { get => overview; set => overview = value; }

        [SerializeField] private PanelButtonsUI panelButtons;
        public PanelButtonsUI PanelButtons { get => panelButtons; set => panelButtons = value; }

        [SerializeField] private PageButtonsUI pageButtons;
        public PageButtonsUI PageButtons { get => pageButtons; set => pageButtons = value; }

        [SerializeField] private GameObject downloadingCases;
        public GameObject DownloadingCasesObject { get => downloadingCases; set => downloadingCases = value; }

        [SerializeField] private ChangeSidePanelScript categoriesToggle;
        public ChangeSidePanelScript CategoriesToggle { get => categoriesToggle; set => categoriesToggle = value; }

        [SerializeField] private ChangeSidePanelScript categoryToggle;
        public ChangeSidePanelScript CategoryToggle { get => categoryToggle; set => categoryToggle = value; }

        [SerializeField] private ScrollRect scrollRect;
        public ScrollRect ScrollRect { get => scrollRect; set => scrollRect = value; }

        protected InfoNeededForMainMenuToHappen CurrentData { get; set; }

        protected void Awake()
        {
            foreach (var encounterView in EncounterViews)
                encounterView.Selected += EncountersView_Selected;

            SetViewButton(EncounterViews[currentViewIndex]);
        }

        public virtual void Display(InfoNeededForMainMenuToHappen data)
        {
            if (data.IsDone)
                ShowCategories(data);
            else
                data.CategoriesLoaded += (categories) => ShowCategories(data);
        }

        protected virtual void ShowCategories(InfoNeededForMainMenuToHappen data)
        {
            CategoriesToggle.Selected += CategoriesToggle_Selected;
            CurrentData = data;
            CategoryGroup.CategorySelected += CategorySelected;
            CategoryGroup.Display(data.Categories.Keys);
            ToggleViewButton.Selected += ChangeView;
        }

        private void CategoriesToggle_Selected()
        {
            EncounterViews[currentViewIndex].GameObject.SetActive(false);
            CategoryGroup.Show();
            CategoryToggle.Hide();
            ScrollRect.content = (RectTransform)CategoryGroup.transform;
            currentCategory = null;
        }

        private string currentCategory;
        private void CategorySelected(string category)
        {
            currentCategory = category;
            ShowCategory();
        }
        private void ShowCategory()
        {
            if (currentCategory == null)
                return;

            CategoryToggle.Show(currentCategory);
            var encounterView = EncounterViews[currentViewIndex];
            encounterView.Display(CurrentData, CurrentData.Categories[currentCategory].Encounters);
            ScrollRect.content = (RectTransform)encounterView.transform;

            DownloadingCasesObject.SetActive(false);
            encounterView.GameObject.SetActive(true);

            CategoryGroup.Hide();
        }


        private void EncountersView_Selected(EncounterDetail encounterInfo)
        {
            Overview.GameObject.SetActive(true);
            Overview.Display(CurrentData, encounterInfo);
        }

        protected void SetViewButton(MainMenuEncountersViewUI encountersViewUI)
        {
            ToggleViewButton.Text.text = encountersViewUI.ViewName;
            ToggleViewButton.Image.sprite = encountersViewUI.ViewSprite;
        }

        protected void ChangeView()
        {
            EncounterViews[currentViewIndex].gameObject.SetActive(false);

            currentViewIndex++;
            if (currentViewIndex >= EncounterViews.Count)
                currentViewIndex = 0;

            SetViewButton(EncounterViews[currentViewIndex]);
            ShowCategory();
        }
    }
}