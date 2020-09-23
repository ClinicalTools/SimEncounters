using TMPro;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileMainTopBar : ReaderBehaviour, IUserEncounterDrawer, IReaderSceneDrawer
    {
        public ReaderOpenMenuButton OpenMenuButton { get => openMenuButton; set => openMenuButton = value; }
        [SerializeField] private ReaderOpenMenuButton openMenuButton;
        public ShowEncounterInfoButton InfoButton { get => infoButton; set => infoButton = value; }
        [SerializeField] private ShowEncounterInfoButton infoButton;
        public TMP_Text TitleLabel { get => titleLabel; set => titleLabel = value; }
        [SerializeField] private TMP_Text titleLabel;

        public void Display(LoadingReaderSceneInfo sceneInfo) => OpenMenuButton.Display(sceneInfo);

        public void Display(UserEncounter userEncounter)
        {
            OpenMenuButton.Display(userEncounter);
            InfoButton.Display(userEncounter);
            TitleLabel.text = userEncounter.Data.Metadata.Title;
        }
    }
}
