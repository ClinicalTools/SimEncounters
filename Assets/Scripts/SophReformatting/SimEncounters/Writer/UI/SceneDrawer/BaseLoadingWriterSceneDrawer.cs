using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseLoadingWriterSceneDrawer : MonoBehaviour, ILoadingWriterSceneDrawer
    {
        public abstract void Display(LoadingWriterSceneInfo sceneInfo);
    }
}