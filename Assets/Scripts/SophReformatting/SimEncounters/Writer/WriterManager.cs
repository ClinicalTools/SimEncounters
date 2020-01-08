using ClinicalTools.ClinicalEncounters.Loader;
using ClinicalTools.SimEncounters.Loading;
using System.Collections;
using System.Diagnostics;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterManager : EncounterSceneManager
    {
        public static WriterManager WriterInstance => (WriterManager)Instance;

        public override void Awake()
        {
            base.Awake();

            if (Instance != this)
                return;

            StartCoroutine(StartScene());
        }

        public IEnumerator StartScene()
        {
            var loadingScreen = new LoadingScreen();
            var demoXml = new DemoXml();
            while (!demoXml.DataXml.IsCompleted || !demoXml.ImagesXml.IsCompleted)
                yield return null;

            var loader = new ClinicalEncounterLoader();
            var watch = Stopwatch.StartNew();
            var encounter = loader.ReadEncounter(demoXml.DataXml.Result, demoXml.ImagesXml.Result);
            watch.Stop();
            UnityEngine.Debug.LogError(watch.ElapsedMilliseconds);
            new EncounterWriter(null, loadingScreen, encounter, (WriterUI)SceneUI);
        }
        //SaveCase

        // loading case
        // save and view button
        // exit to main menu button
        // add section button
        // section buttons
        // help button
    }
}