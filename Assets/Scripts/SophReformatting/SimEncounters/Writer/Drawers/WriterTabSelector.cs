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
        public virtual Button AddButton { get => addButton; set => addButton = value; }
        [SerializeField] private Button addButton;
        public TabCreatorPopup AddTabPopup { get => addTabPopup; set => addTabPopup = value; }
        [SerializeField] private TabCreatorPopup addTabPopup;

        public override event TabSelectedHandler TabSelected;

        protected virtual void Awake()
        {
            AddButton.onClick.AddListener(AddTab);
        }

        private void AddTab()
        {
            var newTab = AddTabPopup.CreateTab();
            newTab.AddOnCompletedListener(AddNewTab);
        }

        private void AddNewTab(Tab tab)
        {
            if (tab == null)
                return;

            CurrentSection.Tabs.Add(tab);
            AddTabButton(CurrentEncounter, tab);

            var tabSelectedArgs = new TabSelectedEventArgs(tab);
            TabSelected?.Invoke(this, tabSelectedArgs);
        }

        protected Encounter CurrentEncounter { get; set; }
        protected Section CurrentSection { get; set; }
        public override void Display(Encounter encounter, Section section)
        {
            CurrentEncounter = encounter;
            CurrentSection = section;
            foreach (var tabButton in TabButtons)
                Destroy(tabButton.Value.gameObject);
            TabButtons.Clear();

            if (section.Tabs.Count == 0) {
                AddTab();
            } else {
                foreach (var tab in section.Tabs)
                    AddTabButton(encounter, section.Tabs[tab.Key]);
            }
        }

        protected Dictionary<Tab, WriterTabToggle> TabButtons { get; } = new Dictionary<Tab, WriterTabToggle>();
        protected void AddTabButton(Encounter encounter, Tab tab)
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