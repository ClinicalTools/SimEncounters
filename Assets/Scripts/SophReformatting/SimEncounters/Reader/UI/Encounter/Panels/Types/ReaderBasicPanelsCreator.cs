using ClinicalTools.SimEncounters.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderBasicPanelsCreator : BaseChildUserPanelsDrawer
    {
        public List<ReaderPanelBehaviour> PanelOptions { get => panelOptions; set => panelOptions = value; }
        [SerializeField] private List<ReaderPanelBehaviour> panelOptions = new List<ReaderPanelBehaviour>();

        protected ReaderPanelBehaviour.Factory ReaderPanelFactory { get; set; }
        [Inject]
        public virtual void Inject(ReaderPanelBehaviour.Factory readerPanelFactory) => ReaderPanelFactory = readerPanelFactory;

        protected OrderedCollection<UserPanel> CurrentPanels { get; set; }
        protected Dictionary<UserPanel, ReaderPanelBehaviour> Children { get; } = new Dictionary<UserPanel, ReaderPanelBehaviour>();

        public override void Display(OrderedCollection<UserPanel> panels, bool active)
        {
            if (CurrentPanels == panels) {
                foreach (var child in Children)
                    child.Value.Select(this, new UserPanelSelectedEventArgs(child.Key, active));
                return;
            }

            CurrentPanels = panels;

            foreach (var child in Children)
                Destroy(child.Value.gameObject);
            
            Children.Clear();
            foreach (var panel in panels.Values)
                DrawPanel(panel, active);
        }

        protected virtual void DrawPanel(UserPanel panel, bool active)
        {
            var prefab = GetChildPanelPrefab(panel);
            if (prefab == null)
                return;

            var panelBehaviour = ReaderPanelFactory.Create(prefab);
            panelBehaviour.transform.SetParent(transform);
            panelBehaviour.Select(this, new UserPanelSelectedEventArgs(panel, active));
            Children.Add(panel, panelBehaviour);
        }

        protected virtual ReaderPanelBehaviour GetChildPanelPrefab(UserPanel childPanel)
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