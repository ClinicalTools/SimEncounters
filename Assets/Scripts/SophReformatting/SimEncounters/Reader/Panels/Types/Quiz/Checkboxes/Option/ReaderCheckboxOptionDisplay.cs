using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderCheckboxOptionDisplay : ReaderPanelDisplay
    {
        public ReaderFeedback Feedback { get; }
        protected ReaderCheckboxOptionUI OptionUI { get; }

        public ReaderCheckboxOptionDisplay(ReaderScene reader, ReaderCheckboxOptionUI optionUI) : base(reader, optionUI)
        {
            OptionUI = optionUI;
            Feedback = new ReaderFeedback(reader, optionUI.Feedback);
        }

        public void GetFeedback()
        {
            bool isOn = OptionUI.OptionToggle.isOn;
            if (isOn || OptionUI.Feedback.OptionType == OptionType.Correct)
                Feedback.ShowFeedback(isOn);
            else
                Feedback.CloseFeedback();
        }
    }
}