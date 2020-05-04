using UnityEngine;

namespace ClinicalTools.SimEncounters.Reader
{
    public abstract class BaseReaderSceneDrawer : MonoBehaviour, IReaderSceneDrawer
    {
        public abstract void Display(LoadingReaderSceneInfo sceneInfo);
    }
}