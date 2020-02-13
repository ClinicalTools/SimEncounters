using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPinsUI : MonoBehaviour
    {
        [SerializeField] private ReaderPinButtonsUI pinButtonsPrefab;
        public virtual ReaderPinButtonsUI PinButtonsPrefab { get => pinButtonsPrefab; set => pinButtonsPrefab = value; }

        [SerializeField] private ReaderDialoguePopupUI dialoguePrefab;
        public virtual ReaderDialoguePopupUI DialoguePrefab { get => dialoguePrefab; set => dialoguePrefab = value; }

        [SerializeField] private ReaderQuizPopupUI quizPrefab;
        public virtual ReaderQuizPopupUI QuizPrefab { get => quizPrefab; set => quizPrefab = value; }
    }
}