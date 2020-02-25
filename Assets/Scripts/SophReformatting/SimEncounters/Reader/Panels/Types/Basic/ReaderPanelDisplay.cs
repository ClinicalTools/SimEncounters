using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPanelDisplay : IReaderPanelDisplay
    {
        protected ReaderScene Reader { get; }
        protected ReaderPanelUI PanelUI { get; }
        protected ReaderPanelCreator ReaderPanelCreator { get; }
        protected List<IReaderPanelDisplay> ChildPanels { get; } = new List<IReaderPanelDisplay>();
        protected IValueField[] ValueFields { get; }

        public ReaderPanelDisplay(ReaderScene reader, ReaderPanelUI panelUI, KeyValuePair<string, Panel> keyedPanel)
        {
            Reader = reader;
            PanelUI = panelUI;
            var panel = keyedPanel.Value;

            CreatePinButtons(reader, panel);

            var valueFieldInitializer = new ReaderValueFieldInitializer(reader);
            ValueFields = valueFieldInitializer.InitializePanelValueFields(PanelUI.gameObject, panel);

            ReaderPanelCreator = new ReaderPanelCreator(reader, PanelUI.ChildrenParent);
            DeserializeChildren(panel.ChildPanels);;
        }

        public void DeserializeChildren(IEnumerable<KeyValuePair<string, Panel>> panels)
        {
            foreach (var keyedPanel in panels) {
                var panelUI = ReaderPanelCreator.Deserialize(keyedPanel, PanelUI.ChildPanelOptions);
                ChildPanels.Add(Reader.PanelDisplayFactory.CreatePanel(panelUI, keyedPanel));
            }
        }

        protected virtual ReaderPinsGroup CreatePinButtons(ReaderScene reader, Panel panel) => reader.Pins.CreateButtons(panel.Pins, PanelUI.transform);
    }
}