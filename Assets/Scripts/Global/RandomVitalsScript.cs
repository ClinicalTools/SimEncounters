using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ClinicalTools.SimEncounters;

public class RandomVitalsScript : MonoBehaviour
{

    private WriterHandler ds;
    private TMP_InputField BPValue1;
    private TMP_InputField BPValue2;
    private TMP_InputField pulseValue;
    private TMP_InputField temperatureValue;
    private TMP_InputField respirationValue;
    private Transform parentTab;

    // Use this for initialization
    void Start()
    {
        ds = WriterHandler.WriterInstance;
        BPValue1 = transform.Find("TMPInputField/BPValue1").GetComponent<TMP_InputField>();
        BPValue2 = transform.Find("TMPInputField/BPValue2").GetComponent<TMP_InputField>();
        pulseValue = transform.Find("TMPInputField/PulseValue").GetComponent<TMP_InputField>();
        temperatureValue = transform.Find("TMPInputField/TempValue").GetComponent<TMP_InputField>();
        respirationValue = transform.Find("TMPInputField/RespValue").GetComponent<TMP_InputField>();

        //Finds the tab this script belongs to
        Transform tempObj = this.transform;

        while (parentTab == null && tempObj != null) {
            if (tempObj.name.EndsWith("Tab")) {
                parentTab = tempObj;
                break;
            } else {
                if (tempObj.parent != null) {
                    tempObj = tempObj.parent;
                } else {
                    Debug.Log("Cannot access parent tab!");
                    return;
                }
            }
        }

        NextFrame.Function(CreateValues);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateValues()
    {
        //if (ds.newTabs.Contains(parentTab)) 
        {
            BPValue1.text = UnityEngine.Random.Range(90, 120).ToString();
            BPValue2.text = UnityEngine.Random.Range(60, 80).ToString();
            pulseValue.text = UnityEngine.Random.Range(60, 100).ToString();
            temperatureValue.text = Mathf.Round(UnityEngine.Random.Range(97.8f, 99.1f)).ToString();
            respirationValue.text = UnityEngine.Random.Range(12, 18).ToString();
        }
    }
}
