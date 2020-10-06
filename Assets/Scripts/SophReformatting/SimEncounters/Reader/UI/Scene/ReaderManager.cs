﻿#if !STANDALONE_SCENE
using ImaginationOverflow.UniversalDeepLinking;
#endif
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class ReaderManager : SceneManager, IReaderSceneDrawer
    {
        public string DefaultEncounterFileName { get => defaultEncounterFileName; set => defaultEncounterFileName = value; }
        [SerializeField] private string defaultEncounterFileName;
        public BaseReaderSceneDrawer ReaderDrawer { get => readerDrawer; set => readerDrawer = value; }
        [SerializeField] private BaseReaderSceneDrawer readerDrawer;

        public List<GameObject> StandaloneSceneObjects { get => standaloneSceneObjects; set => standaloneSceneObjects = value; }
        [SerializeField] private List<GameObject> standaloneSceneObjects;

        public List<GameObject> NonStandaloneSceneObjects { get => nonStandaloneSceneObjects; set => nonStandaloneSceneObjects = value; }
        [SerializeField] private List<GameObject> nonStandaloneSceneObjects;

        protected IMetadataReader MetadataReader { get; set; }
        protected IUserEncounterReader EncounterReader { get; set; }
        protected IEncounterQuickStarter EncounterQuickStarter { get; set; }
        protected QuickActionFactory LinkActionFactory { get; set; }
        [Inject]
        public virtual void Inject(
            IMetadataReader metadataReader, IUserEncounterReader encounterReader, 
            IEncounterQuickStarter encounterQuickStarter, QuickActionFactory linkActionFactory)
        {
            MetadataReader = metadataReader;
            EncounterReader = encounterReader;
            EncounterQuickStarter = encounterQuickStarter;
            LinkActionFactory = linkActionFactory;
        }
        protected override void StartAsInitialScene()
        {
            Screen.fullScreen = false;

            var tempMetadata = new EncounterMetadata() {
                Filename = DefaultEncounterFileName
            };
            var metadataResult = MetadataReader.GetMetadata(User.Guest, tempMetadata);
            metadataResult.AddOnCompletedListener(MetadataRetrieved);

#if STANDALONE_SCENE
            foreach (var standaloneSceneObject in StandaloneSceneObjects)
                standaloneSceneObject.SetActive(true);
            foreach (var nonStandaloneSceneObject in NonStandaloneSceneObjects)
                nonStandaloneSceneObject.SetActive(false);
#else
            foreach (var standaloneSceneObject in StandaloneSceneObjects)
                standaloneSceneObject.SetActive(false);
            foreach (var nonStandaloneSceneObject in NonStandaloneSceneObjects)
                nonStandaloneSceneObject.SetActive(true);
#endif
        }

        protected override void StartAsLaterScene()
        {
            foreach (var standaloneSceneObject in StandaloneSceneObjects)
                standaloneSceneObject.SetActive(false);
            foreach (var nonStandaloneSceneObject in NonStandaloneSceneObjects)
                nonStandaloneSceneObject.SetActive(true);
        }


        public virtual void MetadataRetrieved(TaskResult<EncounterMetadata> metadata)
        {
            if (metadata.Value == null) {
                Debug.LogError("Metadata is null.");
                return;
            }

            UnityEngine.Debug.Log("f0");
            var fullEncounter = EncounterReader.GetUserEncounter(User.Guest, metadata.Value, new EncounterBasicStatus(), SaveType.Demo);
            var sceneInfo = new LoadingReaderSceneInfo(User.Guest, null, fullEncounter);

            Display(sceneInfo);
        }

        protected LoadingReaderSceneInfo SceneInfo { get; set; }
        public void Display(LoadingReaderSceneInfo sceneInfo)
        {
            SceneInfo = sceneInfo;
#if !STANDALONE_SCENE
            DeepLinkManager.Instance.LinkActivated += Instance_LinkActivated;
#endif
            ReaderDrawer.Display(sceneInfo);
        }


#if !STANDALONE_SCENE
        public void TestLink()
        {
            var idNum = "73";
            var dictionary = new Dictionary<string, string>();
            dictionary.Add(IdKey, idNum);
            var linkActivation = new LinkActivation($"lift://encounter?{IdKey}={idNum}", $"{IdKey}={idNum}", dictionary);

            Instance_LinkActivated(linkActivation);
        }
        private const string IdKey = "id";
        public void TestLink2()
        {
            var idNum = "95";
            var dictionary = new Dictionary<string, string>();
            dictionary.Add(IdKey, idNum);
            var linkActivation = new LinkActivation($"lift://encounter?{IdKey}={idNum}", $"{IdKey}={idNum}", dictionary);

            Instance_LinkActivated(linkActivation);
        }

        protected virtual void Instance_LinkActivated(LinkActivation s)
        {
            QuickAction quickAction = LinkActionFactory.GetLinkAction(s);
            if (quickAction.Action == QuickActionType.NA)
                return;

            SceneInfo.Result.RemoveListeners();
            EncounterQuickStarter.StartEncounter(SceneInfo.User, SceneInfo.LoadingScreen, quickAction.EncounterId);
        }
#endif
    }
}