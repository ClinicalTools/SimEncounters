using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderMultipleChoicePanelDisplay : ReaderPanelDisplay
    {
        protected ReaderMultipleChoicePanelUI MultipleChoicePanelUI { get; }
        protected List<ReaderMultipleChoiceOptionDisplay> Options { get; } = new List<ReaderMultipleChoiceOptionDisplay>();
        public ReaderMultipleChoicePanelDisplay(ReaderScene reader, ReaderMultipleChoicePanelUI multipleChoicePanelUI)
            : base(reader, multipleChoicePanelUI)
        {
            MultipleChoicePanelUI = multipleChoicePanelUI;
            
            multipleChoicePanelUI.GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        protected override IReaderPanelDisplay DeserializeChild(KeyValuePair<string, Panel> keyedPanel)
        {
            var panelDisplay = base.DeserializeChild(keyedPanel);
            if (panelDisplay is ReaderMultipleChoiceOptionDisplay option) {
                Options.Add(option);
                option.SetToggleGroup(MultipleChoicePanelUI.ToggleGroup);
                option.Feedback.SetParent(MultipleChoicePanelUI.FeedbackParent);
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