
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderCheckboxesTab : BaseUserTabDrawer
    {
        public virtual Button GetFeedbackButton { get => getFeedbackButton; set => getFeedbackButton = value; }
        [SerializeField] private Button getFeedbackButton;
        public virtual BaseOptionUserPanelsDrawer ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }
        [SerializeField] private BaseOptionUserPanelsDrawer childPanelCreator;

        protected List<BaseReaderOptionPanel> Options { get; set; }

        protected virtual void Awake()
        {
            GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        public override void Display(UserTab userTab)
        {
            Options = ChildPanelCreator.DrawChildPanels(userTab.GetPanels());
        }

        protected virtual void GetFeedback()
        {
            foreach (var option in Options)
                option.GetFeedback();
        }
    }
}