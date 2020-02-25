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
            else if (panelUI is ReaderDialogueChoiceUI)
                return new ReaderDialogueChoiceDisplay(Reader, (ReaderDialogueChoiceUI)panelUI, keyedPanel);
            else if (panelUI is ReaderDialogueChoiceOptionUI)
                return new ReaderDialogueChoiceOptionDisplay(Reader, (ReaderDialogueChoiceOptionUI)panelUI, keyedPanel);
            else if (panelUI is ReaderPanelUI)
                return new ReaderPanelDisplay(Reader, (ReaderPanelUI)panelUI, keyedPanel);
            else
                return default;
        }
        public ReaderCheckboxesPanelDisplay CreateCheckboxesPanel(ReaderCheckboxesPanelUI panelUI, KeyValuePair<string, Panel> keyedPanel)
            => new ReaderCheckboxesPanelDisplay(Reader, panelUI, keyedPanel);

        public ReaderMultipleChoicePanelDisplay CreateMultipleChoicePanel(ReaderMultipleChoicePanelUI panelUI, KeyValuePair<string, Panel> keyedPanel)
            => new ReaderMultipleChoicePanelDisplay(Reader, panelUI, keyedPanel);

        public ReaderOrderableGroupPanelDisplay CreateOrderableGroupPanel(ReaderOrderableGroupPanelUI panelUI, KeyValuePair<string, Panel> keyedPanel)
            => new ReaderOrderableGroupPanelDisplay(Reader, panelUI, keyedPanel);

        public ReaderOrderableItemPanelDisplay CreateOrderableItemPanel(ReaderOrderableItemPanelUI panelUI, KeyValuePair<string, Panel> keyedPanel)
            => new ReaderOrderableItemPanelDisplay(Reader, panelUI, keyedPanel);

        public ReaderDialogueChoiceDisplay CreateDialogueChoicePanel(ReaderDialogueChoiceUI panelUI, KeyValuePair<string, Panel> keyedPanel)
            => new ReaderDialogueChoiceDisplay(Reader, panelUI, keyedPanel);

        public ReaderDialogueChoiceOptionDisplay CreateDialogueChoiceOptionPanel(ReaderDialogueChoiceOptionUI panelUI, KeyValuePair<string, Panel> keyedPanel)
            => new ReaderDialogueChoiceOptionDisplay(Reader, panelUI, keyedPanel);

    }
}