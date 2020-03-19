using ClinicalTools.SimEncounters.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuEncounterUI : MonoBehaviour
    {
        public event Action<EncounterInfo> Selected;

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


        public void Display(InfoNeededForMainMenuToHappen data, EncounterInfo encounterInfo)
        {
            if (InfoViewer != null) {
                if (encounterInfo.MetaGroup.GetLatestInfo() == null)
                    Debug.Log("what");
                new EncounterInfoDisplay(InfoViewer, encounterInfo.MetaGroup.GetLatestInfo());
            }
            SelectButton.onClick.AddListener(() => Selected?.Invoke(encounterInfo));

            if (encounterInfo.UserStatus != null) {
                if (encounterInfo.UserStatus.Completed)
                    CompletedObject.SetActive(true);
                else
                    InProgressObject.SetActive(true);
            }
        }
    }
}