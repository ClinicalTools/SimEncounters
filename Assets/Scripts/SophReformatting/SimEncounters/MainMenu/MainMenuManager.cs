using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class MainMenuManager : EncounterSceneManager
    {
        public static MainMenuManager MainMenuInstance => (MainMenuManager)Instance;

        public override void Awake()
        {
            base.Awake();

            if (Instance != this)
                return;

            StartCoroutine(StartScene());
        }

        public IEnumerator StartScene()
        {
            //new Login();

            new MainMenuScene(User.Guest, (MainMenuUI)SceneUI);
            yield return null;
        }
    }
}