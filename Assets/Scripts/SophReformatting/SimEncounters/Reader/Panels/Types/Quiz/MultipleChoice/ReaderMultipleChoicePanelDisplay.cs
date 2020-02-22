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
            foreach (var child in ChildPanels)
            {
                if (!(child is ReaderMultipleChoiceOptionUI))
                    continue;

                var feedbackChild = (ReaderMultipleChoiceOptionUI)child;
                feedbackChild.OptionToggle.group = MultipleChoicePanelUI.ToggleGroup;
                feedbackChild.Feedback.transform.SetParent(MultipleChoicePanelUI.FeedbackParent);
            }
        }

        protected virtual void GetFeedback()
        {
            foreach (var child in ChildPanels)
            {
                if (!(child is ReaderMultipleChoiceOptionUI))
                    continue;

                var feedbackChild = (ReaderMultipleChoiceOptionUI)child;
                feedbackChild.GetFeedback();
            }
        }
    }
}