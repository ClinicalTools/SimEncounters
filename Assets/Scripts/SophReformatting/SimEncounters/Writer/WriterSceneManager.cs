using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.Writer
{
    public class WriterSceneManager : SceneManager, IWriterSceneDrawer
    {
        public BaseWriterSceneDrawer WriterDrawer { get => writerDrawer; set => writerDrawer = value; }
        [SerializeField] private BaseWriterSceneDrawer writerDrawer;

        public void Display(LoadingWriterSceneInfo sceneInfo) => WriterDrawer.Display(sceneInfo);

        protected IEncounterReaderSelector EncounterReaderSelector { get; set; }
        [Inject] public virtual void Inject(IEncounterReaderSelector readerSelector) => EncounterReaderSelector = readerSelector;

        protected override void StartAsInitialScene()
        {
            var metadata = new EncounterMetadata() {
                Filename = "289342Dave Abbott",
                Title = "Chad Wright",
                Subtitle = "Chronic Knee Pain",
                Audience = "MD/DO/PA/NP",
                Description = "Chad Wright is a 35 yo male presenting with chronic knee pain that started with a high school football injury. " +
                "His pain management has included chronic opioid therapy in recent years. " +
                "He now presents as a new patient requesting a prescription for opioids.",
                Difficulty = Difficulty.Intermediate
            };

            var encounterReader = EncounterReaderSelector.GetEncounterReader(SaveType.Demo);
            var encounter = encounterReader.GetEncounter(User.Guest, metadata);

            var sceneInfo = new LoadingWriterSceneInfo(User.Guest, null, encounter);
            Display(sceneInfo);
        }

        protected override void StartAsLaterScene() { }
    }
}