using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuEncounterUI : MonoBehaviour
    {
        public event Action<MenuEncounter> Selected;

        public virtual Button SelectButton { get => selectButton; set => selectButton = value; }
        [SerializeField] private Button selectButton;
        public virtual EncounterButtonsUI EncounterButtons { get => encounterButtons; set => encounterButtons = value; }
        [SerializeField] private EncounterButtonsUI encounterButtons;
        public virtual EncounterInfoUI InfoViewer { get => infoViewer; set => infoViewer = value; }
        [SerializeField] private EncounterInfoUI infoViewer;
        public virtual GameObject InProgressObject { get => inProgressObject; set => inProgressObject = value; }
        [SerializeField] private GameObject inProgressObject;
        public virtual GameObject CompletedObject { get => completedObject; set => completedObject = value; }
        [SerializeField] private GameObject completedObject;

        public virtual void DisplayForRead(MenuSceneInfo sceneInfo, MenuEncounter encounter)
        {
            Display(sceneInfo, encounter);

            if (EncounterButtons != null)
                EncounterButtons.DisplayForRead(sceneInfo, encounter);
        }

        public virtual void DisplayForEdit(MenuSceneInfo sceneInfo, MenuEncounter encounter)
        {
            Display(sceneInfo, encounter);

            if (EncounterButtons != null)
                EncounterButtons.DisplayForEdit(sceneInfo, encounter);
        }

        protected virtual void Display(MenuSceneInfo sceneInfo, MenuEncounter encounter)
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