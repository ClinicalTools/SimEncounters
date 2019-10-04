using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectCustomColorScript : MonoBehaviour, IPointerDownHandler {
	public Toggle customColorToggle;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public virtual void OnPointerDown(PointerEventData evenDate){
		customColorToggle.isOn = true;
	}
}
