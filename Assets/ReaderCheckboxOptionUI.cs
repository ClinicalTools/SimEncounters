﻿using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderCheckboxOptionUI : ReaderPanelUI
    {
        [SerializeField] private ReaderFeedbackUI feedback;
        public virtual ReaderFeedbackUI Feedback { get => feedback; set => feedback = value; }

        [SerializeField] private Toggle optionToggle;
        public virtual Toggle OptionToggle { get => optionToggle; set => optionToggle = value; }

        protected ReaderFeedback ReaderFeedback { get; set; }

        public override void Initialize(EncounterReader reader, KeyValuePair<string, Panel> keyedPanel)
        {
            base.Initialize(reader, keyedPanel);

            ReaderFeedback = new ReaderFeedback(reader, Feedback);
        }

        public virtual void GetFeedback()
        {
            bool isOn = OptionToggle.isOn;
            if (isOn || Feedback.OptionType == OptionType.Correct)
                ReaderFeedback.ShowFeedback(isOn);
            else
                ReaderFeedback.CloseFeedback();
        }
    }
}