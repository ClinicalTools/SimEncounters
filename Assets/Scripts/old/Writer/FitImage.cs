using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FitImage : UIBehaviour
{
    private Image img;
    private Image parentImg;
    private RectTransform rectTrans;

    protected override void Awake()
    {
        img = GetComponent<Image>();
        parentImg = transform.parent.GetComponent<Image>();
        rectTrans = (RectTransform)transform;
        base.Awake();
    }

    private bool isUpdating = false;
    protected override void OnRectTransformDimensionsChange()
    {
        // Lock to prevent infinite loops
        if (!isUpdating && img) {
            isUpdating = true;
            if (img.sprite) {
                SetRectByImg(img, rectTrans);
            } else {
                SetRectByImg(parentImg, rectTrans);
            }
            isUpdating = false;
        }

        base.OnRectTransformDimensionsChange();
    }

    private Sprite lastSprite = null;
    protected virtual void Update()
    {
        var sprite = img.sprite;
        if (lastSprite != sprite) {
            OnRectTransformDimensionsChange();
            lastSprite = sprite;
        }
    }

    // Sets the transform to match the dimensions of the image, as scaled up as it can be, given its aspect ratio
    public static void SetRectByImg(Image img, RectTransform rectTrans)
    {

        rectTrans.offsetMin = new Vector2(0, 0);
        rectTrans.offsetMax = new Vector2(0, 0);

        var maxWidth = rectTrans.rect.width;
        var maxHeight = rectTrans.rect.height;

        var width = img.preferredWidth / maxWidth;
        var height = img.preferredHeight / maxHeight;

        if (width > height) {
            rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, img.preferredHeight * rectTrans.rect.width / img.preferredWidth);
        } else if (height > width) {
            rectTrans.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, img.preferredWidth * rectTrans.rect.height / img.preferredHeight);
        }
    }
}
