using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterPresetPanelsTabUI : MonoBehaviour, IWriterTabUI
    {
        [SerializeField] private Transform panelsParent;
        public virtual Transform PanelsParent { get => panelsParent; set => panelsParent = value; }

        [SerializeField] private List<WriterPanelUI> presetPanels;
        public virtual List<WriterPanelUI> PresetPanels { get => presetPanels; set => presetPanels = value; }

        protected Tab Tab { get; private set; }
        protected PanelGroupSerializer PanelGroupSerializer { get; } = new PanelGroupSerializer();
        protected WriterPanelCreator WriterPanelCreator { get; private set; }
        protected List<WriterPanelUI> WriterPanels { get; set; }

        public void Initialize(EncounterWriter writer, string tabFolder, Tab tab)
        {
            Tab = tab;
            WriterPanelCreator = new WriterPanelCreator(writer, PanelsParent);

            if (tab.Panels.Count > 0)
                WriterPanels = WriterPanelCreator.Deserialize(tab.Panels, tabFolder);
            else
                WriterPanels = WriterPanelCreator.AddInitialPanels(PresetPanels);
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