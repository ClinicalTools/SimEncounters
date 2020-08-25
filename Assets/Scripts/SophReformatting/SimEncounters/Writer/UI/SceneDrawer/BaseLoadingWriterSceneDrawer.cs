using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseLoadingWriterSceneDrawer : MonoBehaviour, ILoadingWriterSceneDrawer
    {
        public abstract void Display(LoadingWriterSceneInfo sceneInfo);
    }
}