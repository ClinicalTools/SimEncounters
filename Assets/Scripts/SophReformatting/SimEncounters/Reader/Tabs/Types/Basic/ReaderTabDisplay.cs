using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabDisplay : IReaderTabDisplay
    {
        protected ReaderPanelCreator ReaderPanelCreator { get; }
        protected ReaderScene Reader { get; }
        protected ReaderTabUI TabUI { get; }

        public ReaderTabDisplay(ReaderScene reader, ReaderTabUI tabUI)
        {
            Reader = reader;
            TabUI = tabUI;
            ReaderPanelCreator = new ReaderPanelCreator(reader, tabUI.PanelsParent);
        }

        public virtual void Display(KeyValuePair<string, Tab> keyedTab)
        {
            DeserializeChildren(keyedTab.Value.Panels);
        }

        protected virtual void DeserializeChildren(IEnumerable<KeyValuePair<string, Panel>> panels)
        {
            foreach (var keyedPanel in panels)
                DeserializeChild(keyedPanel);
        }

        protected virtual IReaderPanelDisplay DeserializeChild(KeyValuePair<string, Panel> keyedPanel)
        {
            var panelUI = ReaderPanelCreator.Deserialize(keyedPanel, TabUI.PanelOptions);
            var panelDisplay = Reader.PanelDisplayFactory.CreatePanel(panelUI);
            panelDisplay.Display(keyedPanel);
            return panelDisplay;
        }

        public void Destroy() => Object.Destroy(TabUI.GameObject);
    }
}