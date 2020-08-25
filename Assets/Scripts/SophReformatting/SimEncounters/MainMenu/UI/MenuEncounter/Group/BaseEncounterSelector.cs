using System;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public abstract class BaseEncounterSelector : MonoBehaviour
    {
        public abstract event Action<MenuEncounter> EncounterSelected;
        public abstract void DisplayForRead(MenuSceneInfo sceneInfo, IEnumerable<MenuEncounter> encounters);
        public abstract void DisplayForEdit(MenuSceneInfo sceneInfo, IEnumerable<MenuEncounter> encounters);
        public abstract void Initialize();
        public abstract void Hide();
    }
}