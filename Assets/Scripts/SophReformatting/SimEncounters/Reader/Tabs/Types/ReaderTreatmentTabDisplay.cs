using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTreatmentTabDisplay : IReaderTabDisplay
    {
        protected Tab Tab { get; private set; }
        protected ReaderPanelCreator ReaderPanelCreator { get; private set; }
        protected ReaderTreatmentTabUI TabUI { get; private set; }

        public ReaderTreatmentTabDisplay(ReaderScene reader, ReaderTreatmentTabUI tabUI, KeyValuePair<string, Tab> keyedTab)
        {
            Tab = keyedTab.Value;
            TabUI = tabUI;
            ReaderPanelCreator = new ReaderPanelCreator(reader, tabUI.PanelsParent);

            DeserializeChildren(Tab);
        }

        public void DeserializeChildren(Tab tab)
        {
            if (tab.Panels.Count == 0)
                return;

            if (tab.Panels[0].Value.Type != TabUI.TreatmentPanel.Type)
            {
                var basePanel = new Panel(TabUI.TreatmentPanel.Type, tab.Panels);
                ReaderPanelCreator.Deserialize(new KeyValuePair<string, Panel>(null, basePanel), TabUI.TreatmentPanel);
            }
            else
            {
                ReaderPanelCreator.Deserialize(tab.Panels, TabUI.TreatmentPanel);
            }
        }

        public void Destroy()
        {
            Object.Destroy(TabUI.GameObject);
        }
    }
}