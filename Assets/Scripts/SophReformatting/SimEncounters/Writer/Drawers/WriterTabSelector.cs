using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterTabSelector : BaseTabSelector
    {
        public virtual BaseRearrangeableGroup RearrangeableGroup { get => rearrangeableGroup; set => rearrangeableGroup = value; }
        [SerializeField] private BaseRearrangeableGroup rearrangeableGroup;
        public virtual BaseWriterTabToggle TabButtonPrefab { get => tabButtonPrefab; set => tabButtonPrefab = value; }
        [SerializeField] private BaseWriterTabToggle tabButtonPrefab;
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
            RearrangeableGroup.Rearranged += TabsRearranged;
        }

        private void AddTab()
        {
            var newTab = AddTabPopup.CreateTab();
            newTab.AddOnCompletedListener(AddNewTab);
        }

        private void AddNewTab(WaitedResult<Tab> tab)
        {
            if (tab?.Value == null)
                return;

            CurrentSection.Tabs.Add(tab.Value);
            AddTabButton(CurrentEncounter, tab.Value);

            SelectTab(tab.Value);
        }

        protected Encounter CurrentEncounter { get; set; }
        protected Section CurrentSection { get; set; }
        public override void Display(Encounter encounter, Section section)
        {
            CurrentEncounter = encounter;
            CurrentSection = section;
            RearrangeableGroup.Clear();
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

        protected Dictionary<Tab, BaseWriterTabToggle> TabButtons { get; } = new Dictionary<Tab, BaseWriterTabToggle>();
        protected void AddTabButton(Encounter encounter, Tab tab)
        {
            var tabButton = RearrangeableGroup.AddFromPrefab(TabButtonPrefab);
            tabButton.SetToggleGroup(TabsToggleGroup);
            tabButton.Display(encounter, tab);
            tabButton.Selected += () => OnSelected(tab);
            tabButton.Deleted += OnDeleted;
            TabButtons.Add(tab, tabButton);
        }

        protected Tab CurrentTab { get; set; }
        protected void OnSelected(Tab tab)
        {
            var selectedArgs = new TabSelectedEventArgs(tab);
            CurrentTab = tab;
            TabSelected?.Invoke(this, selectedArgs);

            TabButtonsScroll.EnsureChildIsShowing(TabButtons[tab].RectTransform);
        }
        protected void OnDeleted(Tab tab)
        {
            var tabButton = TabButtons[tab];
            rearrangeableGroup.Remove(tabButton);
            TabButtons.Remove(tab);
            CurrentSection.Tabs.Remove(tab);
        }

        public override void SelectTab(Tab tab)
        {
            if (tab == CurrentTab)
                return;

            TabButtons[tab].Select();
        }

        protected virtual void TabsRearranged(object sender, RearrangedEventArgs2 e) 
            => CurrentSection.Tabs.MoveValue(e.NewIndex, e.OldIndex);
    }
}