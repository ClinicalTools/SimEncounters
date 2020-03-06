using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabsGroup : ButtonGroup<Tab>
    {
        public event Action MoveToPreviousSection;
        public event Action MoveToNextSection;

        protected virtual ReaderTabsUI TabsUI { get; }

        protected virtual IReaderTabDisplay ReaderTabDisplay { get; set; }
        protected virtual ReaderScene Reader { get; set; }
        protected virtual Section Section { get; set; }

        protected override int FirstButtonIndex => Section.CurrentTabIndex;

        public ReaderTabsGroup(ReaderScene reader, ReaderTabsUI tabsUI, Section section)
                   : base(section.Tabs)
        {
            Reader = reader;
            TabsUI = tabsUI;
            Section = section;

            CreateInitialButtons(section.Tabs);
            Reader.Footer.NextTab += MoveToNextTab;
            MouseInput.Instance.SwipeHandler.SwipeLeft += MoveToNextTab;
            MouseInput.Instance.SwipeHandler.SwipeRight += MoveToPreviousTab;
        }

        protected override ISelectable<KeyValuePair<string, Tab>> AddButton(KeyValuePair<string, Tab> keyedTab)
        {
            var tabButtonUI = UnityEngine.Object.Instantiate(TabsUI.TabButtonPrefab, TabsUI.TabButtonsParent);
            var tabButton = new ReaderTabButton(Reader, tabButtonUI, keyedTab);
            tabButtonUI.SelectToggle.group = TabsUI.TabsToggleGroup;
            return tabButton;
        }

        protected override void Select(KeyValuePair<string, Tab> keyedTab)
        {
            ReaderTabDisplay?.Destroy();

            var tab = keyedTab.Value;
            var tabFolder = $"Reader/Prefabs/Tabs/{tab.Type} Tab/";
            var tabPrefabPath = $"{tabFolder}{tab.Type.Replace(" ", string.Empty)}Tab";
            var tabPrefabGameObject = Resources.Load(tabPrefabPath) as GameObject;
            var readerTabUI = UnityEngine.Object.Instantiate(tabPrefabGameObject, TabsUI.TabContent).GetComponent<IReaderTabUI>();
            
            ReaderTabDisplay = Reader.TabDisplayFactory.CreateTab(readerTabUI);
            ReaderTabDisplay.Display(keyedTab);

            Section.CurrentTabIndex = Section.Tabs.IndexOf(tab);
            Reader.Footer.SetTab(Section);
        }

        public virtual void Delete()
        {
            ReaderTabDisplay?.Destroy();
            Reader.Footer.NextTab -= MoveToNextTab;
            MouseInput.Instance.SwipeHandler.SwipeLeft -= MoveToNextTab;
            MouseInput.Instance.SwipeHandler.SwipeRight -= MoveToPreviousTab;

            foreach (Transform child in TabsUI.TabButtonsParent)
                UnityEngine.Object.Destroy(child.gameObject);
        }
        protected virtual void MoveToNextTab()
        {
            var oldTabIndex = Section.CurrentTabIndex;
            var tabIndex = Section.MoveToNextTab();
            if (oldTabIndex != tabIndex)
                SelectButtons[tabIndex].Select();
            else
                MoveToNextSection?.Invoke();
        }
        protected virtual void MoveToPreviousTab()
        {
            var oldTabIndex = Section.CurrentTabIndex;
            var tabIndex = Section.MoveToPreviousTab();
            if (oldTabIndex != tabIndex)
                SelectButtons[tabIndex].Select();
            else
                MoveToPreviousSection?.Invoke();
        }
    }
}