using ClinicalTools.UI;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class OtherInstaller : SubcontainerInstaller
    {
        public override void Install(DiContainer container)
        {
            container.Bind<ICurve>().To<AccCurve>().AsTransient();
            container.Bind<IShifter>().To<Shifter>().AsTransient();
        }
    }
}