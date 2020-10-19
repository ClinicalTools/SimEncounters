using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class UserTabSelectorInstaller : MonoInstaller
    {
        public UserTabSelectorBehaviour SelectorBehaviour { get => selectorBehaviour; set => selectorBehaviour = value; }
        [SerializeField] private UserTabSelectorBehaviour selectorBehaviour;

        public override void InstallBindings()
        {
            Container.BindInstance<ISelector<UserTabSelectedEventArgs>>(SelectorBehaviour);
            Container.Bind<ISelector<TabSelectedEventArgs>>().To<Selector<TabSelectedEventArgs>>().AsSingle();
            //Container.BindInstance<ISelector<TabSelectedEventArgs>>(new Selector<TabSelectedEventArgs>());
        }
    }
}