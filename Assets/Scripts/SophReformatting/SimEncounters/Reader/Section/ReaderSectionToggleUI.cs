using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderSectionToggleUI : MonoBehaviour
{
        [SerializeField] private Toggle selectToggle;
        public Toggle SelectToggle { get => selectToggle; set => selectToggle = value; }

        [SerializeField] private Image image;
        public Image Image { get => image; set => image = value; }

        [SerializeField] private Image icon;
        public Image Icon { get => icon; set => icon = value; }

        [SerializeField] private TextMeshProUGUI nameLabel;
        public TextMeshProUGUI NameLabel { get => nameLabel; set => nameLabel = value; }

        [SerializeField] private GameObject visited;
        public GameObject Visited { get => visited; set => visited = value; }
    }
}
