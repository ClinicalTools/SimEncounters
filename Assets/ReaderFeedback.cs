using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderFeedback
    {
        private readonly Color incorrectColor = new Color(0.8470588f, 0.2352941f, 0.2352941f, 0.6f);
        private readonly Color partiallyCorrectColor = new Color(1, 0.7019608f, 0.2862745f, 0.6f);
        private readonly Color correctColor = new Color(0.2627451f, 0.7294118f, 0.282353f, 0.6f);

        protected ReaderFeedbackUI ReaderFeedbackUI { get; }
        public ReaderFeedback(EncounterReader reader, ReaderFeedbackUI readerFeedbackUI)
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

            Color color = GetColor(optionType);
            foreach (var image in ReaderFeedbackUI.ColoredImages)
                image.color = color;

            ReaderFeedbackUI.IsCorrectLabel.text = GetOptionTypeText(optionType, isOn);

            ReaderFeedbackUI.Stripes.gameObject.SetActive(ShowStripes(optionType, isOn));
        }

        protected virtual Color GetColor(OptionType optionType)
        {
            switch (optionType) {
                case OptionType.Correct:
                    return correctColor;
                case OptionType.Incorrect:
                    return incorrectColor;
                case OptionType.PartiallyCorrect:
                    return partiallyCorrectColor;
                default:
                    return Color.white;
            }
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

        public virtual void CloseFeedback()
        {
            ReaderFeedbackUI.gameObject.SetActive(false);
        }
    }
}