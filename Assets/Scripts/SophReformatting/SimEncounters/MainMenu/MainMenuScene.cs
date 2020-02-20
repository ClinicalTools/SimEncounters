using System;
using System.Collections;
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuScene : EncounterScene
    {
        public User User { get; }

        public MainMenuScene(User user, MainMenuUI sceneUI) : base(sceneUI)
        {
            User = user;
            var casesInfo = new MainMenuEncountersInfo(this, sceneUI.Encounters);
        }
    }

}