using ClinicalTools.SimEncounters.Collections;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderFooterWithPreviousButton : BaseReaderFooter
    {
        public virtual TextMeshProUGUI PageInfoLabel { get => pageInfoLabel; set => pageInfoLabel = value; }
        [SerializeField] private TextMeshProUGUI pageInfoLabel;
        public virtual Button NextButton { get => nextButton; set => nextButton = value; }
        [SerializeField] private Button nextButton;
        public virtual Button PreviousButton { get => previousButton; set => previousButton = value; }
        [SerializeField] private Button previousButton;
        public virtual Button FinishButton { get => finishButton; set => finishButton = value; }
        [SerializeField] private Button finishButton;
        public virtual TextMeshProUGUI FinishButtonText { get => finishButtonText; set => finishButtonText = value; }
        [SerializeField] private TextMeshProUGUI finishButtonText;


        public override event UserSectionSelectedHandler SectionSelected;
        public override event UserTabSelectedHandler TabSelected;

        public override event Action Finished;


        protected virtual void Awake()
        {
            NextButton.onClick.AddListener(GoToNext);
            PreviousButton.onClick.AddListener(GoToPrevious);
            FinishButton.onClick.AddListener(() => Finished?.Invoke());
        }

        protected UserEncounter UserEncounter { get; set; }
        protected EncounterNonImageContent NonImageContent => UserEncounter.Data.Content.NonImageContent;
        public override void Display(UserEncounter encounter)
        {
            UserEncounter = encounter;

            if (encounter.IsRead())
                EnableFinishButton();
            else
                encounter.StatusChanged += UpdateFinishButtonActive;
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

        protected UserSection CurrentSection { get; set; }
        public override void SelectSection(UserSection section)
        {
            if (CurrentSection == section)
                return;
            CurrentSection = section;
        }

        protected UserTab CurrentTab { get; set; }
        public override void SelectTab(UserTab tab)
        {
            if (CurrentTab == tab)
                return;
            CurrentTab = tab;

            PageInfoLabel.text = $"Page: {UserEncounter.Data.Content.NonImageContent.GetCurrentTabNumber()}";
        }

        protected virtual bool IsLast<T>(OrderedCollection<T> values, T value) => values.IndexOf(value) == values.Count - 1;

        protected virtual void GoToNext()
        {
            var section = CurrentSection.Data;
            if (section.CurrentTabIndex + 1 < section.Tabs.Count)
                GoToNextTab();
            else if (NonImageContent.CurrentSectionIndex + 1 < NonImageContent.Sections.Count)
                GoToNextSection();
            else
                Finished?.Invoke();
        }
        protected virtual void GoToPrevious()
        {
            if (CurrentSection.Data.CurrentTabIndex > 0)
                GoToNextTab();
            else if (NonImageContent.CurrentSectionIndex > 0)
                GoToNextSection();
        }

        protected virtual void GoToNextSection()
            => GoToSection(NonImageContent.CurrentSectionIndex + 1);
        protected virtual void GoToPreviousSection()
            => GoToSection(NonImageContent.CurrentSectionIndex - 1);
        protected virtual void GoToSection(int sectionIndex)
        {
            var nextSectionKey = NonImageContent.Sections[sectionIndex].Key;
            var nextSection = UserEncounter.GetSection(nextSectionKey);
            var selectedArgs = new UserSectionSelectedEventArgs(nextSection);
            SectionSelected?.Invoke(this, selectedArgs);
        }

        protected virtual void GoToNextTab()
            => GoToTab(CurrentSection.Data.CurrentTabIndex + 1);
        protected virtual void GoToPreviousTab()
            => GoToTab(CurrentSection.Data.CurrentTabIndex - 1);
        protected virtual void GoToTab(int tabIndex)
        {
            var section = CurrentSection.Data;
            var nextTabKey = section.Tabs[tabIndex].Key;
            var nextTab = CurrentSection.GetTab(nextTabKey);
            var selectedArgs = new UserTabSelectedEventArgs(nextTab);
            TabSelected?.Invoke(this, selectedArgs);
        }
    }
}