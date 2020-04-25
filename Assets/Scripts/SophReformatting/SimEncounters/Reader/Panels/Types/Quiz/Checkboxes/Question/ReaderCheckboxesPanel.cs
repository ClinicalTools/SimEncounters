using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseReaderMultipleChoiceOption : BaseReaderOptionPanel
    {
        public abstract void SetToggleGroup(ToggleGroup group);
        public abstract void SetFeedbackParent(Transform feedbackParent);
    }
    public abstract class BaseReaderOptionPanel : BaseReaderPanelUI
    {
        public abstract void GetFeedback();
    }
    public abstract class BaseDialogueOptionsDrawer : MonoBehaviour
    {
        public abstract List<BaseReaderDialogueOption> DrawChildPanels(IEnumerable<UserPanel> childPanels);
    }
    public abstract class BaseOptionUserPanelsDrawer : MonoBehaviour
    {
        public abstract List<BaseReaderOptionPanel> DrawChildPanels(IEnumerable<UserPanel> childPanels);
    }
    public class ReaderCheckboxesPanel : BaseReaderPanelUI
    {
        [SerializeField] private Button getFeedbackButton;
        public virtual Button GetFeedbackButton { get => getFeedbackButton; set => getFeedbackButton = value; }

        [SerializeField] private BaseOptionUserPanelsDrawer childPanelCreator;
        public BaseOptionUserPanelsDrawer ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }

        protected List<BaseReaderOptionPanel> Options { get; set; }

        protected BasicReaderPanelDrawer BasicPanelDrawer { get; set; }
        [Inject]
        public void Inject(BasicReaderPanelDrawer basicReaderPanel) => BasicPanelDrawer = basicReaderPanel;

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