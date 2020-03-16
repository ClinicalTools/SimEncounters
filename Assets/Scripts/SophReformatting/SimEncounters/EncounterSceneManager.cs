using System.Collections;
using UnityEngine;
using ClinicalTools.SimEncounters.MainMenu;

namespace ClinicalTools.SimEncounters
{

    public class EncounterSceneManager : SceneManager
    {
        public static EncounterSceneManager EncounterInstance => (EncounterSceneManager)Instance;
        [field: SerializeField] public LoadingScreenUI LoadingScreenPrefab { get; set; }


        protected ILoadingScreen LoadingScreen => SimEncounters.LoadingScreen.Instance;


        protected ReaderSceneLoader ReaderSceneLoader { get; set; } = new ReaderSceneLoader(new MobileScenePathData());
        protected MainMenuSceneLoader MainMenuSceneLoader { get; set; } = new MainMenuSceneLoader(new MobileScenePathData());


        public virtual void StartReaderScene(User user, EncounterInfo encounterDetail, IEncounterDataReader encounterGetter)
        {
            var x = new ReaderSceneStarter(new MobileScenePathData());
            var info = new InfoNeededForReaderToHappen(user, LoadingScreen, encounterDetail, encounterGetter);
            x.StartScene(this, info);
            //ReaderSceneLoader.StartScene(this, user, encounterGetter);
        }
        public virtual void StartMainMenuScene(User user)
        {
            var x = new MainMenuSceneStarter(new MobileScenePathData());
            var info = new InfoNeededForMainMenuToHappen(user, LoadingScreen, new EncountersInfoReader());
            x.StartScene(this, info);
        }
    }
}