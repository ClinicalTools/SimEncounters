using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterAddablePanelsDrawer : BaseWriterPanelsDrawer
    {
        public BaseRearrangeableGroup ReorderableGroup { get => reorderableGroup; set => reorderableGroup = value; }
        [SerializeField] private BaseRearrangeableGroup reorderableGroup;
        public List<BaseWriterAddablePanel> PanelOptions { get => panelOptions; set => panelOptions = value; }
        [SerializeField] private List<BaseWriterAddablePanel> panelOptions;
        public BaseWriterPanelCreator PanelCreator { get => panelCreator; set => panelCreator = value; }
        [SerializeField] private BaseWriterPanelCreator panelCreator;

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

            var blah = new Something();
            WriterPanels = new OrderedCollection<BaseWriterPanel>();

            var panels = new List<BaseWriterPanel>();
            foreach (var panel in childPanels) {
                var prefab = blah.ChoosePrefab(PanelOptions, panel.Value);
                var panelUI = ReorderableGroup.AddFromPrefab(prefab);
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
            var blah = new Something();
            return blah.SerializeChildren(WriterPanels);
        }

        protected virtual void AddPanel(BaseWriterAddablePanel prefab)
        {
            var panelUI = ReorderableGroup.AddFromPrefab(prefab);
            panelUI.Display(CurrentEncounter);
            panelUI.Deleted += () => PanelDeleted(panelUI);
            WriterPanels.Add(panelUI);
        }

        protected virtual void PanelDeleted(BaseWriterAddablePanel panel)
        {
            ReorderableGroup.Remove(panel);
            WriterPanels.Remove(panel);
        }
    }
}