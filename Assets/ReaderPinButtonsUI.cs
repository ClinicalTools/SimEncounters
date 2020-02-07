using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPinButtonsUI : MonoBehaviour
    {
        [SerializeField] private ReaderPinButtonUI dialogueButtonPrefab;
        public virtual ReaderPinButtonUI DialogueButtonPrefab { get => dialogueButtonPrefab; set => dialogueButtonPrefab = value; }

        [SerializeField] private ReaderPinButtonUI quizButtonPrefab;
        public virtual ReaderPinButtonUI QuizButtonPrefab { get => quizButtonPrefab; set => quizButtonPrefab = value; }

        [SerializeField] private Transform buttonsParent;
        public virtual Transform ButtonsParent { get => buttonsParent; set => buttonsParent = value; }
    }
}
