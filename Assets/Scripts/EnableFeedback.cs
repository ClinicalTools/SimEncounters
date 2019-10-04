using UnityEngine;
using UnityEngine.UI;

public class EnableFeedback : MonoBehaviour
{
    public Toggle SubmitButton;

    // Ensure feedback button is active after an answer was selected
    public void OnValueSelected(bool a)
    {
        SubmitButton.interactable = true;
    }
}
