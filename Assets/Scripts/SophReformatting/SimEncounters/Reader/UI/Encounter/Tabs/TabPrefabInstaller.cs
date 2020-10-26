using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class TabPrefabInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindFactory<string, BaseUserTabDrawer, BaseUserTabDrawer.Factory>()
                      .FromFactory<PrefabResourceFactory<BaseUserTabDrawer>>();
            Container.BindFactory<Object, BaseReaderPanel, BaseReaderPanel.Factory>()
                      .FromFactory<PrefabFactory<BaseReaderPanel>>();
            Container.BindFactory<Object, ReaderPanelBehaviour, ReaderPanelBehaviour.Factory>()
                      .FromFactory<PrefabFactory<ReaderPanelBehaviour>>();
        }
    }
}