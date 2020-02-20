using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialogueChoiceOptionUI : BaseReaderPanelUI
    {
        public event Action<ReaderDialogueChoiceOptionUI> CorrectlySelected;

        [SerializeField] private string type;
        public override string Type { get => type; set => type = value; }

        [SerializeField] private Toggle toggle;
        public virtual Toggle Toggle { get => toggle; set => toggle = value; }

        [SerializeField] private Color onColor;
        public virtual Color OnColor { get => onColor; set => onColor = value; }
        protected Color OffColor { get; set; }

        [SerializeField] private Image border;
        public virtual Image Border { get => border; set => border = value; }
        
        [SerializeField] private ReaderFeedbackUI feedback;
        public virtual ReaderFeedbackUI Feedback { get => feedback; set => feedback = value; }

        protected ReaderFeedback ReaderFeedback { get; set; }

        public override void Initialize(ReaderScene reader, KeyValuePair<string, Panel> keyedPanel)
        {
            base.Initialize(reader, keyedPanel);
            OffColor = Border.color;

            var valueFieldInitializer = new ReaderValueFieldInitializer(reader);
            valueFieldInitializer.InitializePanelValueFields(gameObject, keyedPanel.Value);

            ReaderFeedback = new ReaderFeedback(reader, Feedback);
            Toggle.onValueChanged.AddListener(GetFeedback);
        }

        protected virtual void GetFeedback(bool isOn)
        {
            if (isOn) {
                Border.color = OnColor;
                ReaderFeedback.ShowFeedback(isOn);
                if (Feedback.OptionType == OptionType.Correct)
                    CorrectlySelected?.Invoke(this);
            } else {
                Border.color = OffColor;
                ReaderFeedback.CloseFeedback();
            }
        }

        public void SetGroup(ToggleGroup group)
        {
            Toggle.group = group;
        }
    }
}