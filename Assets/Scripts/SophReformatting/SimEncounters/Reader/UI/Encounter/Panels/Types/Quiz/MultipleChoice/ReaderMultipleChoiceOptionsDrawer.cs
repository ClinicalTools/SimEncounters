
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMultipleChoiceOptionsDrawer : BaseOptionUserPanelsDrawer
    {
        protected List<BaseReaderMultipleChoiceOption> PanelOptions { get => panelOptions; set => panelOptions = value; }
        [SerializeField] private List<BaseReaderMultipleChoiceOption> panelOptions = new List<BaseReaderMultipleChoiceOption>();
        public virtual ToggleGroup ToggleGroup { get => toggleGroup; set => toggleGroup = value; }
        [SerializeField] private ToggleGroup toggleGroup;
        public virtual Transform FeedbackParent { get => feedbackParent; set => feedbackParent = value; }
        [SerializeField] private Transform feedbackParent;

        public override List<BaseReaderOptionPanel> DrawChildPanels(IEnumerable<UserPanel> childPanels)
        {
            var options = new List<BaseReaderOptionPanel>();
            foreach (var childPanel in childPanels) {
                var prefab = GetChildPanelPrefab(childPanel);
                if (prefab == null)
                    continue;

                var option = Instantiate(prefab, transform);
                option.Display(childPanel);
                option.SetToggleGroup(ToggleGroup);
                option.SetFeedbackParent(FeedbackParent);
                options.Add(option);
            }

            return options;
        }

        protected virtual BaseReaderMultipleChoiceOption GetChildPanelPrefab(UserPanel childPanel)
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