using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Reader;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



namespace ClinicalTools.ClinicalEncounters.Reader
{
    public class CECompletionPopup : BaseCompletionPopup
    {
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }
        [SerializeField] private List<Button> closeButtons;
        public Button MenuButton { get => menuButton; set => menuButton = value; }
        [SerializeField] private Button menuButton;
        public TextMeshProUGUI UrlLabel { get => urlLabel; set => urlLabel = value; }
        [SerializeField] private TextMeshProUGUI urlLabel;
        public Button UrlButton { get => urlButton; set => urlButton = value; }
        [SerializeField] private Button urlButton;
        public TMP_InputField CompletionCodeLabel { get => completionCodeLabel; set => completionCodeLabel = value; }
        [SerializeField] private TMP_InputField completionCodeLabel;

        public override event Action ReturnToMenu;
        
        protected virtual void Awake()
        {
            MenuButton.onClick.AddListener(() => ReturnToMenu?.Invoke());
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(Hide);
            UrlButton.onClick.AddListener(StartUrl);
        }

        protected string Url { get; set; }
        public override void Display(Encounter encounter)
        {
            gameObject.SetActive(true);
            if (!(encounter.Metadata is CEEncounterMetadata ceMetadata))
                return;
            Url = ceMetadata.Url;
            UrlLabel.text = ceMetadata.Url;
            CompletionCodeLabel.text = ceMetadata.CompletionCode;
        }

        protected virtual void StartUrl()
        {
            if (string.IsNullOrWhiteSpace(Url))
                return;

            Application.OpenURL(Url);
        }

        protected virtual void Hide() => gameObject.SetActive(false);
    }


}