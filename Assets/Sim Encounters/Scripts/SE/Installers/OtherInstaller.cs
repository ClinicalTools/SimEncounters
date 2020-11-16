using ClinicalTools.UI;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class OtherInstaller : SubcontainerInstaller
    {
        public override void Install(DiContainer container)
        {
            SignalBusInstaller.Install(container);
            container.DeclareSignal<SceneChangedSignal>().OptionalSubscriber();

            container.Bind<ICurve>().To<AccCurve>().AsTransient();
            container.Bind<IShifter>().To<Shifter>().AsTransient();

            container.BindFactory<Object, GameObject, GameObjectFactory>()
                     .FromFactory<PrefabFactory<GameObject>>();
            container.BindFactory<Object, RectTransform, RectTransformFactory>()
                     .FromFactory<PrefabFactory<RectTransform>>();
        }
    }
}