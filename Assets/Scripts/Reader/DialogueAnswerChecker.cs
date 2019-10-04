using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueAnswerChecker : MonoBehaviour {

    public Color colorOn;
    public Color colorOff;
    public Sprite bubbleOn;
    public Sprite bubbleOff;

    private List<Transform> othertoggles;

    private void Start()
    {
        othertoggles = new List<Transform>();

        Transform[] siblings = GetComponent<Toggle>().group.transform.gameObject.GetComponentsInDirectChildren<Transform>();

        foreach(Transform sibling in siblings)
        {
            //if(sibling != transform.parent)
			if (!transform.IsChildOf(sibling))
            {
                othertoggles.Add(sibling);
            }
        }

    }

    public void CheckAnswer(Toggle t)
	{
        GetComponentInParent<DialogueChoiceScript>()?.CheckAnswer(t);

        if (t.isOn)
        {
            GetComponent<Image>().sprite = bubbleOn;
            GetComponent<Image>().color = colorOn;
            t.targetGraphic.GetComponent<LayoutElement>().enabled = true;
        } else
        {
            GetComponent<Image>().sprite = bubbleOff;
            GetComponent<Image>().color = colorOff;
            t.targetGraphic.GetComponent<LayoutElement>().enabled = false;
        }

	}

    public void HideOtherChoices()
    {
        foreach (Transform sibling in othertoggles)
        {
            CanvasGroup cg = sibling.GetComponent<CanvasGroup>();

            cg.alpha = 0.0f;
            cg.interactable = false;
            cg.blocksRaycasts = false;
            sibling.GetComponent<LayoutElement>().ignoreLayout = true;
            sibling.GetComponent<HorizontalLayoutGroup>().enabled = false;
        }
    }
}
