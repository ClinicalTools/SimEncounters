using ClinicalTools.SimEncounters;
using ClinicalTools.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.ClinicalEncounters
{
    public class CEBetterCompletionPopup : BaseCompletionPopup
    {
        public List<Button> CloseButtons { get => closeButtons; set => closeButtons = value; }
        [SerializeField] private List<Button> closeButtons;
        public ReaderOpenMenuButton MenuButton { get => menuButton; set => menuButton = value; }
        [SerializeField] private ReaderOpenMenuButton menuButton;
        public TextMeshProUGUI UrlLabel { get => urlLabel; set => urlLabel = value; }
        [SerializeField] private TextMeshProUGUI urlLabel;
        public Button UrlButton { get => urlButton; set => urlButton = value; }
        [SerializeField] private Button urlButton;
        public TMP_InputField CompletionCodeLabel { get => completionCodeLabel; set => completionCodeLabel = value; }
        [SerializeField] private TMP_InputField completionCodeLabel;
        public Button CopyCodeButton { get => copyCodeButton; set => copyCodeButton = value; }
        [SerializeField] private Button copyCodeButton;
        public Tooltip CopiedTooltip { get => copiedTooltip; set => copiedTooltip = value; }
        [SerializeField] private Tooltip copiedTooltip;

        public override event Action ExitScene { add { } remove { } }

        protected virtual void Awake()
        {
            MenuButton.gameObject.SetActive(true);
            foreach (var closeButton in CloseButtons)
                closeButton.onClick.AddListener(Hide);
            if (UrlButton != null)
                UrlButton.onClick.AddListener(StartUrl);
            CopyCodeButton.onClick.AddListener(CopyCode);
        }

        protected CEEncounterMetadata CurrentMetadata { get; set; }
        public override void CompletionDraw(ReaderSceneInfo sceneInfo)
        {
            MenuButton.Display(sceneInfo.User, sceneInfo.LoadingScreen);
            var encounter = sceneInfo.Encounter.Data;

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
            var textEditor = new TextEditor {
                text = CurrentMetadata.CompletionCode
            };
            textEditor.SelectAll();
            textEditor.Copy();

            CopiedTooltip.Show();
        }
    }
}