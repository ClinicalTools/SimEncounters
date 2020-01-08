using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class TabCreatorUI : MonoBehaviour
    {
        [field: SerializeField] public Button CancelButton { get; set; }
        [field: SerializeField] public Button CreateButton { get; set; }

        [field: SerializeField] public TMP_InputField NameField { get; set; }

        [field: SerializeField] public ToggleGroup TabGroups { get; set; }
        [field: SerializeField] public ToggleGroup TabTypes { get; set; }

        [field: SerializeField] public TextMeshProUGUI DescriptionLabel { get; set; }

        [field: SerializeField] public TabTypeButtonUI TypeButtonPrefab { get; set; }
    }
}