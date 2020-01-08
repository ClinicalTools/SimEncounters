using System;
using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class HexColorField : IChangableValue<Color>
    {
        protected string FloatToHex(float number) => ByteIntToHex(FloatToByteInt(number));
        protected int FloatToByteInt(float number) => (int)(number * 255);
        protected string ByteIntToHex(int number) => number.ToString("X").PadLeft(2, '0');
        protected float HexToFloat(string hex) => Convert.ToInt32(hex, 16) / 255.0f;

        private Color value;
        public Color Value {
            get {
                return value;
            }
            set {
                if (this.value == value)
                    return;

                this.value = value;
                
                UpdateField(value);
                
                ValueChanged?.Invoke(this, Value);
            }
        }

        public event ValueChangedEventHandler<Color> ValueChanged;

        private readonly TMP_InputField hexField;
        public HexColorField(TMP_InputField hexField, Color value)
        {
            this.hexField = hexField;
            Value = value;

            hexField.onValueChanged.AddListener(FieldChanged);

        }
        
        private string hexText;
        private void UpdateField(Color color)
        {
            var redHexString = FloatToHex(color.r);
            var greenHexString = FloatToHex(color.g);
            var blueHexString = FloatToHex(color.b);
            hexText = redHexString + greenHexString + blueHexString;

            hexField.text = hexText;
        }

        private const int RED_START_INDEX = 0;
        private const int GREEN_START_INDEX = 2;
        private const int BLUE_START_INDEX = 4;
        private const int VALUE_HEX_LENGTH = 2;
        private const int COLOR_HEX_LENGTH = 6;
        protected virtual void FieldChanged(string text)
        {
            if (text == hexText)
                return;

            hexText = GetHexText(text);
            if (text != hexText)
                hexField.text = hexText;

            if (hexText.Length != COLOR_HEX_LENGTH)
                return;

            var redHexString = text.Substring(RED_START_INDEX, VALUE_HEX_LENGTH);
            var greenHexString = text.Substring(GREEN_START_INDEX, VALUE_HEX_LENGTH);
            var blueHexString = text.Substring(BLUE_START_INDEX, VALUE_HEX_LENGTH);

            value.r = HexToFloat(redHexString);
            value.g = HexToFloat(greenHexString);
            value.b = HexToFloat(blueHexString);

            ValueChanged?.Invoke(this, value);
        }

        private string GetHexText(string text)
        {
            var newText = "";

            foreach (var ch in text) {
                if (IsHex(ch))
                    newText += ch;

                if (newText.Length == COLOR_HEX_LENGTH)
                    break;
            }

            return newText;
        }

        private bool IsHex(char ch) => char.IsDigit(ch) || IsAlphaHex(ch);

        private const char ALPHA_LOWER_MIN = 'a';
        private const char ALPHA_LOWER_MAX = 'f';
        private const char ALPHA_UPPER_MIN = 'A';
        private const char ALPHA_UPPER_MAX = 'F';
        private bool IsAlphaHex(char ch)
            => IsCharBetween(ch, ALPHA_LOWER_MIN, ALPHA_LOWER_MAX)
                || IsCharBetween(ch, ALPHA_UPPER_MIN, ALPHA_UPPER_MAX);

        private bool IsCharBetween(char ch, char minChar, char maxChar)
            => ch >= minChar && ch <= maxChar;
    }
}
