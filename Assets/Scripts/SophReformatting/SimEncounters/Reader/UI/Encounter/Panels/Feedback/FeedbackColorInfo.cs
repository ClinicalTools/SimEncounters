using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class FeedbackColorInfo
    {
#if MOBILE
        protected virtual Color IncorrectColor { get; } = new Color(0.9372549f, 0.8588235f, 0.8588235f);
        protected virtual Color PartiallyCorrectColor { get; } = new Color(0.9372549f, 0.9124183f, 0.8588235f);
        protected virtual Color CorrectColor { get; } = new Color(0.8627451f, 0.9372549f, 0.8588235f);
        protected virtual Color DefaultColor { get; } = new Color(0.9372549f, 0.9372549f, 0.9372549f);
#else
        protected virtual Color IncorrectColor { get; } = new Color(0.8470588f, 0.2352941f, 0.2352941f, 0.6f);
        protected virtual Color PartiallyCorrectColor { get; } = new Color(1, 0.7019608f, 0.2862745f, 0.6f);
        protected virtual Color CorrectColor { get; } = new Color(0.2627451f, 0.7294118f, 0.282353f, 0.6f);
        protected virtual Color DefaultColor { get; } = new Color(0.9372549f, 0.9372549f, 0.9372549f, 1f);
#endif
        public virtual Color GetColor(OptionType optionType)
        {
            switch (optionType) {
                case OptionType.Correct:
                    return CorrectColor;
                case OptionType.Incorrect:
                    return IncorrectColor;
                case OptionType.PartiallyCorrect:
                    return PartiallyCorrectColor;
                default:
                    return DefaultColor;
            }
        }

        public virtual Color GetDefaultColor() => DefaultColor;
    }
}