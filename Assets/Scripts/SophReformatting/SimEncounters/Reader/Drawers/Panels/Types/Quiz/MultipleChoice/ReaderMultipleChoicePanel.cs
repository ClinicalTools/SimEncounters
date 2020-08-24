
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderMultipleChoicePanel : BaseReaderPanel
    {
        public virtual Button GetFeedbackButton { get => getFeedbackButton; set => getFeedbackButton = value; }
        [SerializeField] private Button getFeedbackButton;

        public virtual BaseOptionUserPanelsDrawer ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }
        [SerializeField] private BaseOptionUserPanelsDrawer childPanelCreator;

        protected List<BaseReaderOptionPanel> Options { get; set; }

        protected IReaderPanelDisplay BasicPanelDrawer { get; set; }
        [Inject] public virtual void Inject(IReaderPanelDisplay basicPanelDrawer) => BasicPanelDrawer = basicPanelDrawer;

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