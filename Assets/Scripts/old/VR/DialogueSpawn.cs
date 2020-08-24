using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSpawn : MonoBehaviour {
    public GameObject prefabButton;
    public RectTransform parentPanel;
    public XMLManager dl;
    public CanvasGroup selectedButton, doctorVoice, patientVoice;
    public GameObject optionsButton, nextButton, optionsText;

    private string[] textArr;

    private int dOptions, currOption, currText = 0;

	// Use this for initialization
	void Start () {
        nextButton.SetActive(false);
        optionsText.SetActive(false);
    }

    // Spawn the options before dialogue occurs
    public void createOptions()
    {
        string[] listOfOptions = dl.getOptionArray();
        dOptions = listOfOptions.Length;
        optionsText.GetComponentInChildren<Text>().text = dl.getTitle(); // Gets the choice text
        optionsText.SetActive(true);

        // For each dialogue option, instantiate a new button, set its parent to the panel, assign text 
        for (int i = 0; i < dOptions; i++)
        {
            Vector3 buttonPos;

            GameObject newButton = (GameObject)Instantiate(prefabButton);
            optionsText.GetComponent<CanvasGroup>().alpha = 1;
            newButton.transform.SetParent(parentPanel);
            buttonPos = newButton.transform.localPosition;

            newButton.transform.localScale = new Vector3(1, 1, 1);
            newButton.transform.localPosition = new Vector3(buttonPos.x, buttonPos.y, 0);
            newButton.GetComponent<DialogueSelect>().dSpawn = this;
            newButton.GetComponentInChildren<Text>().text = listOfOptions[i];

        }

    }

    // Fade all the options but the one selected
    public void optionsFade(DialogueSelect ds)
    {
        // Go through all children and fade all except for the choice text and the selected button
        for (int i = 0; i < parentPanel.transform.childCount; i++)
        {
            CanvasGroup currChild = parentPanel.transform.GetChild(i).gameObject.GetComponent<CanvasGroup>();
            if (currChild.name == "OptionText")
            {
                StartCoroutine(FadeOut(currChild, 1.0f));
                //optionsText.SetActive(false);
            }
            else if (currChild.GetComponent<DialogueSelect>().isSelected == false)
            {
                StartCoroutine(FadeOut(currChild, 1.0f));
            }
            else
            {
                selectedButton = currChild;
                currOption = i-1;
                textArr = dl.getTextArray(currOption);
                Debug.Log(currOption);
            }
        }

        ds.isSelected = false; // Set the selected button to false after the others are faded
        doctorVoice.GetComponentInChildren<Text>().text = textArr[currText]; // Get the text array for the option chosen
        StartCoroutine(FadeIn(doctorVoice, 1.0f)); // Bring in the doctor's dialogue
        currText++;
        optionsButton.SetActive(false);
        nextButton.SetActive(true);

    }

    // Clears the prior dialogue options to make room for the next branch
    public void wipeOptions()
    {
        foreach (Transform child in parentPanel.transform)
        {
            if (child.name == "OptionText")
            {
                // do nothing
            }
            else
            {
                GameObject.Destroy(child.gameObject);
                Debug.Log(child.name);
            }

        }
    }

    // Iterate to the next dialogue when the Next Button is pressed
    public void nextDialogue()
    {
        // If all of the text has been iterated through, fade the speech bubbles and restart with the next branch
        if (currText == textArr.Length)
        {
            optionsButton.SetActive(true);
            nextButton.SetActive(false);
            StartCoroutine(FadeOut(patientVoice, 1.0f));
            StartCoroutine(FadeOut(doctorVoice, 1.0f));
            currText = 0;
            wipeOptions();
            return;
        }

        // Bring up the patient dialogue the first time the doctor dialogue appears
        else if (currText == 1)
        {
            patientVoice.GetComponentInChildren<Text>().text = textArr[currText];
            StartCoroutine(FadeIn(patientVoice, 1.0f));
            StartCoroutine(FadeOut(selectedButton, 1.0f));
        }

        // Every even text is doctor dialogue
        else if (currText % 2 == 0)
        {
            doctorVoice.GetComponentInChildren<Text>().text = textArr[currText];
            StartCoroutine(FadeOut(patientVoice, 1.0f));
        }

        //Every odd text is patient dialogue
        else if (currText % 2 != 0)
        {
            patientVoice.GetComponentInChildren<Text>().text = textArr[currText];
            StartCoroutine(FadeIn(patientVoice, 1.0f));
        }

        currText++;
    }

    // Fade coroutines for buttons
    IEnumerator FadeOut(CanvasGroup cg, float fadeSpeed)
    {
        while (cg.alpha > 0)
        {
            cg.alpha -= Time.deltaTime / fadeSpeed;
            yield return null;
        }
    }

    IEnumerator FadeIn(CanvasGroup cg, float fadeSpeed)
    {
        while (cg.alpha < 1)
        {
            cg.alpha += Time.deltaTime / fadeSpeed;
            yield return null;
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
