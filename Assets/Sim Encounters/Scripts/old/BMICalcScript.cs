using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BMICalcScript : MonoBehaviour
{
    public TMPro.TMP_InputField heightValue;
    public TMPro.TMP_InputField weightValue;

    // Use this for initialization
    void Start()
    {
        Debug.LogWarning("bmicalcscript");
        InvokeRepeating("InitialCalc", 0.1f, 5.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateBMI()
    {
        if (!heightValue.text.Equals("") && !weightValue.text.Equals("")) {
            float myHeight = float.Parse(heightValue.text) * 0.0254f;
            float myWeight = float.Parse(weightValue.text) * 0.453592f;

            float myBMI = myWeight / Mathf.Pow(myHeight, 2f);
            GetComponent<TMPro.TextMeshProUGUI>().text = myBMI.ToString("f1") + " kg/m²";
        }
    }

    private void InitialCalc()
    {
        UpdateBMI();
        CancelInvoke();
    }
}