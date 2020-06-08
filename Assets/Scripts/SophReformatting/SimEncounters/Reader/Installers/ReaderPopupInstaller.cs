using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
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

        public override void InstallBindings()
        {
            Container.BindInstance(DialoguePopup);
            Container.BindInstance(QuizPopup);
            Container.BindInstance(PinButtonsPrefab);
            Container.BindInstance(ImagePopup);
            Container.BindInstance(ConfirmationPopup);

            Container.Bind<IDetailedStatusWriter>().To<DetailedStatusWriter>().AsTransient();
            Container.Bind<ServerDetailedStatusWriter>().To<ServerDetailedStatusWriter>().AsTransient();
            Container.Bind<FileDetailedStatusWriter>().To<FileDetailedStatusWriter>().AsTransient();

            Container.Bind<IReaderPanelDisplay>().To<ReaderPanelDisplay>().AsTransient();
            Container.Bind<FeedbackColorInfo>().To<FeedbackColorInfo>().AsTransient();
            Container.Bind<IParser<Color>>().To<ColorParser>().AsTransient();

            Container.Bind<EncounterStatusSerializer>().To<EncounterStatusSerializer>().AsTransient();
            Container.Bind<SectionStatusSerializer>().To<SectionStatusSerializer>().AsTransient();
            Container.Bind<TabStatusSerializer>().To<TabStatusSerializer>().AsTransient();

            Container.Bind<IParser<EncounterContentStatus>>().To<EncounterContentStatusParser>().AsTransient();
            Container.Bind<ICharEnumeratorParser<SectionStatus>>().To<SectionStatusParser>().AsTransient();
            Container.Bind<ICharEnumeratorParser<TabStatus>>().To<TabStatusParser>().AsTransient();
            Container.Bind<ICharEnumeratorParser<string>>().To<KeyParser>().AsTransient();
        }
    }
}