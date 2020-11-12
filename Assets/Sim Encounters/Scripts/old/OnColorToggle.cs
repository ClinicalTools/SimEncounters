using UnityEngine;
using UnityEngine.UI;

public class OnColorToggle : MonoBehaviour
{
    [field: SerializeField] public Color OnColor { get; set; }

    private void Start()
    {
        var toggle = GetComponent<Toggle>();
        var offColor = toggle.colors.normalColor;

        toggle.onValueChanged.AddListener((selected) => Selected(toggle, offColor));
    }

    private void Selected(Toggle toggle, Color offColor)
    {
        var colorBlock = toggle.colors;
        if (toggle.isOn)
            colorBlock.normalColor = OnColor;
        else
            colorBlock.normalColor = offColor;

        toggle.colors = colorBlock;
    }
}
