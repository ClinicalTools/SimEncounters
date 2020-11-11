
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderCheckboxesPanel : BaseReaderPanel
    {
        public virtual Button GetFeedbackButton { get => getFeedbackButton; set => getFeedbackButton = value; }
        [SerializeField] private Button getFeedbackButton;

        public BaseOptionUserPanelsDrawer ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }
        [SerializeField] private BaseOptionUserPanelsDrawer childPanelCreator;

        protected List<BaseReaderOptionPanel> Options { get; set; }

        protected IReaderPanelDisplay BasicPanelDrawer { get; set; }
        [Inject] public virtual void Inject(IReaderPanelDisplay basicReaderPanel) => BasicPanelDrawer = basicReaderPanel;

        protected virtual void Awake()
        {
            GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        public override void Display(UserPanel userPanel)
        {
            BasicPanelDrawer.Display(userPanel, transform, transform);
            Options = ChildPanelCreator.DrawChildPanels(userPanel.GetChildPanels());
        }
        protected virtual void OnDestroy() => BasicPanelDrawer.Dispose();

        protected virtual void GetFeedback()
        {
            foreach (var option in Options)
                option.GetFeedback();
        }
    }
}