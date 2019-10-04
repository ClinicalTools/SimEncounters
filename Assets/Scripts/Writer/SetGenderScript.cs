using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetGenderScript : MonoBehaviour {
	public void setGender()
	{
		GlobalDataScript gData = GameObject.Find("Canvas").GetComponent<GlobalDataScript>();
		gData.setCharacter (this.GetComponentInChildren<TextMeshProUGUI>().text);
	}
}