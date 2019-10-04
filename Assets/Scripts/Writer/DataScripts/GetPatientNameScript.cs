using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPatientNameScript : MonoBehaviour {

    public InputField firstName, lastName;
    public DataScript ds;
	// Use this for initialization
	void Start () {
        ds = GameObject.Find("GaudyBG").GetComponent<DataScript>();
        
        firstName.onValueChanged.AddListener(delegate { changeFirstName(); });
        lastName.onValueChanged.AddListener(delegate { changeLastName(); });
    }
	
    public void changeFirstName()
    {
        //ds.firstName = firstName.text;
        //Debug.Log(firstName.text);
    }

    public void changeLastName()
    {
        //ds.lastName = lastName.text;
    }
}
