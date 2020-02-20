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

            if (Instance != this)
                return;

            StartCoroutine(StartScene());
        }

        public IEnumerator StartScene()
        {
            var watch = Stopwatch.StartNew();
            var loadingScreen = new LoadingScreen();
            var demoXml = new DemoXml();
            /*
            while (!demoXml.DataXml.IsCompleted || !demoXml.ImagesXml.IsCompleted)
                yield return null;//*/
            debugField.text = watch.ElapsedMilliseconds.ToString();

            var loader = new ClinicalEncounterLoader();
            var encounterInfo = new EncounterInfo() {
                Title = "Chad Wright",
                Subtitle = "Chronic Knee Pain",
                Audience = "MD/DO/PA/NP",
                Description = "Chad Wright is a 35 yo male presenting with chronic knee pain that started with a high school football injury. " +
                "His pain management has included chronic opioid therapy in recent years. " +
                "He now presents as a new patient requesting a prescription for opioids.",
                Difficulty = Difficulty.Intermediate
            };
            encounterInfo.Categories.Add("Pain Management");

            var encounter = loader.ReadEncounter(encounterInfo, demoXml.DataXmlSync, demoXml.ImagesXmlSync);
            //var encounter = loader.ReadEncounter(encounterInfo, demoXml.DataXml.Result, demoXml.ImagesXml.Result);
            //UnityEngine.Debug.LogError(demoXml.DataXml.Result.OuterXml);// watch.ElapsedMilliseconds);
            new ReaderScene(User.Guest, loadingScreen, encounter, (ReaderUI)SceneUI);
            //NextFrame.Function(() => debugField.text = watch.ElapsedMilliseconds.ToString());
            yield return null;
        }
    }
}