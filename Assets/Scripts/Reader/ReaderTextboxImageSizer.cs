using UnityEngine;
using UnityEngine.UI;

public class ReaderTextboxImageSizer : MonoBehaviour
{

    public TMPro.TextMeshProUGUI referenceText;
    private LayoutElement referenceLayout;
    private TMPro.TextMeshProUGUI thisText;
    private LayoutElement linkedLayout;
    int count = 0;
    int runs = 20;
    public bool limit = false;
    // Use this for initialization
    void Start()
    {
        thisText = GetComponent<TMPro.TextMeshProUGUI>();
        if (referenceText == null)
        {
            foreach (TMPro.TextMeshProUGUI tmpu in transform.parent.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
            {
                if (tmpu != thisText)
                {
                    referenceText = tmpu;
                    break;
                }
            }
        }
        linkedLayout = referenceText.linkedTextComponent.GetComponent<LayoutElement>();
        referenceLayout = referenceText.GetComponent<LayoutElement>();
    }

    // Late update gets an earlier call than update for this
    // This should only need to run once, although it currently runs every frame.
    private void LateUpdate()
    {
        thisText = GetComponent<TMPro.TextMeshProUGUI>();
        if (referenceText == null)
        {
            foreach (TMPro.TextMeshProUGUI tmpu in transform.parent.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
            {
                if (tmpu != thisText)
                {
                    referenceText = tmpu;
                    break;
                }
            }
        }
        linkedLayout = referenceText.linkedTextComponent.GetComponent<LayoutElement>();
        referenceLayout = referenceText.GetComponent<LayoutElement>();

        if (limit)
        {
            if (count >= runs)
            {
                return;
            }
        }
        // Remove bold xml to get the proper first character
        var refText = referenceText.text.Replace("<b>", "").Replace("</b>", "");
        var firstText = refText.Substring(0, referenceText.linkedTextComponent.firstVisibleCharacter);

        if (referenceLayout.preferredHeight <= 1)
        {
            var firstWidth = referenceText.rectTransform.rect.width;
            var firstHeight = referenceText.rectTransform.rect.height;
            while (referenceText.GetPreferredValues(firstText, firstWidth, 0).y < firstHeight)
            {
                firstText += '\n';
            }
            referenceLayout.preferredHeight = referenceText.GetPreferredValues(firstText, firstWidth, 0).y;

            // Recalculate first character for linkedText based on new height of reference
            firstText = refText.Substring(0, referenceText.linkedTextComponent.firstVisibleCharacter);
            while (referenceText.GetPreferredValues(firstText, firstWidth, 0).y <= referenceLayout.preferredHeight && firstText.Length < refText.Length)
            {
                firstText += refText[referenceText.linkedTextComponent.firstVisibleCharacter++];
            }
            referenceText.linkedTextComponent.firstVisibleCharacter--;
        }

        thisText.text = refText.Substring(referenceText.linkedTextComponent.firstVisibleCharacter);
        if (referenceText.rectTransform.rect.height < referenceText.preferredHeight)
            linkedLayout.preferredHeight = referenceText.linkedTextComponent.GetPreferredValues(thisText.text, referenceText.linkedTextComponent.rectTransform.rect.width, 0).y;
        else
            linkedLayout.preferredHeight = 0;
        referenceText.linkedTextComponent.fontSize++;
        referenceText.linkedTextComponent.fontSize--;
        referenceText.fontSize++;
        referenceText.fontSize--;
        count++;
    }
}
