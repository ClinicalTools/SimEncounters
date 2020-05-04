using ClinicalTools.SimEncounters.Data;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderManager : SceneManager, IReaderSceneDrawer
    {
        public BaseReaderSceneDrawer ReaderDrawer { get => readerDrawer; set => readerDrawer = value; }
        [SerializeField] private BaseReaderSceneDrawer readerDrawer;

        protected IUserEncounterReaderSelector ReaderSelector { get; set; }
        [Inject] public virtual void Inject(IUserEncounterReaderSelector readerSelector) => ReaderSelector = readerSelector;

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

            var encounterReader = ReaderSelector.GetUserEncounterReader(SaveType.Demo);
            var fullEncounter = encounterReader.GetUserEncounter(User.Guest, metadata, new EncounterBasicStatus());

            var sceneInfo = new LoadingReaderSceneInfo(User.Guest, null, fullEncounter);
            Display(sceneInfo);
        }

        protected override void StartAsLaterScene() { }

        public virtual void Display(LoadingReaderSceneInfo sceneInfo) => ReaderDrawer.Display(sceneInfo);
    }
}