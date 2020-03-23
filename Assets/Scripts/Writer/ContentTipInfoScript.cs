using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ContentTipInfoScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public float delay;                     //The delay before the tool tip shows (default is 1 second)
    public CanvasGroup contentTipCanvas;    //The canvas group which holds the Tool tip that is shown on screen
    public ContentSizeFitter contentTipFitter;    //The size fitter used to fit the Tool tip
    private TextMeshProUGUI description;    //The description text object on the ToolTipCanvas
    private TextMeshProUGUI contentComp;

    /**
	 * Script for the tool tips that show when hovering the mouse over certain elements
	 * This is attached to objects that will have the tool tip display when hovered over
	 */

    // Use this for initialization
    void Start()
    {
        if (GetComponent<TextMeshProUGUI>()) {
            contentComp = GetComponent<TextMeshProUGUI>();
        } else {
            return;
        }
        /*
        if (contentTipCanvas == null) {
            if (GlobalData.contentTip != null) {
                contentTipCanvas = GlobalData.contentTip;
            } else {
                contentTipCanvas = GameObject.Find("ContentTip").GetComponent<CanvasGroup>();
            }
        }
        if (contentTipFitter == null) {
            if (GlobalData.contentTip != null) {
                contentTipFitter = GlobalData.contentTip.GetComponent<ContentSizeFitter>();
            } else {
                contentTipFitter = GameObject.Find("ContentTip").GetComponent<ContentSizeFitter>();
            }
        }
        description = contentTipCanvas.transform.Find("ContentTipText").GetComponent<TextMeshProUGUI>();
        if (delay < .25f) {
            delay = 1.0f;
        }

        if (GetComponent<Button>()) {
            GetComponent<Button>().onClick.AddListener(ClickedButton);
        }*/
    }

    /**
	 * When the pointer starts hovering, start counting
	 */
    public void OnPointerEnter(PointerEventData data)
    {
        if (string.IsNullOrEmpty(contentComp?.text))
            return;

        if (data.pointerEnter.name.Equals(this.name)) {
            InvokeRepeating("UpdateText", .25f, 5.0f);
        }
    }

    /**
	 * Updates the text and resizes the tool tip before it is displayed
	 */
    private void UpdateText()
    {/*
        if (contentComp != null && !contentComp.Equals("")) {
            description.gameObject.SetActive(true);
            description.text = contentComp.text;
        }

        CancelInvoke();
        InvokeRepeating("ShowPopup", delay - .25f, 5.0f);*/
    }

    private void ClickedButton()
    {
        CancelInvoke();
        //contentTipCanvas.alpha = 0;
        //description.gameObject.SetActive(false);
    }

    /**
	 * When the pointer stops hovering
	 */
    public void OnPointerExit(PointerEventData data)
    {
        if (string.IsNullOrEmpty(contentComp?.text))
            return;

        CancelInvoke();
        //contentTipCanvas.alpha = 0;
        //description.gameObject.SetActive(false);
    }

    /**
	 * Show the tool tip
	 */
    private void ShowPopup()
    {
        // Doesn't always update the first time without this
        var parentFitter = description.transform.parent.GetComponent<ContentSizeFitter>();
        parentFitter.enabled = true;

        contentTipCanvas.alpha = 1;
        CancelInvoke();
    }
}
