﻿using ClinicalTools.SimEncounters.Collections;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileFooter : BaseReaderFooter, IReaderSceneDrawer
    {
        public virtual TextMeshProUGUI PageInfoLabel { get => pageInfoLabel; set => pageInfoLabel = value; }
        [SerializeField] private TextMeshProUGUI pageInfoLabel;
        public virtual Button NextButton { get => nextButton; set => nextButton = value; }
        [SerializeField] private Button nextButton;
        public virtual Button PreviousButton { get => previousButton; set => previousButton = value; }
        [SerializeField] private Button previousButton;
        public virtual Button PrimaryFinishButton { get => primaryFinishButton; set => primaryFinishButton = value; }
        [SerializeField] private Button primaryFinishButton;
        public virtual Button SecondaryFinishButton { get => secondaryFinishButton; set => secondaryFinishButton = value; }
        [SerializeField] private Button secondaryFinishButton;


        public override event UserSectionSelectedHandler SectionSelected;
        public override event UserTabSelectedHandler TabSelected;

        public override event Action Completed;

        protected IUserEncounterMenuSceneStarter MenuSceneStarter { get; set; }
        protected AndroidBackButton BackButton { get; set; }
        [Inject]
        public virtual void Inject(IUserEncounterMenuSceneStarter menuSceneStarter, AndroidBackButton backButton)
        {
            MenuSceneStarter = menuSceneStarter;
            BackButton = backButton;
        }
        protected virtual void Awake()
        {
            NextButton.onClick.AddListener(GoToNext);
            if (PreviousButton != null)
                PreviousButton.onClick.AddListener(GoToPrevious);
            PrimaryFinishButton.onClick.AddListener(() => Completed?.Invoke());
            if (SecondaryFinishButton != null)
                SecondaryFinishButton.onClick.AddListener(() => Completed?.Invoke());

            BackButton.Register(BackButtonPressed);
        }

        protected User User { get; set; }
        protected ILoadingScreen LoadingScreen { get; set; }
        public void Display(LoadingReaderSceneInfo sceneInfo)
        {
            User = sceneInfo.User;
            LoadingScreen = sceneInfo.LoadingScreen;
        }

        protected UserEncounter UserEncounter { get; set; }
        protected EncounterNonImageContent NonImageContent
            => UserEncounter.Data.Content.NonImageContent;
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
            => PrimaryFinishButton.gameObject.SetActive(true);

        protected UserSection CurrentSection { get; set; }
        public override void Display(UserSectionSelectedEventArgs eventArgs)
        {
            if (CurrentSection == eventArgs.SelectedSection)
                return;
            CurrentSection = eventArgs.SelectedSection;
        }

        protected UserTab CurrentTab { get; set; }
        private int tabCount;
        private float lastTimeCreated;
        public override void Display(UserTabSelectedEventArgs eventArgs)
        {
            lastTimeCreated = Time.realtimeSinceStartup;

            if (CurrentTab == eventArgs.SelectedTab)
                return;
            CurrentTab = eventArgs.SelectedTab;


            var nonImageContent = UserEncounter.Data.Content.NonImageContent;
            var currentTabNumber = nonImageContent.GetCurrentTabNumber();

            PreviousButton.gameObject.SetActive(currentTabNumber > 1);

            if (tabCount == 0)
                tabCount = nonImageContent.GetTabCount();

            var lastTab = currentTabNumber == tabCount;
            NextButton.gameObject.SetActive(!lastTab);
            if (SecondaryFinishButton != null)
                SecondaryFinishButton.gameObject.SetActive(lastTab);

            PageInfoLabel.text = $"Page: {currentTabNumber}/{tabCount}";
        }

        protected virtual bool IsLast<T>(OrderedCollection<T> values, T value)
            => values.IndexOf(value) == values.Count - 1;

        private const float MinTabChangeTime = 0.0f;

        protected virtual void GoToNext()
        {
            if (Time.realtimeSinceStartup - lastTimeCreated < MinTabChangeTime)
                return;

            var section = CurrentSection.Data;
            if (section.CurrentTabIndex + 1 < section.Tabs.Count)
                GoToNextTab();
            else if (NonImageContent.CurrentSectionIndex + 1 < NonImageContent.Sections.Count)
                GoToNextSection();
            else
                Completed?.Invoke();
        }
        protected virtual void BackButtonPressed()
        {
            BackButton.Register(BackButtonPressed);
            GoToPrevious();
        }
        protected virtual void GoToPrevious()
        {
            if (Time.realtimeSinceStartup - lastTimeCreated < MinTabChangeTime)
                return;

            if (CurrentSection.Data.CurrentTabIndex > 0)
                GoToPreviousTab();
            else if (NonImageContent.CurrentSectionIndex > 0)
                GoToPreviousSection();
            else
                MenuSceneStarter.ConfirmStartingMenuScene(UserEncounter, LoadingScreen);
        }

        protected virtual void GoToNextSection()
        {
            var nextSectionIndex = NonImageContent.CurrentSectionIndex + 1;
            var section = NonImageContent.Sections[nextSectionIndex].Value;
            section.CurrentTabIndex = 0;
            GoToSection(nextSectionIndex, ChangeType.Next);
        }
        protected virtual void GoToPreviousSection()
        {
            var previousSectionIndex = NonImageContent.CurrentSectionIndex - 1;
            var section = NonImageContent.Sections[previousSectionIndex].Value;
            section.CurrentTabIndex = section.Tabs.Count - 1;
            GoToSection(previousSectionIndex, ChangeType.Previous);
        }
        protected virtual void GoToSection(int sectionIndex, ChangeType changeType)
        {
            var nextSectionKey = NonImageContent.Sections[sectionIndex].Key;
            var nextSection = UserEncounter.GetSection(nextSectionKey);
            var selectedArgs = new UserSectionSelectedEventArgs(nextSection, changeType);
            SectionSelected?.Invoke(this, selectedArgs);
        }

        protected virtual void GoToNextTab()
            => GoToTab(CurrentSection.Data.CurrentTabIndex + 1, ChangeType.Next);
        protected virtual void GoToPreviousTab()
            => GoToTab(CurrentSection.Data.CurrentTabIndex - 1, ChangeType.Previous);
        protected virtual void GoToTab(int tabIndex, ChangeType changeType)
        {
            var section = CurrentSection.Data;
            var nextTabKey = section.Tabs[tabIndex].Key;
            var nextTab = CurrentSection.GetTab(nextTabKey);
            var selectedArgs = new UserTabSelectedEventArgs(nextTab, changeType);
            TabSelected?.Invoke(this, selectedArgs);
        }
    }
}