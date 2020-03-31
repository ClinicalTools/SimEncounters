﻿using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderFeedback
    {
        protected virtual FeedbackColorInfo FeedbackColorInfo { get; } = new FeedbackColorInfo();
        protected ReaderFeedbackUI ReaderFeedbackUI { get; }
        public ReaderFeedback(ReaderScene reader, ReaderFeedbackUI readerFeedbackUI)
        {
            ReaderFeedbackUI = readerFeedbackUI;

            ReaderFeedbackUI.CloseButton.onClick.AddListener(CloseFeedback);
        }

        public virtual void ShowFeedback(bool isOn)
        {
            ReaderFeedbackUI.gameObject.SetActive(true);
            foreach (var controlledObject in ReaderFeedbackUI.ControlledObjects)
                controlledObject.SetActive(true);

            var optionType = ReaderFeedbackUI.OptionType;
            foreach (var incorrectObject in ReaderFeedbackUI.IncorrectObjects)
                incorrectObject.SetActive(optionType != OptionType.Correct);

            Color color = FeedbackColorInfo.GetColor(optionType);
            foreach (var image in ReaderFeedbackUI.ColoredImages)
                image.color = color;

            ReaderFeedbackUI.IsCorrectLabel.text = GetOptionTypeText(optionType, isOn);

            ReaderFeedbackUI.Stripes.gameObject.SetActive(ShowStripes(optionType, isOn));
        }


        protected virtual string GetOptionTypeText(OptionType optionType, bool isOn)
        {
            if (optionType == OptionType.Incorrect)
                return "Incorrect";
            else if (optionType == OptionType.PartiallyCorrect)
                return "Partially Correct";
            else if (!isOn)
                return "Missed Correct Response";
            else
                return "Correct";
        }

        protected virtual bool ShowStripes(OptionType optionType, bool isOn) => optionType == OptionType.Correct && !isOn;

        public virtual void SetParent(Transform parent)
            => ReaderFeedbackUI.transform.SetParent(parent);

        public virtual void CloseFeedback()
        {
            Color color = new Color(0.9372549f, 0.9372549f, 0.9372549f, 1f);
            foreach (var image in ReaderFeedbackUI.ColoredImages)
                image.color = color;
            ReaderFeedbackUI.gameObject.SetActive(false);
        }
    }
}