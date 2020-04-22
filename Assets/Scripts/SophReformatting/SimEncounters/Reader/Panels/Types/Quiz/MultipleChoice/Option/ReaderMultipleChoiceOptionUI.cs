using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderMultipleChoiceOptionUI : ReaderPanelUI, IMultipleChoiceOption
    {
        [SerializeField] private ReaderFeedbackUI feedback;
        public virtual ReaderFeedbackUI Feedback { get => feedback; set => feedback = value; }

        [SerializeField] private Toggle optionToggle;
        public virtual Toggle OptionToggle { get => optionToggle; set => optionToggle = value; }

        public virtual void SetToggleGroup(ToggleGroup group)
            => OptionToggle.group = group;

        public virtual void GetFeedback()
        {
            bool isOn = OptionToggle.isOn;
            if (isOn)
                Feedback.ShowFeedback(isOn);
            else
                Feedback.CloseFeedback();
        }

        public void SetFeedbackParent(Transform feedbackParent)
        {
            Feedback.transform.SetParent(feedbackParent);
        }
    }
}