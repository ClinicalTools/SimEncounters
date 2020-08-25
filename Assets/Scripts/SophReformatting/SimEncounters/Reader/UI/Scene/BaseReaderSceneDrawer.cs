using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseReaderSceneDrawer : MonoBehaviour, IReaderSceneDrawer
    {
        public abstract void Display(LoadingReaderSceneInfo sceneInfo);
    }
}