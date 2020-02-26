using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderCheckboxesTabDisplay : ReaderTabDisplay
    {
        protected virtual ReaderCheckboxesTabUI CheckboxesTabUI { get; }
        protected List<ReaderCheckboxOptionDisplay> Options { get; } = new List<ReaderCheckboxOptionDisplay>();

        public ReaderCheckboxesTabDisplay(ReaderScene reader, ReaderCheckboxesTabUI checkboxesTabUI) : base(reader, checkboxesTabUI)
        {
            CheckboxesTabUI = checkboxesTabUI;

            if (checkboxesTabUI.FeedbackParent != null)
                SetFeedbacksParent();

            CheckboxesTabUI.GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        protected override IReaderPanelDisplay DeserializeChild(KeyValuePair<string, Panel> keyedPanel)
        {
            var child = base.DeserializeChild(keyedPanel);
            if (child is ReaderCheckboxOptionDisplay checkboxOption)
                Options.Add(checkboxOption);
            return child;
        }

        protected virtual void SetFeedbacksParent()
        {
            foreach (var child in Options) {
                if (child is ReaderCheckboxOptionDisplay checkboxOption)
                    checkboxOption.Feedback.SetParent(CheckboxesTabUI.FeedbackParent);
            }
        }

        protected virtual void GetFeedback()
        {
            foreach (var child in Options) {
                if (child is ReaderCheckboxOptionDisplay checkboxOption)
                    checkboxOption.GetFeedback();
            }
        }

    }
}