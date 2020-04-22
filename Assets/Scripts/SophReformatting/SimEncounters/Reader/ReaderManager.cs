using ClinicalTools.ClinicalEncounters.Loader;
using ClinicalTools.SimEncounters.Data;
using ClinicalTools.SimEncounters.Loading;
using System.Collections;
using System.Diagnostics;
using Zenject;

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

        protected IEncounterReaderSelector ReaderSelector { get; set; }
        [Inject]
        public virtual void Inject(IEncounterReaderSelector readerSelector)
        {
            ReaderSelector = readerSelector;
        }

        protected virtual void Start()
        {
            if (Instance != this)
                return;

            ShowReader();
        }

        public void ShowReader()
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

            var encounterReader = ReaderSelector.GetEncounterReader(SaveType.Demo);
            var fullEncounter = encounterReader.GetFullEncounter(User.Guest, metadata, new EncounterBasicStatus());

            var sceneInfo = new LoadingEncounterSceneInfo(User.Guest, null, fullEncounter);
            ((ReaderUI)SceneUI).Display(sceneInfo);
        }
    }
}