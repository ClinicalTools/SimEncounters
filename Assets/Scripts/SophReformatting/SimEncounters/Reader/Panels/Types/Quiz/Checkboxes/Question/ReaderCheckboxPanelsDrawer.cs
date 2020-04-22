using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderCheckboxPanelsDrawer : ChildCheckboxPanelsDrawer
    {
        [SerializeField] private List<CheckboxOptionPanel> panelOptions = new List<CheckboxOptionPanel>();
        protected List<CheckboxOptionPanel> PanelOptions { get => panelOptions; set => panelOptions = value; }

        public override List<CheckboxOptionPanel> DrawChildPanels(IEnumerable<UserPanel> childPanels) {
            var options = new List<CheckboxOptionPanel>();
            foreach (var childPanel in childPanels) {
                var prefab = GetChildPanelPrefab(childPanel);
                if (prefab == null)
                    continue;

                var option = Instantiate(prefab, transform);
                options.Add(option);
            }

            return options;
        }

        protected virtual CheckboxOptionPanel GetChildPanelPrefab(UserPanel childPanel)
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