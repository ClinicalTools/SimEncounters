using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuSceneDrawer : BaseMenuSceneDrawer
    {
        public Button LogoutButton { get => logoutButton; set => logoutButton = value; }
        [SerializeField] private Button logoutButton;
        public LoginHandler Login { get => login; set => login = value; }
        [SerializeField] private LoginHandler login;
        public BaseMenuSceneDrawer Encounters { get => encounters; set => encounters = value; }
        [SerializeField] private BaseMenuSceneDrawer encounters;
        public UserDropdownUI UserDropdown { get => userDropdown; set => userDropdown = value; }
        [SerializeField] private UserDropdownUI userDropdown;

        protected virtual void Awake()
        {
            Screen.fullScreen = false;
            if (LogoutButton != null)
                LogoutButton.onClick.AddListener(Logout);
        }

        public LoadingMenuSceneInfo SceneInfo { get; set; }
        public override void Display(LoadingMenuSceneInfo loadingSceneInfo)
        {
            SceneInfo = loadingSceneInfo;
            Encounters.Display(loadingSceneInfo);
        }

        protected virtual void Logout()
        {
            PlayerPrefs.SetInt("StayLoggedIn", 0);
            Login.Logout(SceneInfo.LoadingScreen);
        }

        public override void Hide()
        {
            Encounters.Hide();
        }
    }
}