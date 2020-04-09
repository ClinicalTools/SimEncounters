using ClinicalTools.ClinicalEncounters.Loader;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Loading;
using System.Collections;
using System.Diagnostics;
using System.Xml;

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

            yield return null;
        }

        public void ShowReader(object sender)
        {
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