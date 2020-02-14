using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterTabGroup : ButtonGroup<Tab>
    {
        protected virtual TabsUI TabsUI { get; }

        protected virtual IWriterTabUI WriterTab { get; set; }
        protected virtual EncounterWriter Writer { get; set; }

        public WriterTabGroup(EncounterWriter writer, TabsUI tabsUI, OrderedCollection<Tab> tabs) 
            : base(tabs)
        {
            Writer = writer;
            TabsUI = tabsUI;

            AddListeners();

            CreateInitialButtons(tabs);
        }

        protected virtual void AddListeners()
        {
            TabsUI.AddButton.onClick.RemoveAllListeners();
            TabsUI.AddButton.onClick.AddListener(OpenAddTabPopup);
        }

        protected virtual void OpenAddTabPopup()
        {
            var addTabPopup = Writer.OpenPopup(TabsUI.AddTabPopup);
            IApply<Tab> tabCreator = new TabCreator(addTabPopup, Writer);
            tabCreator.Apply += Add;
        }
        
        protected virtual void Add(Tab tab)
        {

        }

        protected ISelectable<Tab> AddButton(Tab tab)
        {
            var tabButtonUI = Object.Instantiate(TabsUI.TabButtonPrefab, TabsUI.TabButtonsParent);
            ISelectable<Tab> tabButton = new WriterTabButton(Writer, tabButtonUI, tab);
            return tabButton;
        }


        protected override void Select(KeyValuePair<string, Tab> keyedTab)
        {
            var tab = keyedTab.Value;

            WriterTab?.Destroy();

            var tabFolder = $"Writer/Prefabs/Tabs/{tab.Type} Tab/";
            var tabPrefabPath = $"{tabFolder}{tab.Type.Replace(" ", string.Empty)}Tab";
            var tabPrefabGameObject = Resources.Load(tabPrefabPath) as GameObject;
            WriterTab = Object.Instantiate(tabPrefabGameObject, TabsUI.TabContent).GetComponent<IWriterTabUI>();
            WriterTab.Initialize(Writer, tabFolder, tab);
        }

        public virtual void Delete()
        {
            WriterTab?.Destroy();

            foreach (Transform child in TabsUI.TabButtonsParent)
                Object.Destroy(child.gameObject);
        }

        protected override ISelectable<KeyValuePair<string, Tab>> AddButton(KeyValuePair<string, Tab> value)
        {
            throw new System.NotImplementedException();
        }
    }
}