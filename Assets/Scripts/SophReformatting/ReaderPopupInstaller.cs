using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPopupInstaller : MonoInstaller
    {
        [SerializeField] private UserDialoguePinDrawer dialoguePopup;
        [SerializeField] private UserQuizPinDrawer quizPopup;
        [SerializeField] private SpriteDrawer imagePopup;
        [SerializeField] private UserPinGroupDrawer pinButtonsPrefab;

        public static ReaderPopupInstaller Instance { get; set; }

        private void Awake()
        {
            Instance = this;
        }

        public override void InstallBindings()
        {
            Container.BindInstance(dialoguePopup);
            Container.BindInstance(quizPopup);
            Container.BindInstance(pinButtonsPrefab);
            Container.BindInstance(imagePopup);
            
            Container.Bind<BasicReaderPanelDrawer>().To<BasicReaderPanelDrawer>().AsTransient();
            Container.Bind<FeedbackColorInfo>().To<FeedbackColorInfo>().AsTransient();
        }
    }
}