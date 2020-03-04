using System.Collections;
using UnityEngine;
using ClinicalTools.SimEncounters.Loader;
using ClinicalTools.SimEncounters.Writer;
using System.Xml;

namespace ClinicalTools.SimEncounters
{

    public class EncounterSceneManager : SceneManager
    {
        public static EncounterSceneManager EncounterInstance => (EncounterSceneManager)Instance;
        [field: SerializeField] public LoadingScreenUI LoadingScreenPrefab { get; set; }

        protected ReaderSceneLoader ReaderSceneLoader { get; set; } = new ReaderSceneLoader(new MobileScenePathData());
        protected MainMenuSceneLoader MainMenuSceneLoader { get; set; } = new MainMenuSceneLoader(new MobileScenePathData());


        public virtual void StartReaderScene(User user, IEncounterGetter encounterGetter)
        {
            ReaderSceneLoader.StartScene(this, user, encounterGetter);
        }
        public virtual void StartMainMenuScene(User user)
        {
            MainMenuSceneLoader.StartScene(this, user);
        }
    }
}