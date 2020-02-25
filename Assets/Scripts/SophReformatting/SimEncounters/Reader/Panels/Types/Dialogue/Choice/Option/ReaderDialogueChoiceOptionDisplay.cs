using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialogueChoiceOptionDisplay : IReaderPanelDisplay
    {
        public event Action<ReaderDialogueChoiceOptionDisplay> CorrectlySelected;

        public ReaderFeedback Feedback { get; }
        protected ReaderDialogueChoiceOptionUI OptionUI { get; }
        protected Color OffColor { get; }
        protected Color OnColor { get; }

        public ReaderDialogueChoiceOptionDisplay(ReaderScene reader, ReaderDialogueChoiceOptionUI optionUI, KeyValuePair<string, Panel> keyedPanel)
        {
            OptionUI = optionUI;

            OffColor = optionUI.Border.color;
            OnColor = optionUI.OnColor;

            var valueFieldInitializer = new ReaderValueFieldInitializer(reader);
            valueFieldInitializer.InitializePanelValueFields(optionUI.gameObject, keyedPanel.Value);

            Feedback = new ReaderFeedback(reader, optionUI.Feedback);
            optionUI.Toggle.onValueChanged.AddListener(GetFeedback);
        }

        protected virtual void GetFeedback(bool isOn)
        {
            if (isOn)
            {
                OptionUI.Border.color = OnColor;
                Feedback.ShowFeedback(isOn);
                if (OptionUI.Feedback.OptionType == OptionType.Correct)
                    CorrectlySelected?.Invoke(this);
            }
            else
            {
                OptionUI.Border.color = OffColor;
                Feedback.CloseFeedback();
            }
        }

        public void SetGroup(ToggleGroup group)
        {
            OptionUI.Toggle.group = group;
        }

        public void SetActive(bool active) => OptionUI.gameObject.SetActive(active);
    }
}