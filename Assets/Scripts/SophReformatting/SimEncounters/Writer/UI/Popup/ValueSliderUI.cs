using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class ValueSliderUI : MonoBehaviour
    {
        [field: SerializeField] public Slider Slider { get; set; }
        [field: SerializeField] public TextMeshProUGUI ValueLabel { get; set; }
    }
}