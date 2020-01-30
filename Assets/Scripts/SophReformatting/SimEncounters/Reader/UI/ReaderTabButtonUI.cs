using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderTabButtonUI : MonoBehaviour
    {
        [SerializeField] private Button selectButton;
        public Button SelectButton { get => selectButton; set => selectButton = value; }

        [SerializeField] private TextMeshProUGUI nameLabel;
        public TextMeshProUGUI NameLabel { get => nameLabel; set => nameLabel = value; }
    }
}