using UnityEngine.UI;

public class MedicalTestFeedback : EntryFeedback
{
    protected override void DisplayResponse()
    {
        var body = entryTrans.Find("Body");
        SetColor(body.GetComponent<Image>());
        var results = body.Find("Results");
        results.gameObject.SetActive(IsOn);

        base.DisplayResponse();
    }
}
