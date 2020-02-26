using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderCheckboxesPanelDisplay : ReaderPanelDisplay
    {
        protected ReaderCheckboxesPanelUI CheckboxesPanelUI { get; }
        public ReaderCheckboxesPanelDisplay(ReaderScene reader, ReaderCheckboxesPanelUI checkboxesPanelUI)
            : base(reader, checkboxesPanelUI)
        {
            CheckboxesPanelUI = checkboxesPanelUI;
            CheckboxesPanelUI.GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        protected virtual void GetFeedback()
        {
            // TODO 
            foreach (var child in new List<ReaderCheckboxOptionDisplay>())
            {
                if (child is ReaderCheckboxOptionDisplay feedbackChild)
                    feedbackChild.GetFeedback();
            }
        }
    }
}