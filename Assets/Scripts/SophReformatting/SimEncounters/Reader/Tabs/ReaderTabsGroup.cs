﻿using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabsGroup : ButtonGroup<Tab>
    {
        protected virtual ReaderTabsUI TabsUI { get; }

        protected virtual IReaderTabUI ReaderTab { get; set; }
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
        }

        protected override ISelectable<KeyValuePair<string, Tab>> AddButton(KeyValuePair<string, Tab> keyedTab)
        {
            var tabButtonUI = Object.Instantiate(TabsUI.TabButtonPrefab, TabsUI.TabButtonsParent);
            var tabButton = new ReaderTabButton(Reader, tabButtonUI, keyedTab);
            tabButtonUI.SelectToggle.group = TabsUI.TabsToggleGroup;
            return tabButton;
        }


        protected override void Select(KeyValuePair<string, Tab> keyedTab)
        {
            ReaderTab?.Destroy();

            var tab = keyedTab.Value;
            var tabFolder = $"Reader/Prefabs/Tabs/{tab.Type} Tab/";
            var tabPrefabPath = $"{tabFolder}{tab.Type.Replace(" ", string.Empty)}Tab";
            var tabPrefabGameObject = Resources.Load(tabPrefabPath) as GameObject;
            ReaderTab = Object.Instantiate(tabPrefabGameObject, TabsUI.TabContent).GetComponent<IReaderTabUI>();
            ReaderTab.Initialize(Reader, tabFolder, tab);

            Section.CurrentTabIndex = Section.Tabs.IndexOf(tab);
            Reader.Footer.SetTab(Section);
        }

        public virtual void Delete()
        {
            ReaderTab?.Destroy();
            Reader.Footer.NextTab -= MoveToNextTab;

            foreach (Transform child in TabsUI.TabButtonsParent)
                Object.Destroy(child.gameObject);
        }
        protected virtual void MoveToNextTab()
        {
            var tabIndex = Section.MoveToNextTab();
            SelectButtons[tabIndex].Select();
        }
        protected virtual void MoveToPreviousTab()
        {
            var tabIndex = Section.MoveToPreviousTab();
            SelectButtons[tabIndex].Select();
        }
    }
}