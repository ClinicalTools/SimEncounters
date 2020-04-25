using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using ClinicalTools.SimEncounters.Extensions;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderOrderablePanelsDrawer : ChildOrderablePanelsDrawer
    {
        [SerializeField] private List<ReaderOrderableItemPanelUI> panelOptions = new List<ReaderOrderableItemPanelUI>();
        protected List<ReaderOrderableItemPanelUI> PanelOptions { get => panelOptions; set => panelOptions = value; }

        public override List<ReaderOrderableItemPanelUI> DrawChildPanels(IEnumerable<UserPanel> childPanels)
        {
            var shuffeledPanels = ShufflePanels(childPanels);

            var options = new List<ReaderOrderableItemPanelUI>();
            foreach (var childPanel in shuffeledPanels) {
                var prefab = GetChildPanelPrefab(childPanel);
                if (prefab == null)
                    continue;

                var option = Instantiate(prefab, transform);
                option.Display(childPanel);
                options.Add(option);
            }

            return options;
        }

        protected List<UserPanel> ShufflePanels(IEnumerable<UserPanel> panels)
        {
            var shuffeledPanels = new List<UserPanel>(panels);
            if (shuffeledPanels.Count > 1) {
                while (HasSamePanelOrder(shuffeledPanels, panels))
                    shuffeledPanels.Shuffle();
            }
            return shuffeledPanels;
        }

        protected virtual ReaderOrderableItemPanelUI GetChildPanelPrefab(UserPanel childPanel)
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

        private bool HasSamePanelOrder(List<UserPanel> shuffeledPanels, IEnumerable<UserPanel> childPanels)
        {
            var i = 0;
            foreach (var childPanel in childPanels) {
                if (childPanel != shuffeledPanels[i++])
                    return false;
            }
            return true;
        }

    }
}