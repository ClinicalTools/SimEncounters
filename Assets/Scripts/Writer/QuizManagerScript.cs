using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManagerScript : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
    }

    public void CloseQuizEditor()
    {
        Destroy(gameObject);
    }

    /**
	 * Called when the cancel button is pressed.
	 * Pass in the Content which holds the HistoryFieldManagerScript for the quizes
	 */
    public void Cancel(HistoryFieldManagerScript Content)
    {
    }
}
