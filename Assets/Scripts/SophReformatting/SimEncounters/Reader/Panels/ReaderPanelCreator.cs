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


        public T Deserialize<T>(KeyValuePair<string, Panel> panel, List<T> panelOptions)
            where T : BaseReaderPanelUI
        {
            if (panelOptions == null || panelOptions.Count == 0)
                return null;

            var panelPrefab = GetPanelPrefab(panel.Value.Type, panelOptions);
            
            return Deserialize(panelPrefab);
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
        
        public T Deserialize<T>(T panelPrefab)
            where T : BaseReaderPanelUI
        {
            return UnityEngine.Object.Instantiate(panelPrefab, ChildPanelsParent);
        }
    }
}