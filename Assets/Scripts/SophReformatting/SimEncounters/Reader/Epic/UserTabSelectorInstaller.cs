using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserTabSelectorInstaller : MonoInstaller
    {
        public UserTabSelectorBehaviour SelectorBehaviour { get => selectorBehaviour; set => selectorBehaviour = value; }
        [SerializeField] private UserTabSelectorBehaviour selectorBehaviour;

        public override void InstallBindings()
            => Container.BindInstance<ISelector<UserTabSelectedEventArgs>>(SelectorBehaviour);
    }
}