using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public class SceneManager : MonoBehaviour
    {
        public static SceneManager Instance { get; protected set; }
        [field: SerializeField] protected virtual SceneUI SceneUI { get; set; }

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