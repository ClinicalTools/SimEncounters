using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderFeedbackButtonPanelUI : ReaderPanelUI
    {
        [SerializeField] private Button getFeedbackButton;
        public virtual Button GetFeedbackButton { get => getFeedbackButton; set => getFeedbackButton = value; }

        [SerializeField] private Transform feedbackParent;
        public virtual Transform FeedbackParent { get => feedbackParent; set => feedbackParent = value; }

        public override void Initialize(EncounterReader reader, KeyValuePair<string, Panel> keyedPanel)
        {
            base.Initialize(reader, keyedPanel);

            if (FeedbackParent != null)
                SetFeedbacksParent();

            GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        protected virtual void SetFeedbacksParent()
        {
            foreach (var child in ChildPanelOptions) {
                if (!(child is ReaderFeedbackOptionPanelUI))
                    continue;

                var feedbackChild = (ReaderFeedbackOptionPanelUI)child;
                feedbackChild.Feedback.SetParent(FeedbackParent);
            }
        }

        protected virtual void GetFeedback()
        {
            foreach (var child in ChildPanelOptions) {
                if (!(child is ReaderFeedbackOptionPanelUI))
                    continue;

                var feedbackChild = (ReaderFeedbackOptionPanelUI)child;
                feedbackChild.GetFeedback();
            }
        }
    }
}