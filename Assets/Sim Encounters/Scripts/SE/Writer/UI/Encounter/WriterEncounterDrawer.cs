using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterEncounterDrawer : BaseSerializableEncounterDrawer
    {
        public BaseWriterSectionsHandler SectionSelector { get => sectionSelector; set => sectionSelector = value; }
        [SerializeField] private BaseWriterSectionsHandler sectionSelector;
        public BaseTabSelector TabSelector { get => tabSelector; set => tabSelector = value; }
        [SerializeField] private BaseTabSelector tabSelector;
        public BaseSectionDrawer SectionDrawer { get => sectionDrawer; set => sectionDrawer = value; }
        [SerializeField] private BaseSectionDrawer sectionDrawer;
        public BaseTabDrawer TabDrawer { get => tabDrawer; set => tabDrawer = value; }
        [SerializeField] private BaseTabDrawer tabDrawer;

        protected ISelector<EncounterSelectedEventArgs> EncounterSelector2 { get; set; }
        protected ISelector<SectionSelectedEventArgs> SectionSelector2 { get; set; }
        protected ISelector<TabSelectedEventArgs> TabSelector2 { get; set; }
        [Inject]
        public virtual void Inject(
            ISelector<EncounterSelectedEventArgs> encounterSelector,
            ISelector<SectionSelectedEventArgs> sectionSelector,
            ISelector<TabSelectedEventArgs> tabSelector)
        {
            EncounterSelector2 = encounterSelector;
            SectionSelector2 = sectionSelector;
            TabSelector2 = tabSelector;
        }

        protected virtual void Awake()
        {
            SectionSelector.SectionSelected += OnSectionSelected;
            SectionSelector.SectionEdited += OnSectionEdited;
            TabSelector.TabSelected += OnTabSelected;
        }

        protected Encounter Encounter { get; set; }
        public override void Display(Encounter encounter)
        {
            Encounter = encounter;
            EncounterSelector2.Select(this, new EncounterSelectedEventArgs(encounter));

            SectionSelector.Display(encounter);
            var sections = encounter.Content.NonImageContent.Sections;
            var section = sections[encounter.Content.NonImageContent.CurrentSectionIndex].Value;
            OnSectionSelected(this, new SectionSelectedEventArgs(section));
        }

        public override void Serialize() => TabDrawer.Serialize();

        protected virtual Section CurrentSection { get; set; }
        private void OnSectionSelected(object sender, SectionSelectedEventArgs e)
        {
            SectionSelector2.Select(sender, e);
            SelectSection(e.SelectedSection);
        }
        private void SelectSection(Section selectedSection)
        {
            if (CurrentSection == selectedSection)
                return;

            CurrentSection = selectedSection;
            Encounter.Content.NonImageContent.SetCurrentSection(selectedSection);

            SectionSelector.SelectSection(selectedSection);

            TabSelector.Display(Encounter, selectedSection);
            //SectionDrawer.Display(Encounter, selectedSection);

            var currentTab = selectedSection.GetCurrentTabKey();
            if (currentTab != null)
                OnTabSelected(this, new TabSelectedEventArgs(selectedSection.Tabs[currentTab]));
            else
                UnselectTab();
        }
        private void OnSectionEdited(Section section)
        {
            if (section == CurrentSection)
                SectionDrawer.Display(Encounter, section);
        }

        protected virtual Tab CurrentTab { get; set; }
        private void OnTabSelected(object sender, TabSelectedEventArgs e)
        {
            TabSelector2.Select(sender, e);
            SelectTab(e.SelectedTab);
        }
        private void SelectTab(Tab selectedTab)
        {
            if (CurrentTab == selectedTab)
                return;
            if (CurrentTab != null)
                TabDrawer.Serialize();

            CurrentTab = selectedTab;

            CurrentSection.SetCurrentTab(selectedTab);
            TabSelector.SelectTab(selectedTab);
            TabDrawer.Display(Encounter, selectedTab);
        }
        private void UnselectTab()
        {
            if (CurrentTab != null)
                TabDrawer.Serialize();
            CurrentTab = null;
            TabDrawer.Display(Encounter, null);
        }
    }
}