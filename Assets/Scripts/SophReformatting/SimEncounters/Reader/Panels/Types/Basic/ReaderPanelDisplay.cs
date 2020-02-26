using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPanelDisplay : IReaderPanelDisplay
    {
        protected ReaderScene Reader { get; }
        protected ReaderPanelUI PanelUI { get; }

        protected ReaderPanelCreator ReaderPanelCreator { get; }

        public ReaderPanelDisplay(ReaderScene reader, ReaderPanelUI panelUI)
        {
            Reader = reader;
            PanelUI = panelUI;

            ReaderPanelCreator = new ReaderPanelCreator(reader, PanelUI.ChildrenParent);
        }

        public virtual void Display(KeyValuePair<string, Panel> keyedPanel)
        {
            CreatePinButtons(keyedPanel);
            InitializeValueFields(keyedPanel);
            DeserializeChildren(keyedPanel.Value.ChildPanels);
        }

        protected virtual void DeserializeChildren(IEnumerable<KeyValuePair<string, Panel>> panels)
        {
            foreach (var keyedPanel in panels)
                DeserializeChild(keyedPanel);
        }
        protected virtual void DeserializeChild(KeyValuePair<string, Panel> keyedPanel)
        {
            var panelUI = ReaderPanelCreator.Deserialize(keyedPanel, PanelUI.ChildPanelOptions);
            var panelDisplay = Reader.PanelDisplayFactory.CreatePanel(panelUI);
            panelDisplay.Display(keyedPanel);
        }

        protected virtual IValueField[] InitializeValueFields(KeyValuePair<string, Panel> keyedPanel) => Reader.ValueFieldInitializer.InitializePanelValueFields(PanelUI.gameObject, keyedPanel.Value);
        protected virtual ReaderPinsGroup CreatePinButtons(KeyValuePair<string, Panel> keyedPanel) => Reader.Pins.CreateButtons(keyedPanel.Value.Pins, PanelUI.transform);
    }
}