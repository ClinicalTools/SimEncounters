using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class LabeledToggle : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        public Toggle Toggle { get => toggle; set => toggle = value; }

        [SerializeField] private TextMeshProUGUI label;
        public TextMeshProUGUI Label { get => label; set => label = value; }
    }
}