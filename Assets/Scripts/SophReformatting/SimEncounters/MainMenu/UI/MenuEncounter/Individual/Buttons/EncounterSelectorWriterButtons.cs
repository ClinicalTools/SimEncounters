﻿using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class EncounterSelectorWriterButtons : BaseEncounterSelectorButtons
    {
        public virtual Button EditButton { get => editButton; set => editButton = value; }
        [SerializeField] private Button editButton;
        public virtual Button CopyButton { get => copyButton; set => copyButton = value; }
        [SerializeField] private Button copyButton;
        public virtual bool IsTemplate { get => isTemplate; set => isTemplate = value; }
        [SerializeField] private bool isTemplate;

        protected IEncounterStarter EncounterStarter { get; set; }
        protected BaseAddEncounterPopup AddEncounterPopup { get; set; }
        protected virtual ISelectedListener<MenuSceneInfoSelectedEventArgs> SceneInfoSelectedListener { get; set; }
        protected virtual ISelectedListener<MenuEncounterSelectedEventArgs> MenuEncounterSelectedListener { get; set; }
        [Inject] protected virtual void Inject(
            IEncounterStarter encounterStarter,
            ISelectedListener<MenuSceneInfoSelectedEventArgs> sceneInfoSelectedListener,
            ISelectedListener<MenuEncounterSelectedEventArgs> menuEncounterSelectedListener,
            BaseAddEncounterPopup addEncounterPopup) {
            EncounterStarter = encounterStarter;
            AddEncounterPopup = addEncounterPopup;
            SceneInfoSelectedListener = sceneInfoSelectedListener;
            MenuEncounterSelectedListener = menuEncounterSelectedListener;
            MenuEncounterSelectedListener.AddSelectedListener(MenuEncounterSelected);
        }

        protected virtual void Awake()
        {
            if (EditButton != null)
                EditButton.onClick.AddListener(StartEncounter);
            if (CopyButton != null)
                CopyButton.onClick.AddListener(CopyEncounter);
        }

        protected MenuSceneInfo SceneInfo => SceneInfoSelectedListener.CurrentValue.SceneInfo;
        protected MenuEncounter MenuEncounter => MenuEncounterSelectedListener.CurrentValue.Encounter;

        protected virtual void MenuEncounterSelected(object sender, MenuEncounterSelectedEventArgs eventArgs)
        {
            var metadata = eventArgs.Encounter.GetLatestMetadata();
            if (eventArgs.SelectionType != EncounterSelectionType.Edit || IsTemplate != metadata.IsTemplate) {
                gameObject.SetActive(false);
                return;
            }

            gameObject.SetActive(true);

            if (EditButton != null)
                EditButton.gameObject.SetActive(metadata.AuthorAccountId == SceneInfo.User.AccountId);
            if (CopyButton != null)
                CopyButton.gameObject.SetActive(true);
        }

        public override void Hide() => gameObject.SetActive(false);

        public virtual void StartEncounter() => EncounterStarter.StartEncounter(SceneInfo, MenuEncounter);
        public virtual void CopyEncounter() => AddEncounterPopup.Display(SceneInfo, MenuEncounter);
    }
}