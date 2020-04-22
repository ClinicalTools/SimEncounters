using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class CheckboxOptionPanel : BaseReaderPanelUI
    {
        public abstract void GetFeedback();
    }
    public abstract class ChildCheckboxPanelsDrawer : MonoBehaviour
    {
        public abstract List<CheckboxOptionPanel> DrawChildPanels(IEnumerable<UserPanel> childPanels);
    }
    public class ReaderCheckboxesPanel : BaseReaderPanelUI
    {
        [SerializeField] private Button getFeedbackButton;
        public virtual Button GetFeedbackButton { get => getFeedbackButton; set => getFeedbackButton = value; }

        [SerializeField] private ChildCheckboxPanelsDrawer childPanelCreator;
        public ChildCheckboxPanelsDrawer ChildPanelCreator { get => childPanelCreator; set => childPanelCreator = value; }

        protected List<CheckboxOptionPanel> Options { get; set; }

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