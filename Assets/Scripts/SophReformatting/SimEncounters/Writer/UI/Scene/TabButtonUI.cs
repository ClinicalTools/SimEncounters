using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class TabButtonUI : MonoBehaviour
    {
        [SerializeField] private Button selectButton;
        public Button SelectButton { get => selectButton; set => selectButton = value; }

        [SerializeField] private Button editButton;
        public Button EditButton { get => editButton; set => editButton = value; }

        [SerializeField] private TextMeshProUGUI nameLabel;
        public TextMeshProUGUI NameLabel { get => nameLabel; set => nameLabel = value; }
    }
}