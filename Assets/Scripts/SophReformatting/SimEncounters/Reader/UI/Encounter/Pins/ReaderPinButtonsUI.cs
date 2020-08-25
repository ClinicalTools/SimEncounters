using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class ReaderPinButtonsUI : BaseUserPinGroupDrawer
    {
        public virtual UserDialoguePinDrawer DialogueButtonPrefab { get => dialogueButtonPrefab; set => dialogueButtonPrefab = value; }
        [SerializeField] private UserDialoguePinDrawer dialogueButtonPrefab;
        public virtual ReaderQuizPinButton QuizButtonPrefab { get => quizButtonPrefab; set => quizButtonPrefab = value; }
        [SerializeField] private ReaderQuizPinButton quizButtonPrefab;
        public virtual Transform ButtonsParent { get => buttonsParent; set => buttonsParent = value; }
        [SerializeField] private Transform buttonsParent;

        public override void Display(UserPinGroup pinGroup)
        {
            DisplayDialoguePin(pinGroup.DialoguePin);
            DisplayQuizPin(pinGroup.QuizPin);
        }
        protected virtual void DisplayDialoguePin(UserDialoguePin dialoguePin)
        {
            if (dialoguePin == null)
                return;
            var dialogueButton = Instantiate(DialogueButtonPrefab, ButtonsParent);
            dialogueButton.Display(dialoguePin);
        }
        protected virtual void DisplayQuizPin(UserQuizPin quizPin)
        {
            if (quizPin == null)
                return;
            var quizButton = Instantiate(QuizButtonPrefab, ButtonsParent);
            quizButton.Display(quizPin);
        }
    }
}