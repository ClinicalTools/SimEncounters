using TMPro;
using UnityEngine;

public class HexField : MonoBehaviour {
    private TMP_InputField input;

    private void Awake()
    {
        input = GetComponent<TMP_InputField>();
    }

    string hexChars = "0123456789abcdefABCDEF";
    public void UpdateField()
    {
        if (!input)
            return;

        var text = "";

        foreach (var ch in input.text) {
            if (hexChars.Contains("" + ch))
                text += ch;
        }

        input.text = text;
    }
}
