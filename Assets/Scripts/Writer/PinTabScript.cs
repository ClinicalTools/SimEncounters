using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class PinTabScript : MonoBehaviour {

	private Dropdown dd;
	public bool doneLoading; //Used to prevent the pins from being added once the dropdown spawns
	private bool hitOnValueInLoad;
	public bool okToLoad;
	public int value;
	public List<string> myOptions;
	private int i;

	// Use this for initialization
	void Start () {
		dd = transform.GetComponent<Dropdown> ();
		doneLoading = false;
		hitOnValueInLoad = false;
		okToLoad = false;
		dd.value = -1;
		dd.captionImage = null;
		foreach(Dropdown.OptionData option in dd.options){
			myOptions.Add (option.text);
		}
		i = 0;
	}

	/**
	 * Testing with using the caption image to correctly get the selected image
	 * Due to toggles being called upon startup with changed values I had to make this really complicated
	 * Just in order to not have them select when opening the dropdown right away. Still not perfect though.
	 */
	public void SelectedItem(Transform item) {
		dd.captionImage = item.GetComponent<Image>();
	}

	/**
	 * Method called when a pin is selected from the dropdown
	 * Pass in the transform of the pin.
	 */
	public void OptionSelected(Transform t) {
		//Debug.Log (t.name + ", " + t.GetComponent<Toggle> ().isOn);
		if (t.GetComponent<Toggle> ().isOn && myOptions.Contains(t.name.Substring (t.name.IndexOf (": ") + 2))) {
			hitOnValueInLoad = true;
			//Debug.Log ("Hit on value");
			if (!doneLoading)
				return;
		}

		foreach (Dropdown.OptionData obj in dd.options) {
			if (obj.text.Equals (t.name.Substring (t.name.IndexOf (": ") + 2))) {
				value = dd.options.FindIndex ((Dropdown.OptionData od) => od == obj);
			}
		}

		foreach (string obj in myOptions) {
			if (obj.Equals (t.name.Substring (t.name.IndexOf (": ") + 2))) {
				value = myOptions.FindIndex ((string s) => s.Equals (obj));
			}
		}

		//Debug.Log(value + ", " + (dd.options.Count - 1));
		//Debug.Log ("OK" + okToLoad);
		if (doneLoading && okToLoad) {
			Debug.Log ("Done loading and adding");
			hitOnValueInLoad = false;
			AddPinToTab ();
			doneLoading = false;
			okToLoad = false;
			i = 0;
			return;
		}


		//dd.captionImage = t.GetChild (0).GetComponent<Image> ();
		//AddPinToTab ();
		if (value == dd.options.Count - 1) {
			Debug.Log ("Done loading.....");
			doneLoading = true;
			if (hitOnValueInLoad) {
				okToLoad = true;
				this.hitOnValueInLoad = false;
				//i = 0;
			}
		}
		i++;
	}

	/**
	 * Adds the selected pin to the PinArea
	 */
     
	public void AddPinToTab() {
		if (value == -1) {
			return;
		}

		if (doneLoading) {
			//Debug.Log ("All info:");
			Toggle[] toggles = transform.GetComponentsInChildren<Toggle> ();
			/*foreach (Toggle t in toggles) {
				foreach (Dropdown.OptionData obj in dd.options) {
					if (obj.text.Equals (t.name.Substring (t.name.IndexOf (": ") + 2))) {
						//Debug.Log ("Obj/toggle text: " + obj.text);
						//Debug.Log ("Obj index: " + dd.options.FindIndex ((Dropdown.OptionData od) => od == obj));
					}
				}
			}*/

			/*Debug.Log (dd.value);
			int value = dd.value;
			if (dd.options.Count == 3) {
				value = (value + 2) % 3;
			}*/
			//Debug.Log (value);

            
			//toggles = transform.GetComponentsInChildren<Toggle> ();
			foreach (Toggle t in toggles) {
				if (t.name.Contains (":") && t.isOn) {
					//Debug.Log ("index: " + dd.options.FindIndex ((Dropdown.OptionData obj) => obj.text.Equals (t.name.Substring (t.name.IndexOf (": ") + 2))));
					if (value == dd.options.FindIndex ((Dropdown.OptionData obj) => (obj.text.Equals (t.name.Substring (t.name.IndexOf (": ") + 2))))) {

						//Debug.Log (t.name + ", " + t.isOn + ", value: " + value);

						foreach (Dropdown.OptionData obj in dd.options) {
							if (obj.text.Equals (t.name.Substring (t.name.IndexOf (": ") + 2))) {
								Transform parent = transform.parent.Find ("PinArea");
								GameObject gObj = Instantiate (t.transform.Find ("Item Background").gameObject, parent);
								dd.options.Remove (obj);
								myOptions.RemoveAt (value);
								doneLoading = false;
                                if (t.name.Substring(t.name.IndexOf(": ") + 2).Equals("Dialogue"))
                                {
                                    //Button b = gObj.AddComponent<Button>();
                                    //b.onClick.AddListener(delegate { ButtonListenerFunctionsScript.OpenDialogueEditor(b); });
                                }
                                else if (t.name.Substring(t.name.IndexOf(": ") + 2).Equals("Quiz"))
                                {
                                    //Button b = gObj.AddComponent<Button>();
                                    //b.onClick.AddListener(delegate { ButtonListenerFunctionsScript.OpenQuizEditor(b); });

                                }
                                else if (t.name.Substring(t.name.IndexOf(": ") + 2).Equals("Flag"))
                                {
                                    //Button b = gObj.AddComponent<Button>();
                                    //b.onClick.AddListener(delegate { ButtonListenerFunctionsScript.OpenFlagEditor(b); });
                                }
                                else if (t.name.Substring(t.name.IndexOf(": ") + 2).Equals("Event"))
                                {
                                    Button b = gObj.AddComponent<Button>();
                                    b.GetComponent<Image>().color = new Color(255f, 0f, 0f);
                                    //b.onClick.AddListener(delegate { ButtonListenerFunctionsScript.OpenEventEditor(b); });

                                }
                                gObj.tag = "Value";
								gObj.name = obj.text + "Pin";
								value = -1;
								dd.value = 0;
								return;
							}

						}

					}

				}

			}

		}
	}
}
