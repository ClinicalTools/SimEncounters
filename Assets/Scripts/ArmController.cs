using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("left")) {
			transform.localRotation.SetLookRotation(new Vector3(transform.localRotation.x, transform.localRotation.y + .2f, transform.localRotation.z));
		} else if (Input.GetKey("right")) {
			transform.localRotation.SetLookRotation(new Vector3(transform.localRotation.x, transform.localRotation.y - .2f, transform.localRotation.z));

		} else if (Input.GetKey("up")) {
			transform.localRotation.SetLookRotation(new Vector3(transform.localRotation.x + .2f, transform.localRotation.y, transform.localRotation.z));

		} else if (Input.GetKey("down")) {
			transform.localRotation.SetLookRotation(new Vector3(transform.localRotation.x - .2f, transform.localRotation.y, transform.localRotation.z));

		}
	}
}
