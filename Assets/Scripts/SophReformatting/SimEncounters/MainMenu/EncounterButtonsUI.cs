using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class EncounterButtonsUI : MonoBehaviour
    {
        public virtual GameObject ButtonGroup => gameObject;

        public virtual Button ReadButton { get => readButton; set => readButton = value; }
        [SerializeField] private Button readButton;

        public virtual TextMeshProUGUI ReadText { get => readText; set => readText = value; }
        [SerializeField] private TextMeshProUGUI readText;

        public virtual Button EditButton { get => editButton; set => editButton = value; }
        [SerializeField] private Button editButton;

        public virtual Button CopyButton { get => copyButton; set => copyButton = value; }
        [SerializeField] private Button copyButton;
    }
}