using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderFeedbackOptionPanelUI : ReaderPanelUI
    {
        [SerializeField] private Transform feedback;
        public virtual Transform Feedback { get => feedback; set => feedback = value; }


        public virtual void GetFeedback()
        {
            Feedback.gameObject.SetActive(true);
        }
    }
}