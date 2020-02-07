using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderMultipleChoicePanelUI : ReaderPanelUI
    {
        [SerializeField] private Button getFeedbackButton;
        public virtual Button GetFeedbackButton { get => getFeedbackButton; set => getFeedbackButton = value; }

        [SerializeField] private ToggleGroup toggleGroup;
        public virtual ToggleGroup ToggleGroup { get => toggleGroup; set => toggleGroup = value; }

        [SerializeField] private Transform feedbackParent;
        public virtual Transform FeedbackParent { get => feedbackParent; set => feedbackParent = value; }

        public override void Initialize(EncounterReader reader, KeyValuePair<string, Panel> keyedPanel)
        {
            base.Initialize(reader, keyedPanel);

            UpdateChildren();

            GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        protected virtual void UpdateChildren()
        {
            foreach (var child in ChildPanels) {
                if (!(child is ReaderMultipleChoiceOptionUI))
                    continue;

                var feedbackChild = (ReaderMultipleChoiceOptionUI)child;
                feedbackChild.OptionToggle.group = ToggleGroup;
                feedbackChild.Feedback.transform.SetParent(FeedbackParent);
            }
        }

        protected virtual void GetFeedback()
        {
            foreach (var child in ChildPanels) {
                if (!(child is ReaderMultipleChoiceOptionUI))
                    continue;

                var feedbackChild = (ReaderMultipleChoiceOptionUI)child;
                feedbackChild.GetFeedback();
            }
        }
    }
}