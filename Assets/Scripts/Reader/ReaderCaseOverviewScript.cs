using UnityEngine;
using TMPro;
using System;

public class ReaderCaseOverviewScript : MonoBehaviour
{
    // Use this for initialization
    public void RefreshOverview()
    {
        if (GlobalData.caseObj == null)
            return;

        var m = GlobalData.caseObj;

        transform.Find("Border/BG/AuthorInfo/TextName").GetComponent<TextMeshProUGUI>().text = m.patientName.Replace("_", " ");
        transform.Find("Border/BG/AuthorInfo/TextAuthor").GetComponent<TextMeshProUGUI>().text = "by " + m.authorName;
        transform.Find("Border/BG/CaseInfo/RightSide/TextAudience").GetComponent<TextMeshProUGUI>().text = m.audience;
        transform.Find("Border/BG/CaseInfo/RightSide/TextDifficulty").GetComponent<TextMeshProUGUI>().text = m.difficulty;
        transform.Find("Border/BG/CaseInfo/RightSide/TextCategory").GetComponent<TextMeshProUGUI>().text = m.GetTagsAsOneString();
        transform.Find("Border/BG/CaseInfo/LeftSide/CaseDescription/Description/Text").GetComponent<TextMeshProUGUI>().text = m.summary;
        transform.Find("Border/BG/AuthorInfo/ShortCaseDescription").GetComponent<TextMeshProUGUI>().text = m.description;

        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); //Convert from seconds from unix epoch to local DateTime
        transform.Find("Border/BG/CaseInfo/RightSide/Developer/DateModified").GetComponent<TextMeshProUGUI>().text = "Last Modified: " + unixEpoch.AddSeconds(m.dateModified).ToLocalTime();

        Transform imageParent = transform.Find("Border/BG/CaseInfo/RightSide/TextDifficulty");
        switch (m.difficulty)
        {
            case "Beginner":
                imageParent.GetChild(1).gameObject.SetActive(false);
                imageParent.GetChild(2).gameObject.SetActive(false);
                imageParent.GetChild(3).gameObject.SetActive(true);
                break;
            case "Intermediate":
                imageParent.GetChild(1).gameObject.SetActive(false);
                imageParent.GetChild(2).gameObject.SetActive(true);
                imageParent.GetChild(3).gameObject.SetActive(false);
                break;
            case "Advanced":
                imageParent.GetChild(1).gameObject.SetActive(true);
                imageParent.GetChild(2).gameObject.SetActive(false);
                imageParent.GetChild(3).gameObject.SetActive(false);
                break;
            default:
                imageParent.GetChild(1).gameObject.SetActive(true);
                imageParent.GetChild(2).gameObject.SetActive(false);
                imageParent.GetChild(3).gameObject.SetActive(false);
                break;
        }



        Transform yourRatingParent = transform.Find("Border/BG/RatingsPanel/YourRating");
        Transform averageRatingParent = transform.Find("Border/BG/RatingsPanel/AverageRating");
        foreach (Transform t in yourRatingParent)
        {
            t.gameObject.SetActive(false);
        }
        foreach (Transform t in averageRatingParent)
        {
            t.gameObject.SetActive(false);
        }
        //Your rating
        yourRatingParent.GetChild(Tracker.GetCaseData(m.recordNumber).caseRating).gameObject.SetActive(true);
        //Average rating
        averageRatingParent.GetChild(m.rating).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
