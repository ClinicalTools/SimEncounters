using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileDialogueOption : BaseReaderDialogueOption
    {
        public virtual GameObject CorrectlySelectedObject { get => correctlySelectedObject; set => correctlySelectedObject = value; }
        [SerializeField] private GameObject correctlySelectedObject;
        public virtual GameObject NormalObject { get => normalObject; set => normalObject = value; }
        [SerializeField] private GameObject normalObject;
        public virtual Toggle Toggle { get => toggle; set => toggle = value; }
        [SerializeField] private Toggle toggle;
        public virtual Color OnColor { get => onColor; set => onColor = value; }
        [SerializeField] private Color onColor;
        protected Color OffColor { get; set; }
        public virtual Image Border { get => border; set => border = value; }
        [SerializeField] private Image border;
        public virtual ReaderFeedbackUI Feedback { get => feedback; set => feedback = value; }
        [SerializeField] private ReaderFeedbackUI feedback;

        public override event Action<BaseReaderDialogueOption> CorrectlySelected;

        protected IReaderPanelDisplay BasicPanelDrawer { get; set; }
        [Inject] public virtual void Inject(IReaderPanelDisplay basicPanelDrawer) => BasicPanelDrawer = basicPanelDrawer;

        protected virtual void Awake()
        {
            OffColor = OnColor;
            Toggle.onValueChanged.AddListener(GetFeedback);
        }

        public override void Display(UserPanel panel)
            => BasicPanelDrawer.Display(panel, transform, transform);

        protected virtual void GetFeedback(bool isOn)
        {
            if (!isOn) {
                Toggle.interactable = true;
                Border.color = OffColor;
                Feedback.CloseFeedback();
                return;
            } 

            if (Feedback.OptionType != OptionType.Correct) {
                Toggle.interactable = false;
                Border.color = OnColor;
                Feedback.ShowFeedback(isOn);
                return;
            }

            CorrectlySelectedObject.SetActive(true);
            NormalObject.SetActive(false);
            CorrectlySelected?.Invoke(this);
        }

        public override void SetGroup(ToggleGroup group) => Toggle.group = group;

        public override void SetFeedbackParent(Transform parent) => Feedback.SetParent(parent);
        public override void CloseFeedback() => Feedback.CloseFeedback();
    }
}