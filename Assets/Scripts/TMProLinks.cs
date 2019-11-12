using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TMProLinks : MonoBehaviour, IPointerClickHandler
{
    //TMP_
    //TMPro_
    //TextMeshPro
    // I swear to god they don't need any of those prefixes. 
    // You could literally just use the library name "TMPro.UGUI" but no
    TextMeshProUGUI text;

    // Start is called before the first frame update
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        var linkCount = text.textInfo.linkInfo.Length;
        if (linkCount == 0)
            return;

        var linkIndex = TMP_TextUtilities.FindIntersectingLink(text, eventData.position, Camera.current);
        if (linkIndex < 0 || linkIndex >= linkCount)
            return;

        var link = text.textInfo.linkInfo[linkIndex];

        var linkURL = link.GetLinkID();

        Application.OpenURL(linkURL);
    }
}
