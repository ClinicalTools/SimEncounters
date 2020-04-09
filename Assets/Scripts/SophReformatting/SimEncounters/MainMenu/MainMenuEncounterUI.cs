using ClinicalTools.SimEncounters.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuEncounterUI : MonoBehaviour
    {
        public event Action<MenuEncounter> Selected;

        [SerializeField] private Button selectButton;
        public virtual Button SelectButton { get => selectButton; set => selectButton = value; }

        [SerializeField] private EncounterButtonsUI encounterButtons;
        public virtual EncounterButtonsUI EncounterButtons { get => encounterButtons; set => encounterButtons = value; }

        [SerializeField] private EncounterInfoUI infoViewer;
        public virtual EncounterInfoUI InfoViewer { get => infoViewer; set => infoViewer = value; }

        [SerializeField] private GameObject inProgressObject;
        public virtual GameObject InProgressObject { get => inProgressObject; set => inProgressObject = value; }

        [SerializeField] private GameObject completedObject;
        public virtual GameObject CompletedObject { get => completedObject; set => completedObject = value; }


        public void Display(MenuSceneInfo sceneInfo, MenuEncounter encounter)
        {
            if (InfoViewer != null)
                InfoViewer.Display(encounter.GetLatestMetadata());
            SelectButton.onClick.AddListener(() => Selected?.Invoke(encounter));

            if (encounter.Status != null) {
                if (encounter.Status.Completed)
                    CompletedObject.SetActive(true);
                else
                    InProgressObject.SetActive(true);
            }
        }
    }
}