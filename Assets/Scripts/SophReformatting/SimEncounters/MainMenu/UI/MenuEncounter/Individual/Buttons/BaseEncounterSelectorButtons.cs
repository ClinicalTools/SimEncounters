
using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public abstract class BaseEncounterSelectorButtons : MonoBehaviour
    {
        public abstract void Display(MenuSceneInfo sceneInfo, MenuEncounter menuEncounter);
        public abstract void Hide();
    }
}