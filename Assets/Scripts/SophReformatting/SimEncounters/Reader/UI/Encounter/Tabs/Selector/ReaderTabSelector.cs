﻿using ClinicalTools.UI.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderTabSelector : MonoBehaviour
    {
        public virtual Transform TabButtonsParent { get => tabButtonsParent; set => tabButtonsParent = value; }
        [SerializeField] private Transform tabButtonsParent;
        public virtual BaseReaderTabToggle TabButtonPrefab { get => tabButtonPrefab; set => tabButtonPrefab = value; }
        [SerializeField] private BaseReaderTabToggle tabButtonPrefab;
        public virtual ToggleGroup TabsToggleGroup { get => tabsToggleGroup; set => tabsToggleGroup = value; }
        [SerializeField] private ToggleGroup tabsToggleGroup;
        public virtual ScrollRect TabButtonsScroll { get => tabButtonsScroll; set => tabButtonsScroll = value; }
        [SerializeField] private ScrollRect tabButtonsScroll;


        protected ISelector<UserSectionSelectedEventArgs> UserSectionSelector { get; set; }
        protected ISelector<UserTabSelectedEventArgs> UserTabSelector { get; set; }
        [Inject]
        public virtual void Inject(
            ISelector<UserSectionSelectedEventArgs> userSectionSelector,
            ISelector<UserTabSelectedEventArgs> userTabSelector)
        {
            UserSectionSelector = userSectionSelector;
            UserSectionSelector.AddSelectedListener(OnSectionSelected);
            UserTabSelector = userTabSelector;
            UserTabSelector.AddSelectedListener(OnTabSelected);
        }

        protected UserSection Section { get; set; }
        protected virtual void OnSectionSelected(object sender, UserSectionSelectedEventArgs eventArgs)
        {
            if (Section == eventArgs.SelectedSection)
                return;
            Section = eventArgs.SelectedSection;

            foreach (var tabButton in TabButtons)
                Destroy(tabButton.Value.gameObject);
            TabButtons.Clear();

            foreach (var tab in Section.Data.Tabs)
                AddButton(Section.GetTab(tab.Key));
        }

        protected Dictionary<UserTab, BaseReaderTabToggle> TabButtons { get; } = new Dictionary<UserTab, BaseReaderTabToggle>();
        protected void AddButton(UserTab userTab)
        {
            var tabButton = Instantiate(TabButtonPrefab, TabButtonsParent);
            tabButton.SetToggleGroup(TabsToggleGroup);
            tabButton.Display(userTab);
            tabButton.Selected += () => OnSelected(userTab);
            TabButtons.Add(userTab, tabButton);
        }

        protected UserTab CurrentTab { get; set; }
        protected void OnSelected(UserTab tab)
        {
            if (CurrentTab != tab) {
                CurrentTab = tab;
                var selectedArgs = new UserTabSelectedEventArgs(tab, ChangeType.JumpTo);
                UserTabSelector.Select(this, selectedArgs);
            }

            var tabButtonTransform = (RectTransform)TabButtons[tab].transform;
            EnsureButtonIsShowing(tab, tabButtonTransform);
            NextFrame.Function(() => NextFrame.Function(() => EnsureButtonIsShowing(tab, tabButtonTransform)));
        }

        protected virtual void EnsureButtonIsShowing(UserTab tab, RectTransform tabButtonTransform)
        {
            if (tab == CurrentTab && TabButtonsScroll != null)
                TabButtonsScroll.EnsureChildIsShowing((RectTransform)TabButtons[tab].transform);
        }

        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            CurrentTab = eventArgs.SelectedTab;
            TabButtons[CurrentTab].Select();
        }
    }
}