using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderCheckboxPanelsDrawer : BaseOptionUserPanelsDrawer
    {
        protected List<BaseReaderOptionPanel> PanelOptions { get => panelOptions; set => panelOptions = value; }
        [SerializeField] private List<BaseReaderOptionPanel> panelOptions = new List<BaseReaderOptionPanel>();

        public override List<BaseReaderOptionPanel> DrawChildPanels(IEnumerable<UserPanel> childPanels) {
            foreach (Transform child in transform)
                Destroy(child.gameObject);

            var options = new List<BaseReaderOptionPanel>();
            foreach (var childPanel in childPanels) {
                var readerPanel = CreateReaderPanel(childPanel);
                if (readerPanel != null)
                    options.Add(readerPanel);
            }

            return options;
        }

        protected virtual BaseReaderOptionPanel CreateReaderPanel(UserPanel childPanel)
        {
            var prefab = GetChildPanelPrefab(childPanel);
            if (prefab == null)
                return null;

            var option = Instantiate(prefab, transform);
            option.Display(childPanel);
            return option;
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