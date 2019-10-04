using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class QuizQuestionScript
{

    public string questionText;
    public string questionType; //Multiple choice, checkbox, etc.
    public string image; //Image reference string
    public List<Answer> answers;

    public QuizQuestionScript()
    {
        answers = new List<Answer>();
    }

    public QuizQuestionScript(string question)
    {
        questionText = question;
        answers = new List<Answer>();
    }

    public string GetResponse(string answer)
    {
        foreach (Answer ans in answers) {
            if (ans.EqualsAnswer(answer)) {
                var response = ans.response;
                // Prevent starting with the feedback string to avoid redundancy
                if (response.StartsWith(ans.feedback, System.StringComparison.CurrentCultureIgnoreCase)) {
                    response = response.Substring(ans.feedback.Length).Trim();

                    // Remove punctuation mark
                    while (response.Length > 0 && (response[0] == '!' || response[0] == ',' || response[0] == '.')) {
                        response = response.Substring(1).Trim();
                    }
                }

                return response;
            }
        }

        return answers.Find((Answer obj) => obj.EqualsAnswer(answer)).response;
    }

    public string GetFeedback(string answer)
    {
        foreach (Answer ans in answers) {
            if (ans.EqualsAnswer(answer)) {
                return ans.feedback;
            }
        }
        return answers.Find((Answer obj) => obj.EqualsAnswer(answer)).feedback;
    }

    public void AddAnswer(string question, string feedback, string response)
    {
        answers.Add(new Answer(question, feedback, response));
    }

    public class Answer
    {
        public string answer;
        public string response; //The written response
        public string feedback; //Correct, Partially correct, or Incorrect

        public Answer(string a, string f, string r)
        {
            answer = a;
            feedback = f;
            response = r;
        }

        public bool EqualsAnswer(string a)
        {
            return UnityWebRequest.UnEscapeURL(a).Equals(UnityWebRequest.UnEscapeURL(this.answer));
        }
    }

    public override string ToString()
    {
        string s = questionType + ", " + questionText + ": ";
        foreach (Answer a in answers) {
            s += a.answer + ", " + a.feedback + ", " + a.response + "--";
        }
        return s;
    }
}
