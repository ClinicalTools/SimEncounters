using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateStars : MonoBehaviour {

	public LayoutElement mask;
	public readonly int MaxValue = 5;
	private int rating;
	private float width;

	// Use this for initialization
	void Start () {
		if (mask == null) {
			mask = GetComponentInChildren<Mask>()?.GetComponent<LayoutElement>();
		}
		width = transform.parent.GetComponent<RectTransform>().rect.width;

		if (GlobalData.role == GlobalData.Roles.Guest) {
			transform.parent.parent.Find("New").gameObject.SetActive(true);
			transform.parent.parent.Find("CaseRating").gameObject.SetActive(false);
		} else if (GlobalData.caseObj != null) {
			if (Tracker.GetCaseData(GlobalData.caseObj.recordNumber).caseRating > 0) {
				transform.Find("Image").GetChild(
					Tracker.GetCaseData(GlobalData.caseObj.recordNumber).caseRating - 1
				).GetComponent<Toggle>().isOn = true;
			}
		}
	}
	
	public void SetRating(int i)
	{
		if (mask) mask.preferredWidth = width / MaxValue * i;
		rating = i;
	}

	public void UploadRating()
	{
		Tracker.RecordData(Tracker.DataType.rating, GlobalData.caseObj.recordNumber, rating);
	}
}
