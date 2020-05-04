using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialogueOption : BaseReaderDialogueOption
    {
        public virtual Toggle Toggle { get => toggle; set => toggle = value; }
        [SerializeField] private Toggle toggle;
        public virtual Color OnColor { get => onColor; set => onColor = value; }
        [SerializeField] private Color onColor;
        protected Color OffColor { get; set; }
        public virtual Image Border { get => border; set => border = value; }
        [SerializeField] private Image border;
        public virtual ReaderFeedbackUI Feedback { get => feedback; set => feedback = value; }
        [SerializeField] private ReaderFeedbackUI feedback;

        public override event Action<ReaderDialogueOption> CorrectlySelected;

        protected IReaderPanelDisplay BasicPanelDrawer { get; set; }
        [Inject] public virtual void Inject(IReaderPanelDisplay basicPanelDrawer) => BasicPanelDrawer = basicPanelDrawer;

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