using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class TabTypeButtonUI : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI Label { get; set; }
        [field: SerializeField] public Toggle Toggle { get; set; }
    }
}