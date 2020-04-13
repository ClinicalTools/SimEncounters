using ClinicalTools.ClinicalEncounters.Loader;
using ClinicalTools.SimEncounters.Collections;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Loader;
using ClinicalTools.SimEncounters.Loading;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class OverviewUI : MonoBehaviour
    {
        public GameObject GameObject => gameObject;

        [SerializeField] private EncounterInfoUI infoViewer;
        public virtual EncounterInfoUI InfoViewer { get => infoViewer; set => infoViewer = value; }

        [SerializeField] private EncounterButtonsUI encounterButtons;
        public virtual EncounterButtonsUI EncounterButtons { get => encounterButtons; set => encounterButtons = value; }

        [SerializeField] private EncounterButtonsUI templateButtons;
        public virtual EncounterButtonsUI TemplateButtons { get => templateButtons; set => templateButtons = value; }

        [SerializeField] private Button downloadButton;
        public virtual Button DownloadButton { get => downloadButton; set => downloadButton = value; }

        [SerializeField] private Button deleteButton;
        public virtual Button DeleteButton { get => deleteButton; set => deleteButton = value; }

        [SerializeField] private List<Button> hideOverviewButtons;
        public virtual List<Button> HideOverviewButtons { get => hideOverviewButtons; set => hideOverviewButtons = value; }

        [Inject]
        protected IEncounterReaderSelector EncounterReaderSelector { get; set; }

        protected virtual void Awake()
        {
            foreach (var button in HideOverviewButtons)
                button.onClick.AddListener(() => gameObject.SetActive(false));
        }

        protected MenuEncounter CurrentEncounter { get; set; }
        public virtual void Display(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            CurrentEncounter = menuEncounter;

            if (InfoViewer != null) {
                InfoViewer.Display(menuEncounter.GetLatestMetadata());
                if (menuEncounter.Status != null)
                    InfoViewer.YourRating.SetRating(menuEncounter.Status.Rating);
            }

            EncounterButtons.ReadButton.onClick.RemoveAllListeners();
            EncounterButtons.ReadButton.onClick.AddListener(() => ReadCase(sceneInfo, menuEncounter));
        
            SetReadTextButton(menuEncounter.Status);
        }

        public virtual void SetReadTextButton(EncounterBasicStatus basicStatus)
        {
            string text;

            if (basicStatus == null)
                text = "Start Case";
            else if (basicStatus.Completed)
                text = "Review Case";
            else
                text = "Continue Case";

            EncounterButtons.ReadText.text = text;
        }

        public virtual void ReadCase(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            if (menuEncounter.Status == null)
                menuEncounter.Status = new EncounterBasicStatus();

            var metadata = menuEncounter.GetLatestTypedMetada();
            KeyGenerator.ResetKeyGenerator(metadata.Value.RecordNumber);
            IFullEncounterReader encounterReader = EncounterReaderSelector.GetEncounterReader(metadata.Key);

            var encounter = encounterReader.GetFullEncounter(sceneInfo.User, metadata.Value, menuEncounter.Status);
            var encounterSceneInfo = new LoadingEncounterSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, encounter);
            EncounterSceneManager.EncounterInstance.StartReaderScene(encounterSceneInfo);
        }
    }
}