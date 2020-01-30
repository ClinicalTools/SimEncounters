using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPanelCreator
    {
        protected EncounterReader Reader { get; }
        protected Transform ChildPanelsParent { get; }
        public ReaderPanelCreator(EncounterReader reader, Transform childPanelsParent)
        {
            Reader = reader;
            ChildPanelsParent = childPanelsParent;
        }

        public List<ReaderPanelUI> Deserialize(OrderedCollection<Panel> panels, ReaderPanelUI panelPrefab)
        {
            List<ReaderPanelUI> writerPanels = new List<ReaderPanelUI>();
            foreach (var panel in panels) {
                var writerPanel = AddPanel(panelPrefab, panel);
                writerPanels.Add(writerPanel);
            }

            return writerPanels;
        }

        public List<ReaderPanelUI> Deserialize(OrderedCollection<Panel> panels, List<ReaderPanelUI> panelOptions)
        {
            if (panelOptions.Count == 0)
                return null;

            List<ReaderPanelUI> writerPanels = new List<ReaderPanelUI>();
            foreach (var panel in panels) {
                var panelPrefab = GetPanelPrefab(panel.Value.Type, panelOptions);
                var writerPanel = AddPanel(panelPrefab, panel);
                writerPanels.Add(writerPanel);
            }

            return writerPanels;
        }

        protected ReaderPanelUI GetPanelPrefab(string panelType, List<ReaderPanelUI> panelOptions)
        {
            if (panelOptions.Count == 1)
                return panelOptions[0];

            foreach (var panelOption in panelOptions) {
                if (string.Equals(panelType, panelOption.Type, StringComparison.OrdinalIgnoreCase))
                    return panelOption;
            }

            Debug.LogError($"No prefab for panel type \"{panelType}\"");
            return null;
        }
        
        protected ReaderPanelUI AddPanel(ReaderPanelUI panelPrefab, KeyValuePair<string, Panel> keyedPanel)
        {
            var writerPanel = UnityEngine.Object.Instantiate(panelPrefab, ChildPanelsParent);
            writerPanel.Initialize(Reader, keyedPanel);
            return writerPanel;
        }
    }
}