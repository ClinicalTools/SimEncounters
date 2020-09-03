
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPopupInstaller : MonoInstaller
    {
        public UserDialoguePinDrawer DialoguePopup { get => dialoguePopup; set => dialoguePopup = value; }
        [SerializeField] private UserDialoguePinDrawer dialoguePopup;
        public UserQuizPinDrawer QuizPopup { get => quizPopup; set => quizPopup = value; }
        [SerializeField] private UserQuizPinDrawer quizPopup;
        public SpriteDrawer ImagePopup { get => imagePopup; set => imagePopup = value; }
        [SerializeField] private SpriteDrawer imagePopup;
        public BaseUserPinGroupDrawer PinButtonsPrefab { get => pinButtonsPrefab; set => pinButtonsPrefab = value; }
        [SerializeField] private BaseUserPinGroupDrawer pinButtonsPrefab;
        public BaseConfirmationPopup ConfirmationPopup { get => confirmationPopup; set => confirmationPopup = value; }
        [SerializeField] private BaseConfirmationPopup confirmationPopup;
        public AndroidBackButton BackButton { get => backButton; set => backButton = value; }
        [SerializeField] private AndroidBackButton backButton;

        public override void InstallBindings()
        {
            Container.BindInstance(DialoguePopup);
            Container.BindInstance(QuizPopup);
            Container.BindInstance(PinButtonsPrefab);
            Container.BindInstance(ImagePopup);
            Container.BindInstance(ConfirmationPopup);
            Container.BindInstance(BackButton);

            Container.Bind<IStatusWriter>().To<LocalStatusWriter>().AsTransient();

            Container.Bind<IReaderPanelDisplay>().To<ReaderPanelDisplay>().AsTransient();
            Container.Bind<FeedbackColorInfo>().To<FeedbackColorInfo>().AsTransient();
            Container.Bind<IStringDeserializer<Color>>().To<ColorDeserializer>().AsTransient();

            Container.Bind<EncounterContentStatusSerializer>().To<EncounterContentStatusSerializer>().AsTransient();
            Container.Bind<SectionStatusSerializer>().To<SectionStatusSerializer>().AsTransient();
            Container.Bind<TabStatusSerializer>().To<TabStatusSerializer>().AsTransient();

            Container.Bind<IStringDeserializer<EncounterContentStatus>>().To<EncounterContentStatusDeserializer>().AsTransient();
            Container.Bind<ICharEnumeratorDeserializer<SectionStatus>>().To<SectionStatusDeserializer>().AsTransient();
            Container.Bind<ICharEnumeratorDeserializer<TabStatus>>().To<TabStatusDeserializer>().AsTransient();
            Container.Bind<ICharEnumeratorDeserializer<string>>().To<KeyDeserializer>().AsTransient();
        }
    }
}