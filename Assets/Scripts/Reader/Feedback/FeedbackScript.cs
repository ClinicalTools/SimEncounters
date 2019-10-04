using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FeedbackScript : MonoBehaviour
{
    [SerializeField]
    public Transform entryTrans;
    [SerializeField]
    public GameObject shownFeedback;
    [SerializeField]
    protected TextMeshProUGUI shownFeedbackText;
    public Image shownFeedbackBackground;
    //public TextMeshProUGUI loadedFeedbackText;      //The text explaining the feedback response.
    //public TextMeshProUGUI loadedResponseText;     //The response text shown to the user.
    //public TextMeshProUGUI loadedFeedbackResponse; //Correct, Partially Correct, or Incorrect. 
    /*public Dropdown loadedFeedbackResponse;*/ //This is incase I want to do a dropdown for feedback.

    public virtual bool IsOn { get; set; }
    public virtual string Answer { get; set; }
    private string feedback;
    public virtual string Feedback {
        get {
            return feedback;
        }
        set {
            feedback = value?.Trim();
        }
    }
    public virtual string OptionValue { get; set; }
    protected virtual string Evaluation {
        get {
            if (active)
                return OptionValue;
            else if (OptionValue == "Correct")
                return "Incorrect";
            else
                return "Correct";
        }
    }

    private Color correctColor;
    private Color partialCorrectColor;
    private Color incorrectColor;

    /**
	 * This script must be parented to each reader entry which has a button for feedback
	 * and also the parenting entry which shows the feedback.
	 */

    protected virtual void Start()
    {
        correctColor = GlobalData.GDS.correctColor;
        incorrectColor = GlobalData.GDS.incorrectColor;
        partialCorrectColor = GlobalData.GDS.partialCorrectColor;
    }

    public void ReduceAlpha()
    {
        correctColor.a = 0.6f;
        incorrectColor.a = 0.6f;
        partialCorrectColor.a = 0.6f;
    }

    public void DefaultAlpha()
    {
        correctColor.a = 1f;
        incorrectColor.a = 1f;
        partialCorrectColor.a = 1f;
    }

    public void AddStripes(Image i)
    {
        GameObject stripes = Instantiate(Resources.Load("Reader/Prefabs/Stripes") as GameObject, i.transform);
        stripes.name = "Stripes";
        stripes.transform.SetAsFirstSibling();
        //stripes.GetComponent<Image>().color = i.color;
    }


    public void Treatment(bool prescribe)
    {
        if (prescribe) {
            transform.parent.GetChild(1).gameObject.SetActive(!transform.parent.GetChild(1).gameObject.activeInHierarchy);
        } else {
            transform.parent.GetChild(0).gameObject.SetActive(!transform.parent.GetChild(0).gameObject.activeInHierarchy);
        }

        active = prescribe;
        ShowResponse();
    }

    // Whether an action is being performed, if relevant (like prescribing rather than not prescribing
    private bool active = true;


    public virtual void ShowResponse()
    {
        if (shownFeedback == null) {
            //If shown feedback is null, we are a child feedback. 
            FeedbackScript parentScript = transform.parent.GetComponentInParent<FeedbackScript>();
            shownFeedback = parentScript.shownFeedback;
            if (shownFeedbackBackground != null)
                shownFeedbackBackground = parentScript.shownFeedbackBackground;
            //If showing feedback
            shownFeedbackText = parentScript.shownFeedbackText;
        }

        shownFeedback.SetActive(true);

        UpdateColor();
        UpdateText();
    }

    protected virtual void UpdateColor()
    {
        if (shownFeedbackBackground == null)
            return;

        SetColor(shownFeedbackBackground);
    }


    protected virtual void UpdateText()
    {
        shownFeedbackText.text = Output;
    }

    protected virtual string Output => $"{ResponseOutput}\n\n{EvaluationOutput}{FeedbackOutput}";
    protected virtual string ResponsePrefix => IsOn ? "Your" : "Missed Correct";
    protected virtual string ResponseOutput => $"<b>{ResponsePrefix} Response:</b> {Answer}";
    protected virtual string EvaluationOutput => IsOn ? $"<i>{Evaluation}</i>\n" : "";
    protected virtual string FeedbackOutput => Feedback;
       
    /**
	 * This sets the color of the given item based on what shownFeedbackResponse is currently set as
	 */
    public void SetColor(Image i)
    {
        ReduceAlpha();
        switch (Evaluation) {
            case "Correct":
                i.color = correctColor;
                break;
            case "Partially Correct":
                i.color = partialCorrectColor;
                break;
            case "Incorrect":
                i.color = incorrectColor;
                break;
            default:
                i.color = GlobalData.GDS.debugColor;
                break;
        }
        DefaultAlpha();
    }
}
