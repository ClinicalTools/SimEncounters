using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseReaderMultipleChoiceOption : BaseReaderOptionPanel
    {
        public abstract void SetToggleGroup(ToggleGroup group);
        public abstract void SetFeedbackParent(Transform feedbackParent);
    }
}