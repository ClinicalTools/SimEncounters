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

        public ReaderFeedback Feedback { get; protected set; }
        protected ReaderScene Reader { get; }
        protected ReaderDialogueChoiceOptionUI OptionUI { get; }
        protected Color OffColor { get; }
        protected Color OnColor { get; }

        public ReaderDialogueChoiceOptionDisplay(ReaderScene reader, ReaderDialogueChoiceOptionUI optionUI)
        {
            Reader = reader;
            OptionUI = optionUI;

            OffColor = optionUI.Border.color;
            OnColor = optionUI.OnColor;

            Feedback = new ReaderFeedback(Reader, OptionUI.Feedback);
        }

        public void Display(KeyValuePair<string, Panel> keyedPanel)
        {
            Reader.ValueFieldInitializer.InitializePanelValueFields(OptionUI.gameObject, keyedPanel.Value);
            OptionUI.Toggle.onValueChanged.AddListener(GetFeedback);
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