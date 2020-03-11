using System;
using System.Collections.Generic;
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

            SetViewButton(EncounterViews[currentViewIndex]);
            ToggleViewButton.Selected += ChangeView;
        }

        public void Initialize()
        {
            currentViewIndex = 0;
            foreach (var encounterView in EncounterViews)
                encounterView.GameObject.SetActive(false);
        }

        
        protected InfoNeededForMainMenuToHappen CurrentData { get; set; }
        protected Category CurrentCategory { get; set; }
        public virtual void Display(InfoNeededForMainMenuToHappen data, Category category)
        {
            CurrentCategory = category;
            CurrentData = data;
            ShowCategory();
        }

        private void ShowCategory()
        {
            var encounterView = EncounterViews[currentViewIndex];
            encounterView.Display(CurrentData, CurrentCategory.Encounters);
            ScrollRect.content = (RectTransform)encounterView.transform;
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
            EncounterViews[currentViewIndex].Hide();

            currentViewIndex++;
            if (currentViewIndex >= EncounterViews.Count)
                currentViewIndex = 0;

            SetViewButton(EncounterViews[currentViewIndex]);
            ShowCategory();
        }

        public void Hide()
        {
            EncounterViews[currentViewIndex].Hide();

        }
    }
}