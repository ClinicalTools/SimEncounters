﻿using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class EncounterSelectorWriterButtons : BaseEncounterSelectorButtons
    {
        public virtual Button EditButton { get => editButton; set => editButton = value; }
        [SerializeField] private Button editButton;
        public virtual Button CopyButton { get => copyButton; set => copyButton = value; }
        [SerializeField] private Button copyButton;

        protected IEncounterStarter EncounterStarter { get; set; }
        protected IEncounterCopier EncounterCopier { get; set; }
        [Inject] protected virtual void Inject(IEncounterStarter encounterStarter, IEncounterCopier encounterCopier) {
            EncounterStarter = encounterStarter;
            EncounterCopier = encounterCopier;
        }
        protected virtual void Awake()
        {
            if (EditButton != null)
                EditButton.onClick.AddListener(StartEncounter);
            if (CopyButton != null)
                CopyButton.onClick.AddListener(CopyEncounter);
        }
        protected MenuSceneInfo SceneInfo { get; set; }
        protected MenuEncounter MenuEncounter { get; set; }
        public override void Display(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            SceneInfo = sceneInfo;
            MenuEncounter = menuEncounter;

            gameObject.SetActive(true);

            var metadata = menuEncounter.GetLatestMetadata();
            if (EditButton != null)
                EditButton.gameObject.SetActive(metadata.AuthorAccountId == sceneInfo.User.AccountId);
            if (CopyButton != null)
                CopyButton.gameObject.SetActive(true);
        }

        public override void Hide() => gameObject.SetActive(false);

        public virtual void StartEncounter() => EncounterStarter.StartEncounter(SceneInfo, MenuEncounter);
        public virtual void CopyEncounter() => EncounterCopier.CopyEncounter(SceneInfo, MenuEncounter);
    }
}