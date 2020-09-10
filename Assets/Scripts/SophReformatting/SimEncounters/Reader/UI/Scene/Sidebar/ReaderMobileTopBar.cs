using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileTopBar : ReaderBehaviour, IUserEncounterDrawer
    {
        public TMP_Text TitleLabel { get => titleLabel; set => titleLabel = value; }
        [SerializeField] private TMP_Text titleLabel;

        public void Display(UserEncounter userEncounter)
        {
            TitleLabel.text = userEncounter.Data.Metadata.Title;
        }
    }
}
