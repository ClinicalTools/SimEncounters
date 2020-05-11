using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterAddablePanelsDrawer : BaseWriterPanelsDrawer
    {
        public Transform PanelsParent { get => panelsParent; set => panelsParent = value; }
        [SerializeField] private Transform panelsParent;
        public List<OptionWriterPanel> PanelOptions { get => panelOptions; set => panelOptions = value; }
        [SerializeField] private List<OptionWriterPanel> panelOptions;
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

            var optionList = new List<BaseWriterPanel>();
            foreach (var option in PanelOptions)
                optionList.Add(option.PanelPrefab);

            var panels = new List<BaseWriterPanel>();
            foreach (var panel in childPanels) {
                var prefab = blah.ChoosePrefab(optionList, panel.Value);
                var panelUI = Instantiate(prefab, PanelsParent);
                panelUI.Display(encounter, panel.Value);
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

        protected virtual void AddPanel(BaseWriterPanel prefab)
        {
            var panelUI = Instantiate(prefab, PanelsParent);
            panelUI.Display(CurrentEncounter);
            WriterPanels.Add(panelUI);
        }
    }
}