using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseReaderDialogueOption : BaseReaderPanelUI
    {
        public abstract event Action<ReaderDialogueChoiceOptionUI> CorrectlySelected;
        public abstract void SetGroup(ToggleGroup group); 
        public abstract void SetFeedbackParent(Transform parent);
    }

    public class ReaderDialogueChoiceOptionUI : BaseReaderDialogueOption
    {
        [SerializeField] private Toggle toggle;
        public virtual Toggle Toggle { get => toggle; set => toggle = value; }

        [SerializeField] private Color onColor;
        public virtual Color OnColor { get => onColor; set => onColor = value; }
        protected Color OffColor { get; set; }

        [SerializeField] private Image border;
        public virtual Image Border { get => border; set => border = value; }

        [SerializeField] private ReaderFeedbackUI feedback;
        public virtual ReaderFeedbackUI Feedback { get => feedback; set => feedback = value; }

        protected BasicReaderPanelDrawer BasicPanelDrawer { get; set; }

        public override event Action<ReaderDialogueChoiceOptionUI> CorrectlySelected;
        [Inject]
        public void Inject(BasicReaderPanelDrawer basicPanelDrawer) => BasicPanelDrawer = basicPanelDrawer;

        protected virtual void Awake()
        {
            OffColor = OnColor; 
            Toggle.onValueChanged.AddListener(GetFeedback);
        }

        public override void Display(UserPanel panel)
        {
            BasicPanelDrawer.Display(panel, transform, transform);
        }

        protected virtual void GetFeedback(bool isOn)
        {
            if (isOn) {
                Border.color = OnColor;
                Feedback.ShowFeedback(isOn);
                if (Feedback.OptionType == OptionType.Correct)
                    CorrectlySelected?.Invoke(this);
            } else {
                Border.color = OffColor;
                Feedback.CloseFeedback();
            }
        }

        public override void SetGroup(ToggleGroup group) => Toggle.group = group;

        public override void SetFeedbackParent(Transform parent) => Feedback.SetParent(parent);
    }
}