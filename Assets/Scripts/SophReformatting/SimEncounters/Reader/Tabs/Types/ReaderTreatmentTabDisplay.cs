using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTreatmentTabDisplay : IReaderTabDisplay
    {
        protected ReaderScene Reader { get; }
        protected Tab Tab { get; private set; }
        protected ReaderPanelCreator ReaderPanelCreator { get; private set; }
        protected ReaderTreatmentTabUI TabUI { get; private set; }

        public ReaderTreatmentTabDisplay(ReaderScene reader, ReaderTreatmentTabUI tabUI, KeyValuePair<string, Tab> keyedTab)
        {
            Reader = reader;
            Tab = keyedTab.Value;
            TabUI = tabUI;
            ReaderPanelCreator = new ReaderPanelCreator(reader, tabUI.PanelsParent);

            DeserializeChildren(Tab.Panels);
        }

        public void DeserializeChildren(IEnumerable<KeyValuePair<string, Panel>> panels)
        {
            foreach (var keyedPanel in panels) {
                var panelUI = ReaderPanelCreator.Deserialize(TabUI.TreatmentPanel);
                Reader.PanelDisplayFactory.CreatePanel(panelUI, keyedPanel);
            }
        }

        public void Destroy()
        {
            Object.Destroy(TabUI.GameObject);
        }
    }
}