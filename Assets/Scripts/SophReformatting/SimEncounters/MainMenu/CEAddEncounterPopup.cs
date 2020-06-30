﻿using ClinicalTools.SimEncounters;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.MainMenu;
using ClinicalTools.SimEncounters.Writer;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ClinicalTools.ClinicalEncounters.MainMenu
{
    public class CEAddEncounterPopup : BaseAddEncounterPopup
    {
        public TMP_InputField FirstNameField { get => firstNameField; set => firstNameField = value; }
        [SerializeField] private TMP_InputField firstNameField;
        public TMP_InputField LastNameField { get => lastNameField; set => lastNameField = value; }
        [SerializeField] private TMP_InputField lastNameField;
        public TMP_InputField DescriptionField { get => descriptionField; set => descriptionField = value; }
        [SerializeField] private TMP_InputField descriptionField;

        public virtual Button StartCaseButton { get => startCaseButton; set => startCaseButton = value; }
        [SerializeField] private Button startCaseButton;
        public virtual Button CloseButton { get => closeButton; set => closeButton = value; }
        [SerializeField] private Button closeButton;

        protected IEncounterDataReaderSelector DataReaderSelector { get; set; }
        protected IWriterSceneStarter SceneStarter { get; set; }
        [Inject]
        protected virtual void Inject(IEncounterDataReaderSelector dataReaderSelector, IWriterSceneStarter sceneStarter)
        {
            DataReaderSelector = dataReaderSelector;
            SceneStarter = sceneStarter;
        }
        
        protected virtual void Awake()
        {
            StartCaseButton.onClick.AddListener(StartCase);
            CloseButton.onClick.AddListener(Close);
        }

        protected MenuSceneInfo SceneInfo { get; set; }
        protected CEEncounterMetadata CurrentMetadata { get; set; }
        protected WaitableResult<EncounterData> EncounterData { get; set; }
        public override void Display(MenuSceneInfo sceneInfo, MenuEncounter encounter)
        {
            SceneInfo = sceneInfo;
            var metadata = encounter.GetLatestTypedMetada();
            CurrentMetadata = new CEEncounterMetadata(metadata.Value);
            SetFields();
            var dataReader = DataReaderSelector.GetEncounterDataReader(metadata.Key);
            EncounterData = dataReader.GetEncounterData(sceneInfo.User, metadata.Value);
        }

        public override void Display(MenuSceneInfo sceneInfo)
        {
            SceneInfo = sceneInfo;
            CurrentMetadata = new CEEncounterMetadata();
            SetFields();
            var dataReader = DataReaderSelector.GetEncounterDataReader(SaveType.Default);
            EncounterData = dataReader.GetEncounterData(sceneInfo.User, CurrentMetadata);
        }

        protected virtual void SetFields()
        {
            gameObject.SetActive(true);
            FirstNameField.text = "";
            LastNameField.text = "";
            DescriptionField.text = "";
        }
        
        protected virtual void StartCase()
        {
            CurrentMetadata.AuthorAccountId = SceneInfo.User.AccountId;
            CurrentMetadata.FirstName = FirstNameField.text;
            CurrentMetadata.LastName = LastNameField.text;
            CurrentMetadata.Description = DescriptionField.text;
            var rand = new System.Random((int)DateTime.UtcNow.Ticks);
            CurrentMetadata.RecordNumber = rand.Next(100000, 1000000);
            CurrentMetadata.Filename = $"{CurrentMetadata.RecordNumber}{CurrentMetadata.FirstName} {CurrentMetadata.LastName}";
            var encounter = new WaitableResult<Encounter>();
            var writerInfo = new LoadingWriterSceneInfo(SceneInfo.User, SceneInfo.LoadingScreen, encounter);
            EncounterData.AddOnCompletedListener((result) => Something(encounter, result));
            SceneStarter.StartScene(writerInfo);
        }

        protected virtual void Something(WaitableResult<Encounter> encounter, WaitedResult<EncounterData> encounterData)
        {
            var result = new Encounter(CurrentMetadata, encounterData.Value.Content, encounterData.Value.ImageData);
            encounter.SetResult(result);
        }

        protected virtual void Close()
        {
            gameObject.SetActive(false);
        }
    }
}