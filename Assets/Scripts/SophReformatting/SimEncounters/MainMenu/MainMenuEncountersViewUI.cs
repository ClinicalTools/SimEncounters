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



        public event Action<EncounterDetail> Selected;
        protected InfoNeededForMainMenuToHappen CurrentData { get; set; }
        public virtual void Display(InfoNeededForMainMenuToHappen data, List<EncounterDetail> encounters)
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

        protected List<MainMenuEncounterUI> EncounterDisplays { get; } = new List<MainMenuEncounterUI>();
        public virtual void SetCases(List<EncounterDetail> encounters)
        {
            foreach (MainMenuEncounterUI encounterDisplay in EncounterDisplays)
                Destroy(encounterDisplay.gameObject);
            EncounterDisplays.Clear();

            foreach (var encounter in encounters) {
                if (encounter.InfoGroup.GetLatestInfo().IsTemplate)
                    continue;

                var encounterUI = Instantiate(OptionPrefab, OptionsParent);
                encounterUI.Selected += (selectedEncounter) => Selected?.Invoke(selectedEncounter);
                encounterUI.Display(CurrentData, encounter);

                EncounterDisplays.Add(encounterUI);
            }
        }
    }
}