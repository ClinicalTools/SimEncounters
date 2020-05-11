using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterSceneDrawer : BaseWriterSceneDrawer
    {
        public virtual List<Button> MainMenuButtons { get => mainMenuButtons; set => mainMenuButtons = value; }
        [SerializeField] private List<Button> mainMenuButtons;
        public BaseEncounterDrawer EncounterDrawer { get => encounterDrawer; set => encounterDrawer = value; }
        [SerializeField] private BaseEncounterDrawer encounterDrawer;

        public override void Display(LoadingWriterSceneInfo sceneInfo)
        {
            sceneInfo.Result.AddOnCompletedListener(EncounterLoaded);
        }

        protected WriterSceneInfo SceneInfo { get; set; }
        protected virtual void EncounterLoaded(WriterSceneInfo sceneInfo)
        {
            SceneInfo = sceneInfo;
            if (Started)
                ProcessSceneInfo(sceneInfo);
        }

        protected bool Started { get; set; }
        protected virtual void Start()
        {
            Started = true;
            if (SceneInfo != null)
                ProcessSceneInfo(SceneInfo);
        }

        protected virtual void ProcessSceneInfo(WriterSceneInfo sceneInfo)
        {
            EncounterDrawer.Display(sceneInfo.Encounter);
        }
    }
}