using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderCheckboxOption : BaseReaderOptionPanel
    {
        public BaseReaderPanelsCreator ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }
        [SerializeField] private BaseReaderPanelsCreator childPanelCreator;
        public virtual Toggle OptionToggle { get => optionToggle; set => optionToggle = value; }
        [SerializeField] private Toggle optionToggle;
        public virtual ReaderFeedbackUI Feedback { get => feedback; set => feedback = value; }
        [SerializeField] private ReaderFeedbackUI feedback;

        protected IReaderPanelDisplay BasicPanelDrawer { get; set; }
        [Inject] public virtual void Inject(IReaderPanelDisplay basicReaderPanel) => BasicPanelDrawer = basicReaderPanel;

        public override void Display(UserPanel userPanel)
        {
            BasicPanelDrawer.Display(userPanel, transform, transform);
            if (ChildPanelCreator != null)
                ChildPanelCreator.DrawChildPanels(userPanel.GetChildPanels());
        }

        public override void GetFeedback()
        {
            bool isOn = OptionToggle.isOn;
            if (isOn || Feedback.OptionType == OptionType.Correct)
                Feedback.ShowFeedback(isOn);
            else
                Feedback.CloseFeedback();
        }
    }
}