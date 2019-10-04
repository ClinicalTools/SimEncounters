using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class InputFieldResizer : MonoBehaviour
{
    public float minimumHeight = 0;

    private float inputHeight = 0;

    private bool isPasting = true;

    private InputField mainInputField;

    void Start()
    {
        if (GetComponent<TMP_InputField>().characterValidation.Equals(TMP_InputField.CharacterValidation.None)) {
            GetComponent<TMP_InputField>().onValidateInput += delegate (string input, int charIndex, char addedChar) { return MyValidate(input, charIndex, addedChar); };
        }
        NextFrame.Function(ResizeField);
    }

    char MyValidate(string input, int charIndex, char charToValidate)
    {
        if (charToValidate == '	' || charToValidate == GlobalData.EMPTY_WIDTH_SPACE)
            return '\0';
        return charToValidate;
    }

    // Ensures that the coroutine is only called once per frame.
    private bool resizeLock;
    public void ResizeField()
    {
        if (resizeLock)
            return;
        resizeLock = true;

        var inputField = GetComponent<TMP_InputField>();
        if (inputField) {
            var rectTrans = inputField.textComponent.GetComponent<RectTransform>();
            if (rectTrans) {
                rectTrans.offsetMax = new Vector2(0, 0);
                rectTrans.offsetMin = new Vector2(0, 0);
            }
        }

        NextFrame.Function(ActivateResizeTMP);
        NextFrame.Function(delegate { NextFrame.Function(ActivateResizeTMP); });
    }

    public void RefreshParent1()
    {
        HorizontalLayoutGroup parent = transform.GetComponentInParent<HorizontalLayoutGroup>();
        parent.childControlHeight = true;
        LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetComponent<RectTransform>());
        //parent.childControlHeight = false;
        //LayoutRebuilder.ForceRebuildLayoutImmediate (parent.GetComponent<RectTransform> ());
    }

    public void RefreshParent2()
    {
        HorizontalLayoutGroup parent = transform.GetComponentInParent<HorizontalLayoutGroup>();
        parent.childControlHeight = false;
        LayoutRebuilder.ForceRebuildLayoutImmediate(parent.GetComponent<RectTransform>());
    }

    private RectTransform inputCaret;
    // Update is called once per frame
    void LateUpdate()
    {
        if (isPasting) {
            isPasting = false;
            ResizeField();
        }

        // The input caret must be resized manually
        if (inputCaret == null) {
            var caret = GetComponentInChildren<TMP_SelectionCaret>();
            if (caret != null) {
                inputCaret = caret.GetComponent<RectTransform>();
            }
        }
        if (inputCaret != null) {
            inputCaret.offsetMin = new Vector2(0, 0);
            inputCaret.offsetMax = new Vector2(0, 0);
        }
    }

    private void ActivateResizeTMP()
    {
        resizeLock = false;

        if (!isPasting) {
            try {
                if (gameObject == null)
                    return;
            } catch {
                return;
            }

            var text = GetComponent<TMP_InputField>().text;
            if (text.Length > 0 && text[text.Length - 1] == '\n')
                text += ' ';

            //Min = Left/Top, Max = Right/Bottom in the text's rect transform
            float verticalMargins = GetComponent<RectTransform>().rect.width - GetComponent<TMP_InputField>().textComponent.GetComponent<RectTransform>().rect.width;
            //float horizontalMargins = Math.Abs((GetComponent<TMP_InputField>().textComponent.GetComponent<RectTransform>().rect.yMin + GetComponent<RectTransform>().rect.yMax) * 2);
            float horizontalMargins = GetComponent<RectTransform>().rect.height - GetComponent<TMP_InputField>().textComponent.GetComponent<RectTransform>().rect.height;

            inputHeight = GetComponent<LayoutElement>().preferredHeight - horizontalMargins;
            //GetComponent<TMP_InputField>().textComponent.GetComponent<RectTransform>().ForceUpdateRectTransforms();

            float newInputHeight;
            try {
                float width = GetComponent<RectTransform>().sizeDelta.x - verticalMargins;
                newInputHeight = GetComponent<TMP_InputField>().textComponent.GetPreferredValues(text, width, 0).y;
            } catch (Exception e) {
                Debug.LogWarning("THE JUMBLED TEXT BUG JUST HAPPENED. ERROR MESSAGE: " + e.Message);
                //Debug.Log("Hopefully Fixing in 4 seconds");
                //Invoke("ResizeField", 4f);
                return;
                //We could force a redraw here since this bug only seems to happen once in a blue moon for whatever reason
                //But for now I am just forcing a return to avoid breaking things further
            }

            if (newInputHeight != inputHeight) {
                if (newInputHeight > minimumHeight - horizontalMargins) {
                    inputHeight = newInputHeight;


                    if (GetComponent<LayoutElement>() != null) {
                        GetComponent<LayoutElement>().preferredHeight = inputHeight + horizontalMargins;
                    } else {
                        Vector2 newSize = new Vector2(GetComponent<RectTransform>().sizeDelta.x, inputHeight + horizontalMargins);

                        GetComponent<RectTransform>().sizeDelta = newSize;
                    }
                } else {

                    if (GetComponent<LayoutElement>() != null) {
                        GetComponent<LayoutElement>().preferredHeight = minimumHeight;
                    } else {
                        Vector2 newSize = new Vector2(GetComponent<RectTransform>().sizeDelta.x, minimumHeight);

                        GetComponent<RectTransform>().sizeDelta = newSize;
                    }
                }
                GetComponent<TMP_InputField>().textComponent.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                GetComponent<TMP_InputField>().textComponent.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            }
        }
    }
}
