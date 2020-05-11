using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterDialoguePopup : MonoBehaviour
    {
        public Button CancelButton { get => cancelButton; set => cancelButton = value; }
        [SerializeField] private Button cancelButton;
        public Button RemoveButton { get => removeButton; set => removeButton = value; }
        [SerializeField] private Button removeButton;
        public Button ApplyButton { get => applyButton; set => applyButton = value; }
        [SerializeField] private Button applyButton;

        public BaseWriterPanelsDrawer ConversationDrawer { get => conversationDrawer; set => conversationDrawer = value; }
        [SerializeField] private BaseWriterPanelsDrawer conversationDrawer;

        protected BaseConfirmationPopup ConfirmationPopup { get; set; }
        [Inject] public virtual void Inject(BaseConfirmationPopup confirmationPopup) => ConfirmationPopup = confirmationPopup;

        protected virtual void Awake()
        {
            CancelButton.onClick.AddListener(Close);
            RemoveButton.onClick.AddListener(ConfirmRemove);
            ApplyButton.onClick.AddListener(Apply);
        }

        protected WaitableResult<DialoguePin> CurrentWaitableDialogue { get; set; }
        protected DialoguePin CurrentDialogue { get; set; }
        public virtual WaitableResult<DialoguePin> EditDialogue(Encounter encounter, DialoguePin dialoguePin)
        {
            CurrentDialogue = dialoguePin;

            if (CurrentWaitableDialogue?.IsCompleted == false)
                CurrentWaitableDialogue.SetError("New popup opened");

            CurrentWaitableDialogue = new WaitableResult<DialoguePin>();

            gameObject.SetActive(true);

            ConversationDrawer.DrawChildPanels(encounter, dialoguePin.Conversation);

            return CurrentWaitableDialogue;
        }
        protected virtual void Apply()
        {
            CurrentDialogue.Conversation = ConversationDrawer.SerializeChildren();
            CurrentWaitableDialogue.SetResult(CurrentDialogue);

            Close();
        }
        protected virtual void ConfirmRemove() => ConfirmationPopup.ShowConfirmation(Remove, "Confirm", "Yeet");
        protected virtual void Remove()
        {
            CurrentWaitableDialogue.SetResult(null);
            Close();
        }

        protected virtual void Close()
        {
            if (CurrentWaitableDialogue?.IsCompleted == false)
                CurrentWaitableDialogue.SetError("Canceled");

            gameObject.SetActive(false);
        }
    }
}