using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderMultipleChoiceOptionDisplay : ReaderPanelDisplay
    {
        public ReaderFeedback Feedback { get; }
        protected ReaderMultipleChoiceOptionUI MultipleChoiceOptionUI { get; }

        public ReaderMultipleChoiceOptionDisplay(ReaderScene reader, ReaderMultipleChoiceOptionUI multipleChoiceOptionUI, KeyValuePair<string, Panel> keyedPanel)
            : base(reader, multipleChoiceOptionUI, keyedPanel)
        {
            MultipleChoiceOptionUI = multipleChoiceOptionUI;
            Feedback = new ReaderFeedback(reader, multipleChoiceOptionUI.Feedback);
        }

        public virtual void SetToggleGroup(ToggleGroup group)
            => MultipleChoiceOptionUI.OptionToggle.group = group;

        public virtual void GetFeedback()
        {
            bool isOn = MultipleChoiceOptionUI.OptionToggle.isOn;
            if (isOn)
                Feedback.ShowFeedback(isOn);
            else
                Feedback.CloseFeedback();
        }
    }
}