using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class UserDropdownUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI usernameLabel;
        public TextMeshProUGUI UsernameLabel { get => usernameLabel; set => usernameLabel = value; }

        [SerializeField] private Button showOptionsButton;
        public Button ShowOptionsButton { get => showOptionsButton; set => showOptionsButton = value; }

        [SerializeField] private Button settingsButton;
        public Button SettingsButton { get => settingsButton; set => settingsButton = value; }

        [SerializeField] private Button windowedButton;
        public Button WindowedButton { get => windowedButton; set => windowedButton = value; }

        [SerializeField] private Button fullscreenButton;
        public Button FullscreenButton { get => fullscreenButton; set => fullscreenButton = value; }
  
        [SerializeField] private ExitButtonsUI exitButtons;
        public ExitButtonsUI ExitButtons { get => exitButtons; set => exitButtons = value; }
    }
}