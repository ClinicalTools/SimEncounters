using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ExitButtonsUI : MonoBehaviour
    {
        [SerializeField] private Button quitButton;
        public Button QuitButton { get => quitButton; set => quitButton = value; }

        [SerializeField] private Button logoutButton;
        public Button LogoutButton { get => logoutButton; set => logoutButton = value; }

        [SerializeField] private Button changeApplicationButton;
        public Button ChangeApplicationButton { get => changeApplicationButton; set => changeApplicationButton = value; }
    }
}