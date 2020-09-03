using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class WriterSceneManager : SceneManager, ILoadingWriterSceneDrawer
    {
        public string DefaultEncounterFileName { get => defaultEncounterFileName; set => defaultEncounterFileName = value; }
        [SerializeField] private string defaultEncounterFileName;
        public BaseLoadingWriterSceneDrawer WriterDrawer { get => writerDrawer; set => writerDrawer = value; }
        [SerializeField] private BaseLoadingWriterSceneDrawer writerDrawer;

        protected IMetadataReader MetadataReader { get; set; }
        protected IEncounterReader EncounterReader { get; set; }
        [Inject]
        public virtual void Inject(IMetadataReader metadataReader, IEncounterReader encounterReader)
        {
            MetadataReader = metadataReader;
            EncounterReader = encounterReader;
        }
        protected override void StartAsInitialScene()
        {
            var tempMetadata = new EncounterMetadata() {
                Filename = DefaultEncounterFileName
            };
            var metadataResult = MetadataReader.GetMetadata(User.Guest, tempMetadata);
            metadataResult.AddOnCompletedListener(MetadataRetrieved);
        }

        public virtual void MetadataRetrieved(TaskResult<EncounterMetadata> metadata)
        {
            if (!metadata.HasValue()) {
                Debug.LogError("Metadata is null.");
                return;
            }

            var encounter = EncounterReader.GetEncounter(User.Guest, metadata.Value, SaveType.Demo);
            var sceneInfo = new LoadingWriterSceneInfo(User.Guest, null, encounter);
            Display(sceneInfo);
        }

        protected override void StartAsLaterScene() { }


        public void Display(LoadingWriterSceneInfo sceneInfo)
        {
            WriterDrawer.Display(sceneInfo);
            sceneInfo.Result.AddOnCompletedListener(SceneInfoLoaded);
        }
        protected virtual void SceneInfoLoaded(TaskResult<WriterSceneInfo> sceneInfo)
        {
            if (!sceneInfo.HasValue())
                return;
            if (sceneInfo.Value.LoadingScreen != null)
                sceneInfo.Value.LoadingScreen.Stop();
        }
    }
}