using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class WriterMainMenuSceneDrawer : BaseMenuSceneDrawer
    {
        public Button ReaderButton { get => readerButton; set => readerButton = value; }
        [SerializeField] private Button readerButton;
        public Button WriterButton { get => writerButton; set => writerButton = value; }
        [SerializeField] private Button writerButton;

        public MainMenuSceneDrawer WriterMenuDrawer { get => writerMenuDrawer; set => writerMenuDrawer = value; }
        [SerializeField] private MainMenuSceneDrawer writerMenuDrawer;
        public MainMenuSceneDrawer ReaderMenuDrawer { get => readerMenuDrawer; set => readerMenuDrawer = value; }
        [SerializeField] private MainMenuSceneDrawer readerMenuDrawer;

        protected virtual void Awake()
        {
            ReaderButton.onClick.AddListener(StartReader);
            WriterButton.onClick.AddListener(StartWriter);
        }

        public LoadingMenuSceneInfo SceneInfo { get; set; }
        public override void Display(LoadingMenuSceneInfo loadingSceneInfo)
        {
            SceneInfo = loadingSceneInfo;
            gameObject.SetActive(true);
        }

        protected virtual void StartReader() => ReaderMenuDrawer.Display(SceneInfo);
        protected virtual void StartWriter() => WriterMenuDrawer.Display(SceneInfo);
    }


    public class MainMenuSceneDrawer : BaseMenuSceneDrawer
    {
        public Button LogoutButton { get => logoutButton; set => logoutButton = value; }
        [SerializeField] private Button logoutButton;
        public LoginHandler Login { get => login; set => login = value; }
        [SerializeField] private LoginHandler login;
        public MainMenuSceneDrawer Encounters { get => encounters; set => encounters = value; }
        [SerializeField] private MainMenuSceneDrawer encounters;
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
    }
}