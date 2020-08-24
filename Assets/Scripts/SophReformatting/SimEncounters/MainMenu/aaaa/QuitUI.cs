using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class QuitUI : PopupUI
    {
        [SerializeField] private ExitButtonsUI exitButtons;
        public ExitButtonsUI ExitButtons { get => exitButtons; set => exitButtons = value; }

    }
}