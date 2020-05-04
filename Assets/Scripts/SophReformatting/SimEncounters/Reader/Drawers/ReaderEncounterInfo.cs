using ClinicalTools.SimEncounters.Data;
using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderEncounterInfo : BaseUserEncounterDrawer
    {
        public virtual TextMeshProUGUI EncounterTitle { get => encounterTitle; set => encounterTitle = value; }
        [SerializeField] private TextMeshProUGUI encounterTitle;
        public virtual ReaderInfoPopupUI InfoPopup { get => infoPopup; set => infoPopup = value; }
        [SerializeField] private ReaderInfoPopupUI infoPopup;

        public override void Display(UserEncounter userEncounter)
        {
            InfoPopup.Display(userEncounter.Metadata);
            EncounterTitle.text = userEncounter.Metadata.Title;
        }
    }
}