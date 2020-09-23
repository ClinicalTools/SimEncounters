using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderMobileSideBottomBar : ReaderBehaviour, IReaderSceneDrawer, IUserEncounterDrawer
    {
        public ReaderOpenMenuButton OpenMenuButton { get => openMenuButton; set => openMenuButton = value; }
        [SerializeField] private ReaderOpenMenuButton openMenuButton;

        public void Display(LoadingReaderSceneInfo sceneInfo) => OpenMenuButton.Display(sceneInfo);
        public void Display(UserEncounter userEncounter) => OpenMenuButton.Display(userEncounter);
    }
}
