using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class SectionCreator : MonoBehaviour
    {
        private const int LARGE_SPACE = 15;
        private const int SMALL_SPACE = 5;


        // Serializable field are public for easy use in custom editors
        [field: SerializeField] public Button CancelButton { get; set; }
        [field: SerializeField] public Button CreateButton { get; set; }

        [field: Space(LARGE_SPACE)]

        [field: SerializeField] public TMP_InputField SectionNameField { get; set; }

        [field: Space(LARGE_SPACE)]

        [field: SerializeField] public Transform SectionIconsParent { get; set; }


        [field: Space(LARGE_SPACE)]

        [field: SerializeField] public Transform ColorTogglesParent { get; set; }

        [field: Space(SMALL_SPACE)]

        [field: SerializeField] public Image ColorImage { get; set; }

        [field: Space(SMALL_SPACE)]

        [field: SerializeField] public TMP_InputField HexColorField { get; set; }

        [field: Space(SMALL_SPACE)]

        [field: SerializeField] public Slider RedSlider { get; set; }
        [field: SerializeField] public Slider GreenSlider { get; set; }
        [field: SerializeField] public Slider BlueSlider { get; set; }

        [field: Space(SMALL_SPACE)]

        [field: SerializeField] public TextMeshProUGUI RedValueLabel { get; set; }
        [field: SerializeField] public TextMeshProUGUI GreenValueLabel { get; set; }
        [field: SerializeField] public TextMeshProUGUI BlueValueLabel { get; set; }


        private Toggle[] colorToggles;
        protected virtual Toggle[] ColorToggles {
            get {
                if (colorToggles == null)
                    colorToggles = ColorTogglesParent.GetComponentsInChildren<Toggle>();

                return colorToggles;
            }
        }
        private Toggle[] sectionIcons;
        protected virtual Toggle[] SectionIcons {
            get {
                if (sectionIcons == null)
                    sectionIcons = SectionIconsParent.GetComponentsInChildren<Toggle>();

                return sectionIcons;
            }
        }



        protected virtual string RedHexString { get; set; } = "00";
        protected virtual string GreenHexString { get; set; } = "00";
        protected virtual string BlueHexString { get; set; } = "00";
        protected virtual string ColorHexString => RedHexString + GreenHexString + BlueHexString;

        protected string FloatToHex(float number) => ByteIntToHex(FloatToByteInt(number));
        protected int FloatToByteInt(float number) => (int)(number * 255);
        protected string ByteIntToHex(int number) => number.ToString("X").PadLeft(2, '0');
        protected float HexToFloat(string hex) => Convert.ToInt32(hex, 16) / 255.0f;


        protected virtual void Start()
        {
            InitializeColorUI();

            AddListeners();
        }

        /**
         * Referencing methods in the Unity editor is a mess, so I much prefer adding listeners this way. 
         * It makes it much easier to find what methods are actually used and where, as well as it not 
         * forcing methods to be public.
         */
        protected virtual void AddListeners()
        {
            HexColorField.onValueChanged.AddListener(SetHexColor);

            RedSlider.onValueChanged.AddListener(SetRedValue);
            BlueSlider.onValueChanged.AddListener(SetBlueValue);
            GreenSlider.onValueChanged.AddListener(SetGreenValue);

            CancelButton.onClick.AddListener(Cancel);
            CreateButton.onClick.AddListener(AddSection);
        }

        /**
         * Called when selecting a premade color. Updates the color sliders to match
         */
        protected virtual void InitializeColorUI()
        {
            foreach (Toggle colorToggle in ColorToggles) {
                if (!colorToggle.isOn)
                    continue;

                var color = colorToggle.GetComponent<Image>().color;
                //Set the slider values to match the section's current color
                RedSlider.value = color.r;
                GreenSlider.value = color.g;
                BlueSlider.value = color.b;

                RedHexString = FloatToHex(color.r);
                GreenHexString = FloatToHex(color.g);
                BlueHexString = FloatToHex(color.b);

                HexColorField.text = ColorHexString;
            }
        }

        protected virtual void SetHexColor(string hexColor)
        {
            //Set the slider values to match the section's current color
            RedHexString = HexColorField.text.Substring(0, 2);
            GreenHexString = HexColorField.text.Substring(2, 2);
            BlueHexString = HexColorField.text.Substring(4, 2);

            RedSlider.value = HexToFloat(RedHexString);
            GreenSlider.value = HexToFloat(GreenHexString);
            BlueSlider.value = HexToFloat(BlueHexString);
        }

        /// <summary>
        /// Sets the red component of the custom color to a passed value.
        /// </summary>
        /// <param name="value">A float representation of the red value between 0 and 1 (inclusive)</param>
        protected virtual void SetRedValue(float value)
        {
            var color = ColorImage.color;
            color.r = value;
            ColorImage.color = color;

            int byteInt = FloatToByteInt(value);
            RedValueLabel.text = byteInt.ToString();

            RedHexString = ByteIntToHex(byteInt);
            HexColorField.text = ColorHexString;
        }

        /// <summary>
        /// Sets the green component of the custom color to a passed value.
        /// </summary>
        /// <param name="value">A float representation of the green value between 0 and 1 (inclusive)</param>
        protected virtual void SetGreenValue(float value)
        {
            var color = ColorImage.color;
            color.g = value;
            ColorImage.color = color;

            int byteInt = FloatToByteInt(value);
            GreenValueLabel.text = byteInt.ToString();

            GreenHexString = ByteIntToHex(byteInt);
            HexColorField.text = ColorHexString;
        }

        /// <summary>
        /// Sets the blue component of the custom color to a passed value.
        /// </summary>
        /// <param name="value">A float representation of the blue value between 0 and 1 (inclusive)</param>
        protected virtual void SetBlueValue(float value)
        {
            var color = ColorImage.color;
            color.b = value;
            ColorImage.color = color;

            int byteInt = FloatToByteInt(value);
            BlueValueLabel.text = byteInt.ToString();

            BlueHexString = ByteIntToHex(byteInt);
            HexColorField.text = ColorHexString;
        }

        /**
         * Adds a new section from the SectionCreatorBG
         * Pass in the display name
         */
        protected virtual void AddSection()
        {
            //Set the section button's icon as the user's choice
            var icon = GetSectionIcon();
            if (icon == null) {
                Debug.LogError("No icon selected");
                return;
            }

            var color = GetSectionColor();
            //var sectionKey = CreateSection(SectionNameField.text, color, icon);

            //TabManager.Instance.SwitchSection(sectionKey);

            //This script is parented to the add section panel, so let's disable it
            gameObject.SetActive(false);
        }

        /**
         * Gets the section color based on the selected color option.
         */
        protected virtual Color GetSectionColor()
        {
            foreach (Toggle colorToggle in ColorToggles) {
                if (colorToggle.isOn) {
                    return colorToggle.GetComponent<Image>().color;
                }
            }

            return Color.white;
        }

        protected virtual string IconName => "Icon";
        /**
         * Gets the icon image based on the selected section icon.
         */
        protected virtual Image GetSectionIcon()
        {
            foreach (Toggle sectionIcon in SectionIcons) {
                if (sectionIcon != null && sectionIcon.isOn) {
                    return sectionIcon.transform.Find(IconName).GetComponent<Image>();
                }
            }

            return null;
        }



        protected virtual void Cancel()
        {
            Destroy(gameObject);
        }
    }
}