using System;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuSceneDrawer : BaseMenuSceneDrawer, ILogoutHandler
    {
        public Button LogoutButton { get => logoutButton; set => logoutButton = value; }
        [SerializeField] private Button logoutButton;
        public BaseMenuSceneDrawer Encounters { get => encounters; set => encounters = value; }
        [SerializeField] private BaseMenuSceneDrawer encounters;
        public UserDropdownUI UserDropdown { get => userDropdown; set => userDropdown = value; }
        [SerializeField] private UserDropdownUI userDropdown;

        public event Action Logout;

        protected virtual void Awake()
        {
            Screen.fullScreen = false;
            if (LogoutButton != null)
                LogoutButton.onClick.AddListener(LogoutPressed);
        }

        public LoadingMenuSceneInfo SceneInfo { get; set; }
        public override void Display(LoadingMenuSceneInfo loadingSceneInfo)
        {
            SceneInfo = loadingSceneInfo;
            Encounters.Display(loadingSceneInfo);
        }

        protected virtual void LogoutPressed()
            => Logout?.Invoke();

        public override void Hide()
        {
            Encounters.Hide();
        }
    }
}