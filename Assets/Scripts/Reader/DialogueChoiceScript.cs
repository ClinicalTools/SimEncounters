using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueChoiceScript : MonoBehaviour {

	public TextMeshProUGUI answer;

    public GameObject instructions;
    public GameObject showButton;

	// Use this for initialization
	void Start () {
	}
	
	public void HideBelow()
	{
		for (int i = transform.GetSiblingIndex() + 1; i < transform.parent.childCount; i++) {
			transform.parent.GetChild(i).gameObject.SetActive(false);
		}
		transform.parent.parent.Find("Row2/ExitDialogue").gameObject.SetActive(false);
	}

	public void ShowBelow()
	{
		for(int i = transform.GetSiblingIndex() + 1; i < transform.parent.childCount; i++) {
			transform.parent.GetChild(i).gameObject.SetActive(true);
			if (transform.parent.GetChild(i).GetComponent<DialogueChoiceScript>()) {
				return;
			}
		}
		transform.parent.parent.Find("Row2/ExitDialogue").gameObject.SetActive(true);
	}

	public void SetAnswer(TextMeshProUGUI t)
	{
		answer = t;
	}

	public void CheckAnswer(Toggle choice)
	{
		if (choice.isOn) {
			answer = choice.transform.parent.Find("Feedback/OptionTypeValue").GetComponent<TextMeshProUGUI>(); //For wide choices
			//answer = choice.transform.parent.parent.Find("Feedback/OptionTypeValue").GetComponent<Text>(); //For shrunken choices
			FeedbackScript feedback = GetComponentInChildren<FeedbackScript>();
			feedback.OptionValue = answer.text;
			feedback.shownFeedbackBackground = choice.transform.parent.GetComponent<Image>();
			feedback.Feedback = choice.transform.parent.Find("Feedback/FeedbackBG/FeedbackValue").GetComponent<TextMeshProUGUI>().text;
            feedback.IsOn = choice.isOn;
            //feedback.loadedFeedbackText = choice.transform.parent.parent.Find("Feedback/FeedbackBG/FeedbackValue").GetComponent<Text>();

            feedback.ShowResponse();


			//Comment out these lines to not display per-answer feedback. Remove the first statement to always show C/PC/I
			if (feedback.Feedback.Equals("")) {
				feedback.Feedback = null;
				feedback.GetComponent<CollapseContentScript>().SetTarget(false);
			} else {
				feedback.SetColor(feedback.GetComponent<Image>());
				feedback.GetComponent<CollapseContentScript>().SetTarget(true);
				if (!feedback.OptionValue.Equals("Correct")) {
                    // !!TODO: make sure dialogue choices still work properly
					//feedback.shownFeedbackText = feedback.shownFeedbackText.text + "\n\nPlease try again!";
				}
			}

			if (answer.text.Equals("Correct")) {
                choice.GetComponent<DialogueAnswerChecker>().HideOtherChoices();
                HideInstructions();
				ShowBelow();
				return;
			}
		} else {
            //Comment out this line to keep the color border around previously selected dialogue
            if (choice.transform.parent.GetComponent<Image>()) {
                choice.transform.parent.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            }
		}
	}

    public void HideInstructions()
    {
        if(instructions != null)
        {
            instructions.SetActive(false);
        }

        if(showButton != null)
        {
            showButton.SetActive(true);
        }
    }
}
