public class DialogueFeedback : FeedbackScript
{
    protected override string Output {
        get {
            var str = $"{EvaluationOutput}{FeedbackOutput}";
            if (OptionValue != "Correct")
                str += $"\n\nPlease try again!";
            
            return str;
        }
    }
}
