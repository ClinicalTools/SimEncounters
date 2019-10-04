using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;
using TMPro;


public class ToolTipInfoScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public string TitleText = "";			//Title of the tool tip
	public string DescriptionText = "";		//Description of the tool tip
	public float Delay;					//The delay before the tool tip shows (default is 1 second)
	public CanvasGroup ToolTipCanvas;	//The canvas group which holds the Tool tip that is shown on screen
	public string tooltipName = "";
	private TextMeshProUGUI Title;					//The title text object on the ToolTipCanvas
	private TextMeshProUGUI Description;			//The description text object on the ToolTipCanvas

	/**
	 * Script for the tool tips that show when hovering the mouse over certain elements
	 * This is attached to objects that will have the tool tip display when hovered over
	 */

	// Use this for initialization
	void Start () {
		if (!tooltipName.Equals("")) {
			string fullPath = Application.streamingAssetsPath + "/Instructions/Tooltips.csv";
			try {
				StreamReader reader = new StreamReader(fullPath);

				string lineText = reader.ReadLine();
				while (!reader.EndOfStream) {
					string[] line = lineText.Split(',');
					if (line[0].Equals(tooltipName)) {
						TitleText = line[1];
						DescriptionText = line[2];
						break;
					}
					lineText = reader.ReadLine();
				}
				reader.Close();
			} catch (DirectoryNotFoundException e) {
				Debug.LogWarning(e.Message);
			} catch (FileNotFoundException e) {
				Debug.LogWarning(e.Message);
			}
		}

		if (TitleText.Equals ("") && DescriptionText.Equals ("")) {
			enabled = false;
		}

		if (ToolTipCanvas == null) {
			if (GlobalData.toolTip != null) {
				ToolTipCanvas = GlobalData.toolTip;
			} else {
				ToolTipCanvas = GameObject.Find ("Tooltip").GetComponent<CanvasGroup> ();
			}
		}
		Title = ToolTipCanvas.transform.Find ("ToolTipTitle").GetComponent<TextMeshProUGUI> ();
		Description = ToolTipCanvas.transform.Find ("ToolTipDescription").GetComponent<TextMeshProUGUI> ();
		if (Delay <.25f) {
			Delay = 1.0f;
		}

		if (GetComponent<Button> ()) {
			GetComponent<Button> ().onClick.AddListener (ClickedButton);
		}
	}

	/**
	 * When the pointer starts hovering, start counting
	 */
	public void OnPointerEnter(PointerEventData data) {
		if (data.pointerEnter.name.Equals (this.name)) {
			InvokeRepeating ("UpdateText", .25f, 5.0f);
		}
	}

	/**
	 * Updates the text and resizes the tool tip before it is displayed
	 */
	private void UpdateText() {
		if (TitleText != null && !TitleText.Equals ("")) {
			Title.gameObject.SetActive (true);
			Title.text = TitleText;
		}
		if (DescriptionText != null && !DescriptionText.Equals ("")) {
			Description.gameObject.SetActive (true);
			Description.text = DescriptionText;
		}
		CancelInvoke ();
		InvokeRepeating ("ShowPopup", Delay - .25f, 5.0f);
	}

	private void ClickedButton () {
		CancelInvoke ();
		ToolTipCanvas.alpha = 0;
		Description.gameObject.SetActive (false);
		Title.gameObject.SetActive (false);
	}

	/**
	 * When the pointer stops hovering
	 */
	public void OnPointerExit(PointerEventData data) {
		CancelInvoke ();
		ToolTipCanvas.alpha = 0;
		Description.gameObject.SetActive (false);
		Title.gameObject.SetActive (false);
	}

	/**
	 * Show the tool tip
	 */
	private void ShowPopup() {
		ToolTipCanvas.alpha = 1;
		CancelInvoke ();
	}
}
