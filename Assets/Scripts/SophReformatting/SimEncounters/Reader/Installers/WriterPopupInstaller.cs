using ClinicalTools.SimEncounters.Writer;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
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
        public override void InstallBindings()
        {
            Container.BindInstance(DialoguePopup);
            Container.BindInstance(QuizPopup);
            Container.BindInstance(ConfirmationPopup);
            Container.BindInstance(SectionEditorPopup);
            Container.BindInstance(TabEditorPopup);
            Container.Bind<IParser<Color>>().To<ColorParser>().AsTransient();
        }
    }
}