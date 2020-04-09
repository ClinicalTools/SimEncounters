using ClinicalTools.ClinicalEncounters.Loader;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Loading;
using System.Collections;
using System.Diagnostics;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderManager : EncounterSceneManager
    {
        [UnityEngine.SerializeField] TMPro.TextMeshProUGUI debugField;
        public static ReaderManager ReaderInstance => (ReaderManager)Instance;

        public override void Awake()
        {
            base.Awake();
        }

        protected virtual void Start()
        {

            if (Instance != this)
                return;

            //StartCoroutine(StartScene());
        }

        public IEnumerator StartScene()
        {
            
            yield return null;
        }

        public void ShowReader()
        {
            var watch = Stopwatch.StartNew();
            /*
            var encounterInfo = new EncounterMetadata() {
                Title = "Chad Wright",
                Subtitle = "Chronic Knee Pain",
                Audience = "MD/DO/PA/NP",
                Description = "Chad Wright is a 35 yo male presenting with chronic knee pain that started with a high school football injury. " +
                "His pain management has included chronic opioid therapy in recent years. " +
                "He now presents as a new patient requesting a prescription for opioids.",
                Difficulty = Difficulty.Intermediate
            };
            */
            watch.Stop();

            //ReaderSceneLoader.StartReader(this, User.Guest, encounter);
        }
    }
}