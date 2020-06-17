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

        protected IEncounterReader EncounterReader { get; set; }
        [Inject] public virtual void Inject(IEncounterReader encounterReader) => EncounterReader = encounterReader;

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

            var encounter = EncounterReader.GetEncounter(User.Guest, metadata, SaveType.Demo);
            var sceneInfo = new LoadingWriterSceneInfo(User.Guest, null, encounter);
            Display(sceneInfo);
        }

        protected override void StartAsLaterScene() { }
    }
}