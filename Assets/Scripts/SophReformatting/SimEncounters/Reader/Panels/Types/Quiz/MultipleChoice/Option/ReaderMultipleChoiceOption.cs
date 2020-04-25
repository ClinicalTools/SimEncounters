using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderMultipleChoiceOption : BaseReaderMultipleChoiceOption
    {
        [SerializeField] private ReaderFeedbackUI feedback;
        public virtual ReaderFeedbackUI Feedback { get => feedback; set => feedback = value; }

        [SerializeField] private Toggle optionToggle;
        public virtual Toggle OptionToggle { get => optionToggle; set => optionToggle = value; }

        protected BasicReaderPanelDrawer BasicPanelDrawer { get; set; }
        [Inject]
        public void Inject(BasicReaderPanelDrawer basicPanelDrawer) => BasicPanelDrawer = basicPanelDrawer;

        public override void Display(UserPanel panel)
        {
            BasicPanelDrawer.Display(panel, transform, transform);
        }

        public override void SetToggleGroup(ToggleGroup group)
            => OptionToggle.group = group;

        public override void GetFeedback()
        {
            bool isOn = OptionToggle.isOn;
            if (isOn)
                Feedback.ShowFeedback(isOn);
            else
                Feedback.CloseFeedback();
        }

        public override void SetFeedbackParent(Transform feedbackParent)
        {
            Feedback.transform.SetParent(feedbackParent);
        }
    }
}