using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class BaseWriterSceneDrawer : MonoBehaviour
    {
        public abstract void Display(WriterSceneInfo sceneInfo);
    }
}