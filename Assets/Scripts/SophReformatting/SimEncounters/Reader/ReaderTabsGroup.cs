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

        public ReaderTabsGroup(EncounterReader reader, ReaderTabsUI tabsUI, OrderedCollection<Tab> tabs)
                   : base(tabs)
        {
            Reader = reader;
            TabsUI = tabsUI;

            CreateInitialButtons(tabs);
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
        }

        public virtual void Delete()
        {
            ReaderTab?.Destroy();

            foreach (Transform child in TabsUI.TabButtonsParent)
                Object.Destroy(child.gameObject);
        }
    }
    public interface IReaderTabUI
    {
        Transform PanelsParent { get; set; }

        void Initialize(EncounterReader reader, string tabFolder, Tab tab);
        void Serialize();
        void Destroy();
    }
}