using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LayoutGroupFixScript : MonoBehaviour {

	void Awake() {
		Debug.LogWarning("LayoutGroupFixScript");
		if (GetComponent<VerticalLayoutGroup> ()) {
			GetComponent<VerticalLayoutGroup> ().enabled = false;
		}

		if (GetComponent<HorizontalLayoutGroup> ()) {
			GetComponent<HorizontalLayoutGroup> ().enabled = false;
		}
	}


	// Use this for initialization
	void Start () {
		NextFrame.Function (Fix);

		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void Fix() {
		if (GetComponent<VerticalLayoutGroup> ()) {
			GetComponent<VerticalLayoutGroup> ().enabled = true;
		}

		if (GetComponent<HorizontalLayoutGroup> ()) {
			GetComponent<HorizontalLayoutGroup> ().enabled = true;
		}
	}
}
