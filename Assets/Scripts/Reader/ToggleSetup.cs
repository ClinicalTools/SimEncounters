using UnityEngine;
using UnityEngine.UI;

public class ToggleSetup : MonoBehaviour {
    void Start()
    {
        var toggle = GetComponent<Toggle>();

        toggle.group = GetComponentInParent<ToggleGroup>();

        // Ensure feedback button is active after an answer was selected
        var enableFeedback = GetComponentInParent<EnableFeedback>();
        if (enableFeedback)
            toggle.onValueChanged.AddListener(enableFeedback.OnValueSelected);
    }
}
