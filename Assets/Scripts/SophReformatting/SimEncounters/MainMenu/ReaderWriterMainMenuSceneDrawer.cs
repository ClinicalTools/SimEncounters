using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class ReaderWriterMainMenuSceneDrawer : BaseMenuSceneDrawer
    {
        public Button ReaderButton { get => readerButton; set => readerButton = value; }
        [SerializeField] private Button readerButton;
        public Button WriterButton { get => writerButton; set => writerButton = value; }
        [SerializeField] private Button writerButton;

        public BaseMenuSceneDrawer WriterMenuDrawer { get => writerMenuDrawer; set => writerMenuDrawer = value; }
        [SerializeField] private BaseMenuSceneDrawer writerMenuDrawer;
        public BaseMenuSceneDrawer ReaderMenuDrawer { get => readerMenuDrawer; set => readerMenuDrawer = value; }
        [SerializeField] private BaseMenuSceneDrawer readerMenuDrawer;

        protected virtual void Awake()
        {
            ReaderButton.onClick.AddListener(StartReader);
            WriterButton.onClick.AddListener(StartWriter);
        }

        public LoadingMenuSceneInfo SceneInfo { get; set; }
        public override void Display(LoadingMenuSceneInfo loadingSceneInfo)
        {
            SceneInfo = loadingSceneInfo;
            gameObject.SetActive(true);
        }

        protected virtual void StartReader() => ReaderMenuDrawer.Display(SceneInfo);
        protected virtual void StartWriter() => WriterMenuDrawer.Display(SceneInfo);
    }
}