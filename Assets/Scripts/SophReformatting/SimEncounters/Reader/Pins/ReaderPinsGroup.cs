using ClinicalTools.SimEncounters.Data;
using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderPinsGroup
    {
        protected ReaderScene Reader { get; }
        protected ReaderPinButtonsUI PinButtonsUI { get; }
        protected PinData PinData { get; }
        public ReaderPinsGroup(ReaderScene reader, ReaderPinButtonsUI pinButtonsUI, PinData pinData)
        {
            Reader = reader;
            PinButtonsUI = pinButtonsUI;
            PinData = pinData;

            InitializePinButtons(pinData);
        }
        
        protected virtual void InitializePinButtons(PinData pinData)
        {
            InitializeDialoguePin(pinData.Dialogue);
            InitializeQuizPin(pinData.Quiz);
        }

        protected virtual void InitializeDialoguePin(DialoguePin pin)
        {
            if (pin == null)
                return;

            var button = Object.Instantiate(PinButtonsUI.DialogueButtonPrefab, PinButtonsUI.ButtonsParent);
            button.Button.onClick.AddListener(() => Reader.Pins.ShowDialogue(pin));
        }

        protected virtual void InitializeQuizPin(QuizPin pin)
        {
            if (pin == null)
                return;

            var button = Object.Instantiate(PinButtonsUI.QuizButtonPrefab, PinButtonsUI.ButtonsParent);
            button.Button.onClick.AddListener(() => Reader.Pins.ShowQuiz(pin));
        }
    }
}