using ClinicalTools.SimEncounters.Data;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuEncounterUI : MonoBehaviour
    {
        public event Action<EncounterDetail> Selected;

        [SerializeField] private Button selectButton;
        public virtual Button SelectButton { get => selectButton; set => selectButton = value; }

        [SerializeField] private EncounterButtonsUI encounterButtons;
        public virtual EncounterButtonsUI EncounterButtons { get => encounterButtons; set => encounterButtons = value; }

        [SerializeField] private EncounterInfoUI infoViewer;
        public virtual EncounterInfoUI InfoViewer { get => infoViewer; set => infoViewer = value; }


        public void Display(InfoNeededForMainMenuToHappen data, EncounterDetail encounterInfo)
        {
            if (InfoViewer != null) {
                if (encounterInfo.InfoGroup.GetLatestInfo() == null)
                    Debug.Log("what");
                new EncounterInfoDisplay(InfoViewer, encounterInfo.InfoGroup.GetLatestInfo());
            }
            SelectButton.onClick.AddListener(() => Selected?.Invoke(encounterInfo));
        }
    }
}