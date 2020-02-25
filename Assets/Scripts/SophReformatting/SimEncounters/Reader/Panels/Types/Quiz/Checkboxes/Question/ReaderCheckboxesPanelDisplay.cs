using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderCheckboxesPanelDisplay : ReaderPanelDisplay
    {
        protected ReaderCheckboxesPanelUI CheckboxesPanelUI { get; }
        public ReaderCheckboxesPanelDisplay(ReaderScene reader, ReaderCheckboxesPanelUI checkboxesPanelUI, KeyValuePair<string, Panel> keyedPanel)
            : base(reader, checkboxesPanelUI, keyedPanel)
        {
            CheckboxesPanelUI = checkboxesPanelUI;
            CheckboxesPanelUI.GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        protected virtual void GetFeedback()
        {
            foreach (var child in ChildPanels)
            {
                if (child is ReaderCheckboxOptionDisplay feedbackChild)
                    feedbackChild.GetFeedback();
            }
        }
    }
}