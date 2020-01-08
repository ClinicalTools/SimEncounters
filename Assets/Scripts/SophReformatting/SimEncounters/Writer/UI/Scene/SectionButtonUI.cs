using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class SectionButtonUI : MonoBehaviour
    {
        [field: SerializeField] public Button SelectButton { get; set; }
        [field: SerializeField] public Button EditButton { get; set; }
        [field: SerializeField] public Image Image { get; set; }
        [field: SerializeField] public Image Icon { get; set; }
        [field: SerializeField] public TextMeshProUGUI NameLabel { get; set; }
    }
}