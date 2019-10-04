using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberEntriesScript : MonoBehaviour {

	public TMPro.TextMeshProUGUI number;
	//public string divider;

	// Use this for initialization
	void Awake () {
		int idx = transform.GetSiblingIndex() + 1;
		number.text = idx + ".";
		//number.text = idx + divider;
	}
}
