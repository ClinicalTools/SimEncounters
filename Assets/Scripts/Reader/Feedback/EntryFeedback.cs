using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Feedback that appears directly under an entry, rather than below the question.
/// </summary>
public class EntryFeedback : FeedbackScript
{
    protected GameObject stripes;
    protected Transform feedbackTrans;

    protected override string Output => $"{EvaluationOutput}{FeedbackOutput}";
    protected override string EvaluationOutput => IsOn ? $"<i>{Evaluation}</i>\n" : $"<i>Missed Correct Response</i>\n";

    public override void ShowResponse()
    {
        ClearVariables();
        SetupVariables();

        UpdateColor();
        UpdateText();

        DisplayResponse();
    }

    protected virtual void ClearVariables()
    {
        feedbackTrans = null;
        shownFeedback = null;
        shownFeedbackBackground = null;
        shownFeedbackText = null;

        Answer = "";
        Feedback = "";

        stripes = null;
    }
    protected virtual void SetupVariables()
    {
        if (!feedbackTrans)
            feedbackTrans = entryTrans.Find("Feedback");

        var shownFeedbackTrans = feedbackTrans.Find("Feedback");
        if (!shownFeedback)
            shownFeedback = shownFeedbackTrans.gameObject;
        if (!shownFeedbackBackground)
            shownFeedbackBackground = shownFeedback.GetComponent<Image>();
        if (!shownFeedbackText)
            shownFeedbackText = shownFeedbackTrans.Find("Text").GetComponent<TextMeshProUGUI>();
        if (string.IsNullOrEmpty(Answer))
            Answer = entryTrans.GetComponentInChildren<Toggle>().GetComponentInChildren<TextMeshProUGUI>().text;
        if (string.IsNullOrEmpty(Feedback))
            Feedback = feedbackTrans.Find("FeedbackValue").GetComponent<TextMeshProUGUI>().text;

        if (!stripes)
            stripes = shownFeedbackTrans.Find("Stripes").gameObject;
    }

    protected virtual void DisplayResponse()
    {
        SetColor(shownFeedback.GetComponent<Image>());

        feedbackTrans.gameObject.SetActive(true);
        shownFeedback.gameObject.SetActive(true);
        var canvasGrp = feedbackTrans.GetComponent<CanvasGroup>();
        canvasGrp.alpha = 1;
        canvasGrp.interactable = true;
        canvasGrp.blocksRaycasts = true;
        feedbackTrans.GetComponent<LayoutElement>().ignoreLayout = false;

        stripes.SetActive(!IsOn);
    }
}
