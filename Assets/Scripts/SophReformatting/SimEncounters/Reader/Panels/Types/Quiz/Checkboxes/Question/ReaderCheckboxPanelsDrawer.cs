using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderCheckboxPanelsDrawer : BaseOptionUserPanelsDrawer
    {
        [SerializeField] private List<BaseReaderOptionPanel> panelOptions = new List<BaseReaderOptionPanel>();
        protected List<BaseReaderOptionPanel> PanelOptions { get => panelOptions; set => panelOptions = value; }

        public override List<BaseReaderOptionPanel> DrawChildPanels(IEnumerable<UserPanel> childPanels) {
            var options = new List<BaseReaderOptionPanel>();
            foreach (var childPanel in childPanels) {
                var prefab = GetChildPanelPrefab(childPanel);
                if (prefab == null)
                    continue;

                var option = Instantiate(prefab, transform);
                option.Display(childPanel);
                options.Add(option);
            }

            return options;
        }

        protected virtual BaseReaderOptionPanel GetChildPanelPrefab(UserPanel childPanel)
        {
            var type = childPanel.Data.Type;

            if (PanelOptions.Count == 1)
                return PanelOptions[0];

            foreach (var panelOption in PanelOptions) {
                if (string.Equals(type, panelOption.Type, StringComparison.OrdinalIgnoreCase))
                    return panelOption;
            }

            Debug.LogError($"No prefab for panel type \"{type}\"");
            return null;
        }
    }
}