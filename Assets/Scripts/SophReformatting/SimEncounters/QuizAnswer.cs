using System.Collections.Generic;

namespace ClinicalTools.SimEncounters
{
    public class QuizAnswer
    {
        public string QuestionKey { get; }
        public List<string> AnswerKeys { get; } = new List<string>();

        public QuizAnswer(string questionKey)
        {
            QuestionKey = questionKey;
        }
    }
}