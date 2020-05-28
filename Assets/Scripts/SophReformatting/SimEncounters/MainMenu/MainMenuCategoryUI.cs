using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuCategoryUI : BaseEncounterSelector
    {
        public SidebarUI Sidebar { get => sidebar; set => sidebar = value; }
        [SerializeField] private SidebarUI sidebar;
        public List<BaseViewEncounterSelector> EncounterViews { get => encounterViews; set => encounterViews = value; }
        [SerializeField] private List<BaseViewEncounterSelector> encounterViews;
        public ToggleViewButtonUI ToggleViewButton { get => toggleViewButton; set => toggleViewButton = value; }
        [SerializeField] private ToggleViewButtonUI toggleViewButton;
        public ScrollRect ScrollRect { get => scrollRect; set => scrollRect = value; }
        [SerializeField] private ScrollRect scrollRect;

        private int currentViewIndex = 0;

        public override event Action<MenuEncounter> EncounterSelected;

        protected void Awake()
        {
            foreach (var encounterView in EncounterViews)
                encounterView.EncounterSelected += EncountersView_Selected;

            ToggleViewButton.Display(GetNextView());
            ToggleViewButton.Selected += ChangeView;

            Sidebar.SearchStuff.SortingOrder.SortingOrderChanged += (sortingOrder) => ShowCategory();
            Sidebar.SearchStuff.Filters.FilterChanged += (filter) => ShowCategory();
        }

        public override void Initialize()
        {
            EncounterViews[currentViewIndex].Hide();
            currentViewIndex = 0;
        }

        protected MenuSceneInfo SceneInfo { get; set; }
        protected IEnumerable<MenuEncounter> CurrentEncounters { get; set; }
        public override void Display(MenuSceneInfo sceneInfo, IEnumerable<MenuEncounter> encounters)
        {
            SceneInfo = sceneInfo;
            CurrentEncounters = encounters;

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
            var encounters = new List<MenuEncounter>(FilterEncounterDetails(CurrentEncounters));
            encounters.Sort(Sidebar.SearchStuff.SortingOrder.Comparison);

            var encounterView = EncounterViews[currentViewIndex];
            encounterView.Display(SceneInfo, encounters);
            ScrollRect.content = (RectTransform)encounterView.transform;
            ScrollRect.verticalNormalizedPosition = 1;
        }


        private void EncountersView_Selected(MenuEncounter menuEncounter) => EncounterSelected?.Invoke(menuEncounter);
        
        protected void ChangeView()
        {
            EncounterViews[currentViewIndex].Hide();

            currentViewIndex++;
            if (currentViewIndex >= EncounterViews.Count)
                currentViewIndex = 0;

            ToggleViewButton.Display(GetNextView());
            ShowCategory();
        }

        protected BaseViewEncounterSelector GetNextView()
        {
            var viewIndex = (currentViewIndex + 1) % EncounterViews.Count;
            return EncounterViews[viewIndex];
        }

        public override void Hide()
        {
            EncounterViews[currentViewIndex].Hide();
            ToggleViewButton.Hide();
            Sidebar.Hide();
        }
    }
}