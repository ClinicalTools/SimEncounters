using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderMultipleChoicePanelDisplay : ReaderPanelDisplay
    {
        protected ReaderMultipleChoicePanelUI MultipleChoicePanelUI { get; }
        public ReaderMultipleChoicePanelDisplay(ReaderScene reader, ReaderMultipleChoicePanelUI multipleChoicePanelUI)
            : base(reader, multipleChoicePanelUI)
        {
            MultipleChoicePanelUI = multipleChoicePanelUI;
            // TODO
            UpdateChildren();

            multipleChoicePanelUI.GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        protected virtual void UpdateChildren()
        {
            // TODO 
            foreach (var child in new List<ReaderMultipleChoiceOptionDisplay>())
                if (child is ReaderMultipleChoiceOptionDisplay option) {
                    option.SetToggleGroup(MultipleChoicePanelUI.ToggleGroup);
                    option.Feedback.SetParent(MultipleChoicePanelUI.FeedbackParent);
                }
        }

        protected virtual void GetFeedback()
        {
            // TODO 
            foreach (var child in new List<ReaderMultipleChoiceOptionDisplay>()) {
                if (child is ReaderMultipleChoiceOptionDisplay option)
                    option.GetFeedback();
            }
        }
    }
}