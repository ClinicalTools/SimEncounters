using ClinicalTools.SimEncounters.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public interface ICompletionHandler
    {
        event Action Completed;
        void Complete();
    }
    public class CompletionHandler : ICompletionHandler
    {
        public event Action Completed;
        public virtual void Complete() => Completed?.Invoke();
    }

    public interface ILinearEncounterNavigator
    {
        bool HasNext();
        void GoToNext();
        bool HasPrevious();
        void GoToPrevious();
    }
    public class LinearEncounterNavigator : ILinearEncounterNavigator
    {
        protected ISelector<UserEncounterSelectedEventArgs> UserEncounterSelector { get; set; }
        protected ISelector<UserSectionSelectedEventArgs> UserSectionSelector { get; set; }
        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; set; }
        [Inject]
        public virtual void Inject(
            ISelector<UserEncounterSelectedEventArgs> userEncounterSelector,
            ISelector<UserSectionSelectedEventArgs> userSectionSelector,
            ISelector<UserTabSelectedEventArgs> userTabSelector)
        {
            UserEncounterSelector = userEncounterSelector;
            UserEncounterSelector.AddSelectedListener(OnEncounterSelected);
            UserSectionSelector = userSectionSelector;
            UserSectionSelector.AddSelectedListener(OnSectionSelected);
            UserTabSelector = userTabSelector;
            UserTabSelector.AddSelectedListener(OnTabSelected);
        }

        private int tabCount;
        private int tabNumber;
        protected UserEncounter UserEncounter { get; set; }
        protected EncounterNonImageContent NonImageContent
            => UserEncounter.Data.Content.NonImageContent;
        protected virtual void OnEncounterSelected(object sender, UserEncounterSelectedEventArgs eventArgs)
        {
            UserEncounter = eventArgs.Encounter;
            tabCount = NonImageContent.GetTabCount();
        }

        protected UserSection CurrentSection { get; set; }
        protected virtual void OnSectionSelected(object sender, UserSectionSelectedEventArgs eventArgs)
            => CurrentSection = eventArgs.SelectedSection;


        protected UserTab CurrentTab { get; set; }
        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            if (CurrentTab == eventArgs.SelectedTab)
                return;
            CurrentTab = eventArgs.SelectedTab;
            tabNumber = NonImageContent.GetCurrentTabNumber();
        }

        protected virtual bool IsLast<T>(OrderedCollection<T> values, T value)
            => values.IndexOf(value) == values.Count - 1;

        public virtual bool HasNext() => tabNumber > 1;
        public virtual void GoToNext()
        {
            if (CurrentSection.Data.CurrentTabIndex + 1 < CurrentSection.Data.Tabs.Count)
                GoToNextTab();
            else if (NonImageContent.CurrentSectionIndex + 1 < NonImageContent.Sections.Count)
                GoToNextSection();
        }
        public virtual bool HasPrevious() => tabNumber < tabCount;
        public virtual void GoToPrevious()
        {
            if (CurrentSection.Data.CurrentTabIndex > 0)
                GoToPreviousTab();
            else if (NonImageContent.CurrentSectionIndex > 0)
                GoToPreviousSection();
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
            UserSectionSelector.Select(this, selectedArgs);
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
            UserTabSelector.Select(this, selectedArgs);
        }
    }



    public class BackButtonEncounterNavigation
    {
        protected ILinearEncounterNavigator LinearEncounterNavigator { get; set; }
        protected IUserEncounterMenuSceneStarter MenuSceneStarter { get; set; }
        protected AndroidBackButton BackButton { get; set; }
        [Inject]
        public virtual void Inject(
            ILinearEncounterNavigator linearEncounterNavigator,
            IUserEncounterMenuSceneStarter menuSceneStarter,
            AndroidBackButton backButton)
        {
            LinearEncounterNavigator = linearEncounterNavigator;            
            MenuSceneStarter = menuSceneStarter;
            
            BackButton = backButton;
            BackButton.Register(BackButtonThing);
        }

        protected virtual void BackButtonThing()
        {
            BackButton.Register(BackButtonThing);
            if (LinearEncounterNavigator.HasPrevious())
                LinearEncounterNavigator.GoToPrevious();
            //else MenuSceneStarter.ConfirmStartingMenuScene(async,);

        }
    }

    public class ReaderMobileFooter : MonoBehaviour
    {
        public virtual Button NextButton { get => nextButton; set => nextButton = value; }
        [SerializeField] private Button nextButton;
        public virtual Button PreviousButton { get => previousButton; set => previousButton = value; }
        [SerializeField] private Button previousButton;
        public virtual Button PrimaryFinishButton { get => primaryFinishButton; set => primaryFinishButton = value; }
        [SerializeField] private Button primaryFinishButton;

        protected ILinearEncounterNavigator LinearEncounterNavigator { get; set; }
        protected ISelector<UserEncounterSelectedEventArgs> UserEncounterSelector { get; set; }
        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; set; }
        protected ICompletionHandler CompletionHandler { get; set; }
        protected IUserEncounterMenuSceneStarter MenuSceneStarter { get; set; }
        [Inject]
        public virtual void Inject(
            ILinearEncounterNavigator linearEncounterNavigator,
            ISelector<UserEncounterSelectedEventArgs> userEncounterSelector,
            ISelector<UserTabSelectedEventArgs> userTabSelector,
            ICompletionHandler completionHandler,
            IUserEncounterMenuSceneStarter menuSceneStarter)
        {
            UserEncounterSelector = userEncounterSelector;
            UserEncounterSelector.AddSelectedListener(OnEncounterSelected);
            UserTabSelector = userTabSelector;
            UserTabSelector.AddSelectedListener(OnTabSelected);

            LinearEncounterNavigator = linearEncounterNavigator;
            CompletionHandler = completionHandler;
            MenuSceneStarter = menuSceneStarter;
        }

        protected virtual void Awake()
        {
            NextButton.onClick.AddListener(GoToNext);
            if (PreviousButton != null)
                PreviousButton.onClick.AddListener(GoToPrevious);
            PrimaryFinishButton.onClick.AddListener(Complete);
        }

        protected UserEncounter UserEncounter { get; set; }
        protected EncounterNonImageContent NonImageContent
            => UserEncounter.Data.Content.NonImageContent;
        protected virtual void OnEncounterSelected(object sender, UserEncounterSelectedEventArgs eventArgs)
        {
            UserEncounter = eventArgs.Encounter;

            if (UserEncounter.IsRead())
                EnableFinishButton();
            else
                UserEncounter.StatusChanged += UpdateFinishButtonActive;
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

        protected UserTab CurrentTab { get; set; }
        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            if (CurrentTab == eventArgs.SelectedTab)
                return;
            CurrentTab = eventArgs.SelectedTab;

            if (PreviousButton != null)
                PreviousButton.gameObject.SetActive(LinearEncounterNavigator.HasPrevious());

            NextButton.gameObject.SetActive(LinearEncounterNavigator.HasNext());
        }

        protected virtual void GoToNext() => LinearEncounterNavigator.GoToNext();
        protected virtual void GoToPrevious() => LinearEncounterNavigator.GoToPrevious();
        protected virtual void Complete() => CompletionHandler.Complete();
    }
}