using ClinicalTools.SimEncounters.Collections;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderFooter : BaseReaderFooter
    {
        public virtual TextMeshProUGUI PageInfoLabel { get => pageInfoLabel; set => pageInfoLabel = value; }
        [SerializeField] private TextMeshProUGUI pageInfoLabel;
        public virtual Button NextTabButton { get => nextTabButton; set => nextTabButton = value; }
        [SerializeField] private Button nextTabButton;
        public virtual Button NextSectionButton { get => nextSectionButton; set => nextSectionButton = value; }
        [SerializeField] private Button nextSectionButton;
        public virtual Button FinishButton { get => finishButton; set => finishButton = value; }
        [SerializeField] private Button finishButton;
        public virtual TextMeshProUGUI FinishButtonText { get => finishButtonText; set => finishButtonText = value; }
        [SerializeField] private TextMeshProUGUI finishButtonText;


        public override event UserSectionSelectedHandler SectionSelected;
        public override event UserTabSelectedHandler TabSelected;

        public override event Action Completed;


        protected virtual void Awake()
        {
            NextTabButton.onClick.AddListener(GoToNextTab);
            NextSectionButton.onClick.AddListener(GoToNextSection);
            FinishButton.onClick.AddListener(() => Completed?.Invoke());
        }

        protected UserEncounter UserEncounter { get; set; }
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
            if (FinishButtonText != null)
                FinishButtonText.color = Color.white;
        }

        protected UserSection CurrentSection { get; set; }
        public override void Display(UserSectionSelectedEventArgs eventArgs)
        {
            if (CurrentSection == eventArgs.SelectedSection)
                return;
            CurrentSection = eventArgs.SelectedSection;
        }

        protected UserTab CurrentTab { get; set; }
        public override void Select(UserTabSelectedEventArgs eventArgs)
        {
            if (CurrentTab == eventArgs.SelectedTab)
                return;
            CurrentTab = eventArgs.SelectedTab;

            bool nextTabButtonVisible = false, nextSectionButtonVisible = false, finishButtonVisible = false;
            if (IsLast(CurrentSection.Data.Tabs, CurrentTab.Data)) {
                if (IsLast(UserEncounter.Data.Content.NonImageContent.Sections, CurrentSection.Data))
                    finishButtonVisible = true;
                else
                    nextSectionButtonVisible = true;
            } else {
                nextTabButtonVisible = true;
            }
            NextTabButton.gameObject.SetActive(nextTabButtonVisible);
            NextSectionButton.gameObject.SetActive(nextSectionButtonVisible);
            FinishButton.gameObject.SetActive(finishButtonVisible);

            PageInfoLabel.text = $"Page: {UserEncounter.Data.Content.NonImageContent.GetCurrentTabNumber()}";

        }

        protected virtual bool IsLast<T>(OrderedCollection<T> values, T value) => values.IndexOf(value) == values.Count - 1;

        protected virtual void GoToNextSection()
        {
            var content = UserEncounter.Data.Content;
            var nextSectionIndex = content.NonImageContent.CurrentSectionIndex + 1;
            var nextSectionKey = content.NonImageContent.Sections[nextSectionIndex].Key;
            var nextSection = UserEncounter.GetSection(nextSectionKey);
            var selectedArgs = new UserSectionSelectedEventArgs(nextSection, ChangeType.Next);
            SectionSelected?.Invoke(this, selectedArgs);
        }
        protected virtual void GoToNextTab()
        {
            var section = CurrentSection.Data;
            var nextTabIndex = section.CurrentTabIndex + 1;
            var nextTabKey = section.Tabs[nextTabIndex].Key;
            var nextTab = CurrentSection.GetTab(nextTabKey);
            var selectedArgs = new UserTabSelectedEventArgs(nextTab, ChangeType.Next);
            TabSelected?.Invoke(this, selectedArgs);
        }
    }
}