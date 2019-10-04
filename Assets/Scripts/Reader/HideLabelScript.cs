using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideLabelScript : MonoBehaviour {

	public GameObject[] labels;
	public bool displayNA;
	public string customDisplay;
	public string customHide;
	
	public void HideLabel()
	{
		foreach (GameObject obj in labels) {
			if (obj != null) {
				obj.SetActive(false);
			}
		}
		gameObject.SetActive(false);
	}
}
