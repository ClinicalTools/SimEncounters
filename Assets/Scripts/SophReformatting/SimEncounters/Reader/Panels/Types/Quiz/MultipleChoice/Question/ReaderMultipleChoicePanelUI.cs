using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public interface IMultipleChoiceOption
    {
        ReaderFeedbackUI Feedback { get; }
        void GetFeedback();
        void SetToggleGroup(ToggleGroup group);
        void SetFeedbackParent(Transform feedbackParent);
    }
    public class ReaderMultipleChoicePanelUI : ReaderPanelUI
    {
        [SerializeField] private Button getFeedbackButton;
        public virtual Button GetFeedbackButton { get => getFeedbackButton; set => getFeedbackButton = value; }

        [SerializeField] private ToggleGroup toggleGroup;
        public virtual ToggleGroup ToggleGroup { get => toggleGroup; set => toggleGroup = value; }

        [SerializeField] private Transform feedbackParent;
        public virtual Transform FeedbackParent { get => feedbackParent; set => feedbackParent = value; }

        protected List<IMultipleChoiceOption> Options { get; } = new List<IMultipleChoiceOption>();

        protected virtual void Awake()
        {
            GetFeedbackButton.onClick.AddListener(GetFeedback);
        }
        protected override UserPanelDrawer DeserializeChild(UserPanel userPanel)
        {
            var panelDisplay = base.DeserializeChild(userPanel);
            if (panelDisplay is IMultipleChoiceOption option) {
                Options.Add(option);
                option.SetToggleGroup(ToggleGroup);
                option.Feedback.SetParent(FeedbackParent);
            }
            return panelDisplay;
        }

        protected virtual void GetFeedback()
        {
            foreach (var option in Options)
                option.GetFeedback();
        }
    }
}