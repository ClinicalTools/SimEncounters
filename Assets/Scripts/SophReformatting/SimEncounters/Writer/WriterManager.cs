﻿using ClinicalTools.ClinicalEncounters.Loader;
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

            var encounterMeta = new EncounterMetaGroup {
                Filename = "Chad_Wright"
            };
            var encounterInfo = new EncounterInfo(0, encounterMeta);
            demoXml.GetEncounterXml(User.Guest, encounterInfo);

            yield return null;
        }

        public void ShowReader(object sender, EncounterXmlRetrievedEventArgs e)
        {
            var loader = new ClinicalEncounterLoader();
            var watch = Stopwatch.StartNew();
            var encounterInfo = new EncounterMetaGroup();
            var encounter = loader.ReadEncounter(e.DataXml, e.ImagesXml);
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