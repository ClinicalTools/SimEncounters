﻿using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuUI : SceneUI
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
            LogoutButton.onClick.AddListener(Login.Logout);
        }

        public virtual void Display(InfoNeededForMainMenuToHappen data)
        {
            Encounters.Display(data);
        }
    }
}