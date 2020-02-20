using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuUI : SceneUI
    {
        [SerializeField] private MainMenuEncountersUI encounters;
        public MainMenuEncountersUI Encounters { get => encounters; set => encounters = value; }

        [SerializeField] private UserDropdownUI userDropdown;
        public UserDropdownUI UserDropdown { get => userDropdown; set => userDropdown = value; }
    }
}