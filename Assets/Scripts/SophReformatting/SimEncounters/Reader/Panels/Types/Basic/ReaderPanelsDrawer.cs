using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPanelsDrawer : ChildPanelsDrawer
    {
        [SerializeField] private List<BaseReaderPanelUI> panelOptions = new List<BaseReaderPanelUI>();
        protected List<BaseReaderPanelUI> PanelOptions { get => panelOptions; set => panelOptions = value; }

        public override List<BaseReaderPanelUI> DrawChildPanels(IEnumerable<UserPanel> childPanels)
        {
            var panels = new List<BaseReaderPanelUI>();
            foreach (var childPanel in childPanels) {
                var prefab = GetChildPanelPrefab(childPanel);
                if (prefab == null)
                    continue;

                var panel = Instantiate(prefab, transform);
                panels.Add(panel);
            }

            return panels;
        }

        protected virtual BaseReaderPanelUI GetChildPanelPrefab(UserPanel childPanel)
        {
            var type = childPanel.Data.Type;

            if (panelOptions.Count == 1)
                return panelOptions[0];

            foreach (var panelOption in panelOptions) {
                if (string.Equals(type, panelOption.Type, StringComparison.OrdinalIgnoreCase))
                    return panelOption;
            }

            Debug.LogError($"No prefab for panel type \"{type}\"");
            return null;
        }
    }
}