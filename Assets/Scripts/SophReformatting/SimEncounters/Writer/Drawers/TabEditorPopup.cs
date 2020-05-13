using ClinicalTools.SimEncounters.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.Writer
{
    public class TabEditorPopup : MonoBehaviour
    {
        public Button CancelButton { get => cancelButton; set => cancelButton = value; }
        [SerializeField] private Button cancelButton;
        public Button ApplyButton { get => applyButton; set => applyButton = value; }
        [SerializeField] private Button applyButton;
        public Button RemoveButton { get => removeButton; set => removeButton = value; }
        [SerializeField] private Button removeButton;

        public TMP_InputField NameField { get => nameField; set => nameField = value; }
        [SerializeField] private TMP_InputField nameField;

        protected BaseConfirmationPopup ConfirmationPopup { get; set; }
        [Inject] public virtual void Inject(BaseConfirmationPopup confirmationPopup) => ConfirmationPopup = confirmationPopup;

        protected virtual void Awake()
        {
            CancelButton.onClick.AddListener(Close);
            ApplyButton.onClick.AddListener(Apply);
            RemoveButton.onClick.AddListener(ConfirmRemove);
        }
        protected WaitableResult<Tab> CurrentWaitableTab { get; set; }
        protected Tab CurrentTab { get; set; }
        public virtual WaitableResult<Tab> EditTab(Encounter encounter, Tab tab)
        {
            CurrentTab = tab;

            gameObject.SetActive(true);

            NameField.text = tab.Name;

            if (CurrentWaitableTab?.IsCompleted == false)
                CurrentWaitableTab.SetError("New popup opened");
            CurrentWaitableTab = new WaitableResult<Tab>();
            return CurrentWaitableTab;
        }
        protected virtual void Apply()
        {
            CurrentTab.Name = NameField.text;

            CurrentWaitableTab.SetResult(CurrentTab);
            CurrentWaitableTab = null;

            Close();
        }

        protected virtual void ConfirmRemove() => ConfirmationPopup.ShowConfirmation(Remove, "Confirm", "Yeet");
        protected virtual void Remove()
        {
            CurrentWaitableTab.SetResult(null);
            Close();
        }

        protected virtual void Close()
        {
            if (CurrentWaitableTab?.IsCompleted == false)
                CurrentWaitableTab.SetError("Canceled");

            gameObject.SetActive(false);
        }
    }
}