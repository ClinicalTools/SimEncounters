using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Reader;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.ClinicalEncounters.Reader
{
    public class TextCopier
    {
        public void CopyText(string text)
        {

        }
    }

    public class CECompletionPopup : BaseCompletionPopup
    {
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }
        [SerializeField] private List<Button> closeButtons;
        public Button ContinueButton { get => continueButton; set => continueButton = value; }
        [SerializeField] private Button continueButton;
        public Button MenuButton { get => menuButton; set => menuButton = value; }
        [SerializeField] private Button menuButton;
        public TextMeshProUGUI UrlLabel { get => urlLabel; set => urlLabel = value; }
        [SerializeField] private TextMeshProUGUI urlLabel;
        public Button UrlButton { get => urlButton; set => urlButton = value; }
        [SerializeField] private Button urlButton;
        public TMP_InputField CompletionCodeLabel { get => completionCodeLabel; set => completionCodeLabel = value; }
        [SerializeField] private TMP_InputField completionCodeLabel;
        public Button CopyCodeButton { get => copyCodeButton; set => copyCodeButton = value; }
        [SerializeField] private Button copyCodeButton;

        public override event Action ExitScene;

        protected virtual void Awake()
        {
#if !STANDALONE_SCENE
            MenuButton.gameObject.SetActive(true);
            MenuButton.onClick.AddListener(() => ExitScene?.Invoke());
            ContinueButton.gameObject.SetActive(false);
#else
            MenuButton.gameObject.SetActive(false);
            ContinueButton.gameObject.SetActive(true);
            ContinueButton.onClick.AddListener(Hide);
#endif
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(Hide);
            if (UrlButton != null)
                UrlButton.onClick.AddListener(StartUrl);
            CopyCodeButton.onClick.AddListener(CopyCode);
        }

        protected CEEncounterMetadata CurrentMetadata { get; set; }
        public override void Display(Encounter encounter)
        {
            gameObject.SetActive(true);
            if (!(encounter.Metadata is CEEncounterMetadata ceMetadata))
                return;
            CurrentMetadata = ceMetadata;
            if (UrlLabel != null)
                UrlLabel.text = ceMetadata.Url;
            CompletionCodeLabel.text = ceMetadata.CompletionCode;
        }

        protected virtual void StartUrl()
        {
            if (!string.IsNullOrWhiteSpace(CurrentMetadata.Url))
                Application.OpenURL(CurrentMetadata.Url);
        }

        protected virtual void Hide() => gameObject.SetActive(false);

        protected virtual void CopyCode()
        {
#if UNITY_WEBGL
            //WebGLCopyAndPasteAPI.PassCopyToBrowser(CurrentMetadata.CompletionCode);
#else
            var textEditor = new TextEditor {
                text = CurrentMetadata.CompletionCode
            };
            textEditor.SelectAll();
            textEditor.Copy();
#endif
        }
    }


}