using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseWriterSceneDrawer : MonoBehaviour, IWriterSceneDrawer
    {
        public abstract void Display(LoadingWriterSceneInfo sceneInfo);
    }
}