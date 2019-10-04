using TMPro;

public class TreatmentFeedback : EntryFeedback
{
    public virtual string Outcome { get; set; }

    protected override string Output {
        get {
            if (OptionValue == "Correct" && !string.IsNullOrEmpty(Outcome))
                return $"{EvaluationOutput}{OutcomeOutput}\n{FeedbackOutput}";
            else
                return base.Output;
        }
    }
    protected virtual string OutcomeOutput => $"<b>Treatment Outcome:</b>\n{Outcome}";
    protected override string FeedbackOutput => $"<b>Feedback:</b>\n{Feedback}";

    protected override void SetupVariables()
    {
        base.SetupVariables();

        Outcome = feedbackTrans.Find("OutcomeValue").GetComponent<TextMeshProUGUI>().text;
    }
}
