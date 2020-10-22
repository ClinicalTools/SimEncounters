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
        }

        protected UserEncounter UserEncounter { get; set; }
        protected EncounterNonImageContent NonImageContent
            => UserEncounter.Data.Content.NonImageContent;
        protected Section CurrentSection
            => NonImageContent.Sections[NonImageContent.CurrentSectionIndex].Value;
        protected virtual void OnEncounterSelected(object sender, UserEncounterSelectedEventArgs eventArgs)
            => UserEncounter = eventArgs.Encounter;


        protected UserSection CurrentUserSection { get; set; }
        protected virtual void OnSectionSelected(object sender, UserSectionSelectedEventArgs eventArgs)
            => CurrentUserSection = eventArgs.SelectedSection;

        public virtual bool HasNext() => HasNextSection() || HasNextTab();
        protected virtual bool HasNextSection() => NonImageContent.CurrentSectionIndex + 1 < NonImageContent.Sections.Count;
        protected virtual bool HasNextTab() => CurrentSection.CurrentTabIndex + 1 < CurrentSection.Tabs.Count;
        public virtual void GoToNext()
        {
            if (HasNextTab())
                GoToNextTab();
            else if (HasNextSection())
                GoToNextSection();
        }

        public virtual bool HasPrevious() => HasPreviousSection() || HasPreviousTab();
        protected virtual bool HasPreviousSection() => NonImageContent.CurrentSectionIndex != 0;
        protected virtual bool HasPreviousTab() => CurrentSection.CurrentTabIndex != 0;
        public virtual void GoToPrevious()
        {
            if (HasPreviousTab())
                GoToPreviousTab();
            else if (HasPreviousSection())
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
            => GoToTab(CurrentSection.CurrentTabIndex + 1, ChangeType.Next);
        protected virtual void GoToPreviousTab()
            => GoToTab(CurrentSection.CurrentTabIndex - 1, ChangeType.Previous);
        protected virtual void GoToTab(int tabIndex, ChangeType changeType)
        {
            var section = CurrentSection;
            var nextTabKey = section.Tabs[tabIndex].Key;
            var nextTab = CurrentUserSection.GetTab(nextTabKey);
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
            UserTabSelector = userTabSelector;

            LinearEncounterNavigator = linearEncounterNavigator;
            CompletionHandler = completionHandler;
            MenuSceneStarter = menuSceneStarter;
        }
        protected virtual void Start()
        {
            UserEncounterSelector.AddSelectedListener(OnEncounterSelected);
            UserTabSelector.AddSelectedListener(OnTabSelected);
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