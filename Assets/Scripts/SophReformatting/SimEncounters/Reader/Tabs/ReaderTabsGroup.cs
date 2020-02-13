using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabsGroup : ButtonGroup<Tab>
    {
        protected virtual ReaderTabsUI TabsUI { get; }

        protected virtual IReaderTabUI ReaderTab { get; set; }
        protected virtual EncounterReader Reader { get; set; }
        protected virtual Section Section { get; set; }

        protected override int FirstButtonIndex => Section.CurrentTabIndex;

        public ReaderTabsGroup(EncounterReader reader, ReaderTabsUI tabsUI, Section section)
                   : base(section.Tabs)
        {
            Reader = reader;
            TabsUI = tabsUI;
            Section = section;

            CreateInitialButtons(section.Tabs);
            Reader.Footer.NextTab += MoveToNextTab;
        }

        protected override ISelectable<Tab> AddButton(Tab tab)
        {
            var tabButtonUI = Object.Instantiate(TabsUI.TabButtonPrefab, TabsUI.TabButtonsParent);
            var tabButton = new ReaderTabButton(Reader, tabButtonUI, tab);
            tabButtonUI.SelectToggle.group = TabsUI.TabsToggleGroup;
            return tabButton;
        }


        protected override void Select(Tab tab)
        {
            ReaderTab?.Destroy();

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