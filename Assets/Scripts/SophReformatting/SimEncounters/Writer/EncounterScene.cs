using UnityEngine;

namespace ClinicalTools.SimEncounters.Writer
{
    public class EncounterScene
    {
        protected virtual SceneUI SceneUI { get; }

        public EncounterScene(SceneUI sceneUI)
        {
            SceneUI = sceneUI;
        }

        public virtual T OpenPanel<T>(T panel) where T : MonoBehaviour
            => Object.Instantiate(panel, SceneUI.PopupsParent);
    }
}