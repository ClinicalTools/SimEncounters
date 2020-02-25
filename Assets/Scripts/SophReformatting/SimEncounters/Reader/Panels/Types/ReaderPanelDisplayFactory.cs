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
            if (panelUI is ReaderCheckboxesPanelUI checkboxesPanelUI)
                return new ReaderCheckboxesPanelDisplay(Reader, checkboxesPanelUI, keyedPanel);
            else if (panelUI is ReaderCheckboxOptionUI checkboxOptionUI)
                return new ReaderCheckboxOptionDisplay(Reader, checkboxOptionUI, keyedPanel);
            else if (panelUI is ReaderMultipleChoicePanelUI multipleChoicePanelUI)
                return new ReaderMultipleChoicePanelDisplay(Reader, multipleChoicePanelUI, keyedPanel);
            else if (panelUI is ReaderMultipleChoiceOptionUI multipleChoiceOptionUI)
                return new ReaderMultipleChoiceOptionDisplay(Reader, multipleChoiceOptionUI, keyedPanel);
            else if (panelUI is ReaderOrderableGroupPanelUI orderableGroupPanelUI)
                return new ReaderOrderableGroupPanelDisplay(Reader, orderableGroupPanelUI, keyedPanel);
            else if (panelUI is ReaderOrderableItemPanelUI orderableItemPanelUI)
                return new ReaderOrderableItemPanelDisplay(Reader, orderableItemPanelUI, keyedPanel);
            else if (panelUI is ReaderDialogueEntryUI dialogueEntryUI)
                return new ReaderDialogueEntryDisplay(Reader, dialogueEntryUI, keyedPanel);
            else if (panelUI is ReaderDialogueChoiceUI dialogueChoiceUI)
                return new ReaderDialogueChoiceDisplay(Reader, dialogueChoiceUI, keyedPanel);
            else if (panelUI is ReaderDialogueChoiceOptionUI dialogueChoiceOptionUI)
                return new ReaderDialogueChoiceOptionDisplay(Reader, dialogueChoiceOptionUI, keyedPanel);
            else if (panelUI is ReaderPanelUI basicPanelUI)
                return new ReaderPanelDisplay(Reader, basicPanelUI, keyedPanel);
            else
                return default;
        }
        public ReaderCheckboxesPanelDisplay CreateCheckboxesPanel(ReaderCheckboxesPanelUI panelUI, KeyValuePair<string, Panel> keyedPanel)
            => new ReaderCheckboxesPanelDisplay(Reader, panelUI, keyedPanel);
        public ReaderCheckboxOptionDisplay CreateCheckboxOptionPanel(ReaderCheckboxOptionUI panelUI, KeyValuePair<string, Panel> keyedPanel)
            => new ReaderCheckboxOptionDisplay(Reader, panelUI, keyedPanel);

        public ReaderMultipleChoiceOptionDisplay CreateMultipleChoiceOptionPanel(ReaderMultipleChoiceOptionUI panelUI, KeyValuePair<string, Panel> keyedPanel)
            => new ReaderMultipleChoiceOptionDisplay(Reader, panelUI, keyedPanel);

        public ReaderMultipleChoicePanelDisplay CreateMultipleChoicePanel(ReaderMultipleChoicePanelUI panelUI, KeyValuePair<string, Panel> keyedPanel)
            => new ReaderMultipleChoicePanelDisplay(Reader, panelUI, keyedPanel);

        public ReaderOrderableGroupPanelDisplay CreateOrderableGroupPanel(ReaderOrderableGroupPanelUI panelUI, KeyValuePair<string, Panel> keyedPanel)
            => new ReaderOrderableGroupPanelDisplay(Reader, panelUI, keyedPanel);

        public ReaderOrderableItemPanelDisplay CreateOrderableItemPanel(ReaderOrderableItemPanelUI panelUI, KeyValuePair<string, Panel> keyedPanel)
            => new ReaderOrderableItemPanelDisplay(Reader, panelUI, keyedPanel);

        public ReaderDialogueEntryDisplay CreateDialogueEntryPanel(ReaderDialogueEntryUI panelUI, KeyValuePair<string, Panel> keyedPanel)
            => new ReaderDialogueEntryDisplay(Reader, panelUI, keyedPanel);

        public ReaderDialogueChoiceDisplay CreateDialogueChoicePanel(ReaderDialogueChoiceUI panelUI, KeyValuePair<string, Panel> keyedPanel)
            => new ReaderDialogueChoiceDisplay(Reader, panelUI, keyedPanel);

        public ReaderDialogueChoiceOptionDisplay CreateDialogueChoiceOptionPanel(ReaderDialogueChoiceOptionUI panelUI, KeyValuePair<string, Panel> keyedPanel)
            => new ReaderDialogueChoiceOptionDisplay(Reader, panelUI, keyedPanel);

    }
}