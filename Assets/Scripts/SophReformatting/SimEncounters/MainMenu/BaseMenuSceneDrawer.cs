using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public abstract class BaseMenuSceneDrawer : MonoBehaviour, IMenuSceneDrawer
    {
        public abstract void Display(LoadingMenuSceneInfo sceneInfo);

        public abstract void Hide();
    }
}