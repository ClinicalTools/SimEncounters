using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class UpdateName : MonoBehaviour {

	public GameObject charEditor;
	private string firstName;
	private string lastName;

	/**
	 * Most of the script has been disabled. uncomment everything and remove any early return statements to bring anything back
	 */

	void Awake() {
		//charEditor = GameObject.Find ("Canvas").transform.Find ("CharacterEditorPanel").gameObject;
	}

	// Use this for initialization
	void Start () {
		firstName = GlobalData.firstName;
		lastName = GlobalData.lastName;
		NextFrame.Function (AssignName);
		/*charEditor = GameObject.Find ("Canvas").transform.Find ("CharacterEditorPanel").gameObject;
		if (charEditor == null) {
			print ("HI");
			charEditor = GameObject.Find ("Canvas").transform.GetChild (0).gameObject;
		}*/
	}

	void AssignName() {
		//return;
		if (firstName != null && !firstName.Equals ("")) {
			//print(transform.name + ": " + GlobalData.firstName + ", " + GlobalData.lastName);
			switch(transform.name) {
			case "FirstNameValue":
				transform.GetComponent<TMPro.TMP_InputField> ().text = firstName;
				break;
			case "LastNameValue":
				transform.GetComponent<TMPro.TMP_InputField> ().text = lastName;
				break;
			/*default:
				transform.GetComponent<UnityEngine.UI.InputField> ().text = GlobalData.firstName + " " + GlobalData.lastName;
				break;*/
			}
		}
	}

	public void UpdateFirstName() {
		GlobalData.firstName = transform.GetComponent<TMPro.TMP_InputField> ().text;
	}

	public void UpdateLastName() {
		GlobalData.lastName = transform.GetComponent<TMPro.TMP_InputField> ().text;
	}

	public void UpdateNameFromCharacterEditor() {
		/*string[] name = transform.GetComponent<UnityEngine.UI.InputField> ().text.Split (' ');
		GlobalData.firstName = transform.GetComponent<UnityEngine.UI.InputField> ().text.Split (' ') [0];
		if (name.Length > 1) {
			GlobalData.lastName = transform.GetComponent<UnityEngine.UI.InputField> ().text.Split (' ') [1];
		}*/
	}

	public void UpdateHeight(Text t) {
		/*return; //Disabling this for now
		if (charEditor == null) {
			return;
		}
		DataScript ds = GameObject.Find ("GaudyBG").GetComponent<DataScript> ();
		TabManager tm = ds.GetComponent<TabManager> ();
		if (charEditor.activeInHierarchy) {
			if (ds.GetData (tm.getCurrentSection ()).GetCurrentTab ().type.Equals ("Personal Info")) {
				tm.TabContentPar.transform.Find ("Personal InfoTab/Scroll View/Viewport/Content/LabEntry: 3/Row2/HeightValue").GetComponent<InputField> ().text = t.text;
				tm.TabContentPar.transform.Find ("Personal InfoTab/Scroll View/Viewport/Content/LabEntry: 3/Row2/BMIValue").GetComponent<BMICalcScript> ().UpdateBMI ();
			} else {
				//Replace the data in Personal Info Tab's data. Must find the reference to the tabinfoscript to do so.
				TabInfoScript tabInfo;
				foreach (string section in ds.GetSectionsList()) {
					foreach (string tab in ds.GetData(section).GetTabList()) {
						if ((tabInfo = ds.GetData (section).GetTabInfo (tab)).type.Equals ("Personal Info")) {
							tabInfo.data = Regex.Replace (tabInfo.data, "<HeightValue>\\d*</HeightValue>", "<HeightValue>" + t.text + "</HeightValue>");
							return;
						}
					}
				}
			}
		} else {
			charEditor.transform.Find ("SidePanel/MainPanel/BodyEditorPanel/Height/Row0/HeightValue").GetComponent<InputField> ().text = t.text;
			charEditor.transform.Find ("SidePanel/MainPanel/BodyEditorPanel/Height/Row1/HeightSlider").GetComponent<Slider> ().value = float.Parse(t.text);
		}*/
	}

	public void UpdateWeight(Text t) {
		return; //Disabling this for now
		/*
		if (charEditor == null) {
			return;
		}
		DataScript ds = GameObject.Find ("GaudyBG").GetComponent<DataScript> ();
		TabManager tm = ds.GetComponent<TabManager> ();
		if (charEditor.activeInHierarchy) {
			if (ds.GetData (tm.getCurrentSection ()).GetCurrentTab ().type.Equals ("Personal Info")) {
				tm.TabContentPar.transform.Find ("Personal InfoTab/Scroll View/Viewport/Content/LabEntry: 3/Row2/WeightValue").GetComponent<InputField> ().text = t.text;
				tm.TabContentPar.transform.Find ("Personal InfoTab/Scroll View/Viewport/Content/LabEntry: 3/Row2/BMIValue").GetComponent<BMICalcScript> ().UpdateBMI ();
			} else {
				//Replace the data in Personal Info Tab's data. Must find the reference to the tabinfoscript to do so.
				TabInfoScript tabInfo;
				foreach (string section in ds.GetSectionsList()) {
					foreach (string tab in ds.GetData(section).GetTabList()) {
						if ((tabInfo = ds.GetData (section).GetTabInfo (tab)).type.Equals ("Personal Info")) {
							tabInfo.data = Regex.Replace (tabInfo.data, "<WeightValue>\\d*</WeightValue>", "<WeightValue>" + t.text + "</WeightValue>");
							return;
						}
					}
				}
			}
		} else {
			charEditor.transform.Find ("SidePanel/MainPanel/BodyEditorPanel/Weight/Row0/WeightValue").GetComponent<InputField> ().text = t.text;
			charEditor.transform.Find ("SidePanel/MainPanel/BodyEditorPanel/Weight/Row1/WeightSlider").GetComponent<Slider> ().value = float.Parse(t.text);
		}*/
	}

	public void UpdateAge(Text t) {
		return; //Disabling this for now
		/*
		if (charEditor == null) {
			return;
		}
		DataScript ds = GameObject.Find ("GaudyBG").GetComponent<DataScript> ();
		TabManager tm = ds.GetComponent<TabManager> ();
		if (charEditor.activeInHierarchy) {
			if (ds.GetData (tm.getCurrentSection ()).GetCurrentTab ().type.Equals ("Personal Info")) {
				tm.TabContentPar.transform.Find ("Personal InfoTab/Scroll View/Viewport/Content/LabEntry: 0/Row3/AgeValue").GetComponent<InputField> ().text = t.text;
			} else {
				//Replace the data in Personal Info Tab's data. Must find the reference to the tabinfoscript to do so.
				TabInfoScript tabInfo;
				foreach (string section in ds.GetSectionsList()) {
					foreach (string tab in ds.GetData(section).GetTabList()) {
						if ((tabInfo = ds.GetData (section).GetTabInfo (tab)).type.Equals ("Personal Info")) {
							tabInfo.data = Regex.Replace (tabInfo.data, "<AgeValue>\\d*</AgeValue>", "<AgeValue>" + t.text + "</AgeValue>");
							return;
						}
					}
				}
			}
		} else {
			charEditor.transform.Find ("SidePanel/MainPanel/BodyEditorPanel/Age/Row0/AgeValue").GetComponent<InputField> ().text = t.text;
			charEditor.transform.Find ("SidePanel/MainPanel/BodyEditorPanel/Age/Row1/AgeSlider").GetComponent<Slider> ().value = float.Parse(t.text);
		}*/
	}

	public void UpdateWeightNextFrame(Text t) {
		return; //Disabling this for now
		/*
		if (charEditor == null)
			Awake ();
		if (charEditor.activeInHierarchy) {
			NextFrame.Function (delegate {
				UpdateWeight (t);
			});
		}*/
	}

	public void UpdateHeightNextFrame(Text t) {
		return; //Disabling this for now
		/*
		if (charEditor == null)
			Awake ();
		if (charEditor.activeInHierarchy) {
			NextFrame.Function (delegate {
				UpdateHeight (t);
			});
		}*/
	}

	public void UpdateAgeNextFrame(Text t) {
		//NextFrame.Function (delegate{UpdateAge(t);});
	}

	public void UpdateValues() {
		/*return; //Disabling this for now

		string charHeight = "";
		string charWeight = "";
		string charAge = "";


		if (charEditor == null) {
			return;
		}
		DataScript ds = GameObject.Find ("GaudyBG").GetComponent<DataScript> ();
		TabManager tm = ds.GetComponent<TabManager> ();
		if (charEditor.activeInHierarchy) {
			if (ds.GetData (tm.getCurrentSection ()).GetCurrentTab ().type.Equals ("Personal Info")) {
				string personalInfoHeight = "Personal InfoTab/Scroll View/Viewport/Content/LabEntry: 3/Row2/HeightValue";
				string personalInfoWeight = "Personal InfoTab/Scroll View/Viewport/Content/LabEntry: 3/Row2/WeightValue";
				string personalInfoBMI = "Personal InfoTab/Scroll View/Viewport/Content/LabEntry: 3/Row2/BMIValue";
				string personalInfoAge = "Personal InfoTab/Scroll View/Viewport/Content/LabEntry: 0/Row3/AgeValue";

				tm.TabContentPar.transform.Find (personalInfoWeight).GetComponent<InputField> ().text = charWeight;
				tm.TabContentPar.transform.Find (personalInfoHeight).GetComponent<InputField> ().text = charHeight;
				tm.TabContentPar.transform.Find (personalInfoAge).GetComponent<InputField> ().text = charAge; 
				tm.TabContentPar.transform.Find ("Personal InfoTab/Scroll View/Viewport/Content/LabEntry: 3/Row2/WeightValue").GetComponent<BMICalcScript> ().UpdateBMI ();


			} else {
				//Replace the data in Personal Info Tab's data. Must find the reference to the tabinfoscript to do so.
				TabInfoScript tabInfo;
				foreach (string section in ds.GetSectionsList()) {
					foreach (string tab in ds.GetData(section).GetTabList()) {
						if ((tabInfo = ds.GetData (section).GetTabInfo (tab)).type.Equals ("Personal Info")) {
							//tabInfo.data = Regex.Replace (tabInfo.data, "<WeightValue>\\d*</WeightValue>", "<WeightValue>" + t.text + "</WeightValue>");
							return;
						}
					}
				}
			}
		} else {
			//charEditor.transform.Find ("SidePanel/MainPanel/BodyEditorPanel/Weight/Row0/WeightValue").GetComponent<InputField> ().text = t.text;
			//charEditor.transform.Find ("SidePanel/MainPanel/BodyEditorPanel/Weight/Row1/WeightSlider").GetComponent<Slider> ().value = float.Parse(t.text);
		}*/
	}
}
