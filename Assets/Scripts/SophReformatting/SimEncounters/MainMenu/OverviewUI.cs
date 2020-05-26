﻿using ClinicalTools.SimEncounters.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ReaderWriterSelector : MonoBehaviour
    {
        public virtual Button InfoViewer { get => infoViewer; set => infoViewer = value; }
        [SerializeField] private Button infoViewer;
        public virtual Button StartReader { get => startReader; set => startReader = value; }
        [SerializeField] private Button startReader;

    }

    public class OverviewUI : MonoBehaviour
    {
        public virtual EncounterInfoUI InfoViewer { get => infoViewer; set => infoViewer = value; }
        [SerializeField] private EncounterInfoUI infoViewer;
        public virtual EncounterButtonsUI EncounterButtons { get => encounterButtons; set => encounterButtons = value; }
        [SerializeField] private EncounterButtonsUI encounterButtons;
        public virtual DeleteDownloadHandler DeleteDownloadHandler { get => deleteDownloadHandler; set => deleteDownloadHandler = value; }
        [SerializeField] private DeleteDownloadHandler deleteDownloadHandler;
        public virtual List<Button> HideOverviewButtons { get => hideOverviewButtons; set => hideOverviewButtons = value; }
        [SerializeField] private List<Button> hideOverviewButtons;

        protected IReaderSceneStarter ReaderSceneStarter { get; set; }
        protected IUserEncounterReaderSelector EncounterReaderSelector { get; set; }
        [Inject]
        public virtual void Inject(IReaderSceneStarter readerSceneStarter, IUserEncounterReaderSelector encounterReaderSelector)
        {
            ReaderSceneStarter = readerSceneStarter;
            EncounterReaderSelector = encounterReaderSelector;
        }

        protected virtual void Awake()
        {
            foreach (var button in HideOverviewButtons)
                button.onClick.AddListener(() => gameObject.SetActive(false));
        }

        protected MenuEncounter CurrentEncounter { get; set; }
        public virtual void DisplayForRead(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            Display(sceneInfo, menuEncounter);

            if (EncounterButtons != null)
                EncounterButtons.DisplayForRead(sceneInfo, menuEncounter);
        }
        public virtual void DisplayForEdit(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            Display(sceneInfo, menuEncounter);

            if (EncounterButtons != null)
                EncounterButtons.DisplayForEdit(sceneInfo, menuEncounter);
        }

        protected virtual void Display(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            gameObject.SetActive(true);

            CurrentEncounter = menuEncounter;

            if (InfoViewer != null) {
                InfoViewer.Display(menuEncounter.GetLatestMetadata());
                if (menuEncounter.Status != null)
                    InfoViewer.YourRating.SetRating(menuEncounter.Status.Rating);
            }

            if (DeleteDownloadHandler != null)
                DeleteDownloadHandler.Display(sceneInfo, menuEncounter);
        }

        public virtual void ReadCase(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            if (menuEncounter.Status == null)
                menuEncounter.Status = new EncounterBasicStatus();

            var metadata = menuEncounter.GetLatestTypedMetada();
            IUserEncounterReader encounterReader = EncounterReaderSelector.GetUserEncounterReader(metadata.Key);

            var encounter = encounterReader.GetUserEncounter(sceneInfo.User, metadata.Value, menuEncounter.Status);
            var encounterSceneInfo = new LoadingReaderSceneInfo(sceneInfo.User, sceneInfo.LoadingScreen, encounter);
            ReaderSceneStarter.StartScene(encounterSceneInfo);
        }
    }
}