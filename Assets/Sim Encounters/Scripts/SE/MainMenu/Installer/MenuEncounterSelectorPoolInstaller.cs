using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MenuEncounterSelectorPoolInstaller : MonoInstaller
    {
        public MainMenuEncounterUI EncounterSelectorPrefab { get => encounterSelectorPrefab; set => encounterSelectorPrefab = value; }
        [SerializeField] private MainMenuEncounterUI encounterSelectorPrefab;
        public Transform PoolParent { get => poolParent; set => poolParent = value; }
        [SerializeField] private Transform poolParent;
        public int InitialSize { get => initialSize; set => initialSize = value; }
        [SerializeField] private int initialSize;

        public override void InstallBindings()
        {
            Container.BindMemoryPool<MainMenuEncounterUI, MainMenuEncounterUI.Pool>()
                .WithInitialSize(InitialSize)
                .FromComponentInNewPrefab(EncounterSelectorPrefab)
                .UnderTransform(PoolParent);
        }
    }
}