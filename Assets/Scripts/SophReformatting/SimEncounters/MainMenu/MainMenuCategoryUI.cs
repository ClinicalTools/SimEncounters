using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuCategoryUI : MonoBehaviour
    {
        [SerializeField] private SidebarUI sidebar;
        public SidebarUI Sidebar { get => sidebar; set => sidebar = value; }

        [SerializeField] private OverviewUI overview;
        public OverviewUI Overview { get => overview; set => overview = value; }

        int currentViewIndex = 0;
        [SerializeField] private List<MainMenuEncountersViewUI> encounterViews;
        public List<MainMenuEncountersViewUI> EncounterViews { get => encounterViews; set => encounterViews = value; }

        [SerializeField] private ToggleViewButtonUI toggleViewButton;
        public ToggleViewButtonUI ToggleViewButton { get => toggleViewButton; set => toggleViewButton = value; }

        [SerializeField] private ScrollRect scrollRect;
        public ScrollRect ScrollRect { get => scrollRect; set => scrollRect = value; }

        protected void Awake()
        {
            foreach (var encounterView in EncounterViews)
                encounterView.Selected += EncountersView_Selected;

            SetViewButton(GetNextView());
            ToggleViewButton.Selected += ChangeView;
        }

        public void Initialize()
        {
            EncounterViews[currentViewIndex].Hide();
            currentViewIndex = 0;
        }
        
        protected InfoNeededForMainMenuToHappen CurrentData { get; set; }
        protected Category CurrentCategory { get; set; }
        public virtual void Display(InfoNeededForMainMenuToHappen data, Category category)
        {
            CurrentCategory = category;
            CurrentData = data;
            ShowCategory();

            Sidebar.Show();
            Sidebar.SearchStuff.SortingOrder.SortingOrderChanged += (sortingOrder) => ShowCategory();
            Sidebar.SearchStuff.Filters.FilterChanged += (filter) => ShowCategory();
            ToggleViewButton.Show();
        }

        private IEnumerable<EncounterInfo> FilterEncounterDetails(IEnumerable<EncounterInfo> encounters)
        {
            var filter = Sidebar.SearchStuff.Filters.EncounterFilter;
            return encounters.Where(e => filter(e));
        }

        private void ShowCategory()
        {
            var encounters = new List<EncounterInfo>(FilterEncounterDetails(CurrentCategory.Encounters));
            encounters.Sort(Sidebar.SearchStuff.SortingOrder.Comparison);

            var encounterView = EncounterViews[currentViewIndex];
            encounterView.Display(CurrentData, encounters);
            if (CurrentCategory.Name.Equals("Obesity", StringComparison.InvariantCultureIgnoreCase))
                encounterView.HideMoreComingSoon();
            else
                encounterView.ShowMoreComingSoon();
            ScrollRect.content = (RectTransform)encounterView.transform;
            ScrollRect.verticalNormalizedPosition = 1;
        }


        private void EncountersView_Selected(EncounterInfo encounterInfo)
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