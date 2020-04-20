using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabsUI : TabSelector
    {
        [SerializeField] private Transform tabButtonsParent;
        public virtual Transform TabButtonsParent { get => tabButtonsParent; set => tabButtonsParent = value; }

        [SerializeField] private ReaderTabToggleUI tabButtonPrefab;
        public virtual ReaderTabToggleUI TabButtonPrefab { get => tabButtonPrefab; set => tabButtonPrefab = value; }

        [SerializeField] private ToggleGroup tabsToggleGroup;
        public virtual ToggleGroup TabsToggleGroup { get => tabsToggleGroup; set => tabsToggleGroup = value; }

        [SerializeField] private ScrollRect tabButtonsScroll;

        public virtual ScrollRect TabButtonsScroll { get => tabButtonsScroll; set => tabButtonsScroll = value; }

        public override event TabSelectedHandler TabSelected;
        public EncounterSceneInfo SceneInfo { get; set; }
        public override void Display(UserSection userSection)
        {
            foreach (var tabButton in TabButtons)
                Destroy(tabButton.Value.gameObject);
            TabButtons.Clear();

            foreach (var tab in userSection.Data.Tabs)
                AddButton(userSection.GetTab(tab.Key));
        }

        protected Dictionary<UserTab, ReaderTabToggleUI> TabButtons { get; } = new Dictionary<UserTab, ReaderTabToggleUI>();
        protected void AddButton(UserTab userTab)
        {
            var tabButton = Instantiate(TabButtonPrefab, TabButtonsParent);
            tabButton.SetToggleGroup(TabsToggleGroup);
            tabButton.Display(userTab);
            tabButton.Selected += () => OnSelected(userTab);
            TabButtons.Add(userTab, tabButton);
        }

        protected UserTab CurrentTab { get; set; }
        protected void OnSelected(UserTab keyedTab)
        {
            var selectedArgs = new TabSelectedEventArgs(keyedTab);
            CurrentTab = keyedTab;
            TabSelected?.Invoke(this, selectedArgs);
        }


        public override void SelectTab(UserTab userTab)
        {
            if (userTab == CurrentTab)
                return;

            TabButtons[userTab].Select();
        }
    }
}