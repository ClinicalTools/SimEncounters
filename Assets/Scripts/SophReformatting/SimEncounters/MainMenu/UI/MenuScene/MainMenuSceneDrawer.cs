using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class MainMenuSceneDrawer : BaseMenuSceneDrawer, ILogoutHandler
    {
        public List<Button> LogoutButtons { get => logoutButtons; set => logoutButtons = value; }
        [SerializeField] private List<Button> logoutButtons;
        public BaseMenuSceneDrawer Encounters { get => encounters; set => encounters = value; }
        [SerializeField] private BaseMenuSceneDrawer encounters;

        public event Action Logout;

        protected virtual void Awake()
        {
            Screen.fullScreen = false;
            foreach (var logoutButton in LogoutButtons)
                logoutButton.onClick.AddListener(LogoutPressed);
        }

        public LoadingMenuSceneInfo SceneInfo { get; set; }
        public override void Display(LoadingMenuSceneInfo loadingSceneInfo)
        {
            SceneInfo = loadingSceneInfo;
            Encounters.Display(loadingSceneInfo);
        }

        protected virtual void LogoutPressed()
        {
            Encounters.Hide();
            Logout?.Invoke();
        }
        public override void Hide() => Encounters.Hide();
    }
}