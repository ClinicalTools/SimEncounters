using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;


//Should be parented to the SaveCaseBG Gameobject
public class SaveCaseScript : MonoBehaviour {

    public bool isDynamic;
	public List<string> tags;				//List of all tabs in the system
	private List<string> enteredTags;		//List of tabs that have been selected
	public Transform tagInputField;			//The input field where users enter tags
	public GameObject[] SettingsToggle;
	private Transform SuggestionsParent;	//The transform object for the AutoComplete suggestions
	public Font font;						//The font used for the suggestions
	private Dropdown dd;					//Dropdown for suggestions
	private bool boolSwitch = true;			//Testing value to make it easy to switch b/n dropdown and custom suggestions
	private bool isDeleting;				//Whether or not tags are being deleted
	private List<AutoCompleteEntryScript> tagObjs;		//List of the game objects for each tag suggestion
    //private List<string> combo;             // List for currently existing events AND flags
	private int selectedTagPos;				//Position of the selected tag suggestion
	private int nolooppls;					//For testing. Prevent infinite looping
	private bool preventLooping = false; 	//For debugging/working safety. Will potentially have to implement protection for final version
    private string csvPath;
    //private FlagEventScript fes;            // Flag Event Script for handling flag and event lists
    private InputField input;               // Handles all input field text for tags
	private bool downPressed;
	private bool justAdded;

    /**
	 * Parented to the SaveCaseBG Gameobject, this handles the autocomplete suggestions for tags
	 */

    void Awake() {
		if (tagInputField != null) {
			SuggestionsParent = tagInputField.Find ("Suggestions/Viewport/Content");
			dd = tagInputField.parent.GetComponentInChildren<Dropdown> ();
		}
	}

	/**
	 * Initializes variables
	 */
	void Start() {
        //fes = GameObject.Find("GaudyBG").GetComponent<FlagEventScript>();
		selectedTagPos = -1;
		tagObjs = new List<AutoCompleteEntryScript> ();
		isDeleting = false;
		downPressed = false;
		justAdded = false;
		nolooppls = 0;
		tags = new List<string> ();
		enteredTags = new List<string> ();
		csvPath = Application.streamingAssetsPath + "/Medical Panels/";
        DirectoryInfo dir = new DirectoryInfo(csvPath);
        FileInfo[] info = dir.GetFiles("*.csv");

        if (this.isDynamic == false)
        {
            if (this.name == "EventEditorBG(Clone)")
            {
                //tags.AddRange(fes.events);
            } else if (this.name == "FlagEventEditorBG(Clone)") {
                //tags.AddRange(fes.flags);
            } else if (this.GetComponent<MedicalTestsHandlerScript>().entryParent.name.StartsWith("ColumnLabTests")) {
                foreach (FileInfo f in info)
                {
                    string noExtension = f.Name.Replace(".csv", "");
                    tags.Add(noExtension);
                }
            } else if (this.GetComponent<MedicalTestsHandlerScript>().entryParent.name.StartsWith("TestNameValue")) {
                foreach (FileInfo f in info)
                {
                    StreamReader reader = new StreamReader(csvPath + f.Name);
                    string file = reader.ReadToEnd();
                    string[] lines = file.Split("\n"[0]);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string testName = lines[i].Split(',')[0];
                        tags.Add(testName);
                    }
                    reader.Close();
                }
            } else if (this.name.StartsWith("LabEntry")) {
                tags.AddRange(new List<string> { "Complete Blood Count", "Comprehensive Metabolic Panel", "Lipid Panel" });
            }
        }
        else if (this.isDynamic)
        {
            tags.AddRange(new List<string> { 
				//"AA", "Testing", "Tag1", "Tag2", "Tag3", "Tag33", "Wow look at this tag!", "Wonderful", "Tag333"
			});

			StreamReader reader = new StreamReader (Application.streamingAssetsPath + "/Instructions/Tags.csv");
			string line = reader.ReadLine (); //Skip first line of headers
			while (!reader.EndOfStream) {
				line = reader.ReadLine ();
				if (line.Equals ("END OF FILE")) {
					break;
				} else if (line.Equals ("")) {
					continue;
				}
				tags.Add (line);
			}
        }

        if (tagInputField == null) {
			return;
		}
		input = tagInputField.GetComponent<InputField>();
		//tagInputField = transform.Find ("SaveCasePanel/Content/Row3/TagsValue"); //transform.GetComponentInChildren<InputField>();
		SuggestionsParent = tagInputField.Find("Suggestions/Viewport/Content");
        //Debug.Log(SuggestionsParent);

        if (this.isDynamic)
        {
            //Add tags which were loaded in from XML
            string startingTags = tagInputField.GetComponent<InputFieldOverrideScript>().text;
            if (startingTags != null && !startingTags.Equals(""))
            {
                tagInputField.GetComponent<InputFieldOverrideScript>().text = "";
                string[] selectedTags = Regex.Split(startingTags, "; ");
                //Debug.Log(string.Join(",", selectedTags));
                foreach (string s in selectedTags)
                {
                    if (!s.Equals(""))
                    {
                        AddTag(s);
                    }
                }
            }
        }
	}

	public void ToggleSettings() {
		foreach (GameObject obj in SettingsToggle) {
			obj.SetActive (!obj.activeInHierarchy);
		}
	}

    // Update list of tags for next event/flag entry
    public void UpdateList(Text input)
    {
        bool tagCompare = tags.Contains(input.text);
        //bool flagCompare = fes.flags.Contains(input.text);

        // If it doesn't exist in the list, add it to the list
        if (tagCompare == false)
        {
            tags.Add(input.text);
        }
        
    }

	/**
	 * Checks to see if the user is entering in any tabs and handle things accordingly
	 */
	void Update() {
		if (tagObjs.Count > 0 && SuggestionsParent.gameObject.activeInHierarchy) {
			if (Input.GetKeyDown ("down")) { //Arrow key down. Navigate suggested tags
				if (selectedTagPos < tagObjs.Count - 1) {
					if (selectedTagPos >= 0) {
						tagObjs [selectedTagPos].selected = false;
						tagObjs [selectedTagPos].UpdateImage ();
					}
					selectedTagPos++;
					tagObjs [selectedTagPos].selected = true;
					tagObjs [selectedTagPos].UpdateImage ();
				}
			} else if (Input.GetKeyDown ("up")) { //Arrow key up. Navigate suggested tags
				if (selectedTagPos > 0) {
					tagObjs [selectedTagPos].selected = false;
					tagObjs [selectedTagPos].UpdateImage ();
					selectedTagPos--;
					tagObjs [selectedTagPos].selected = true;
					tagObjs [selectedTagPos].UpdateImage ();
					//tagInputField.GetComponent<InputFieldOverrideScript> ().UpdateCursor ();
					updateCursor (input);
				}
			} else if (Input.GetKeyDown ("right")) { //Submits the chosen tag
				//tagObjs [selectedTagPos].SubmitChoiceToParent ();

				if (selectedTagPos >= 0) {

					AddTag (tagObjs [selectedTagPos].transform.Find ("Text").GetComponent<Text> ().text);
					tagObjs.RemoveRange (0, tagObjs.Count);
					if (SuggestionsParent.childCount > 0) {
						foreach (Transform obj in SuggestionsParent.GetComponentsInChildren<Transform>()) {
							if (obj.name != SuggestionsParent.name)
								Destroy (obj.gameObject);
						}
					}
					tagInputField.GetComponent<InputField> ().Select ();
					//tagInputField.GetComponent<InputField> ().UpdateCursor ();
					updateCursor (input);
					selectedTagPos = -1;

				}
			} else if (Input.GetKeyDown (KeyCode.Tab) && tagInputField.GetComponent<InputField> ().isFocused) {
				tagInputField.GetComponent<InputField> ().text += ";";
				DisplayAutoCompleteResults ();
			} else if (Input.anyKey && !Input.GetMouseButton (0)) { //If any key is pressed other than a click, update the list and its positioning
				if (selectedTagPos >= 0) {
					Scrollbar sb = SuggestionsParent.transform.parent.parent.GetComponentInChildren<Scrollbar> ();
					if (sb == null || tagObjs [0] == null) { //Break if scroll bar not visible or if tagObjs has no accessable object this frame
						return;
					}
					ScrollRect sr = SuggestionsParent.transform.GetComponentInParent<ScrollRect> ();
					if (sr == null) {
						sr = SuggestionsParent.transform.parent.parent.GetComponent<ScrollRect> ();
					}

					Rect srRect = sr.transform.GetComponent<RectTransform> ().rect;
					float contentHeight = SuggestionsParent.transform.GetComponent<RectTransform> ().rect.height;
					float tagBarHeight = tagObjs [0].transform.GetComponent<RectTransform> ().rect.height;
					float topOfViewport = sr.transform.GetComponent<RectTransform> ().rect.height / 2 + sr.transform.position.y;


					if (tagObjs [selectedTagPos].transform.position.y - tagBarHeight * 2 < topOfViewport - sr.transform.GetComponent<RectTransform> ().rect.height) {
						//If the entry is below the scroll bar
						float bottomLine = (((selectedTagPos + 2) * tagBarHeight) - srRect.height) / (contentHeight - srRect.height);
						bottomLine = 1 - Mathf.Clamp (bottomLine, 0, 1);
						sb.value = bottomLine;
					} else {
						if (selectedTagPos == 0) {
							sb.value = 1;
						} else if (tagObjs [selectedTagPos].transform.position.y + tagBarHeight * 2 > topOfViewport) {
							float topLine = (selectedTagPos - 1) * tagBarHeight / (contentHeight - srRect.height);
							topLine = 1 - Mathf.Clamp (topLine, 0, 1);
							sb.value = topLine;
						}
					}
				}
			}
		} else {
			//Arrow key down. Navigate suggested tags
			if (Input.GetKeyDown("down") || Input.GetKeyDown(KeyCode.DownArrow)) {
				if (EventSystem.current.currentSelectedGameObject != null && EventSystem.current.currentSelectedGameObject.transform.IsChildOf(transform)) {
					downPressed = true;
					tagObjs.Clear();
					DisplayAutoCompleteResults();
				}
			}
		}

	}

	public void ShowSuggestionsFromArrow()
	{
		if (SuggestionsParent.gameObject.activeInHierarchy) {
			tagInputField.Find("Suggestions").gameObject.SetActive(false);
		} else {
			tagInputField.Find("Suggestions").GetComponent<GraphicRaycaster>().enabled = true;
			downPressed = true;
			tagObjs.Clear();
			EventSystem.current.SetSelectedGameObject(tagInputField.gameObject, new BaseEventData(EventSystem.current));
			updateCursor(tagInputField.GetComponent<InputField>());
			DisplayAutoCompleteResults();
		}
	}

	/**
	 * Creates the dropdown of suggested tags
	 * This is called when the inputfield value is changed
	 */
	public void DisplayAutoCompleteResults() {
		if (tagObjs == null) {
			Start();
		}

		//Clear previous options
		if (dd != null) {
			dd.ClearOptions ();
			//dd = tagInputField.parent.GetComponentInChildren<Dropdown> ();
		}
		if (SuggestionsParent == null) {
			SuggestionsParent = tagInputField.Find ("Suggestions/Viewport/Content");
		}
        //SuggestionsParent.gameObject.SetActive(true);\
		tagInputField.Find("Suggestions").gameObject.SetActive(true);
		tagInputField.Find("Suggestions").GetComponent<CanvasGroup>().blocksRaycasts = false;
		if (SuggestionsParent.childCount > 0) {
			tagObjs.Clear();
			foreach (Transform obj in SuggestionsParent.GetComponentsInChildren<Transform>()) {
				if (obj.name != SuggestionsParent.name)
					Destroy (obj.gameObject);
			}
		}

		//Get the most recent tag entry
		input = tagInputField.GetComponent<InputField> ();
		string tagInput = input.text;
		if (tagInput.EndsWith ("; ") && !downPressed) { //TagInput only ends with this after successfully entering a tag. 
			//We don't want another runthrough, so return;
			tagObjs.Clear ();
			justAdded = false; //We successfully averted after adding. Set this to false
			tagInputField.Find("Suggestions").gameObject.SetActive(false);
			return;
		}

		//Since adding tags means updating the input text, this automatically gets called again
		//We return so that we don't get any undesired effects.
		if (justAdded) {
			justAdded = false;
			return;
		}

		List<string> selectedTags = new List<string>(Regex.Split(tagInput, "; "));
		if (selectedTags.Count > 0) {
			tagInput = selectedTags [selectedTags.Count - 1]; //Gets the tag that's being input currently
			tagInput = tagInput.TrimStart ();
			//Debug.Log (selectedTags.Count + "," + enteredTags.Count);
		}

		//If there are less tags input than there are entered tags, the user selected and deleted tags
		//So we remove the ones they deleted from the entered tags list
		if (selectedTags.Count < enteredTags.Count || (selectedTags.Count == enteredTags.Count && !tagInput.EndsWith(";"))) {
			List<string> tagsToRemove = new List<string> ();
			foreach (string s in enteredTags) {
				if (!selectedTags.Contains (s))
					tagsToRemove.Add (s);
			}
			foreach (string s in tagsToRemove) {
				enteredTags.Remove (s);
			}
			isDeleting = false; //deleting was already taken care of.

			//To not display results if the user has nothing typed in (after deleting)
			if (!downPressed && input.text.Equals("")) {
				return;
			}
		}

		//To prevent looping which I encountered. Not currently used (preventLooping is false)
		nolooppls++;
		if (nolooppls > 100 && preventLooping) {
			input.text = "";
			nolooppls = 0;
			tagInput = "";
			return;
		}

		//If the user pressed the end tag key or is deleting a tag
		bool endCharPressed = tagInput.EndsWith(";");
		if (endCharPressed) {
			tagInput = tagInput.Remove (tagInput.Length - 1);
		}
		if (endCharPressed && enteredTags.Count > 0) {
			/*
			input.text = "";
			Debug.Log ("Deleting:" + tagInput);
			enteredTags.Remove (enteredTags[0]);

			return;
			*/
		}
		//Debug.Log (string.Join (",", enteredTags.ToArray ()));
		//Debug.Log (isDeleting + "," + justAdded + "," + tagInput);
		if (enteredTags.Contains(tagInput)) {
			Debug.Log (input.text + "....");
			Debug.Log (Regex.Matches (input.text.ToLower (), "\\b" + tagInput.ToLower () + "((;)|$)").Count);

			//If the user is deleting a tag
			if (Regex.Matches (input.text.ToLower (), "\\b" + tagInput.ToLower () + "((;)|$)").Count == 1) {
				enteredTags.Remove (tagInput);
				isDeleting = true;
				Debug.Log ("Deleting..." + tagInput);
				selectedTags [selectedTags.Count - 1] = "";
                input.text = string.Join("; ", selectedTags.ToArray()) + tagInput;
                return;
			} else if (!endCharPressed) { //If the user is trying to add a duplicate tag
				Debug.Log ("Cannot add two duplicate tags!");
				isDeleting = true;
				selectedTags [selectedTags.Count - 1] = "";
                input.text = string.Join("; ", selectedTags.ToArray()) + tagInput;
				return;
			}
		}


		//Find any matching tags from the list of all possible tags. Remove any that have already been entered
		List<string> matchingTags = tags.FindAll (s => s.StartsWith (tagInput, true, null));
		foreach (string tag in enteredTags) {
			if (matchingTags.Contains (tag)) {
				matchingTags.Remove (tag);
			}
		}

		//Ways to detect that a tag has been entered
		if (matchingTags.Count == 1 && !enteredTags.Contains(matchingTags[0])) {
			if (endCharPressed || tagInput.ToLower ().Equals (matchingTags [0].ToLower ())) { //If end char pressed or entered tag matches last available
				if (isDeleting) { //if deleting, do not add duplicate tag
					isDeleting = false;
				} else {
					Debug.Log ("Matching tag found");
					enteredTags.Add (matchingTags [0]);
					selectedTags [selectedTags.Count - 1] = matchingTags [0];
					justAdded = true;
					if (this.isDynamic == false) {
						input.text = string.Join("; ", selectedTags.ToArray()) + "";
					} else {
						input.text = string.Join("; ", selectedTags.ToArray()) + "; ";
					}
					updateCursor(input);
					tagObjs.Clear ();
					tagInputField.Find("Suggestions").gameObject.SetActive(false);
					return;
				}
			}
		}

		//If it gets here, deleting a tag has been handled. Safe to set false (Useful when more than one available tag after deleting)
		isDeleting = false; 

		if (matchingTags.Count == 0) {
			tagInputField.Find("Suggestions").GetComponent<GraphicRaycaster>().enabled = false;
		} else {
			tagInputField.Find("Suggestions").GetComponent<GraphicRaycaster>().enabled = true;
		}

		if (endCharPressed) {
            if (matchingTags.Count > 0) { //If there are more than one matching tags and end char pressed, select the top tag
				Debug.Log ("Matching tag found");
				matchingTags.Sort ();
				enteredTags.Add (matchingTags [0]);
				selectedTags [selectedTags.Count - 1] = matchingTags [0];
				justAdded = true;
				if (this.isDynamic == false) {
					input.text = string.Join("; ", selectedTags.ToArray()) + "";
				} else {
					input.text = string.Join("; ", selectedTags.ToArray()) + "; ";
				}
				updateCursor(input);
				Debug.Log (matchingTags [0]);
				tagObjs.Clear ();
				tagInputField.Find("Suggestions").gameObject.SetActive(false);
				return;
			} else {
				Debug.Log ("No Matching tags found. Please enter another");
				selectedTags [selectedTags.Count - 1] = "";
                if (this.name == "SaveCaseBG")
                {
                    input.text = string.Join("; ", selectedTags.ToArray()) + tagInput;
                }
				return;
			}
		}

		//To not display results if the user has nothing typed in (after deleting)
		if (!downPressed && input.text.Equals("")) {
			return;
		}

		downPressed = false;

		//If no matching tag or end char has not been pressed, then show the available tags 
		int i = 0;
		int max = 4;
		//boolSwitch is true currently
		if (boolSwitch) { //boolSwitch was used to test an actual dropdown vs my custom one. Keeping it like this for now
			if (selectedTagPos > matchingTags.Count - 1) {
				selectedTagPos = -1;
			}
			tagObjs.Clear ();
			matchingTags.Sort ();
			tagInputField.Find("Suggestions").GetComponent<CanvasGroup>().blocksRaycasts = true;
			foreach (string s in matchingTags) {
				GameObject tr = Resources.Load (GlobalData.resourcePath + "/Prefabs/TagsValue") as GameObject;
				GameObject tagSuggestion = Instantiate (tr, SuggestionsParent);
				//tagSuggestion.GetComponent<LayoutElement> ().ignoreLayout = true;
				tagSuggestion.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = s;
				tagSuggestion.name = "Label: " + s;
				tagSuggestion.GetComponent<AutoCompleteEntryScript> ().Initiate(i, false);
				//Rect r = tagSuggestion.GetComponent<RectTransform> ().rect;
				//Rect tagInputFieldRect = tagInputField.GetComponent<RectTransform> ().rect;

				//r.x = -tagInputFieldRect.width / 2 + r.width / 2;
				//r.y = tagInputFieldRect.y - (tagInputFieldRect.height * i) - r.height / 2;
				//tagSuggestion.transform.localPosition = r.position;
				//r.width = tagInputFieldRect.width;
				i++;
				tagObjs.Add (tagSuggestion.GetComponent<AutoCompleteEntryScript>());
				if (i > max) {
					//Uncomment this break line in order to set a max number of suggested tags
					;//break;
				}
			}
			//tagInputField.GetComponentInChildren<ScrollRect> ().verticalScrollbar.value = 1;
			//SuggestionsParent.parent.parent.Find("Scrollbar Vertical").GetComponent<Scrollbar> ().value = 1;
			updateCursor (input);
		} else {
			dd.Hide ();
			dd.ClearOptions ();
			dd.AddOptions (matchingTags);
			//dd.transform.gameObject.SetActive (true);
			dd.RefreshShownValue();

			dd.Show ();
			dd.OnDeselect (null);
			input.Select ();
		    updateCursor (input);
		}
		//input.ForceLabelUpdate ();
	}


	/**
	 * This is to be used when forcing the suggestions to show from a button
	 * Only applicable when not dynamic (aka only one tag is allowed)
	 */
	public void ForceShowSuggestions()
	{
		//Find any matching tags from the list of all possible tags. Remove any that have already been entered
		List<string> matchingTags = new List<string>(tags);

		//Handle case if more than one tag makes its way into the enteredTags list
		if (enteredTags.Count > 1) {
			enteredTags.RemoveAt(0);
		}

		foreach (string tag in enteredTags) {
			print(tag);
			if (matchingTags.Contains(tag)) {
				matchingTags.Remove(tag);
			}
		}

		tagObjs.Clear();

		if (matchingTags.Count == tags.Count - enteredTags.Count && matchingTags.Count == SuggestionsParent.childCount && tagInputField.Find("Suggestions").gameObject.activeInHierarchy) {
			foreach (Transform obj in SuggestionsParent.GetComponentsInChildren<Transform>()) {
				if (obj.name != SuggestionsParent.name)
					Destroy(obj.gameObject);
			}
			tagInputField.Find("Suggestions").gameObject.SetActive(false);
			return;
		}

		if (SuggestionsParent.childCount > 0) {
			foreach (Transform obj in SuggestionsParent.GetComponentsInChildren<Transform>()) {
				if (obj.name != SuggestionsParent.name)
					Destroy(obj.gameObject);
			}
		}

		tagInputField.Find("Suggestions").GetComponent<GraphicRaycaster>().enabled = true;
		tagInputField.Find("Suggestions").gameObject.SetActive(true);
		tagInputField.Find("Suggestions").GetComponent<CanvasGroup>().blocksRaycasts = false;

		//If no matching tag or end char has not been pressed, then show the available tags 
		int i = 0;
		int max = 4;
		if (selectedTagPos > matchingTags.Count - 1) {
			selectedTagPos = -1;
		}
		matchingTags.Sort();
		print(matchingTags.Count);
		tagInputField.Find("Suggestions").GetComponent<CanvasGroup>().blocksRaycasts = true;
		GlobalData.resourcePath = "Writer";
		foreach (string s in matchingTags) {
			GameObject tr = Resources.Load(GlobalData.resourcePath + "/Prefabs/TagsValue") as GameObject;
			GameObject tagSuggestion = Instantiate(tr, SuggestionsParent);
			tagSuggestion.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = s;
			tagSuggestion.name = "Label: " + s;
			tagSuggestion.GetComponent<AutoCompleteEntryScript>().Initiate(i, false);
			i++;
			tagObjs.Add(tagSuggestion.GetComponent<AutoCompleteEntryScript>());
			if (i > max) {
				//Uncomment this break line in order to set a max number of suggested tags
				//break;
			}
		}
		//tagInputField.GetComponentInChildren<ScrollRect> ().verticalScrollbar.value = 1;
		//SuggestionsParent.parent.parent.Find("Scrollbar Vertical").GetComponent<Scrollbar> ().value = 1;
		updateCursor(input);
	}

	public bool IsSafeToPublish() {
		string input = tagInputField.GetComponent<InputField> ().text;
		string[] inputTags = input.Split (new string[] {"; "}, System.StringSplitOptions.RemoveEmptyEntries);
		if (inputTags.Length == 0) {
			return true;
		}
		input = inputTags [inputTags.Length - 1];
		if (!tags.Contains (input)) {
			return false;
		}
		return true;
	}

    private void updateCursor(InputField input)
    {
        input.MoveTextEnd(false);
        input.selectionAnchorPosition = input.selectionFocusPosition = input.caretPosition;
    }

    /**
	 * Adds a tag to the list of entered (chosen) tags
	 */
    public void AddTag(string tag) {
		if (!enteredTags.Contains(tag)) {
			input = tagInputField.GetComponent<InputField> ();
			string[] tags = Regex.Split(input.text, "; ");
			tags[tags.Length - 1] = tag;
			justAdded = true;
			enteredTags.Add(tag);
			tagObjs.Clear();
			if (this.isDynamic == false)
			{
				input.text = string.Join("; ", tags) + "";
			}
			else
			{
				input.text = string.Join("; ", tags) + "; ";
			}

			tagInputField.Find("Suggestions").GetComponent<CanvasGroup>().blocksRaycasts = false;
			tagInputField.Find("Suggestions").gameObject.SetActive(false);
		}
		updateCursor(input);
	}
}
