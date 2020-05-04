using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuSceneDrawer : BaseMenuSceneDrawer
    {
        [SerializeField] private Button logoutButton;
        public Button LogoutButton { get => logoutButton; set => logoutButton = value; }

        [SerializeField] private LoginHandler login;
        public LoginHandler Login { get => login; set => login = value; }

        [SerializeField] private MainMenuEncountersUI encounters;
        public MainMenuEncountersUI Encounters { get => encounters; set => encounters = value; }

        [SerializeField] private UserDropdownUI userDropdown;
        public UserDropdownUI UserDropdown { get => userDropdown; set => userDropdown = value; }

        protected virtual void Awake()
        {
            Screen.fullScreen = false;
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
    }
}