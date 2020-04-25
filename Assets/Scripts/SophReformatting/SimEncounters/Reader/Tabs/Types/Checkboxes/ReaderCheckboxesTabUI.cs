using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderCheckboxesTabUI : ReaderTabUI
    {
        [SerializeField] private Button getFeedbackButton;
        public virtual Button GetFeedbackButton { get => getFeedbackButton; set => getFeedbackButton = value; }

        [SerializeField] private Transform feedbackParent;
        public virtual Transform FeedbackParent { get => feedbackParent; set => feedbackParent = value; }
    }
}