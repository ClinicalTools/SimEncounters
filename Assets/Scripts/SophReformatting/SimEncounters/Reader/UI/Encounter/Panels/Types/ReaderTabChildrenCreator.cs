using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderTabChildrenCreator : MonoBehaviour
    {
        public List<ReaderPanelBehaviour> PanelOptions { get => panelOptions; set => panelOptions = value; }
        [SerializeField] private List<ReaderPanelBehaviour> panelOptions = new List<ReaderPanelBehaviour>();

        protected ISelector<UserTabSelectedEventArgs> TabSelector { get; set; }
        protected ReaderPanelBehaviour.Factory ReaderPanelFactory { get; set; }
        [Inject]
        public virtual void Inject(
            ISelector<UserTabSelectedEventArgs> tabSelector,
            ReaderPanelBehaviour.Factory readerPanelFactory)
        {
            TabSelector = tabSelector;
            ReaderPanelFactory = readerPanelFactory;
        }

        protected virtual void Start() => TabSelector.AddSelectedListener(OnTabSelected);
        protected virtual void OnDestroy() => TabSelector.RemoveSelectedListener(OnTabSelected);

        protected UserTab CurrentTab { get; set; }
        protected Dictionary<UserPanel, ReaderPanelBehaviour> Children { get; } = new Dictionary<UserPanel, ReaderPanelBehaviour>();
        protected virtual void OnTabSelected(object sender, UserTabSelectedEventArgs eventArgs)
        {
            if (CurrentTab == eventArgs.SelectedTab) {
                foreach (var child in Children)
                    child.Value.Select(this, new UserPanelSelectedEventArgs(child.Key, eventArgs.ChangeType != ChangeType.Inactive));
                return;
            }

            CurrentTab = eventArgs.SelectedTab;

            foreach (var child in Children)
                Destroy(child.Value.gameObject);
            DrawChildPanels(eventArgs.SelectedTab.GetPanels(), eventArgs.ChangeType);
        }

        protected virtual void DrawChildPanels(IEnumerable<UserPanel> childPanels, ChangeType changeType)
        {
            Children.Clear();
            foreach (var childPanel in childPanels) {
                var prefab = GetChildPanelPrefab(childPanel);
                if (prefab == null)
                    continue;

                var panel = ReaderPanelFactory.Create(prefab);
                panel.transform.SetParent(transform);
                panel.Select(this, new UserPanelSelectedEventArgs(childPanel, changeType != ChangeType.Inactive));
                Children.Add(childPanel, panel);
            }
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