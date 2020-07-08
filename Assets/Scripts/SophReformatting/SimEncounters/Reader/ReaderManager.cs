using ClinicalTools.ClinicalEncounters;
using ClinicalTools.SimEncounters.Data;
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

        protected IMetadataReader MetadataReader { get; set; }
        protected IUserEncounterReader EncounterReader { get; set; }
        [Inject]
        public virtual void Inject(IMetadataReader metadataReader, IUserEncounterReader encounterReader)
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

        protected override void StartAsLaterScene() { }


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
        public virtual void Display(LoadingReaderSceneInfo sceneInfo) => ReaderDrawer.Display(sceneInfo);
    }
}