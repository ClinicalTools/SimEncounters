using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class ColorUI : MonoBehaviour
    {
        [field: SerializeField] protected Transform ColorTogglesParent { get; set; }

        [field: SerializeField] public TMP_InputField HexColorField { get; set; }

        [field: SerializeField] public Toggle CustomColorToggle { get; set; }

        [field: SerializeField] public ColorSlidersUI Sliders { get; set; }


        private Toggle[] colorToggles;
        public virtual Toggle[] ColorToggles {
            get {
                if (colorToggles == null)
                    colorToggles = ColorTogglesParent.GetComponentsInChildren<Toggle>();

                return colorToggles;
            }
        }
    }
}