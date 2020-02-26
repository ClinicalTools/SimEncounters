using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTreatmentTabDisplay : IReaderTabDisplay
    {
        protected ReaderScene Reader { get; }
        protected ReaderPanelCreator ReaderPanelCreator { get; }
        protected ReaderTreatmentTabUI TabUI { get; }

        public ReaderTreatmentTabDisplay(ReaderScene reader, ReaderTreatmentTabUI tabUI)
        {
            Reader = reader;
            TabUI = tabUI;
            ReaderPanelCreator = new ReaderPanelCreator(reader, tabUI.PanelsParent);

        }

        public virtual void Display(KeyValuePair<string, Tab> keyedTab)
        {
            DeserializeChildren(keyedTab.Value.Panels);
        }

        public void DeserializeChildren(IEnumerable<KeyValuePair<string, Panel>> panels)
        {
            foreach (var keyedPanel in panels) {
                var panelUI = ReaderPanelCreator.Deserialize(TabUI.TreatmentPanel);
                var panelDisplay = Reader.PanelDisplayFactory.CreatePanel(panelUI);
                panelDisplay.Display(keyedPanel);
            }
        }

        public void Destroy() => Object.Destroy(TabUI.GameObject);
    }
}