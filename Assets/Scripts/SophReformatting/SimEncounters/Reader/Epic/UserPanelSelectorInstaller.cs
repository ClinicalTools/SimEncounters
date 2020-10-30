using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserPanelSelectorInstaller : MonoInstaller
    {
        public UserPanelSelectorBehaviour SelectorBehaviour { get => selectorBehaviour; set => selectorBehaviour = value; }
        [SerializeField] private UserPanelSelectorBehaviour selectorBehaviour;

        public override void InstallBindings()
            => Container.BindInterfacesTo(SelectorBehaviour.GetType()).FromInstance(SelectorBehaviour);
    }
}