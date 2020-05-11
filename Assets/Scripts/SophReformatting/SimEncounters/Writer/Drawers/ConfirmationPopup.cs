using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class ConfirmationPopup : BaseConfirmationPopup
    {
        public TextMeshProUGUI Title { get => title; set => title = value; }
        [SerializeField] private TextMeshProUGUI title;
        public TextMeshProUGUI Description { get => description; set => description = value; }
        [SerializeField] private TextMeshProUGUI description;
        public Button CancelButton { get => cancelButton; set => cancelButton = value; }
        [SerializeField] private Button cancelButton;
        public Button ConfirmButton { get => confirmButton; set => confirmButton = value; }
        [SerializeField] private Button confirmButton;

        protected Action ConfirmationAction { get; set; }

        protected virtual void Awake()
        {
            CancelButton.onClick.AddListener(Close);
            ConfirmButton.onClick.AddListener(Confirm);
        }

        public override void ShowConfirmation(Action confirmAction, string title, string description)
        {
            ConfirmationAction = confirmAction;
            Title.text = title;
            Description.text = description;
            gameObject.SetActive(true);
        }

        protected virtual void Confirm()
        {
            ConfirmationAction?.Invoke();
            Close();
        }

        protected virtual void Close()
        {
            ConfirmationAction = null;
            gameObject.SetActive(false);
        }
    }
}