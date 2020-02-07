using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabToggleUI : MonoBehaviour
    {
        [SerializeField] private Toggle selectToggle;
        public Toggle SelectToggle { get => selectToggle; set => selectToggle = value; }

        [SerializeField] private TextMeshProUGUI nameLabel;
        public TextMeshProUGUI NameLabel { get => nameLabel; set => nameLabel = value; }
    }
}