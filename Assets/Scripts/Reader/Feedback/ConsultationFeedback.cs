using TMPro;

public class ConsultationFeedback : EntryFeedback
{
    public virtual string ConsultantResponse { get; set; }

    protected override string Output {
        get {
            if (!string.IsNullOrEmpty(ConsultantResponse))
                return $"{EvaluationOutput}{ConsultantResponseOutput}\n{FeedbackOutput}";
            else
                return base.Output;
        }
    }
    protected virtual string ConsultantResponseOutput => $"<b>Consultant Response:</b>\n{ConsultantResponse}";
    protected override string FeedbackOutput => $"<b>Feedback:</b>\n{Feedback}";

    protected override void SetupVariables()
    {
        base.SetupVariables();

        ConsultantResponse = feedbackTrans.Find("ResponseValue").GetComponent<TextMeshProUGUI>().text;
    }
}
