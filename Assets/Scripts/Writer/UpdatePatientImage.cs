using UnityEngine;
using UnityEngine.UI;

public class UpdatePatientImage : MonoBehaviour
{
    void Start()
    {
        var sprite = GameObject.Find("GaudyBG").GetComponent<ReaderDataScript>().GetImage(GlobalData.patientImageID)?.sprite;
        if (sprite != null) {
            Rect imageRect = sprite.rect;
            var layoutElem = transform.parent.GetComponent<LayoutElement>();
            if (layoutElem)
                layoutElem.preferredHeight = layoutElem.preferredWidth * imageRect.height / imageRect.width;

            var img = GetComponent<Image>();
            if (img)
                img.sprite = sprite;
        }
    }
}
