using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance { get; protected set; }
        [SerializeField] private SceneUI sceneUI;
        public virtual SceneUI SceneUI { get => sceneUI; set => sceneUI = value; }

        public virtual void Awake()
        {
            if (Instance == null)
                InitializeInstance();
            else
                SetInstanceVariables();
        }

        protected virtual void InitializeInstance()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        protected virtual void SetInstanceVariables()
        {
            Instance.SceneUI = SceneUI;
            Destroy(gameObject);
        }
    }
}