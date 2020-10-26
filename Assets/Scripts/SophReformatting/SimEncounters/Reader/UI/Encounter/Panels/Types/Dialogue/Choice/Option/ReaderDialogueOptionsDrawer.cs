﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderDialogueOptionsDrawer : BaseDialogueOptionsDrawer
    {
        protected List<BaseReaderDialogueOption> PanelOptions { get => panelOptions; set => panelOptions = value; }
        [SerializeField] private List<BaseReaderDialogueOption> panelOptions = new List<BaseReaderDialogueOption>();
        protected ToggleGroup ToggleGroup { get => toggleGroup; set => toggleGroup = value; }
        [SerializeField] private ToggleGroup toggleGroup;
        public virtual Transform FeedbackParent { get => feedbackParent; set => feedbackParent = value; }
        [SerializeField] private Transform feedbackParent;

        protected BaseReaderPanel.Factory ReaderPanelFactory { get; set; }
        [Inject] public virtual void Inject(BaseReaderPanel.Factory readerPanelFactory) => ReaderPanelFactory = readerPanelFactory;

        public override List<BaseReaderDialogueOption> DrawChildPanels(IEnumerable<UserPanel> childPanels)
        {
            var options = new List<BaseReaderDialogueOption>();
            foreach (var childPanel in childPanels) {
                var prefab = GetChildPanelPrefab(childPanel);
                if (prefab == null)
                    continue;

                var option = (BaseReaderDialogueOption)ReaderPanelFactory.Create(prefab);
                option.transform.SetParent(transform);
                option.transform.localScale = Vector3.one;
                option.Display(childPanel);
                option.SetGroup(toggleGroup);
                if (FeedbackParent != null)
                    option.SetFeedbackParent(FeedbackParent);
                options.Add(option);
            }

            return options;
        }

        protected virtual BaseReaderDialogueOption GetChildPanelPrefab(UserPanel childPanel)
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