using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderEncounterInfo : BaseUserEncounterDrawer
    {
        public virtual TextMeshProUGUI EncounterTitle { get => encounterTitle; set => encounterTitle = value; }
        [SerializeField] private TextMeshProUGUI encounterTitle;
        public virtual ReaderEncounterInfoPopupUI InfoPopup { get => infoPopup; set => infoPopup = value; }
        [SerializeField] private ReaderEncounterInfoPopupUI infoPopup;

        public override void Display(UserEncounter userEncounter)
        {
            InfoPopup.ShowEncounterInfo(userEncounter);
            InfoPopup.gameObject.SetActive(false);
            EncounterTitle.text = userEncounter.Data.Metadata.Title;
        }
    }
}