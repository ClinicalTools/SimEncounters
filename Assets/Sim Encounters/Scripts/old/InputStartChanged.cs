using TMPro;
using UnityEngine;

/// <summary>
/// Calls the input field's on value changed method at start.
/// </summary>
public class InputStartChanged : MonoBehaviour {

	// Use this for initialization
	void Start()
    {
        var input = gameObject.GetComponent<TMP_InputField>();
        NextFrame.Function(
            delegate { input.onValueChanged.Invoke(input.text); });
	}
}
