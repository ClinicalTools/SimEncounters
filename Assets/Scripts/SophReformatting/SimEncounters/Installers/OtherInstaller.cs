using ClinicalTools.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class OtherInstaller : SubcontainerInstaller
    {
        public override void Install(DiContainer container)
        {
            SignalBusInstaller.Install(Container);
            Container.DeclareSignal<SceneChangedSignal>().OptionalSubscriber();

            container.Bind<ICurve>().To<AccCurve>().AsTransient();
            container.Bind<IShifter>().To<Shifter>().AsTransient();
        }
    }
}