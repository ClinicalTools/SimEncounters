using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewCaseSpawnPanelsScript : MonoBehaviour {

	public List<string> examList;

	void Awake () {
		
	}

	// Use this for initialization
	void Start () {
		//NextFrame.Function (CreateBlanks);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void CreateBlanks () {
		if (GetComponent<HistoryFieldManagerScript> () != null) {
			foreach (string examType in examList) {
				GetComponent<HistoryFieldManagerScript> ().OpenAddEntryPanel ();
			}
			foreach (string examType in examList) {

				if (transform.Find ("LabEntry: " + examList.IndexOf (examType) + "/Row0/PanelNameValue")) {
					transform.Find ("LabEntry: " + examList.IndexOf (examType) + "/Row0/PanelNameValue").GetComponent<TMPro.TextMeshProUGUI> ().text = examType;
				}

				if (transform.Find ("LabEntry: " + examList.IndexOf (examType) + "/Row0/CollapseCheckBoxToggle")) {
					transform.Find ("LabEntry: " + examList.IndexOf (examType) + "/Row0/CollapseCheckBoxToggle").GetComponent<Toggle> ().isOn = false;
				}

				if (transform.Find ("LabEntry: " + examList.IndexOf (examType) + "/Row0/WithinNormalLimitsToggle")) {
					transform.Find ("LabEntry: " + examList.IndexOf (examType) + "/Row0/WithinNormalLimitsToggle").GetComponent<Toggle> ().isOn = true;
				}
			}
		}
	}
}
