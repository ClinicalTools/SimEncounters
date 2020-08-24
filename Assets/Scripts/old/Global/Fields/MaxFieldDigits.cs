using TMPro;
using UnityEngine;

public class MaxFieldDigits : MonoBehaviour {
    private TMP_InputField input;
    [SerializeField] private int maxDigits = 0;

    private void Awake()
    {
        input = GetComponent<TMP_InputField>();
    }

    public void UpdateField()
    {
        var text = "";

        var digitCount = 0;
        foreach (var ch in input.text) {
            if (char.IsDigit(ch) && ++digitCount > maxDigits)
                continue;

            text += ch;
        }

        input.text = text;
    }
}
