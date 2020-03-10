using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class ChangeSidePanelScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public event Action Selected;

    private Toggle toggle;
    protected Toggle Toggle {
        get {
            if (toggle == null)
                toggle = GetComponent<Toggle>();
            return toggle;
        }
    }
    public TextMeshProUGUI myText;
    public Color myOnText;
    public Color myOffText;
    public Color myHoverText;

    // Use this for initialization
    protected void Start()
    {
        Toggle.onValueChanged.AddListener((isOn) => ToggleThis(isOn));
        ToggleThis(Toggle.isOn);
    }

    protected void ToggleThis(bool isOn)
    {
        // 195 dark and 115 light
        if (isOn) {
            Selected?.Invoke();
            Toggle.interactable = false;
            myText.color = myOnText;
        } else {
            Toggle.interactable = true;
            myText.color = myOffText;
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (!Toggle.isOn)
            myText.color = myHoverText;
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (!Toggle.isOn)
            myText.color = myOffText;
    }

    public void Select()
    {
        Toggle.isOn = true;
    }

    public void Show(string text)
    {
        myText.text = text;
        gameObject.SetActive(true);
        Select();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
