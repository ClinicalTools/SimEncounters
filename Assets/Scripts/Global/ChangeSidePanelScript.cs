using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChangeSidePanelScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public CanvasGroup myPanel;
	private Button myButton;
	public TMPro.TextMeshProUGUI myText;
	public Color myOnText;
	public Color myOffText;
	public Color myHoverText;

	// Use this for initialization
	void Start () {
		myButton = GetComponent<Button> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void ToggleThis(bool toggle){
		if (myButton == null) {
			Start();
		}

		// 195 dark and 115 light
		if (toggle) {
			/*myPanel.alpha = 1.0f;
			myPanel.interactable = true;
			myPanel.blocksRaycasts = true;*/
			myButton.interactable = false;
			myText.color = myOnText;
		} else {
			/*myPanel.alpha = 0.0f;
			myPanel.interactable = false;
			myPanel.blocksRaycasts = false;*/
			myButton.interactable = true;
			myText.color = myOffText;
		}
	}

	public void OnPointerEnter(PointerEventData data){
		if (myButton.interactable) {
			myText.color = myHoverText;
		}
	}

	public void OnPointerExit(PointerEventData data){
		if (myButton.interactable) {
			myText.color = myOffText;
		}
	}

}
