using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public abstract class BaseWriterSceneDrawer : MonoBehaviour
    {
        public abstract void Display(WriterSceneInfo sceneInfo);
    }
}