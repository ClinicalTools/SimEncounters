﻿using ClinicalTools.Collections;
using ClinicalTools.UI;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterAddablePanelsDrawer : BaseWriterPanelsDrawer
    {
        public BaseRearrangeableGroup ReorderableGroup { get => reorderableGroup; set => reorderableGroup = value; }
        [SerializeField] private BaseRearrangeableGroup reorderableGroup;
        public List<BaseWriterAddablePanel> PanelOptions { get => panelOptions; set => panelOptions = value; }
        [SerializeField] private List<BaseWriterAddablePanel> panelOptions;
        public BaseWriterPanelCreator PanelCreator { get => panelCreator; set => panelCreator = value; }
        [SerializeField] private BaseWriterPanelCreator panelCreator;

        protected BaseWriterPanel.Factory PanelFactory { get; set; }
        [Inject] public virtual void Inject(BaseWriterPanel.Factory panelFactory) => PanelFactory = panelFactory;

        protected virtual void Awake()
        {
            PanelCreator.AddPanel += AddPanel;
            PanelCreator.Initialize(PanelOptions);
        }

        protected Encounter CurrentEncounter { get; set; }
        protected virtual OrderedCollection<BaseWriterPanel> WriterPanels { get; set; } = new OrderedCollection<BaseWriterPanel>();

        public override List<BaseWriterPanel> DrawChildPanels(Encounter encounter, OrderedCollection<Panel> childPanels)
        {
            CurrentEncounter = encounter;

            foreach (var writerPanel in WriterPanels.Values)
                Destroy(writerPanel.gameObject);

            var childrenPanelManager = new ChildrenPanelManager();
            WriterPanels = new OrderedCollection<BaseWriterPanel>();

            var panels = new List<BaseWriterPanel>();
            foreach (var panel in childPanels) {
                var prefab = childrenPanelManager.ChoosePrefab(PanelOptions, panel.Value);
                var panelUI = InstantiatePanel(prefab);
                ReorderableGroup.Add(panelUI);
                panelUI.Display(encounter, panel.Value);
                panelUI.Deleted += () => PanelDeleted(panelUI);
                panels.Add(panelUI);
                WriterPanels.Add(panel.Key, panelUI);
            }

            return panels;
        }

        public override List<BaseWriterPanel> DrawDefaultChildPanels(Encounter encounter)
        {
            CurrentEncounter = encounter;
            WriterPanels = new OrderedCollection<BaseWriterPanel>();

            var panels = new List<BaseWriterPanel>();

            return panels;
        }

        public override OrderedCollection<Panel> SerializeChildren()
        {
            var blah = new ChildrenPanelManager();
            return blah.SerializeChildren(WriterPanels);
        }

        protected virtual void AddPanel(BaseWriterAddablePanel prefab)
        {
            var panelUI = InstantiatePanel(prefab);
            ReorderableGroup.Add(panelUI);
            panelUI.Display(CurrentEncounter);
            panelUI.Deleted += () => PanelDeleted(panelUI);
            WriterPanels.Add(panelUI);
        }

        protected virtual void PanelDeleted(BaseWriterAddablePanel panel)
        {
            ReorderableGroup.Remove(panel);
            WriterPanels.Remove(panel);
        }

        protected virtual BaseWriterAddablePanel InstantiatePanel(BaseWriterAddablePanel writerPanelPrefab)
        {
            var writerPanel = (BaseWriterAddablePanel)PanelFactory.Create(writerPanelPrefab);
            writerPanel.transform.SetParent(ReorderableGroup.transform);
            writerPanel.transform.localScale = Vector3.one;
            return writerPanel;
        }
    }
}