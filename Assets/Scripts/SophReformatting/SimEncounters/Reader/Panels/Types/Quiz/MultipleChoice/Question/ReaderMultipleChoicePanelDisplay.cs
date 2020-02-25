using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderMultipleChoicePanelDisplay : ReaderPanelDisplay
    {
        protected ReaderMultipleChoicePanelUI MultipleChoicePanelUI { get; }
        public ReaderMultipleChoicePanelDisplay(ReaderScene reader, ReaderMultipleChoicePanelUI multipleChoicePanelUI, KeyValuePair<string, Panel> keyedPanel)
            : base(reader, multipleChoicePanelUI, keyedPanel)
        {
            MultipleChoicePanelUI = multipleChoicePanelUI;

            UpdateChildren();

            multipleChoicePanelUI.GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        protected virtual void UpdateChildren()
        {
            foreach (var child in ChildPanels) {
                if (child is ReaderMultipleChoiceOptionDisplay option) {
                    option.SetToggleGroup(MultipleChoicePanelUI.ToggleGroup);
                    option.Feedback.SetParent(MultipleChoicePanelUI.FeedbackParent);
                }
            }
        }

        protected virtual void GetFeedback()
        {
            foreach (var child in ChildPanels) {
                if (child is ReaderMultipleChoiceOptionDisplay option)
                    option.GetFeedback();
            }
        }
    }
}