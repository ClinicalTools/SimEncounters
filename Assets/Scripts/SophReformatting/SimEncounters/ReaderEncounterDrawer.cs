using ClinicalTools.SimEncounters.Data;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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
        public BaseUserEncounterDrawer GeneralEncounterDrawer { get => generalEncounterDrawer; set => generalEncounterDrawer = value; }
        [SerializeField] private BaseUserEncounterDrawer generalEncounterDrawer;

        public event Action Finish;

        private EncounterStatusSerializer statusSerializer;
        private IParser<EncounterContentStatus> statusDeserializer;
        [Inject] public virtual void Inject(EncounterStatusSerializer statusSerializer, IParser<EncounterContentStatus> statusDeserializer)
        {
            this.statusSerializer = statusSerializer;
            this.statusDeserializer = statusDeserializer;
        }

        protected virtual void Awake()
        {
            SectionSelector.SectionSelected += OnSectionSelected;
            Footer.SectionSelected += OnSectionSelected;
            TabSelector.TabSelected += OnTabSelected;
            Footer.TabSelected += OnTabSelected;
            Footer.Finished += Finished;
            FinishButton.onClick.AddListener(Finished);
        }

        private void Finished()
        {
            var x = statusSerializer.Serialize(UserEncounter.Status.ContentStatus);
            var status2 = statusDeserializer.Parse(x);
            var y = statusSerializer.Serialize(status2);
            Debug.Log($"{x}\n{y}");

            Finish?.Invoke();
        }

        protected UserEncounter UserEncounter { get; set; }
        public override void Display(UserEncounter userEncounter)
        {
            GeneralEncounterDrawer.Display(userEncounter);

            UserEncounter = userEncounter;
            Footer.Display(userEncounter);
            SectionSelector.Display(userEncounter);

            var currentSection = userEncounter.Data.Content.GetCurrentSectionKey();
            SelectSection(userEncounter.GetSection(currentSection));
        }

        protected virtual UserSection CurrentSection { get; set; }
        private void OnSectionSelected(object sender, UserSectionSelectedEventArgs e) => SelectSection(e.SelectedSection);
        private void SelectSection(UserSection selectedSection)
        {
            if (CurrentSection == selectedSection)
                return;

            CurrentSection = selectedSection;
            UserEncounter.Data.Content.SetCurrentSection(selectedSection.Data);

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