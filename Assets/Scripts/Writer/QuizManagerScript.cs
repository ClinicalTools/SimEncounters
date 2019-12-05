using ClinicalTools.SimEncounters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManagerScript : MonoBehaviour
{

    private WriterHandler ds;
    // Use this for initialization
    void Start()
    {
        ds = WriterHandler.WriterInstance;
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
        if (!ds.GetQuizes().ContainsKey(Content.RefreshUniquePath())) {
            if (Content.GetPin().transform.Find("Item Background Off")) {
                Content.GetPin().transform.Find("Item Background Off").gameObject.SetActive(true);
                Content.GetPin().transform.Find("Item Background On").gameObject.SetActive(false);
            } else {
                Destroy(Content.GetPin());
            }
        }
        CloseQuizEditor();
    }
}
