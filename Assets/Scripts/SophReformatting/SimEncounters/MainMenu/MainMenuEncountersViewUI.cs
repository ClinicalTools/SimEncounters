﻿using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public abstract class BaseViewEncounterSelector : BaseEncounterSelector
    {
        public abstract string ViewName { get; set; }
        public abstract Sprite ViewSprite { get; set; }
    }

    public class MainMenuEncountersViewUI : BaseViewEncounterSelector
    {
        public override string ViewName { get => viewName; set => viewName = value; }
        [SerializeField] private string viewName;
        public override Sprite ViewSprite { get => viewSprite; set => viewSprite = value; }
        [SerializeField] private Sprite viewSprite;
        public Button NewCaseButton { get => newCaseButton; set => newCaseButton = value; }
        [SerializeField] private Button newCaseButton;
        public Transform OptionsParent { get => optionsParent; set => optionsParent = value; }
        [SerializeField] private Transform optionsParent;
        public MainMenuEncounterUI OptionPrefab { get => optionPrefab; set => optionPrefab = value; }
        [SerializeField] private MainMenuEncounterUI optionPrefab;


        public override event Action<MenuEncounter> EncounterSelected;
        protected MenuSceneInfo CurrentSceneInfo { get; set; }
        public override void Display(MenuSceneInfo sceneInfo, IEnumerable<MenuEncounter> encounters)
        {
            gameObject.SetActive(true);
            CurrentSceneInfo = sceneInfo;

            foreach (MainMenuEncounterUI encounterDisplay in EncounterDisplays)
                Destroy(encounterDisplay.gameObject);
            EncounterDisplays.Clear();

            foreach (var encounter in encounters)
                SetEncounter(encounter);
        }

        public override void Hide()
        {
            foreach (var encounterDisplay in EncounterDisplays)
                Destroy(encounterDisplay.gameObject);
            EncounterDisplays.Clear();

            gameObject.SetActive(false);
        }

        protected List<MainMenuEncounterUI> EncounterDisplays { get; } = new List<MainMenuEncounterUI>();
        protected virtual void SetEncounter(MenuEncounter encounter)
        {
            var encounterUI = Instantiate(OptionPrefab, OptionsParent);
            encounterUI.Selected += (selectedEncounter) => EncounterSelected?.Invoke(selectedEncounter);
            encounterUI.DisplayForRead(CurrentSceneInfo, encounter);

            EncounterDisplays.Add(encounterUI);
        }

        public override void Initialize() { }
    }
}