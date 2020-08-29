using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ConfirmationPopup : BaseConfirmationPopup
    {
        public TextMeshProUGUI Title { get => title; set => title = value; }
        [SerializeField] private TextMeshProUGUI title;
        public TextMeshProUGUI Description { get => description; set => description = value; }
        [SerializeField] private TextMeshProUGUI description;
        public List<Button> CancelButtons { get => cancelButtons; set => cancelButtons = value; }
        [SerializeField] private List<Button> cancelButtons;
        public Button ConfirmButton { get => confirmButton; set => confirmButton = value; }
        [SerializeField] private Button confirmButton;

#if MOBILE
        protected AndroidBackButton BackButton { get; set; }
        [Inject]
        public virtual void Inject(AndroidBackButton backButton)
            => BackButton = backButton;
#endif

        protected Action ConfirmationAction { get; set; }
        protected Action CancellationAction { get; set; }


        protected virtual void Awake()
        {
            foreach (var cancelButton in CancelButtons)
                cancelButton.onClick.AddListener(Cancel);
            ConfirmButton.onClick.AddListener(Confirm);
        }

        public override void ShowConfirmation(Action confirmAction, string title, string description)
            => ShowConfirmation(confirmAction, null, title, description);
        public override void ShowConfirmation(Action confirmAction, Action cancelAction, string title, string description)
        {
            ConfirmationAction = confirmAction;
            CancellationAction = cancelAction;
            Title.text = title;
            Description.text = description;
            gameObject.SetActive(true);
#if MOBILE
            BackButton.Register(Cancel);
#endif
        }

        protected virtual void Confirm()
        {
            ConfirmationAction?.Invoke();
            Close();
        }

        protected virtual void Cancel()
        {
            CancellationAction?.Invoke();
            Close();
        }

        protected virtual void Close()
        {
            ConfirmationAction = null;
            CancellationAction = null;
            gameObject.SetActive(false);
#if MOBILE
            BackButton.Deregister(Cancel);
#endif
        }
    }
}