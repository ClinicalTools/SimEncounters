using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterTabSelector : BaseTabSelector
    {
        public virtual Transform TabButtonsParent { get => tabButtonsParent; set => tabButtonsParent = value; }
        [SerializeField] private Transform tabButtonsParent;
        public virtual WriterTabToggle TabButtonPrefab { get => tabButtonPrefab; set => tabButtonPrefab = value; }
        [SerializeField] private WriterTabToggle tabButtonPrefab;
        public virtual ToggleGroup TabsToggleGroup { get => tabsToggleGroup; set => tabsToggleGroup = value; }
        [SerializeField] private ToggleGroup tabsToggleGroup;
        public virtual ScrollRect TabButtonsScroll { get => tabButtonsScroll; set => tabButtonsScroll = value; }
        [SerializeField] private ScrollRect tabButtonsScroll;

        public override event TabSelectedHandler TabSelected;

        public override void Display(Encounter encounter, Section section)
        {
            foreach (var tabButton in TabButtons)
                Destroy(tabButton.Value.gameObject);
            TabButtons.Clear();

            foreach (var tab in section.Tabs)
                AddButton(encounter, section.Tabs[tab.Key]);
        }

        protected Dictionary<Tab, WriterTabToggle> TabButtons { get; } = new Dictionary<Tab, WriterTabToggle>();
        protected void AddButton(Encounter encounter, Tab tab)
        {
            var tabButton = Instantiate(TabButtonPrefab, TabButtonsParent);
            tabButton.SetToggleGroup(TabsToggleGroup);
            tabButton.Display(encounter, tab);
            tabButton.Selected += () => OnSelected(tab);
            TabButtons.Add(tab, tabButton);
        }

        protected Tab CurrentTab { get; set; }
        protected void OnSelected(Tab tab)
        {
            var selectedArgs = new TabSelectedEventArgs(tab);
            CurrentTab = tab;
            TabSelected?.Invoke(this, selectedArgs);
        }

        public override void SelectTab(Tab tab)
        {
            if (tab == CurrentTab)
                return;

            TabButtons[tab].Select();
        }
    }
}