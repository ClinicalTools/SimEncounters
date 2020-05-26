﻿using ClinicalTools.SimEncounters.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuEncountersViewUI : MonoBehaviour
    {
        public string ViewName { get => viewName; set => viewName = value; }
        [SerializeField] private string viewName;
        public Sprite ViewSprite { get => viewSprite; set => viewSprite = value; }
        [SerializeField] private Sprite viewSprite;
        public Button NewCaseButton { get => newCaseButton; set => newCaseButton = value; }
        [SerializeField] private Button newCaseButton;
        public Transform OptionsParent { get => optionsParent; set => optionsParent = value; }
        [SerializeField] private Transform optionsParent;
        public MainMenuEncounterUI OptionPrefab { get => optionPrefab; set => optionPrefab = value; }
        [SerializeField] private MainMenuEncounterUI optionPrefab;
        public GameObject MoreComingSoonObject { get => moreComingSoonObject; set => moreComingSoonObject = value; }
        [SerializeField] private GameObject moreComingSoonObject;


        public event Action<MenuEncounter> Selected;
        protected MenuSceneInfo CurrentSceneInfo { get; set; }
        public virtual void Display(MenuSceneInfo sceneInfo, IEnumerable<MenuEncounter> encounters)
        {
            gameObject.SetActive(true);
            CurrentSceneInfo = sceneInfo;

            SetEncounters(encounters);
        }

        public void Hide()
        {
            foreach (var encounterDisplay in EncounterDisplays)
                Destroy(encounterDisplay.gameObject);
            EncounterDisplays.Clear();

            gameObject.SetActive(false);
        }

        public void ShowMoreComingSoon()
        {
            if (MoreComingSoonObject == null)
                return;

            MoreComingSoonObject.transform.SetSiblingIndex(transform.childCount - 1);
            MoreComingSoonObject.SetActive(true);
        }
        public void HideMoreComingSoon()
        {
            if (MoreComingSoonObject != null)
                MoreComingSoonObject.SetActive(false);
        }

        protected List<MainMenuEncounterUI> EncounterDisplays { get; } = new List<MainMenuEncounterUI>();
        public virtual void SetEncounters(IEnumerable<MenuEncounter> encounters)
        {
            foreach (MainMenuEncounterUI encounterDisplay in EncounterDisplays)
                Destroy(encounterDisplay.gameObject);
            EncounterDisplays.Clear();

            foreach (var encounter in encounters) {
                if (encounter.GetLatestMetadata().IsTemplate)
                    continue;

                var encounterUI = Instantiate(OptionPrefab, OptionsParent);
                encounterUI.Selected += (selectedEncounter) => Selected?.Invoke(selectedEncounter);
                encounterUI.Display(CurrentSceneInfo, encounter);

                EncounterDisplays.Add(encounterUI);
            }
        }
    }
}