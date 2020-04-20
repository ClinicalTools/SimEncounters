using ClinicalTools.SimEncounters.Data;
using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderEncounterInfoUI : UserEncounterDrawer
    {
        [SerializeField] private TextMeshProUGUI encounterTitle;
        public virtual TextMeshProUGUI EncounterTitle { get => encounterTitle; set => encounterTitle = value; }

        [SerializeField] private ReaderInfoPopupUI infoPopup;
        public virtual ReaderInfoPopupUI InfoPopup { get => infoPopup; set => infoPopup = value; }

        public override void Display(UserEncounter userEncounter)
        {
            InfoPopup.Display(userEncounter.Metadata);
            EncounterTitle.text = userEncounter.Metadata.Title;
        }
    }
}