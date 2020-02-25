using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabDisplay : IReaderTabDisplay
    {
        protected Tab Tab { get; private set; }
        protected ReaderPanelCreator ReaderPanelCreator { get; }
        protected List<IReaderPanelDisplay> ReaderPanels { get; } = new List<IReaderPanelDisplay>();
        protected ReaderTabUI TabUI { get; }

        public ReaderTabDisplay(ReaderScene reader, ReaderTabUI tabUI, KeyValuePair<string, Tab> keyedTab)
        {
            TabUI = tabUI;
            Tab = keyedTab.Value;
            ReaderPanelCreator = new ReaderPanelCreator(reader, tabUI.PanelsParent);

            if (Tab.Panels.Count > 0) {
                foreach (var keyedPanel in Tab.Panels) {
                    var panelUI = ReaderPanelCreator.Deserialize(keyedPanel, tabUI.PanelOptions);
                    var panelDisplay = reader.PanelDisplayFactory.CreatePanel(panelUI, keyedPanel);
                    ReaderPanels.Add(panelDisplay);
                }
            }
        }

        public void Destroy() => Object.Destroy(TabUI.GameObject);
    }
}