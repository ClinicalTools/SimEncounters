using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SerialScript {

	private string serial;
	private int defaultLength = 10;

	// Use this for initialization
	void Start () {
		
	}

	public string GenerateSerial() {
		return GenerateSerial (defaultLength);
	}

	public string GenerateSerial(int len) {
		serial = Guid.NewGuid ().ToString ("N").Substring (0, len);
		if (GameObject.Find ("GaudyBG").GetComponent<DataScript> ().GetImageKeys().Contains (serial)) { //If duplicate, recalculate UID
			return GenerateSerial (len);
		}
		return serial;
	}

	public string GetSerial() {
		return serial;
	}

	public void SetSerial(string s) {
		serial = s;
	}
}
