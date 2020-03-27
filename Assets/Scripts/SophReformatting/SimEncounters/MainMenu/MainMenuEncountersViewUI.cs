using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuEncountersViewUI : MonoBehaviour
    {
        public GameObject GameObject => gameObject;
        
        [SerializeField] private string viewName;
        public string ViewName { get => viewName; set => viewName = value; }

        [SerializeField] private Sprite viewSprite;
        public Sprite ViewSprite { get => viewSprite; set => viewSprite = value; }

        [SerializeField] private Button newCaseButton;
        public Button NewCaseButton { get => newCaseButton; set => newCaseButton = value; }

        [SerializeField] private Transform optionsParent;
        public Transform OptionsParent { get => optionsParent; set => optionsParent = value; }

        [SerializeField] private MainMenuEncounterUI optionPrefab;
        public MainMenuEncounterUI OptionPrefab { get => optionPrefab; set => optionPrefab = value; }

        [SerializeField] private GameObject moreComingSoonObject;
        public GameObject MoreComingSoonObject { get => moreComingSoonObject; set => moreComingSoonObject = value; }

        public event Action<EncounterInfo> Selected;
        protected InfoNeededForMainMenuToHappen CurrentData { get; set; }
        public virtual void Display(InfoNeededForMainMenuToHappen data, IEnumerable<EncounterInfo> encounters)
        {
            gameObject.SetActive(true);
            CurrentData = data;

            SetCases(encounters);
        }

        public void Hide()
        {
            foreach (MainMenuEncounterUI encounterDisplay in EncounterDisplays)
                Destroy(encounterDisplay.gameObject);
            EncounterDisplays.Clear();

            gameObject.SetActive(false);
        }

        public void ShowMoreComingSoon()
        {
            MoreComingSoonObject.transform.SetSiblingIndex(transform.childCount - 1);
            MoreComingSoonObject.SetActive(true);
        }
        public void HideMoreComingSoon()
        {
            MoreComingSoonObject.SetActive(false);
        }

        protected List<MainMenuEncounterUI> EncounterDisplays { get; } = new List<MainMenuEncounterUI>();
        public virtual void SetCases(IEnumerable<EncounterInfo> encounters)
        {
            foreach (MainMenuEncounterUI encounterDisplay in EncounterDisplays)
                Destroy(encounterDisplay.gameObject);
            EncounterDisplays.Clear();

            foreach (var encounter in encounters) {
                if (encounter.MetaGroup.GetLatestInfo().IsTemplate)
                    continue;

                var encounterUI = Instantiate(OptionPrefab, OptionsParent);
                encounterUI.Selected += (selectedEncounter) => Selected?.Invoke(selectedEncounter);
                encounterUI.Display(CurrentData, encounter);

                EncounterDisplays.Add(encounterUI);
            }
        }
    }
}