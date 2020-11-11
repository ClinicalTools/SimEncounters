using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollbarResetScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		NextFrame.Function (ResetScroll);
	}

	private void ResetScroll(){
		if (this != null) {
			if (GetComponent<Scrollbar>()) {
				GetComponent<Scrollbar>().value = 1f;
			}
		}
	}

	void Update () {
		if (GetComponent<Scrollbar> ().value > 1.0f || GetComponent<Scrollbar> ().value < 0.0f) {
			ResetScroll ();
		}
	}
}
