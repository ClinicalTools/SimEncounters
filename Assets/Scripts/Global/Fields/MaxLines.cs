﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Sets the maximum number of lines that are displayed for a TextMeshProUGUI.
public class MaxLines : MonoBehaviour
{
    public int maxLines;

    TextMeshProUGUI tmpu;
    LayoutElement layoutElem;

    void Start()
    {
        tmpu = GetComponent<TextMeshProUGUI>();
        layoutElem = GetComponent<LayoutElement>();
    }

    string currText;
    // Updates if it is too big.
    // Currently doesn't update if the text becomes smaller.
    void OnRectTransformDimensionsChange()
    {
        if (layoutElem != null && layoutElem.preferredHeight <= 0 && tmpu.preferredHeight > tmpu.GetPreferredValues(" ").y * maxLines)
        {
            layoutElem.preferredHeight = (tmpu.GetPreferredValues(" ").y + 1)* maxLines;

            currText = tmpu.text;
        }
    }
}
