using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPanelDrawerSelector
    {
        public T GetPanelPrefab<T>(string panelType, List<T> panelOptions)
            where T : BaseReaderPanelUI
        {
            if (panelOptions.Count == 1)
                return panelOptions[0];

            foreach (var panelOption in panelOptions) {
                if (string.Equals(panelType, panelOption.Type, StringComparison.OrdinalIgnoreCase))
                    return panelOption;
            }

            Debug.LogError($"No prefab for panel type \"{panelType}\"");
            return null;
        }
    }

    public class ReaderTabUI : UserTabDrawer, IReaderTabUI
    {
        public GameObject GameObject => gameObject;
        
        [SerializeField] private Transform panelsParent;
        public virtual Transform PanelsParent { get => panelsParent; set => panelsParent = value; }

        [SerializeField] private List<BaseReaderPanelUI> panelOptions;
        public virtual List<BaseReaderPanelUI> PanelOptions { get => panelOptions; set => panelOptions = value; }


        protected void Inject()
        {

        }

        public override void Display(UserTab userTab)
        {

            DeserializeChildren(userTab.GetPanels());
        }

        protected virtual void DeserializeChildren(IEnumerable<UserPanel> panels)
        {
            foreach (var userPanel in panels)
                DeserializeChild(userPanel);
        }

        protected virtual UserPanelDrawer DeserializeChild(UserPanel userPanel)
        {
            var selector = new ReaderPanelDrawerSelector();
            var panelPrefab = selector.GetPanelPrefab(userPanel.Data.Type, PanelOptions);
            var panelUI = Instantiate(panelPrefab, PanelsParent);
            panelUI.Display(userPanel);
            return panelUI;
        }

        public void Destroy() => Destroy(GameObject);
    }
}