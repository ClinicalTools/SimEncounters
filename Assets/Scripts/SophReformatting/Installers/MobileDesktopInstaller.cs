using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class MobileDesktopInstaller : MonoInstaller
    {
        public List<MonoInstaller> DesktopInstallers { get => desktopInstallers; set => desktopInstallers = value; }
        [SerializeField] private List<MonoInstaller> desktopInstallers;
        public List<MonoInstaller> MobileInstallers { get => mobileInstallers; set => mobileInstallers = value; }
        [SerializeField] private List<MonoInstaller> mobileInstallers;

        public override void InstallBindings()
        {
#if UNITY_STANDALONE
            foreach (var installer in DesktopInstallers)
                installer.InstallBindings();
#else
            foreach (var installer in MobileInstallers)
                installer.InstallBindings();
#endif
        }
    }
}