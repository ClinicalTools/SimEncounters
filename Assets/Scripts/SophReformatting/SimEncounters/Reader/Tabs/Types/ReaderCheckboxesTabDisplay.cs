using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderCheckboxesTabDisplay : ReaderTabDisplay
    {
        protected virtual ReaderCheckboxesTabUI CheckboxesTabUI {get;}

        public ReaderCheckboxesTabDisplay(ReaderScene reader, ReaderCheckboxesTabUI checkboxesTabUI, KeyValuePair<string, Tab> keyedTab) : base(reader, checkboxesTabUI, keyedTab)
        {
            CheckboxesTabUI = checkboxesTabUI;

            if (checkboxesTabUI.FeedbackParent != null)
                SetFeedbacksParent();

            CheckboxesTabUI.GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        protected virtual void SetFeedbacksParent()
        {
            foreach (var child in ReaderPanels)
            {
                if (!(child is ReaderCheckboxOptionUI))
                    continue;

                var feedbackChild = (ReaderCheckboxOptionUI)child;
                feedbackChild.Feedback.transform.SetParent(CheckboxesTabUI.FeedbackParent);
            }
        }

        protected virtual void GetFeedback()
        {
            foreach (var child in ReaderPanels)
            {
                if (!(child is ReaderCheckboxOptionUI))
                    continue;

                var feedbackChild = (ReaderCheckboxOptionUI)child;
                feedbackChild.GetFeedback();
            }
        }

    }
}