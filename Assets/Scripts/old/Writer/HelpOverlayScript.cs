using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpOverlayScript : MonoBehaviour {

	public void ToggleOverlay()
    {
        Canvas overlay = GameObject.Find("Canvas/OverlayBG").GetComponent<Canvas>();
        CanvasGroup group = GameObject.Find("Canvas/OverlayBG").GetComponent<CanvasGroup>();

        if (GetComponent<Toggle>().isOn)
        {
            group.alpha = 1.0f;
            group.interactable = true;
            group.blocksRaycasts = true;
            overlay.overrideSorting = true;
            WriterEventManager.ShowOverlay();
        } else
        {
            WriterEventManager.HideOverlay();
            overlay.overrideSorting = false;
            group.alpha = 0.0f;
            group.interactable = false;
            group.blocksRaycasts = false;
        }
    }

    public void HideOverlay()
    {
        GetComponent<Toggle>().isOn = false;
    }
}
