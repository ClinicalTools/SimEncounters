using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderDialoguePopupUI : ReaderPopupUI
    {
        [SerializeField] private ReaderDialogueEntryUI dialogueEntryLeft;
        public ReaderDialogueEntryUI DialogueEntryLeft { get => dialogueEntryLeft; set => dialogueEntryLeft = value; }

        [SerializeField] private ReaderDialogueEntryUI dialogueEntryRight;
        public ReaderDialogueEntryUI DialogueEntryRight { get => dialogueEntryRight; set => dialogueEntryRight = value; }

        [SerializeField] private ReaderDialogueChoiceUI dialogueChoice;
        public ReaderDialogueChoiceUI DialogueChoice { get => dialogueChoice; set => dialogueChoice = value; }

        [SerializeField] private Transform panelsParent;
        public Transform PanelsParent { get => panelsParent; set => panelsParent = value; }
    }
}