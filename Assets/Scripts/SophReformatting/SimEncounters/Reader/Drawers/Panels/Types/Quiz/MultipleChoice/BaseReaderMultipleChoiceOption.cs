using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseReaderMultipleChoiceOption : BaseReaderOptionPanel
    {
        public abstract void SetToggleGroup(ToggleGroup group);
        public abstract void SetFeedbackParent(Transform feedbackParent);
    }
}