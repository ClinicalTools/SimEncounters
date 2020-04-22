using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderCheckboxOptionUI : ReaderPanelUI, ICheckboxOption
    {
        [SerializeField] private ReaderFeedbackUI feedback;
        public virtual ReaderFeedbackUI Feedback { get => feedback; set => feedback = value; }

        [SerializeField] private Toggle optionToggle;
        public virtual Toggle OptionToggle { get => optionToggle; set => optionToggle = value; }

        public void GetFeedback()
        {
            bool isOn = OptionToggle.isOn;
            if (isOn || Feedback.OptionType == OptionType.Correct)
                Feedback.ShowFeedback(isOn);
            else
                Feedback.CloseFeedback();
        }
    }
}