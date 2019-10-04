using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField]
    private static LoadingScreenManager instance;

    private void StartA()
    {
        if (instance == null) {
            //instance = this;
        }
    }

    public static LoadingScreenManager Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<LoadingScreenManager>();
                if (instance == null) {
                    GameObject obj = new GameObject("I AM A LOADING SCREEN");
                    obj.transform.SetParent(GameObject.Find("Canvas").transform);
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    obj.AddComponent<CanvasGroup>();
                    instance = obj.AddComponent<LoadingScreenManager>();
                }
            }
            return instance;
        }
    }

    private CanvasGroup group;
    private CanvasGroup Group {
        get {
            if (group == null)
                group = GetComponent<CanvasGroup>();

            return group;
        }
    }

    private bool fading = false;
    public void Fade()
    {
        Group.alpha = .99998f;
        fading = true;
    }

    public void Update()
    {
        // Continue fading if starting to fade
        if (fading) {
            // Ensure the load hasn't been reset
            if (Group.alpha > .99999f) {
                fading = false;
                return;
            }

            Group.alpha = (Group.alpha - Time.deltaTime / 2f);

            if (Group.alpha < 0.00001f) {
                Group.alpha = 1;
                gameObject.SetActive(false);
                fading = false;
            }
        }
    }
}
