using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderFooterUI : Footer
    {
        [SerializeField] private TextMeshProUGUI pageInfoLabel;
        public virtual TextMeshProUGUI PageInfoLabel { get => pageInfoLabel; set => pageInfoLabel = value; }
        
        [SerializeField] private Button nextTabButton;
        public virtual Button NextTabButton { get => nextTabButton; set => nextTabButton = value; }

        [SerializeField] private Button nextSectionButton;
        public virtual Button NextSectionButton { get => nextSectionButton; set => nextSectionButton = value; }

        [SerializeField] private Button finishCaseButton;
        public virtual Button FinishCaseButton { get => finishCaseButton; set => finishCaseButton = value; }


        public override event SectionSelectedHandler SectionSelected;
        public override event TabSelectedHandler TabSelected;


        protected virtual void Awake()
        {
            NextTabButton.onClick.AddListener(GoToNextTab);
            NextSectionButton.onClick.AddListener(GoToNextSection);
        }

        protected UserEncounter UserEncounter { get; set; }
        public override void Display(UserEncounter userEncounter)
        {
            UserEncounter = userEncounter;
        }

        protected UserSection CurrentSection { get; set; }
        public override void SelectSection(UserSection userSection)
        {
            if (CurrentSection == userSection)
                return;
            CurrentSection = userSection;
        }

        protected UserTab CurrentTab { get; set; }
        public override void SelectTab(UserTab userTab)
        {
            if (CurrentTab == userTab)
                return;
            CurrentTab = userTab;

            bool nextTabButtonVisible = false, nextSectionButtonVisible = false, finishButtonVisible = false;
            if (IsLast(CurrentSection.Data.Tabs, userTab.Data)) {
                if (IsLast(UserEncounter.Data.Content.Sections, CurrentSection.Data))
                    finishButtonVisible = true;
                else
                    nextSectionButtonVisible = true;
            } else {
                nextTabButtonVisible = true;
            }
            NextTabButton.gameObject.SetActive(nextTabButtonVisible);
            NextSectionButton.gameObject.SetActive(nextSectionButtonVisible);
            FinishCaseButton.gameObject.SetActive(finishButtonVisible);

        }

        protected virtual bool IsLast<T>(OrderedCollection<T> values, T value) => values.IndexOf(value) == values.Count - 1;

        protected virtual void GoToNextSection()
        {
            var content = UserEncounter.Data.Content;
            var nextSectionIndex = content.CurrentSectionIndex + 1;
            var nextSectionKey = content.Sections[nextSectionIndex].Key;
            var nextSection = UserEncounter.GetSection(nextSectionKey);
            var selectedArgs = new SectionSelectedEventArgs(nextSection);
            SectionSelected?.Invoke(this, selectedArgs);
        }
        protected virtual void GoToNextTab()
        {
            var section = CurrentSection.Data;
            var nextTabIndex = section.CurrentTabIndex + 1;
            var nextTabKey = section.Tabs[nextTabIndex].Key;
            var nextTab = CurrentSection.GetTab(nextTabKey);
            var selectedArgs = new TabSelectedEventArgs(nextTab);
            TabSelected?.Invoke(this, selectedArgs);
        }
    }
}