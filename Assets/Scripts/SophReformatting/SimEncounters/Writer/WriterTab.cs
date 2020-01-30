using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterTab
    {
        /*private readonly TabUI tabUI;
        protected Tab Tab { get; }

        private readonly string tabFolder;
        private readonly EncounterWriter Writer;
        public WriterTab(EncounterWriter writer, Transform tabContent, Tab tab)
        {
            Tab = tab;
            Writer = writer;
            tabFolder = $"Writer/Prefabs/Tabs/{tab.Type} Tab/";
            var tabPrefabPath = $"{tabFolder}{tab.Type.Replace(" ", string.Empty)}Tab";
            var tabPrefab = Resources.Load(tabPrefabPath) as GameObject;
            tabUI = Object.Instantiate(tabPrefab, tabContent.transform).GetComponent<TabUI>();

            if (tab.Panels.Count > 0)
                Deserialize(tabUI.PanelsParent, tab.Panels);
            else
                AddDefaultTabs(tabUI);
        }

        public void Deserialize(Transform panelsParent, OrderedCollection<Panel> panels)
        {
            foreach (var panel in panels) {
                var panelPrefabPath = $"{tabFolder}{panel.Value.Type}";
                var panelPrefab = Resources.Load(panelPrefabPath) as GameObject;
                var panelUI = Object.Instantiate(panelPrefab, panelsParent).GetComponent<WriterPanelUI>();
                var writerPanel = new WriterPanel(Writer, panelUI, panel);
                WriterPanels.Add(writerPanel);
            }
        }

        protected List<WriterPanel> WriterPanels { get; } = new List<WriterPanel>();
        public void AddDefaultTabs(IWriterTabUI tabUI)
        {
            foreach (var panelPrefab in tabUI.PresetPanels) {
                var panelUI = Object.Instantiate(panelPrefab, tabUI.PanelsParent).GetComponent<WriterPanelUI>();
                var writerPanel = new WriterPanel(Writer, panelUI, new Panel(""));
                WriterPanels.Add(writerPanel);
            }
        }

        public void Serialize()
        {
            foreach (var writerPanel in WriterPanels) {
                writerPanel.Serialize();
                if (writerPanel.Key == null)
                    Tab.Panels.Add(writerPanel.Panel);
                writerPanel.UpdateIndex();
            }
            WriterPanels.Sort();

            foreach (var writerPanel in WriterPanels)
                Tab.Panels.MoveValue(writerPanel.Index, writerPanel.Panel);

            Object.Destroy(tabUI.gameObject);
        }*/
    }
}