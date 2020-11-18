using ClinicalTools.UI;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterPopupInstaller : MonoInstaller
    {
        public WriterDialoguePopup DialoguePopup { get => dialoguePopup; set => dialoguePopup = value; }
        [SerializeField] private WriterDialoguePopup dialoguePopup;
        public WriterQuizPopup QuizPopup { get => quizPopup; set => quizPopup = value; }
        [SerializeField] private WriterQuizPopup quizPopup;
        public SectionEditorPopup SectionEditorPopup { get => sectionEditorPopup; set => sectionEditorPopup = value; }
        [SerializeField] private SectionEditorPopup sectionEditorPopup;
        public TabEditorPopup TabEditorPopup { get => tabEditorPopup; set => tabEditorPopup = value; }
        [SerializeField] private TabEditorPopup tabEditorPopup;
        public BaseConfirmationPopup ConfirmationPopup { get => confirmationPopup; set => confirmationPopup = value; }
        [SerializeField] private BaseConfirmationPopup confirmationPopup;
        public BaseSpriteSelector SpriteSelector { get => spriteSelector; set => spriteSelector = value; }
        [SerializeField] private BaseSpriteSelector spriteSelector;
        public BaseSpriteSelector PatientSpriteSelector { get => patientSpriteSelector; set => patientSpriteSelector = value; }
        [SerializeField] private BaseSpriteSelector patientSpriteSelector;
        public BaseMessageHandler MessageHandler { get => messageHandler; set => messageHandler = value; }
        [SerializeField] private BaseMessageHandler messageHandler;
        public SwipeManager SwipeManager { get => swipeManager; set => swipeManager = value; }
        [SerializeField] private SwipeManager swipeManager;

        public override void InstallBindings()
        {
            Container.BindInstance(DialoguePopup);
            Container.BindInstance(QuizPopup);
            Container.BindInstance(SectionEditorPopup);
            Container.BindInstance(TabEditorPopup);
            Container.BindInstance(ConfirmationPopup);
            Container.BindInstance(SpriteSelector).WhenNotInjectedInto<CEWriterEncounterDrawer>();
            Container.BindInstance(PatientSpriteSelector).WhenInjectedInto<CEWriterEncounterDrawer>();

            Container.BindInstance(SwipeManager);

            Container.BindInstance(MessageHandler);
        }
    }
}