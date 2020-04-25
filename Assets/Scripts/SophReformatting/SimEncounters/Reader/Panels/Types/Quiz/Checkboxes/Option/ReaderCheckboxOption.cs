using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderCheckboxOption : BaseReaderOptionPanel
    {
        [SerializeField] private BaseChildPanelsDrawer childPanelCreator;
        public BaseChildPanelsDrawer ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }

        [SerializeField] private Toggle optionToggle;
        public virtual Toggle OptionToggle { get => optionToggle; set => optionToggle = value; }

        [SerializeField] private ReaderFeedbackUI feedback;
        public virtual ReaderFeedbackUI Feedback { get => feedback; set => feedback = value; }

        protected BasicReaderPanelDrawer BasicPanelDrawer { get; set; }
        [Inject]
        public void Inject(BasicReaderPanelDrawer basicReaderPanel) => BasicPanelDrawer = basicReaderPanel;

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