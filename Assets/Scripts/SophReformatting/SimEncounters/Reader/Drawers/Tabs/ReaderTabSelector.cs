using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabSelector : BaseUserTabSelector
    {
        public virtual Transform TabButtonsParent { get => tabButtonsParent; set => tabButtonsParent = value; }
        [SerializeField] private Transform tabButtonsParent;
        public virtual ReaderTabToggle TabButtonPrefab { get => tabButtonPrefab; set => tabButtonPrefab = value; }
        [SerializeField] private ReaderTabToggle tabButtonPrefab;
        public virtual ToggleGroup TabsToggleGroup { get => tabsToggleGroup; set => tabsToggleGroup = value; }
        [SerializeField] private ToggleGroup tabsToggleGroup;
        public virtual ScrollRect TabButtonsScroll { get => tabButtonsScroll; set => tabButtonsScroll = value; }
        [SerializeField] private ScrollRect tabButtonsScroll;

        public override event UserTabSelectedHandler TabSelected;
        public override void Display(UserSection userSection)
        {
            foreach (var tabButton in TabButtons)
                Destroy(tabButton.Value.gameObject);
            TabButtons.Clear();

            foreach (var tab in userSection.Data.Tabs)
                AddButton(userSection.GetTab(tab.Key));
        }

        protected Dictionary<UserTab, ReaderTabToggle> TabButtons { get; } = new Dictionary<UserTab, ReaderTabToggle>();
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
            var selectedArgs = new UserTabSelectedEventArgs(tab);
            CurrentTab = tab;
            TabSelected?.Invoke(this, selectedArgs);
            TabButtonsScroll.EnsureChildIsShowing((RectTransform)TabButtons[tab].transform);
        }

        public override void SelectTab(UserTab userTab)
        {
            if (userTab == CurrentTab)
                return;

            TabButtons[userTab].Select();
        }
    }
}