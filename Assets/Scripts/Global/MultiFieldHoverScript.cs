using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class MultiFieldHoverScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public List<InputField> groupInputFields;
    public List<Button> groupButtons;
    public List<GameObject> groupImages;
    public List<GameObject> groupActiveImages;
    public CanvasGroup realInputField;
    public TextMeshProUGUI inputText;
    public RectTransform inputCaret;
    public TextMeshProUGUI thisText;
    public TextMeshProUGUI linkedText;
    public CanvasGroup thisPlaceholder;

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach (InputField iField in groupInputFields) {
            iField.OnPointerEnter(eventData);
        }
        foreach (Button iButton in groupButtons) {
            iButton.OnPointerEnter(eventData);
        }
        foreach (GameObject image in groupImages) {
            image.SetActive(false);
        }
        foreach (GameObject image in groupActiveImages) {
            image.SetActive(true);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (InputField iField in groupInputFields) {
            iField.OnPointerExit(eventData);
        }
        foreach (Button iButton in groupButtons) {
            iButton.OnPointerExit(eventData);
        }
        foreach (GameObject image in groupImages) {
            image.SetActive(true);
        }
        foreach (GameObject image in groupActiveImages) {
            image.SetActive(false);
        }
    }

    public void ReceiveText()
    {
        if (GetComponent<TMP_InputField>()) {
            GetComponent<TMP_InputField>().text = inputText.text;
        } else if (thisText) {
            if (thisPlaceholder) {
                if (!inputText.text.Equals("")) {
                    thisPlaceholder.alpha = 0.0f;
                } else {
                    thisPlaceholder.alpha = 1.0f;
                }
                UpdateText();
            }
        }
    }

    // Ensures that UpdateTextOnNextFrame can only be called once per paste
    private bool pastingLock;
    private bool isSmall;
    public void UpdateText()
    {
        if (!pastingLock) {
            NextFrame.Function(UpdateTextOnNextFrame);
            pastingLock = true;

            inputText.rectTransform.offsetMax = Vector2.zero;
            inputText.rectTransform.offsetMin = Vector2.zero;
            if (!inputCaret) {
                inputCaret = (RectTransform)inputText.rectTransform.parent.Find("CustomContentValue Input Caret").transform;
            }
            inputCaret.offsetMax = Vector2.zero;
            inputCaret.offsetMin = Vector2.zero;
        }

        return;
    }

    // The main purpose of this is to do what linked text should do in TextMeshPro
    private void UpdateTextOnNextFrame()
    {
        pastingLock = false;

        var text = inputText.text;
        // You have to have linked text to get the first character from, even though this text doesn't display properly
        // Much to my dismay, firstOverflowCharacter doesn't work
        // The linkedTextComponent will also have an empty string as the text, so you have to trim it by hand
        var linkedStart = thisText.linkedTextComponent.firstVisibleCharacter;

        thisText.text = text;
        // Ensure the start point needs the second text area
        // firstVisibleCharacter isn't updated on linkedTextComponent if it isn't used, 
        // so you have to make sure it is used by checking the main text's preferred height vs its actual height.
        if (linkedStart > 0 && linkedStart < text.Length &&
            thisText.GetPreferredValues(text).y > thisText.rectTransform.rect.height) {
            // If you don't add ' ', it won't get the height of new lines correctly afaik
            linkedText.text = text.Substring(linkedStart) + ' ';
            // Have to add a bit of a barrier (currently doing 6) to allow letters like 'g' to not clip. 
            linkedText.transform.parent.GetComponent<LayoutElement>().preferredHeight =
                linkedText.GetPreferredValues().y + 6;
        } else {
            linkedText.text = "";
            linkedText.transform.parent.GetComponent<LayoutElement>().preferredHeight = 0;
        }

        // Sometimes text boundaries get messed up, so fix them
        inputText.rectTransform.offsetMax = Vector2.zero;
        inputText.rectTransform.offsetMin = Vector2.zero;
        // Caret can get deleted and remade, so need to make sure it isn't null
        if (!inputCaret) {
            inputCaret = (RectTransform)inputText.rectTransform.parent.Find("CustomContentValue Input Caret").transform;
        }
        inputCaret.offsetMax = Vector2.zero;
        inputCaret.offsetMin = Vector2.zero;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        realInputField.alpha = 1.0f;
        realInputField.interactable = true;
        realInputField.blocksRaycasts = true;
        if (realInputField.GetComponent<InputField>()) {
            realInputField.GetComponent<InputField>().OnPointerClick(eventData);
        } else if (realInputField.GetComponent<TMP_InputField>()) {
            realInputField.GetComponent<TMP_InputField>().OnPointerClick(eventData);
        }
    }
}
