using ClinicalTools.ClinicalEncounters.Loader;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Loading;
using System.Collections;
using System.Diagnostics;
using System.Xml;

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
            var loadingScreen = new LoadingScreen();

            var demoXml = new DemoXml(new FilePathManager(), new FileXmlReader());
            demoXml.Completed += ShowReader;

            var encounterInfoGroup = new EncounterInfoGroup {
                Filename = "Chad_Wright"
            };
            demoXml.GetEncounterXml(User.Guest, encounterInfoGroup);

            yield return null;
        }

        public void ShowReader(object sender, EncounterXmlRetrievedEventArgs e)
        {
            var loader = new ClinicalEncounterLoader();
            var watch = Stopwatch.StartNew();
            var encounterInfo = new EncounterInfo() {
                Title = "Chad Wright",
                Subtitle = "Chronic Knee Pain",
                Audience = "MD/DO/PA/NP",
                Description = "Chad Wright is a 35 yo male presenting with chronic knee pain that started with a high school football injury. " +
                "His pain management has included chronic opioid therapy in recent years. " +
                "He now presents as a new patient requesting a prescription for opioids.",
                Difficulty = Difficulty.Intermediate
            };
            var encounterInfoGroup = new EncounterInfoGroup();
            encounterInfoGroup.CurrentInfo = encounterInfo;
            encounterInfo.Categories.Add("Pain Management");
            var encounter = loader.ReadEncounter(encounterInfoGroup, e.DataXml, e.ImagesXml);
            watch.Stop();

            new ReaderScene(User.Guest, null, encounter, (ReaderUI)SceneUI);
        }
    }
}