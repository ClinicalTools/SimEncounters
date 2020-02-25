using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderMultipleChoiceOptionUI : ReaderPanelUI
    {
        [SerializeField] private ReaderFeedbackUI feedback;
        public virtual ReaderFeedbackUI Feedback { get => feedback; set => feedback = value; }

        [SerializeField] private Toggle optionToggle;
        public virtual Toggle OptionToggle { get => optionToggle; set => optionToggle = value; }
    }
}