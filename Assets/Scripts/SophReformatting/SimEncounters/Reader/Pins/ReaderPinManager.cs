using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPinManager
    {
        protected ReaderScene Reader { get; }
        protected ReaderPinsUI PinsUI { get; }
        public ReaderPinManager(ReaderScene reader, ReaderPinsUI pinsUI)
        {
            Reader = reader;
            PinsUI = pinsUI;
        }

        public ReaderPinsGroup CreateButtons(PinData pinData, Transform parent)
        {
            if (pinData == null || !pinData.HasPin())
                return null;
            return null;
            //var pinButtonsUI = Object.Instantiate(PinsUI.PinButtonsPrefab, parent);
            //return new ReaderPinsGroup(Reader, pinButtonsUI, pinData);
        }

        public ReaderDialoguePopup ShowDialogue(DialoguePin pin)
        {
            var popupUI = Reader.Popups.Open(PinsUI.DialoguePrefab);
            return new ReaderDialoguePopup(Reader, popupUI, pin);
        }

        public ReaderQuizPopup ShowQuiz(QuizPin pin)
        {
            var popupUI = Reader.Popups.Open(PinsUI.QuizPrefab);
            return new ReaderQuizPopup(Reader, popupUI, pin);
        }
    }
}