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

            var demoXml = new DemoXml(new FilePathManager(), new FileXmlReader());
            demoXml.Completed += ShowReader;

            var encounterInfoGroup = new EncounterInfoGroup {
                Filename = "Chad_Wright"
            };
            demoXml.GetEncounterXml(User.Guest, encounterInfoGroup);

            yield return null;
        }

        public void ShowReader(XmlDocument dataXml, XmlDocument imagesXml)
        {
            var loader = new ClinicalEncounterLoader();
            var watch = Stopwatch.StartNew();
            var encounterInfo = new EncounterInfo();
            var encounter = loader.ReadEncounter(encounterInfo, dataXml, imagesXml);
            watch.Stop();

            new EncounterWriter(null, null, encounter, (WriterUI)SceneUI);
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