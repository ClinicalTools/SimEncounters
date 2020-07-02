using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseReaderDialogueOption : BaseReaderPanel
    {
        public abstract event Action<ReaderDialogueOption> CorrectlySelected;
        public abstract void SetGroup(ToggleGroup group);
        public abstract void SetFeedbackParent(Transform parent);
        public abstract void CloseFeedback();
    }
}