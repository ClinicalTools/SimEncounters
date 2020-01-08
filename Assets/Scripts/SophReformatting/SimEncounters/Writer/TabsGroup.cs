using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class TabsGroup
    {
        protected virtual TabsUI TabsUI { get; }
        protected virtual OrderedCollection<Tab> Tabs { get; }

        protected virtual WriterTab WriterTab { get; set; }
        protected virtual EncounterWriter Writer { get; set; }
        // tab changed event

        public TabsGroup(TabsUI tabsUI, OrderedCollection<Tab> tabs, EncounterWriter writer)
        {
            TabsUI = tabsUI;
            Tabs = tabs;
            Writer = writer;

            AddListeners();
            foreach (var tab in tabs)
                AddTabButton(tab.Value);
        }

        protected virtual void AddListeners()
        {
            TabsUI.AddButton.onClick.RemoveAllListeners();
            TabsUI.AddButton.onClick.AddListener(OpenAddTabPopup);
        }

        protected virtual void OpenAddTabPopup()
        {
            var addTabPopup = Writer.OpenPanel(TabsUI.AddTabPopup);
            IApply<Tab> tabCreator = new TabCreator(addTabPopup, Writer);
            tabCreator.Apply += Add;
        }

        protected virtual void Add(Tab tab)
        {
            Tabs.Add(tab);
            AddTabButton(tab);
        }

        protected virtual TabButton AddTabButton(Tab tab)
        {
            var tabButtonUI = Object.Instantiate(TabsUI.TabButtonPrefab, TabsUI.TabButtonsParent);
            var tabButton = new TabButton(tabButtonUI, tab, Writer);
            tabButton.Selected += Select;
            //TabsUI.on
            // tab changed call SelectTab
            return tabButton;
        }


        protected virtual void Select(Tab tab)
        {
            WriterTab = new WriterTab(TabsUI.TabContent, tab, Writer);
            // tab changed event
        }


    }
}