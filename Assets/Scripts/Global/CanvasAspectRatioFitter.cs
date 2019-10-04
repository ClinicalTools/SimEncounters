using UnityEngine;
using UnityEngine.UI;

public class CanvasAspectRatioFitter : MonoBehaviour {
    private int width, height;
    private CanvasScaler scaler;
    private float originalRefX;

    private void Awake()
    {
        scaler = GetComponent<CanvasScaler>();
        originalRefX = scaler.referenceResolution.x;
    }

    private void Update()
    {
        if (height != Screen.height || width != Screen.width) {
            height = Screen.height;
            width = Screen.width;

            var referenceRes = scaler.referenceResolution;
            referenceRes.x = referenceRes.y * width / height;
            if (referenceRes.x < originalRefX)
                referenceRes.x = originalRefX;

            scaler.referenceResolution = referenceRes;
        }
    }
}
