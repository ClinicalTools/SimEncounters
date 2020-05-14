using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterPresetTitledPanelsDrawer : BaseWriterPanelsDrawer
    {
        public Transform PanelsParent { get => panelsParent; set => panelsParent = value; }
        [SerializeField] private Transform panelsParent;
        public BaseWriterPanel PanelPrefab { get => panelPrefab; set => panelPrefab = value; }
        [SerializeField] private BaseWriterPanel panelPrefab;
        public List<string> PanelNames { get => panelNames; set => panelNames = value; }
        [SerializeField] private List<string> panelNames;

        protected virtual OrderedCollection<BaseWriterPanel> WriterPanels { get; set; } = new OrderedCollection<BaseWriterPanel>();


        public override List<BaseWriterPanel> DrawChildPanels(Encounter encounter, OrderedCollection<Panel> childPanels)
        {
            foreach (var writerPanel in WriterPanels.Values)
                Destroy(writerPanel.gameObject);

            var blah = new Something();
            WriterPanels = new OrderedCollection<BaseWriterPanel>();

            var panels = new List<BaseWriterPanel>();
            foreach (var panel in childPanels) {
                var panelUI = Instantiate(PanelPrefab, PanelsParent);
                panelUI.Display(encounter, panel.Value);
                panels.Add(panelUI);
                WriterPanels.Add(panel.Key, panelUI);
            }

            return panels;
        }

        public override List<BaseWriterPanel> DrawDefaultChildPanels(Encounter encounter)
        {
            WriterPanels = new OrderedCollection<BaseWriterPanel>();

            var panels = new List<BaseWriterPanel>();
            foreach (var panelName in PanelNames) {
                var panel = new Panel(PanelPrefab.Type);
                panel.Values.Add("PanelNameValue", panelName);
                var panelUI = Instantiate(panelPrefab, PanelsParent);
                panelUI.Display(encounter, panel);
                panels.Add(panelUI);
                WriterPanels.Add(panelUI);
            }

            return panels;
        }

        public override OrderedCollection<Panel> SerializeChildren()
        {
            var blah = new Something();
            return blah.SerializeChildren(WriterPanels);
        }

    }
}