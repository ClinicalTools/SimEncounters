﻿
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialoguePinButton : UserDialoguePinDrawer
    {
        [SerializeField] private Button button;
        public virtual Button Button { get => button; set => button = value; }

        protected UserDialoguePinDrawer DialoguePopup { get; set; }

        [Inject] public virtual void Inject(UserDialoguePinDrawer dialoguePopup)
        {
            DialoguePopup = dialoguePopup;
        }

        public override void Display(UserDialoguePin dialoguePin)
        {
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() => DialoguePopup.Display(dialoguePin));
        }
    }
}