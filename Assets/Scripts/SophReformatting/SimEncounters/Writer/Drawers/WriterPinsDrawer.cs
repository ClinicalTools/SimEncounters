using ClinicalTools.SimEncounters.Data;
using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterPinsDrawer : BaseWriterPinsDrawer
    {
        public Button DialoguePinButton { get => dialoguePinButton; set => dialoguePinButton = value; }
        [SerializeField] private Button dialoguePinButton;
        public Button QuizPinButton { get => quizPinButton; set => quizPinButton = value; }
        [SerializeField] private Button quizPinButton;

        protected virtual WriterDialoguePopup DialoguePopup { get; set; }
        protected virtual WriterQuizPopup QuizPopup { get; set; }
        [Inject] public void Inject(WriterDialoguePopup dialoguePopup, WriterQuizPopup quizPopup)
        {
            DialoguePopup = dialoguePopup;
            QuizPopup = quizPopup;
        }

        protected virtual void Awake()
        {
            DialoguePinButton.onClick.AddListener(EditDialogue);
            QuizPinButton.onClick.AddListener(EditQuiz);
        }

        protected Encounter CurrentEncounter { get; set; }
        protected PinData CurrentPinData { get; set; }
        public override void Display(Encounter encounter, PinData pinData)
        {
            CurrentEncounter = encounter;
            CurrentPinData = pinData;

            QuizPinButton.image.color = GetButtonColor(CurrentPinData?.Quiz != null);
            DialoguePinButton.image.color = GetButtonColor(CurrentPinData?.Dialogue != null);
        }

        public override PinData Serialize() => CurrentPinData;

        protected virtual void EditDialogue()
        {
            DialoguePin dialogue;
            if (CurrentPinData.Dialogue != null)
                dialogue = CurrentPinData.Dialogue;
            else
                dialogue = new DialoguePin();

            var newDialogue = DialoguePopup.EditDialogue(CurrentEncounter, dialogue);
            newDialogue.AddOnCompletedListener((result) => SetDialogue(newDialogue));
        }

        protected virtual void SetDialogue(WaitableResult<DialoguePin> dialogue)
        {
            if (dialogue.IsError)
                return;

            CurrentPinData.Dialogue = dialogue.Result;
            DialoguePinButton.image.color = GetButtonColor(dialogue.Result != null);
        }

        protected virtual void EditQuiz()
        {
            QuizPin quiz;
            if (CurrentPinData.Quiz != null)
                quiz = CurrentPinData.Quiz;
            else
                quiz = new QuizPin();

            var newQuiz = QuizPopup.EditQuiz(CurrentEncounter, quiz);
            newQuiz.AddOnCompletedListener((result) => SetQuiz(newQuiz));
        }

        protected virtual void SetQuiz(WaitableResult<QuizPin> quiz)
        {
            if (quiz.IsError)
                return;

            CurrentPinData.Quiz = quiz.Result;
            QuizPinButton.image.color = GetButtonColor(quiz.Result != null);
        }

        protected Color OffColor { get; } = new Color(0.8823529f, 0.8823529f, 0.8823529f);
        protected Color OnColor { get; } = new Color(0.6603774f, 0.7452832f, 1f);
        protected Color GetButtonColor(bool isOn) => isOn ? OnColor : OffColor;
    }
}