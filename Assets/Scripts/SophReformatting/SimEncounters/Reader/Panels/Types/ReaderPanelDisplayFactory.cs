using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPanelDisplayFactory
    {
        protected ReaderScene Reader { get; }
        public ReaderPanelDisplayFactory(ReaderScene reader)
        {
            Reader = reader;
        }

        public IReaderPanelDisplay CreatePanel(IReaderPanelUI panelUI, KeyValuePair<string, Panel> keyedPanel)
        {
            if (panelUI is ReaderCheckboxesPanelUI)
                return new ReaderCheckboxesPanelDisplay(Reader, (ReaderCheckboxesPanelUI)panelUI, keyedPanel);
            else if (panelUI is ReaderMultipleChoicePanelUI)
                return new ReaderMultipleChoicePanelDisplay(Reader, (ReaderMultipleChoicePanelUI)panelUI, keyedPanel);
            else if (panelUI is ReaderOrderableGroupPanelUI)
                return new ReaderOrderableGroupPanelDisplay(Reader, (ReaderOrderableGroupPanelUI)panelUI, keyedPanel);
            else if (panelUI is ReaderOrderableItemPanelUI)
                return new ReaderOrderableItemPanelDisplay(Reader, (ReaderOrderableItemPanelUI)panelUI, keyedPanel);
            else if (panelUI is ReaderPanelUI)
                return new ReaderPanelDisplay(Reader, (ReaderPanelUI)panelUI, keyedPanel);
            else
                return default;
        }
    }
}