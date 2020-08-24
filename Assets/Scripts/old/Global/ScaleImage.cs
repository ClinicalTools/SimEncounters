using UnityEngine;
using UnityEngine.UI;

public class ScaleImage : MonoBehaviour {
    private RectTransform rectTransform;
    private Image img;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        img = GetComponent<Image>();
    }

    private float height, width;
    private void Update()
    {
        if (height != rectTransform.rect.height) {
            height = rectTransform.rect.height;
            width = (float)img.mainTexture.width / img.mainTexture.height * height;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }
        if (width != rectTransform.rect.width) {
            width = rectTransform.rect.width;
            height = (float)img.mainTexture.height / img.mainTexture.width * width;
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
    }
}
