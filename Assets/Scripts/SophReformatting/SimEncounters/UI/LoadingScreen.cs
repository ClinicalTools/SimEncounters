using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ClinicalTools.SimEncounters
{
    public abstract class LoginThing : MonoBehaviour, ILoginManager
    {
        public abstract WaitableResult<User> Login();
    }

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


            StartCoroutine(InitialHide());
        }
        public IEnumerator InitialHide()
        {
            yield return new WaitForSeconds(1.5f);

            while (CanvasGroup.alpha > 0)
            {
                yield return null;
                CanvasGroup.alpha -= Time.deltaTime / FADE_TIME;
            }
            gameObject.SetActive(false);
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
            //CanvasGroup.alpha = 0;
        }

        public virtual void Stop()
        {
            //if (gameObject.activeInHierarchy)
                //StartCoroutine(Hide());
        }

        private const float FADE_TIME = 1;
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