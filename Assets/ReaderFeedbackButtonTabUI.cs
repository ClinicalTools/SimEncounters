using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderFeedbackButtonTabUI : ReaderTabUI
    {
        [SerializeField] private Button getFeedbackButton;
        public virtual Button GetFeedbackButton { get => getFeedbackButton; set => getFeedbackButton = value; }

        [SerializeField] private Transform feedbackParent;
        public virtual Transform FeedbackParent { get => feedbackParent; set => feedbackParent = value; }

        public override void Initialize(EncounterReader reader, string tabFolder, Tab tab)
        {
            base.Initialize(reader, tabFolder, tab);

            if (FeedbackParent != null)
                SetFeedbacksParent();

            GetFeedbackButton.onClick.AddListener(GetFeedback);
        }

        protected virtual void SetFeedbacksParent()
        {
            foreach (var child in ReaderPanels) {
                if (!(child is ReaderFeedbackOptionPanelUI))
                    continue;

                var feedbackChild = (ReaderFeedbackOptionPanelUI)child;
                feedbackChild.Feedback.transform.SetParent(FeedbackParent);
            }
        }

        protected virtual void GetFeedback()
        {
            foreach (var child in ReaderPanels) {
                if (!(child is ReaderFeedbackOptionPanelUI))
                    continue;

                var feedbackChild = (ReaderFeedbackOptionPanelUI)child;
                feedbackChild.GetFeedback();
            }
        }
    }
}