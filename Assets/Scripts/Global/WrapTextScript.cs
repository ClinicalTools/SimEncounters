using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WrapTextScript : MonoBehaviour {
	public float minimumHeight = 0;

	private float inputHeight = 0;
	private float inputWidth = 0;
	private GUISkin mySkin;

	private bool isPasting = true;

	private InputField mainInputField;
	public InputField nextInputField;

	private GlobalDataScript gData;

	void OnGUI(){
		Event e = Event.current;
		if (e.type == EventType.KeyDown && e.control && e.keyCode == KeyCode.V)
		{
			// CTRL + Z
			isPasting = true;
		}
	}

	// Use this for initialization
	void Awake () {

	}

	void Start () {
		gData = GameObject.Find ("Canvas").GetComponent<GlobalDataScript> ();
		mySkin = Resources.Load (gData.resourcesPath + "/CEStyle") as GUISkin;
		mainInputField = GetComponent<InputField> ();
		mainInputField.onValidateInput += delegate(string input, int charIndex, char addedChar) { return MyValidate(input, charIndex, addedChar ); };

		if (!mainInputField.text.Equals ("")) {
			NextFrame.Function (RelocateText);
		}
	}

	char MyValidate(string input, int charIndex, char charToValidate)
	{
		if (charToValidate == '	')
			return '\0';
		return charToValidate;
	}

	public void RecieveText(InputField newText){
		mainInputField.text = newText.text;
		NextFrame.Function (RelocateText);
	}

	public void ResizeField(){
		if (!isPasting) {
			inputHeight = GetComponent<LayoutElement> ().preferredHeight - 7;

			GUIContent thisContent = new GUIContent ();
			thisContent.text = GetComponent<InputField> ().text;

			float newInputHeight = mySkin.textField.CalcHeight (thisContent, GetComponent<RectTransform> ().sizeDelta.x - 20);

			if (newInputHeight != inputHeight) {
				if (newInputHeight > minimumHeight - 7) {
					inputHeight = newInputHeight;


					if (GetComponent<LayoutElement> () != null) {
						GetComponent<LayoutElement> ().preferredHeight = inputHeight + 7;
					} else {
						Vector2 newSize = new Vector2 (GetComponent<RectTransform> ().sizeDelta.x, inputHeight + 7);

						GetComponent<RectTransform> ().sizeDelta = newSize;
					}
				} else {

					if (GetComponent<LayoutElement> () != null) {
						GetComponent<LayoutElement> ().preferredHeight = minimumHeight;
					} else {
						Vector2 newSize = new Vector2 (GetComponent<RectTransform> ().sizeDelta.x, minimumHeight);

						GetComponent<RectTransform> ().sizeDelta = newSize;
					}
				}
			}
		}
	}

	public void RelocateText(){
		if (!isPasting) {
			bool hasOverflow = false;
			inputHeight = GetComponent<RectTransform> ().rect.height - 7;
			inputWidth = GetComponent<RectTransform> ().rect.width - 20; 

			string[] stringArray = mainInputField.text.Split(new string[] {" "}, System.StringSplitOptions.None);

			string newNextText = "";
			GUIContent thisContent = new GUIContent ();
			thisContent.text = stringArray [0];

            List<string> richTextPrefix = new List<string>();
            List<string> richTextSuffix = new List<string>();

            for (int i = 1; i < stringArray.Length; i++) {
				if (!hasOverflow) {
					thisContent.text = thisContent.text + " " + stringArray [i];

                    if(stringArray[i].Contains("<b>")){
						richTextPrefix.Add("<b>");
                        richTextSuffix.Add("</b>");
					}
					if(stringArray[i].Contains("<i>")){
                        richTextPrefix.Add("<i>");
                        richTextSuffix.Add("</i>");
                    }
					if(stringArray[i].Contains("<color")){
						string[] colorTag = stringArray [i].Split (new string[] { ">" }, System.StringSplitOptions.None);
						for (int j = 0; j < colorTag.Length; j++) {
							if (colorTag [j].StartsWith ("<color")) {
								richTextPrefix.Add(colorTag[j] + ">");
							}
						}
						richTextSuffix.Add("</color>");
					}
					if(stringArray[i].Contains("<size")){
						string[] sizeTag = stringArray [i].Split (new string[] { ">" }, System.StringSplitOptions.None);
						for (int j = 0; j < sizeTag.Length; j++) {
							if (sizeTag [j].StartsWith ("<size")) {
								richTextPrefix.Add(sizeTag[j] + ">");
							}
						}
						richTextSuffix.Add("</size>");
					}

					if(stringArray[i].Contains("</b>") || stringArray[i].Contains("</i>") || stringArray[i].Contains("</color>") || stringArray[i].Contains("</size>")){
						richTextPrefix.RemoveAt(richTextPrefix.Count - 1);
						richTextSuffix.RemoveAt(richTextSuffix.Count - 1);
					}

                    if (mySkin.textField.CalcHeight (thisContent, inputWidth) > inputHeight) {
						for (int j = richTextSuffix.Count -1; j >= 0; j--) {
							thisContent.text = thisContent.text + richTextSuffix[j];
						}

                        for (int j = 0; j < richTextPrefix.Count; j++)
                        {
                            newNextText = newNextText + richTextPrefix[j];
                        }

                        newNextText = newNextText + stringArray [i] + " ";

						hasOverflow = true;
					} else {
					}
				} else {
					newNextText = newNextText + stringArray [i] + " ";
				}
			}

			//mainInputField.text = newMainText;
			nextInputField.text = newNextText;
            mainInputField.text = thisContent.text;
			if (nextInputField.GetComponent<WrapTextScript> ()) {
				nextInputField.GetComponent<WrapTextScript> ().RelocateText ();
			}
		}
	}



	// Update is called once per frame
	void LateUpdate () {
		if (isPasting) {
			isPasting = false;
			RelocateText ();
		}
	}
}
