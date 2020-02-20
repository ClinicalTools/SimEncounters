using System.Collections;
using System.Diagnostics;

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
            new MainMenuScene(User.Guest, (MainMenuUI)SceneUI);
            yield return null;
        }
    }
}