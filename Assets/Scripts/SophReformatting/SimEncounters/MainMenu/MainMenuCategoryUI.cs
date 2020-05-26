using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuCategoryUI : MonoBehaviour
    {
        public SidebarUI Sidebar { get => sidebar; set => sidebar = value; }
        [SerializeField] private SidebarUI sidebar;
        public OverviewUI Overview { get => overview; set => overview = value; }
        [SerializeField] private OverviewUI overview;
        public List<MainMenuEncountersViewUI> EncounterViews { get => encounterViews; set => encounterViews = value; }
        [SerializeField] private List<MainMenuEncountersViewUI> encounterViews;
        public ToggleViewButtonUI ToggleViewButton { get => toggleViewButton; set => toggleViewButton = value; }
        [SerializeField] private ToggleViewButtonUI toggleViewButton;
        public ScrollRect ScrollRect { get => scrollRect; set => scrollRect = value; }
        [SerializeField] private ScrollRect scrollRect;

        int currentViewIndex = 0;

        protected void Awake()
        {
            foreach (var encounterView in EncounterViews)
                encounterView.Selected += EncountersView_Selected;

            SetViewButton(GetNextView());
            ToggleViewButton.Selected += ChangeView;

            Sidebar.SearchStuff.SortingOrder.SortingOrderChanged += (sortingOrder) => ShowCategory();
            Sidebar.SearchStuff.Filters.FilterChanged += (filter) => ShowCategory();
        }

        public void Initialize()
        {
            EncounterViews[currentViewIndex].Hide();
            currentViewIndex = 0;
        }
        
        protected MenuSceneInfo SceneInfo { get; set; }
        protected Category CurrentCategory { get; set; }
        public virtual void Display(MenuSceneInfo sceneInfo, Category category)
        {
            CurrentCategory = category;
            SceneInfo = sceneInfo;

            ShowCategory();

            Sidebar.Show();
            ToggleViewButton.Show();
        }


        private IEnumerable<MenuEncounter> FilterEncounterDetails(IEnumerable<MenuEncounter> encounters)
        {
            var filter = Sidebar.SearchStuff.Filters.EncounterFilter;
            return encounters.Where(e => filter(e));
        }

        private void ShowCategory()
        {
            var encounters = new List<MenuEncounter>(FilterEncounterDetails(CurrentCategory.Encounters));
            encounters.Sort(Sidebar.SearchStuff.SortingOrder.Comparison);

            var encounterView = EncounterViews[currentViewIndex];
            encounterView.Display(SceneInfo, encounters);
            if (CurrentCategory != null && CurrentCategory.Name.Equals("Obesity", StringComparison.InvariantCultureIgnoreCase))
                encounterView.HideMoreComingSoon();
            else
                encounterView.ShowMoreComingSoon();
            ScrollRect.content = (RectTransform)encounterView.transform;
            ScrollRect.verticalNormalizedPosition = 1;
        }


        private void EncountersView_Selected(MenuEncounter encounterInfo)
        {
            Overview.DisplayForRead(SceneInfo, encounterInfo);
        }

        protected void SetViewButton(MainMenuEncountersViewUI encountersViewUI)
        {
            ToggleViewButton.Text.text = encountersViewUI.ViewName;
            ToggleViewButton.Image.sprite = encountersViewUI.ViewSprite;
        }

        protected void ChangeView()
        {
            EncounterViews[currentViewIndex].Hide();

            currentViewIndex++;
            if (currentViewIndex >= EncounterViews.Count)
                currentViewIndex = 0;

            SetViewButton(GetNextView());
            ShowCategory();
        }

        protected MainMenuEncountersViewUI GetNextView()
        {
            var viewIndex = (currentViewIndex + 1) % EncounterViews.Count;
            return EncounterViews[viewIndex];
        }

        public void Hide()
        {
            EncounterViews[currentViewIndex].Hide();
            ToggleViewButton.Hide();
            Sidebar.Hide();
        }
    }
}