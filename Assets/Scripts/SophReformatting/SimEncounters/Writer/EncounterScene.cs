using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class EncounterScene
    {
        protected virtual SceneUI SceneUI { get; }

        public EncounterScene(SceneUI sceneUI)
        {
            SceneUI = sceneUI;
        }

        public virtual T OpenPopup<T>(T popup) where T : MonoBehaviour
            => Object.Instantiate(popup, SceneUI.PopupsParent);
    }
}