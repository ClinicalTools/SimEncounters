using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterPanelCreator
    {
        protected EncounterWriter Writer { get; }
        protected Transform ChildPanelsParent { get; }
        public WriterPanelCreator(EncounterWriter writer, Transform childPanelsParent)
        {
            Writer = writer;
            ChildPanelsParent = childPanelsParent;
        }

        public List<WriterPanelUI> Deserialize(OrderedCollection<Panel> panels, WriterPanelUI panelPrefab)
        {
            List<WriterPanelUI> writerPanels = new List<WriterPanelUI>();
            foreach (var panel in panels) {
                var writerPanel = AddPanel(panelPrefab, panel.Value);
                writerPanel.Key = panel.Key;
                writerPanels.Add(writerPanel);
            }

            return writerPanels;
        }

        public List<WriterPanelUI> Deserialize(OrderedCollection<Panel> panels, List<LabeledPanelUI> panelOptions)
        {
            if (panelOptions.Count == 0)
                return null;

            List<WriterPanelUI> writerPanels = new List<WriterPanelUI>();
            foreach (var panel in panels) {
                var panelPrefab = GetPanelPrefab(panel.Value.Type, panelOptions);
                var writerPanel = AddPanel(panelPrefab, panel.Value);
                writerPanel.Key = panel.Key;
                writerPanels.Add(writerPanel);
            }

            return writerPanels;
        }

        protected WriterPanelUI GetPanelPrefab(string panelType, List<LabeledPanelUI> panelOptions)
        {
            if (panelOptions.Count == 1)
                return panelOptions[0].PanelUI;

            foreach (var panelOption in panelOptions) { 
                if (string.Equals(panelType, panelOption.Label, StringComparison.OrdinalIgnoreCase)) 
                    return panelOption.PanelUI;
            }

            Debug.LogError($"No prefab for panel type \"{panelType}\"");
            return null;
        }

        public List<WriterPanelUI> Deserialize(OrderedCollection<Panel> panels, string prefabFolder)
        {
            List<WriterPanelUI> writerPanels = new List<WriterPanelUI>();
            foreach (var panel in panels) {
                var writerPanel = AddPanel(panel.Value, prefabFolder);
                writerPanel.Key = panel.Key;
                writerPanels.Add(writerPanel);
            }

            return writerPanels;
        }

        public List<WriterPanelUI> AddInitialPanels(IEnumerable<WriterPanelUI> writerPanelUIs)
        {
            List<WriterPanelUI> writerPanels = new List<WriterPanelUI>();
            foreach (var panelPrefab in writerPanelUIs)
                writerPanels.Add(AddPanel(panelPrefab));

            return writerPanels;
        }

        protected WriterPanelUI AddPanel(Panel panel, string prefabFolder)
        {
            var panelPrefabPath = $"{prefabFolder}{panel.Type.Replace(" ", "")}";
            var panelPrefabObject = Resources.Load(panelPrefabPath) as GameObject;
            var panelPrefab = panelPrefabObject.GetComponent<WriterPanelUI>();
            return AddPanel(panelPrefab, panel);
        }

        public WriterPanelUI AddPanel(WriterPanelUI panelPrefab)
        {
            var panel = new Panel(panelPrefab.name);
            return AddPanel(panelPrefab, panel);
        }

        protected WriterPanelUI AddPanel(WriterPanelUI panelPrefab, Panel panel)
        {
            var writerPanel = UnityEngine.Object.Instantiate(panelPrefab, ChildPanelsParent);
            writerPanel.Initialize(Writer, panel);
            return writerPanel;
        }
    }
}