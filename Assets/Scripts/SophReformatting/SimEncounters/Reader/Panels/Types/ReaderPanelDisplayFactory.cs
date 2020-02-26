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

        public IReaderPanelDisplay DisplayPanel(IReaderPanelUI panelUI, KeyValuePair<string, Panel> keyedPanel) {
            return default;
        }

        public IReaderPanelDisplay CreatePanel(IReaderPanelUI panelUI)
        {
            if (panelUI is ReaderCheckboxesPanelUI checkboxesPanelUI)
                return new ReaderCheckboxesPanelDisplay(Reader, checkboxesPanelUI);
            else if (panelUI is ReaderCheckboxOptionUI checkboxOptionUI)
                return new ReaderCheckboxOptionDisplay(Reader, checkboxOptionUI);
            else if (panelUI is ReaderMultipleChoicePanelUI multipleChoicePanelUI)
                return new ReaderMultipleChoicePanelDisplay(Reader, multipleChoicePanelUI);
            else if (panelUI is ReaderMultipleChoiceOptionUI multipleChoiceOptionUI)
                return new ReaderMultipleChoiceOptionDisplay(Reader, multipleChoiceOptionUI);
            else if (panelUI is ReaderOrderableGroupPanelUI orderableGroupPanelUI)
                return new ReaderOrderableGroupPanelDisplay(Reader, orderableGroupPanelUI);
            else if (panelUI is ReaderOrderableItemPanelUI orderableItemPanelUI)
                return new ReaderOrderableItemPanelDisplay(Reader, orderableItemPanelUI);
            else if (panelUI is ReaderDialogueEntryUI dialogueEntryUI)
                return new ReaderDialogueEntryDisplay(Reader, dialogueEntryUI);
            else if (panelUI is ReaderDialogueChoiceUI dialogueChoiceUI)
                return new ReaderDialogueChoiceDisplay(Reader, dialogueChoiceUI);
            else if (panelUI is ReaderDialogueChoiceOptionUI dialogueChoiceOptionUI)
                return new ReaderDialogueChoiceOptionDisplay(Reader, dialogueChoiceOptionUI);
            else if (panelUI is ReaderPanelUI basicPanelUI)
                return new ReaderPanelDisplay(Reader, basicPanelUI);
            else
                return default;
        }

        public ReaderCheckboxesPanelDisplay CreateCheckboxesPanel(ReaderCheckboxesPanelUI panelUI)
            => new ReaderCheckboxesPanelDisplay(Reader, panelUI);
        public ReaderCheckboxOptionDisplay CreateCheckboxOptionPanel(ReaderCheckboxOptionUI panelUI)
            => new ReaderCheckboxOptionDisplay(Reader, panelUI);

        public ReaderMultipleChoiceOptionDisplay CreateMultipleChoiceOptionPanel(ReaderMultipleChoiceOptionUI panelUI)
            => new ReaderMultipleChoiceOptionDisplay(Reader, panelUI);

        public ReaderMultipleChoicePanelDisplay CreateMultipleChoicePanel(ReaderMultipleChoicePanelUI panelUI)
            => new ReaderMultipleChoicePanelDisplay(Reader, panelUI);

        public ReaderOrderableGroupPanelDisplay CreateOrderableGroupPanel(ReaderOrderableGroupPanelUI panelUI)
            => new ReaderOrderableGroupPanelDisplay(Reader, panelUI);

        public ReaderOrderableItemPanelDisplay CreateOrderableItemPanel(ReaderOrderableItemPanelUI panelUI)
            => new ReaderOrderableItemPanelDisplay(Reader, panelUI);

        public ReaderDialogueEntryDisplay CreateDialogueEntryPanel(ReaderDialogueEntryUI panelUI)
            => new ReaderDialogueEntryDisplay(Reader, panelUI);

        public ReaderDialogueChoiceDisplay CreateDialogueChoicePanel(ReaderDialogueChoiceUI panelUI)
            => new ReaderDialogueChoiceDisplay(Reader, panelUI);

        public ReaderDialogueChoiceOptionDisplay CreateDialogueChoiceOptionPanel(ReaderDialogueChoiceOptionUI panelUI)
            => new ReaderDialogueChoiceOptionDisplay(Reader, panelUI);

    }
}