using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderEncounterInfoUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI encounterTitle;
        public virtual TextMeshProUGUI EncounterTitle { get => encounterTitle; set => encounterTitle = value; }

        [SerializeField] private ReaderInfoPopupUI infoPopup;
        public virtual ReaderInfoPopupUI InfoPopup { get => infoPopup; set => infoPopup = value; }
    }
}