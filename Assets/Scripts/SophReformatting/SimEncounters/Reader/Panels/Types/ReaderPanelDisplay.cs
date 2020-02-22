using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPanelDisplay : IReaderPanelDisplay
    {
        protected ReaderPanelUI PanelUI { get; }
        protected ReaderPanelCreator ReaderPanelCreator { get; private set; }
        protected List<ReaderPanelUI> ChildPanels { get; set; }
        protected IValueField[] ValueFields { get; set; }

        public ReaderPanelDisplay(ReaderScene reader, ReaderPanelUI panelUI, KeyValuePair<string, Panel> keyedPanel)
        {
            PanelUI = panelUI;
            var panel = keyedPanel.Value;

            CreatePinButtons(reader, panel);

            var valueFieldInitializer = new ReaderValueFieldInitializer(reader);
            ValueFields = valueFieldInitializer.InitializePanelValueFields(PanelUI.gameObject, panel);

            ReaderPanelCreator = new ReaderPanelCreator(reader, PanelUI.ChildrenParent);
            ChildPanels = ReaderPanelCreator.Deserialize(panel.ChildPanels, PanelUI.ChildPanelOptions);
        }

        protected virtual ReaderPinsGroup CreatePinButtons(ReaderScene reader, Panel panel) => reader.Pins.CreateButtons(panel.Pins, PanelUI.transform);
    }
}