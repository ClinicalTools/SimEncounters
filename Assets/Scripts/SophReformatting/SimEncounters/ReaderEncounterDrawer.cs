using ClinicalTools.SimEncounters.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public interface ICompletable
    {
        event Action Finish;
    }

    public class ReaderEncounterDrawer : BaseUserEncounterDrawer, ICompletable
    {
        public BaseUserSectionSelector SectionSelector { get => sectionSelector; set => sectionSelector = value; }
        [SerializeField] private BaseUserSectionSelector sectionSelector;
        public BaseUserTabSelector TabSelector { get => tabSelector; set => tabSelector = value; }
        [SerializeField] private BaseUserTabSelector tabSelector;
        public BaseUserSectionDrawer SectionDrawer { get => sectionDrawer; set => sectionDrawer = value; }
        [SerializeField] private BaseUserSectionDrawer sectionDrawer;
        public BaseUserTabDrawer TabDrawer { get => tabDrawer; set => tabDrawer = value; }
        [SerializeField] private BaseUserTabDrawer tabDrawer;
        public BaseReaderFooter Footer { get => footer; set => footer = value; }
        [SerializeField] private BaseReaderFooter footer;
        public Button FinishButton { get => finishButton; set => finishButton = value; }
        [SerializeField] private Button finishButton;
        public virtual TextMeshProUGUI FinishButtonText { get => finishButtonText; set => finishButtonText = value; }
        [SerializeField] private TextMeshProUGUI finishButtonText;
        public BaseUserEncounterDrawer GeneralEncounterDrawer { get => generalEncounterDrawer; set => generalEncounterDrawer = value; }
        [SerializeField] private BaseUserEncounterDrawer generalEncounterDrawer;

        public event Action Finish;

        protected virtual void Awake()
        {
            SectionSelector.SectionSelected += OnSectionSelected;
            Footer.SectionSelected += OnSectionSelected;
            TabSelector.TabSelected += OnTabSelected;
            Footer.TabSelected += OnTabSelected;
            Footer.Finished += Finished;
            FinishButton.onClick.AddListener(Finished);
#if STANDALONE_SCENE
            FinishButton.gameObject.SetActive(false);
#endif
        }

        private void Finished() => Finish?.Invoke();

        protected UserEncounter UserEncounter { get; set; }
        public override void Display(UserEncounter userEncounter)
        {
            // TODO: remove listeners from previously displayed UserEncounter

            GeneralEncounterDrawer.Display(userEncounter);

            UserEncounter = userEncounter;
            Footer.Display(userEncounter);
            SectionSelector.Display(userEncounter);

            var currentSection = userEncounter.Data.Content.NonImageContent.GetCurrentSectionKey();
            SelectSection(userEncounter.GetSection(currentSection));

            if (userEncounter.IsRead())
                EnableFinishButton();
            else
                userEncounter.StatusChanged += UpdateFinishButtonActive;
        }

        protected virtual void UpdateFinishButtonActive()
        {
            if (!UserEncounter.IsRead())
                return;
            UserEncounter.StatusChanged -= UpdateFinishButtonActive; 
            EnableFinishButton();
        }

        protected virtual void EnableFinishButton()
        {
            FinishButton.interactable = true;
            //FinishButton.image.color = Color.white;
            FinishButtonText.color = Color.white;
        }

        protected virtual UserSection CurrentSection { get; set; }
        private void OnSectionSelected(object sender, UserSectionSelectedEventArgs e) => SelectSection(e.SelectedSection);
        private void SelectSection(UserSection selectedSection)
        {
            if (CurrentSection == selectedSection)
                return;

            CurrentSection = selectedSection;
            UserEncounter.Data.Content.NonImageContent.SetCurrentSection(selectedSection.Data);

            SectionSelector.SelectSection(selectedSection);
            Footer.SelectSection(selectedSection);

            TabSelector.Display(selectedSection);
            SectionDrawer.Display(selectedSection);

            var currentTab = selectedSection.Data.GetCurrentTabKey();
            SelectTab(selectedSection.GetTab(currentTab));
        }

        protected virtual UserTab CurrentTab { get; set; }
        private void OnTabSelected(object sender, UserTabSelectedEventArgs e) => SelectTab(e.SelectedTab);
        private void SelectTab(UserTab selectedTab)
        {
            if (CurrentTab == selectedTab)
                return;
            CurrentTab = selectedTab;

            CurrentSection.Data.SetCurrentTab(selectedTab.Data);
            TabSelector.SelectTab(selectedTab);
            Footer.SelectTab(selectedTab);
            TabDrawer.Display(selectedTab);

            selectedTab.SetRead(true);
        }
    }
}