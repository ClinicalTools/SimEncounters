using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiagnosisCountScript : MonoBehaviour {

    // When an entry is created, get amount of children then set the entires label to that amount
    void Start () {
        int amountOfChildren = transform.parent.childCount;
        //string number = transform.Find("Content/Row0/DiagnosisNumberLabel").GetComponent<Text>().text;
		int index = transform.GetSiblingIndex() + 1;
		transform.Find("Content/Row0/DiagnosisNumberLabel").GetComponent<TMPro.TextMeshProUGUI>().text = "# " + index.ToString();
	
    }

    // After an entry is dragged, re-order the entries with the correct number label
    public void ReorderEntries()
    {
        int i = 1;

        foreach(Transform child in transform.parent)
        {
            if (child.name == "placeholder")
            {
                continue;
            }

            child.Find("Content/Row0/DiagnosisNumberLabel").GetComponent<TMPro.TextMeshProUGUI>().text = "# " + i;
            i++;
        }
        
    }
}
