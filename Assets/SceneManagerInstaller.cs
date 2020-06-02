using ClinicalTools.SimEncounters.MainMenu;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class SceneManagerInstaller : MonoInstaller
    {
        public string DesktopMenuScene { get => desktopMenuScene; set => desktopMenuScene = value; }
        [SerializeField] private string desktopMenuScene;
        public string DesktopReaderScene { get => desktopReaderScene; set => desktopReaderScene = value; }
        [SerializeField] private string desktopReaderScene;
        public string MobileMenuScene { get => mobileMenuScene; set => mobileMenuScene = value; }
        [SerializeField] private string mobileMenuScene;
        public string MobileReaderScene { get => mobileReaderScene; set => mobileReaderScene = value; }
        [SerializeField] private string mobileReaderScene;
        public string WriterScene { get => writerScene; set => writerScene = value; }
        [SerializeField] private string writerScene;

        public override void InstallBindings()
        {
            Container.Bind<IMenuSceneStarter>().To<MenuSceneStarter>().AsTransient();
            Container.Bind<IReaderSceneStarter>().To<ReaderSceneStarter>().AsTransient();
            Container.Bind<IWriterSceneStarter>().To<WriterSceneStarter>().AsTransient();

            Container.Bind<string>().FromInstance(WriterScene).WhenInjectedInto<WriterSceneStarter>();
#if UNITY_STANDALONE
            Container.Bind<string>().FromInstance(DesktopMenuScene).WhenInjectedInto<MenuSceneStarter>();
            Container.Bind<string>().FromInstance(DesktopReaderScene).WhenInjectedInto<ReaderSceneStarter>();
#else
            Container.Bind<string>().FromInstance(MobileMenuScene).WhenInjectedInto<MenuSceneStarter>();
            Container.Bind<string>().FromInstance(MobileReaderScene).WhenInjectedInto<ReaderSceneStarter>();
#endif
        }
    }
}