using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPanelCreator
    {
        protected ReaderScene Reader { get; }
        protected Transform ChildPanelsParent { get; }
        public ReaderPanelCreator(ReaderScene reader, Transform childPanelsParent)
        {
            Reader = reader;
            ChildPanelsParent = childPanelsParent;
        }

        public List<T> Deserialize<T>(IEnumerable<KeyValuePair<string, Panel>> panels, T panelPrefab)
            where T : BaseReaderPanelUI
        {
            List<T> writerPanels = new List<T>();
            foreach (var panel in panels) {
                var writerPanel = Deserialize(panel, panelPrefab);
                writerPanels.Add(writerPanel);
            }

            return writerPanels;
        }

        public List<T> Deserialize<T>(IEnumerable<KeyValuePair<string, Panel>> panels, List<T> panelOptions)
            where T : BaseReaderPanelUI
        {
            if (panelOptions == null || panelOptions.Count == 0)
                return null;

            List<T> writerPanels = new List<T>();
            foreach (var panel in panels) {
                var panelPrefab = GetPanelPrefab(panel.Value.Type, panelOptions);
                var writerPanel = Deserialize(panel, panelPrefab);
                writerPanels.Add(writerPanel);
            }

            return writerPanels;
        }

        protected T GetPanelPrefab<T>(string panelType, List<T> panelOptions)
            where T : BaseReaderPanelUI
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
        
        public T Deserialize<T>(KeyValuePair<string, Panel> keyedPanel, T panelPrefab)
            where T : BaseReaderPanelUI
        {
            var writerPanel = UnityEngine.Object.Instantiate(panelPrefab, ChildPanelsParent);
            writerPanel.Initialize(Reader, keyedPanel);
            return writerPanel;
        }
    }
}