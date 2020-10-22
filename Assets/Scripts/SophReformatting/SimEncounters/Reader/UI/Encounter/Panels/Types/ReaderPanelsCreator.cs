﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPanelsCreator : BaseReaderPanelsCreator
    {
        public List<BaseReaderPanel> PanelOptions { get => panelOptions; set => panelOptions = value; }
        [SerializeField] private List<BaseReaderPanel> panelOptions = new List<BaseReaderPanel>();

        protected BaseReaderPanel.Factory ReaderPanelFactory { get; set; }
        [Inject] public virtual void Inject(BaseReaderPanel.Factory readerPanelFactory) => ReaderPanelFactory = readerPanelFactory;

        public override List<BaseReaderPanel> DrawChildPanels(IEnumerable<UserPanel> childPanels)
        {
            var panels = new List<BaseReaderPanel>();
            foreach (var childPanel in childPanels) {
                var prefab = GetChildPanelPrefab(childPanel);
                if (prefab == null)
                    continue;

                var panel = ReaderPanelFactory.Create(prefab);
                panel.transform.SetParent(transform);
                panel.Display(childPanel);
                panels.Add(panel);
            }

            return panels;
        }

        protected virtual BaseReaderPanel GetChildPanelPrefab(UserPanel childPanel)
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