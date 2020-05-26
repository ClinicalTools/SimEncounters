using ClinicalTools.SimEncounters.Data;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class EncounterSelectorWriterButtons : BaseEncounterSelectorButtons
    {
        public override event Action Start;
        public override event Action Copy;

        public virtual Button EditButton { get => editButton; set => editButton = value; }
        [SerializeField] private Button editButton;
        public virtual Button CopyButton { get => copyButton; set => copyButton = value; }
        [SerializeField] private Button copyButton;

        protected virtual void Awake()
        {
            EditButton.onClick.AddListener(() => Start?.Invoke());
            CopyButton.onClick.AddListener(() => Copy?.Invoke());
        }
        public override void Display(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter)
        {
            gameObject.SetActive(true);

            var metadata = menuEncounter.GetLatestMetadata();
            if (EditButton != null)
                EditButton.gameObject.SetActive(metadata.AuthorAccountId == sceneInfo.User.AccountId);
            if (CopyButton != null)
                CopyButton.gameObject.SetActive(true);
        }

        public override void Hide() => gameObject.SetActive(false);
    }
}