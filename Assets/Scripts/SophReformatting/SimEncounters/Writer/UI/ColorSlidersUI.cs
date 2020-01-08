using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class ColorSlidersUI : MonoBehaviour
    {
        [field: SerializeField] public ValueSliderUI Red { get; set; }
        [field: SerializeField] public ValueSliderUI Green { get; set; }
        [field: SerializeField] public ValueSliderUI Blue { get; set; }
    }
}