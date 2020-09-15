using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileSideTopBar : ReaderBehaviour, IUserEncounterDrawer
    {
        public ShowEncounterInfoButton InfoButton { get => infoButton; set => infoButton = value; }
        [SerializeField] private ShowEncounterInfoButton infoButton;
        public TMP_Text TitleLabel { get => titleLabel; set => titleLabel = value; }
        [SerializeField] private TMP_Text titleLabel;

        public void Display(UserEncounter userEncounter)
        {
            InfoButton.Display(userEncounter);
            TitleLabel.text = userEncounter.Data.Metadata.Title;
        }
    }
}
