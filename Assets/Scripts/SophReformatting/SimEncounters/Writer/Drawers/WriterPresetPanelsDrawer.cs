using ClinicalTools.SimEncounters.Collections;

using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterPresetPanelsDrawer : BaseWriterPanelsDrawer
    {
        public Transform PanelsParent { get => panelsParent; set => panelsParent = value; }
        [SerializeField] private Transform panelsParent;
        public List<BaseWriterPanel> PresetPanels { get => presetPanels; set => presetPanels = value; }
        [SerializeField] private List<BaseWriterPanel> presetPanels;

        protected virtual OrderedCollection<BaseWriterPanel> WriterPanels { get; set; } = new OrderedCollection<BaseWriterPanel>();
        public override List<BaseWriterPanel> DrawChildPanels(Encounter encounter, OrderedCollection<Panel> childPanels)
        {
            foreach (var writerPanel in WriterPanels.Values)
                Destroy(writerPanel.gameObject);

            var blah = new Something();
            WriterPanels = new OrderedCollection<BaseWriterPanel>();

            var panels = new List<BaseWriterPanel>();
            foreach (var panel in childPanels) {
                var prefab = blah.ChoosePrefab(PresetPanels, panel.Value);
                var panelUI = Instantiate(prefab, PanelsParent);
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
            foreach (var panelPrefab in PresetPanels) {
                var panelUI = Instantiate(panelPrefab, PanelsParent);
                panelUI.Display(encounter);
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