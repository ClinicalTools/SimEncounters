using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPinManager
    {
        protected EncounterReader Reader { get; }
        protected ReaderPinsUI PinsUI { get; }
        public ReaderPinManager(EncounterReader reader, ReaderPinsUI pinsUI)
        {
            Reader = reader;
            PinsUI = pinsUI;
        }

        public ReaderPinsGroup CreateButtons(PinData pinData, Transform parent)
        {
            if (pinData == null || !pinData.HasPin())
                return null;
            var pinButtonsUI = Object.Instantiate(PinsUI.PinButtonsPrefab, parent);
            return new ReaderPinsGroup(Reader, pinButtonsUI, pinData);
        }

        public ReaderDialoguePopup ShowDialogue(DialoguePin pin)
        {
            var popupUI = Reader.OpenPopup(PinsUI.DialoguePrefab);
            return new ReaderDialoguePopup(Reader, popupUI, pin);
        }

        public ReaderQuizPopup ShowQuiz(QuizPin pin)
        {
            var popupUI = Reader.OpenPopup(PinsUI.QuizPrefab);
            return new ReaderQuizPopup(Reader, popupUI, pin);
        }
    }
}