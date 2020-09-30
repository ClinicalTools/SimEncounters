using UnityEngine;

public class SomethingEpic : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        var rectTrans = (RectTransform)transform;
        text.text = $"rect: {rectTrans.rect}\nscale: {rectTrans.localScale}";
    }
}
