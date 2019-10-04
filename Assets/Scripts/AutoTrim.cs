using TMPro;
using UnityEngine;

// Automatically trims the text in the field after editing, so all devices behave consistently.
public class AutoTrim : MonoBehaviour
{
    TMP_InputField field;

    // Use this for initialization
    void Start()
    {
        field = GetComponent<TMP_InputField>();
        field.onEndEdit.AddListener(
            delegate { field.text = field.text.Trim(); }
        );
    }
}
