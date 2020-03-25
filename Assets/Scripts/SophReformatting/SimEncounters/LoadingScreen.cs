using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// As much as I'd love to escape singletons, there isn't a great way to set variables in the editor and still pass classes between scenes.
    /// </remarks>
    public class LoadingScreen : MonoBehaviour, ILoadingScreen
    {
        public static LoadingScreen Instance { get; protected set; }

        [SerializeField] private CanvasGroup canvasGroup;
        public CanvasGroup CanvasGroup { get => canvasGroup; set => canvasGroup = value; }

        protected virtual void Awake()
        {
            if (Instance != null) {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            CanvasGroup.alpha = 0;
        }

        public virtual void Stop()
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(Hide());
        }

        private const float FADE_TIME = 2;
        public IEnumerator Hide()
        {
            while (CanvasGroup.alpha > 0) {
                yield return null;
                CanvasGroup.alpha -= Time.deltaTime / FADE_TIME;
            }
            gameObject.SetActive(false);
        }

    }
}