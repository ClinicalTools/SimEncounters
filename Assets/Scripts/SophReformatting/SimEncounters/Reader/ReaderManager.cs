using ClinicalTools.SimEncounters.Data;
using ImaginationOverflow.UniversalDeepLinking;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderManager : SceneManager, IReaderSceneDrawer
    {
        public string DefaultEncounterFileName { get => defaultEncounterFileName; set => defaultEncounterFileName = value; }
        [SerializeField] private string defaultEncounterFileName;
        public BaseReaderSceneDrawer ReaderDrawer { get => readerDrawer; set => readerDrawer = value; }
        [SerializeField] private BaseReaderSceneDrawer readerDrawer;

        public List<GameObject> StandaloneSceneObjects { get => standaloneSceneObjects; set => standaloneSceneObjects = value; }
        [SerializeField] private List<GameObject> standaloneSceneObjects;
        // I can't find a good antonym for standalone
        // Variable naming is truly the hardest part of programming
        public List<GameObject> NonStandaloneSceneObjects { get => nonStandaloneSceneObjects; set => nonStandaloneSceneObjects = value; }
        [SerializeField] private List<GameObject> nonStandaloneSceneObjects;

        protected IMetadataReader MetadataReader { get; set; }
        protected IUserEncounterReader EncounterReader { get; set; }
        protected IEncounterQuickStarter EncounterQuickStarter { get; set; }
        [Inject]
        public virtual void Inject(IMetadataReader metadataReader, IUserEncounterReader encounterReader, IEncounterQuickStarter encounterQuickStarter)
        {
            MetadataReader = metadataReader;
            EncounterReader = encounterReader;
            EncounterQuickStarter = encounterQuickStarter;
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


        public virtual void MetadataRetrieved(WaitedResult<EncounterMetadata> metadata)
        {
            if (metadata.Value == null) {
                Debug.LogError("Metadata is null.");
                return;
            }

            var fullEncounter = EncounterReader.GetUserEncounter(User.Guest, metadata.Value, new EncounterBasicStatus(), SaveType.Demo);
            var sceneInfo = new LoadingReaderSceneInfo(User.Guest, null, fullEncounter);

            Display(sceneInfo);
        }

        protected LoadingReaderSceneInfo SceneInfo { get; set; }
        public void Display(LoadingReaderSceneInfo sceneInfo)
        {
            SceneInfo = sceneInfo;
            DeepLinkManager.Instance.LinkActivated += Instance_LinkActivated;
            ReaderDrawer.Display(sceneInfo);
        }

        private const string RecordNumberKey = "r";
        protected virtual void Instance_LinkActivated(LinkActivation s)
        {
            if (!s.QueryString.ContainsKey(RecordNumberKey))
                return;

            var recordNumberStr = s.QueryString[RecordNumberKey];
            if (!int.TryParse(recordNumberStr, out int recordNumber))
                return;

            SceneInfo.Result.RemoveListeners();
            EncounterQuickStarter.StartEncounter(SceneInfo.User, SceneInfo.LoadingScreen, recordNumber);
        }
    }
}