using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ChangeSidePanelScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
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
    void Start()
    {
        Toggle.onValueChanged.AddListener((isOn) => ToggleThis(isOn));
        ToggleThis(Toggle.isOn);
    }

    public void ToggleThis(bool isOn)
    {
        // 195 dark and 115 light
        if (isOn) {
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

}
