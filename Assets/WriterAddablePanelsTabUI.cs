using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterAddablePanelsTabUI : MonoBehaviour, IWriterTabUI
    {
        [SerializeField] private Transform panelsParent;
        public virtual Transform PanelsParent { get => panelsParent; set => panelsParent = value; }

        [SerializeField] private List<LabeledPanelUI> panelOptions;
        public virtual List<LabeledPanelUI> PanelOptions { get => panelOptions; set => panelOptions = value; }

        [SerializeField] private Button addPanelButton;
        public virtual Button AddPanelButton { get => addPanelButton; set => addPanelButton = value; }

        protected Tab Tab { get; private set; }
        protected PanelGroupSerializer PanelGroupSerializer { get; } = new PanelGroupSerializer();
        protected WriterPanelCreator WriterPanelCreator { get; private set; }
        protected List<WriterPanelUI> WriterPanels { get; set; }

        public void Initialize(EncounterWriter writer, string tabFolder, Tab tab)
        {
            Tab = tab;
            WriterPanelCreator = new WriterPanelCreator(writer, PanelsParent);

            if (tab.Panels.Count > 0)
                WriterPanels = WriterPanelCreator.Deserialize(tab.Panels, PanelOptions);
            else
                AddPanel();

            AddPanelButton.onClick.AddListener(() => AddPanel());
        }

        public void AddPanel()
        {
            if (PanelOptions.Count == 1) {
                var writerPanel = WriterPanelCreator.AddPanel(PanelOptions[0].PanelUI);
                WriterPanels.Add(writerPanel);
            } else if (PanelOptions.Count > 1) {
                var writerPanel = WriterPanelCreator.AddPanel(PanelOptions[0].PanelUI);
                WriterPanels.Add(writerPanel);
            }
        }

        public void Serialize()
        {
            PanelGroupSerializer.Serialize(WriterPanels, Tab.Panels);
        }

        public void Destroy()
        {
            Serialize();
            Destroy(gameObject);
        }
    }
}