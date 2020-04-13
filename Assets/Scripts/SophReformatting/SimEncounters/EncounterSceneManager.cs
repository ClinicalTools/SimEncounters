using System.Collections;
using UnityEngine;
using ClinicalTools.SimEncounters.MainMenu;
using System.Collections.Generic;
using Zenject;

namespace ClinicalTools.SimEncounters
{

    public class EncounterSceneManager : SceneManager
    {
        public static EncounterSceneManager EncounterInstance => (EncounterSceneManager)Instance;
        [field: SerializeField] public LoadingScreenUI LoadingScreenPrefab { get; set; }


        protected ILoadingScreen LoadingScreen => SimEncounters.LoadingScreen.Instance;

        [Inject] protected MainMenuSceneStarter MainMenuSceneStarter { get; set; }
        [Inject] protected ReaderSceneStarter ReaderSceneStarter { get; set; }

        public void Initialize(MainMenuSceneStarter mainMenuSceneStarter, ReaderSceneStarter readerSceneStarter)
        {
            MainMenuSceneStarter = mainMenuSceneStarter;
            ReaderSceneStarter = readerSceneStarter;
        }

        public virtual void StartReaderScene(LoadingEncounterSceneInfo encounterSceneInfo)
        {
            ReaderSceneStarter.StartScene(this, encounterSceneInfo);
        }
        public virtual void StartMainMenuScene(LoadingMenuSceneInfo menuSceneInfo)
        {
            MainMenuSceneStarter.StartScene(this, menuSceneInfo);
        }
    }
}