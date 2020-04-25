using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderMultipleChoicePanelDisplay : ReaderPanelDisplay
    {
        protected ReaderMultipleChoicePanel MultipleChoicePanelUI { get; }
        protected List<ReaderMultipleChoiceOptionDisplay> Options { get; } = new List<ReaderMultipleChoiceOptionDisplay>();
        public ReaderMultipleChoicePanelDisplay(ReaderScene reader, ReaderMultipleChoicePanel multipleChoicePanelUI)
            : base(reader, null)
        {
            MultipleChoicePanelUI = multipleChoicePanelUI;
            
            multipleChoicePanelUI.GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        protected override IReaderPanelDisplay DeserializeChild(KeyValuePair<string, Panel> keyedPanel)
        {
            var panelDisplay = base.DeserializeChild(keyedPanel);
            if (panelDisplay is ReaderMultipleChoiceOptionDisplay option) {
                Options.Add(option);
            }
            return panelDisplay;
        }

        protected virtual void GetFeedback()
        {
            foreach (var option in Options)
                option.GetFeedback();
        }
    }
}