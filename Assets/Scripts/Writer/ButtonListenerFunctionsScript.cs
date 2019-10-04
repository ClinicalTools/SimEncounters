using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ButtonListenerFunctionsScript
{

    /**
	 * Reference this script for any global methods that need to be called by different buttons.
	 * Currently it is used mainly for pin buttons
	 */

    private static GameObject pinObject;
    private static Transform pinArea;


    // Get the current Pin Area and pin that is being used for an editor pane
    public static void assign(Transform pArea, GameObject pObject)
    {
        pinArea = pArea;
        pinObject = pObject;
        addPin();
    }

    // Move the pin to the pin area
    public static void addPin()
    {
        pinObject.transform.SetParent(pinArea);
    }

    /**
	 * Opens the dialogue editor
	 * Pass in the pin button pressed.
	 */
    public static void OpenDialogueEditor(Button b)
    {//GameObject panel) {
        if (Input.GetMouseButton(1)) { //If right clicked
            Debug.Log("OKAY");
            return;
        }
        Transform t = GameObject.Find("GaudyBG").transform.Find("DialogueEditorBG");
        t.gameObject.SetActive(true);
        t.GetComponentInChildren<HistoryFieldManagerScript>().GrabPin(b.transform);
        t.GetComponentInChildren<DialogueManagerScript>().PopulateDialogue(b.transform);

        //panel.GetComponentInChildren<DialogueManagerScript> ().PopulateDialogue (b.transform);
        //panel.GetComponentInChildren<HistoryFieldManagerScript>().GrabPin(b.transform);
    }

    // Opens the Flag Editor after pin is pressed
    public static void OpenFlagEditor(Button b, GameObject panel)
    {
        //Transform t = GameObject.Find("GaudyBG").transform.Find("FlagEditorBG");
        //t.gameObject.SetActive(true);
        panel.GetComponentInChildren<HistoryFieldManagerScript>().GrabPin(b.transform);
    }

    // Opens the Event Editor after pin is pressed
    public static void OpenEventEditor(Button b, GameObject panel)
    {
        //Transform t = GameObject.Find("GaudyBG").transform.Find("EventEditorBG");
        //t.gameObject.SetActive(true);
        panel.GetComponentInChildren<HistoryFieldManagerScript>().GrabPin(b.transform);
    }

    /**
	 * Method called by the added quiz pin to open the quiz editor
	 */
    public static void OpenQuizEditor(Button b, GameObject panel)
    {
        //Transform t = GameObject.Find ("GaudyBG").transform.Find ("QuizEditorBG");
        //t.gameObject.SetActive (true);
        panel.GetComponentInChildren<HistoryFieldManagerScript>().GrabPin(b.transform);
        //Transform questionParent = t.Find ("QuizEditorPanel/Content/ScrollView/Viewport/Content");
        Transform questionParent = panel.transform.Find("QuizEditorPanel/Content/ScrollView/Viewport/Content");
        List<Transform> previousQuestions = new List<Transform>();
        for (int i = 0; i < questionParent.childCount; i++) {
            previousQuestions.Add(questionParent.GetChild(i));
        }
        foreach (Transform trans in previousQuestions) {
            trans.gameObject.SetActive(false);
            GameObject.Destroy(trans.gameObject);
        }

        //Must make the quiz panel work. 
        panel.GetComponentInChildren<HistoryFieldManagerScript>().PopulateQuiz(b.transform);
    }

}
