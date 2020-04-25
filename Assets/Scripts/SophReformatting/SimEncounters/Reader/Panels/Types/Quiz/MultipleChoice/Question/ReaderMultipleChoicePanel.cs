using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public interface IMultipleChoiceOption
    {
        ReaderFeedbackUI Feedback { get; }
        void GetFeedback();
        void SetToggleGroup(ToggleGroup group);
        void SetFeedbackParent(Transform feedbackParent);
    }
    public class ReaderMultipleChoicePanel : BaseReaderPanelUI
    {
        [SerializeField] private Button getFeedbackButton;
        public virtual Button GetFeedbackButton { get => getFeedbackButton; set => getFeedbackButton = value; }

        [SerializeField] private BaseOptionUserPanelsDrawer childPanelCreator;
        public virtual BaseOptionUserPanelsDrawer ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }

        protected List<BaseReaderOptionPanel> Options { get; set; }

        protected BasicReaderPanelDrawer BasicPanelDrawer { get; set; }
        [Inject]
        public void Inject(BasicReaderPanelDrawer basicPanelDrawer) => BasicPanelDrawer = basicPanelDrawer;

        protected virtual void Awake()
        {
            GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        public override void Display(UserPanel userPanel)
        {
            BasicPanelDrawer.Display(userPanel, transform, transform);
            Options = ChildPanelCreator.DrawChildPanels(userPanel.GetChildPanels());
        }

        protected virtual void GetFeedback()
        {
            foreach (var option in Options)
                option.GetFeedback();
        }
    }
}