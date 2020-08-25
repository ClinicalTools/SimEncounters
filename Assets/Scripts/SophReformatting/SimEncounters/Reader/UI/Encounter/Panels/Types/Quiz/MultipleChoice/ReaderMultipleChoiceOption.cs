
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMultipleChoiceOption : BaseReaderMultipleChoiceOption
    {
        public virtual ReaderFeedbackUI Feedback { get => feedback; set => feedback = value; }
        [SerializeField] private ReaderFeedbackUI feedback;
        public virtual Toggle OptionToggle { get => optionToggle; set => optionToggle = value; }
        [SerializeField] private Toggle optionToggle;

        protected IReaderPanelDisplay BasicPanelDrawer { get; set; }
        [Inject] public virtual void Inject(IReaderPanelDisplay basicPanelDrawer) => BasicPanelDrawer = basicPanelDrawer;

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