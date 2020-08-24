using UnityEngine;
using UnityEngine.UI;

public class RefreshSizeFitter : MonoBehaviour
{
    private ContentSizeFitter fitter;
    private void Awake()
    {
        fitter = GetComponent<ContentSizeFitter>();

        fitter.enabled = false;
    }

    private void Update()
    {
        if (fitter)
            fitter.enabled = true;
    }
}
