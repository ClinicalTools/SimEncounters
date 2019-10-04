using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SendEmailCertificateScript : MonoBehaviour {

	public TMP_InputField emailField;
	public Toggle addToEmailList;
	public GameObject nextScreen;
    private ReaderDataScript ds;            //Reference to DataScript
                                            //This script will be parented to the screen that will be disabled

    private string serverAddress = GlobalData.serverAddress + "SendCaseConfirmation.php";

    private void Awake()
    {
        var BG = GameObject.Find("GaudyBG");
        ds = BG.GetComponentInChildren<ReaderDataScript>();
    }

    public void OpenEndCasePanel()
    {
        var finished = true;
        var sections = ds.GetSectionsList();
        foreach (var section in sections) {
            if (!ds.GetData(section).AllTabsVisited()) {
                finished = false;
                break;
            }
        }
        if (finished)
		    Tracker.RecordData(Tracker.DataType.finishedCase, GlobalData.caseObj.recordNumber, true);

		GameObject panel = Instantiate(Resources.Load("Reader/Prefabs/Panels/FinishedCasePanel") as GameObject, GameObject.Find("GaudyBG").transform);
		panel.name = "FinishedCasePanel";
		if (Application.platform == RuntimePlatform.WebGLPlayer) {
			//panel.transform.Find("ConfirmActionPanel/Old/WebGLScreen").gameObject.SetActive(true);
			//panel.transform.Find("ConfirmActionPanel/Old/CongratsScreen").gameObject.SetActive(false);
		} else {
			if (!GlobalData.role.Equals(GlobalData.Roles.Guest)) {
				//panel.transform.Find("ConfirmActionPanel/Old/SendEmailScreen").GetComponent<SendEmailCertificateScript>().emailField.text = GlobalData.email;
			}

			GetCasesToDisplay();

			if (GlobalData.recommendedCases != null && GlobalData.recommendedCases.Length == 1) {
				//Course case
				Transform caseParent = panel.transform.Find("ConfirmActionPanel/New/NextCase");
				caseParent.gameObject.SetActive(true);
				foreach(MenuCase m in GlobalData.recommendedCases) {
					if (m == null) {
						GameObject go2 = new GameObject("Empty Case");
						go2.AddComponent<RectTransform>();
						go2.transform.SetParent(caseParent, false);
					} else {
						GameObject go = Instantiate(Resources.Load("Menu/Prefabs/Panels/RecommendedCase") as GameObject, caseParent);
						PopulateRecommendedCase(go.transform, m);
						InitCaseButtons(go, m);
					}
				}
			} else {// if (GlobalData.recommendedCases.Length == 2) {
				//Recommending cases
				Transform caseParent = panel.transform.Find("ConfirmActionPanel/New/RecommendedCases");
				caseParent.gameObject.SetActive(true);
				for(int i = 0; i < GlobalData.recommendedCases.Length; i++) {
					if (GlobalData.recommendedCases[i] == null) {
						//GameObject go2 = new GameObject("Empty Case");
						GameObject go3 = Instantiate(Resources.Load("Menu/Prefabs/Panels/EmptyCase") as GameObject, caseParent);
					} else {
						GameObject go = Instantiate(Resources.Load("Menu/Prefabs/Panels/RecommendedCase") as GameObject, caseParent);
						PopulateRecommendedCase(go.transform, GlobalData.recommendedCases[i]);
						InitCaseButtons(go, GlobalData.recommendedCases[i]);
					}
				}
			}

		}
	}

	private void PopulateRecommendedCase(Transform caseObj, MenuCase m)
	{
		string patientName = "ImageRows/Rows/Row0/NameData";
		string author = "ImageRows/Rows/Row1/AuthorData";
		string audience = "Row3/AudienceData";
		string tags = "Row2/TagData";
		string text = "Text";
		string filename = "Filename";
		string difficultyTextLoc = "Row3/DifficultyData";
		string beginnerImage = "Row3/BeginnerImage";
		string intermediateImage = "Row3/IntermediateImage";
		string advancedImage = "Row3/AdvancedImage";

		caseObj.Find(patientName).GetComponentInChildren<TextMeshProUGUI>().text = m.patientName.Replace("_", " "); //patientname will need to go here eventually
		caseObj.Find(author).GetComponentInChildren<TextMeshProUGUI>().text = m.description;//"by " + menuItems [menuItems.Count - 1].authorName;
		caseObj.Find(audience).GetComponentInChildren<TextMeshProUGUI>().text = m.audience; //Need audience variable?
		caseObj.Find(tags).GetComponentInChildren<TextMeshProUGUI>().text = m.GetTagsAsOneString();
		caseObj.Find(text).GetComponentInChildren<TextMeshProUGUI>().text = m.description + ", " + m.filename.Replace(".ced", "");
		caseObj.Find(filename).GetComponentInChildren<TextMeshProUGUI>().text = m.filename;
		string difficulty = m.difficulty;
		caseObj.transform.Find(difficultyTextLoc).GetComponentInChildren<TextMeshProUGUI>().text = difficulty;

		caseObj.transform.Find(beginnerImage).gameObject.SetActive(false);
		caseObj.transform.Find(intermediateImage).gameObject.SetActive(false);
		caseObj.transform.Find(advancedImage).gameObject.SetActive(false);
		switch (difficulty) {
			case "Beginner":
				caseObj.transform.Find(beginnerImage).gameObject.SetActive(true);
				break;
			case "Intermediate":
				caseObj.transform.Find(intermediateImage).gameObject.SetActive(true);
				break;
			case "Advanced":
				caseObj.transform.Find(advancedImage).gameObject.SetActive(true);
				break;
			default:
				caseObj.transform.Find(advancedImage).gameObject.SetActive(true);
				break;
		}

		if (m.completed) {
			//Enable completed thingy
			caseObj.transform.Find("CaseCompleted").gameObject.SetActive(true);
		}
	}

	private void InitCaseButtons(GameObject caseObj, MenuCase m)
	{
		//Setup the edit button on the case object to load the case
		//Button editButton = caseObj.transform.Find("ImageRows/Rows/Row0/EditButton").GetComponentInChildren<Button>();
		Button readButton;
		if (GlobalData.GDS.isMobile) {
			readButton = caseObj.transform.Find("Row4/OpenReader")?.GetComponent<Button>();
		} else {
			readButton = caseObj.transform.Find("Row4/OpenReader").GetComponent<Button>();
		}

		readButton?.onClick.AddListener(delegate
		{
			//Load recommended courses here
			//--------
			LoadReader(m);
		});

		Button loadInfoButton = caseObj.GetComponent<Button>();

		//Setup the case object button to load the info panel
		loadInfoButton.onClick.AddListener(delegate
		{
			LoadInfoPanel(m);
		});
	}

	private void GetCasesToDisplay()
	{
		if (GlobalData.recommendedCases == null || GlobalData.recommendedCases.Length == 2) {
			//Not a course
			FindRecommendedCases();
		} else {
			//Course
			LinkedListNode<MenuCase> iterator = GlobalData.courseCases.First;
			while (iterator.Value != GlobalData.caseObj) {
				iterator = iterator.Next;
			}

			GlobalData.recommendedCases = new MenuCase[1];
			if (iterator.Next == null) {
				FindRecommendedCases();
			} else {
				GlobalData.recommendedCases[0] = iterator.Next.Value;
			}
		}
	}

	private void FindRecommendedCases()
	{
		//Find case by tag
		GlobalData.recommendedCases = new MenuCase[2];
		List<string> tags = new List<string>(GlobalData.caseObj.tags);
		List<int> indexes = new List<int>();
		for (int i = 0; i < tags.Count; i++) {
			indexes.Add(i);
		}

		print(string.Join(",", tags.ToArray()) + "::" + string.Join("-", indexes.ToArray()));

		//Find case by tag
		int tagIdx = -1;
		foreach (string tag in tags) {
			if (indexes.Count == 0) {
				break;
			}

			bool useRandomTag = false;
			//Could filter which tags are picked more here
			if (useRandomTag) {
				tagIdx = indexes[UnityEngine.Random.Range(0, indexes.Count)];
				indexes.Remove(tagIdx);
			} else {
				tagIdx++;
			}

			string tagString = GlobalData.caseObj.tags[tagIdx];
			print(tagString);
			List<MenuCase> tagMatches = GlobalData.allDownloadedCases.FindAll((MenuCase m) => m.CheckCaseTags(tagString) && !m.Equals(GlobalData.caseObj));
			print(tagMatches.Count);
			List <CaseScores> cScores = new List<CaseScores>();
			foreach(MenuCase menu in tagMatches) {
				CaseScores cs = new CaseScores() {
					mCase = menu,
					score = 0
				};
				cScores.Add(cs);
			}

			if (tagMatches.Count > 0) {
				//Assign score to each case in the list? This could help

				//Difficulty sorting/scoring
				ApplyDifficultyScores(cScores);

				//Tag relevance sorting
				ApplyTagScores(cScores, tagString);

				//Description/keyword sorting??
				//Too much right now

				//Rating sorting
				ApplyRatingScores(cScores);

				//Completed case sorting
				ApplyCompletedScore(cScores);


				float totalScore = 0;
				foreach(CaseScores caseItem in cScores) {
					caseItem.score = Mathf.Pow(caseItem.score, 2);
					totalScore += caseItem.score;
				}

				cScores.Sort((y, x) => x.score.CompareTo(y.score));
				foreach (CaseScores caseItem in cScores) {
					print(caseItem);
				}


				float selected = Random.Range(0, totalScore);
				totalScore = 0;

				foreach (CaseScores caseItem in cScores) {
					totalScore += caseItem.score;
					if (selected < totalScore) {
						//This is the chosen case
						GlobalData.recommendedCases[0] = caseItem.mCase;
						break;
					}
				}



				//GlobalData.recommendedCases[0] = tagMatches[UnityEngine.Random.Range(0, tagMatches.Count)];
				break;
			}
		}
		//If no tag matches, provide a 'default' recommended case here
		//----------

		//Find case by author
		List<MenuCase> authorMatches = GlobalData.allDownloadedCases.FindAll((MenuCase m) => m.authorName.Equals(GlobalData.caseObj.authorName) && !m.Equals(GlobalData.caseObj));
		print(authorMatches.Count);
		List<MenuCase> CheckList = new List<MenuCase>();
		if ((CheckList = authorMatches.FindAll((MenuCase m) => m.difficulty.Equals(GlobalData.caseObj.difficulty))).Count > 0) {
			authorMatches = CheckList;
		}
		if (authorMatches.Count > 0) {
			int tries = 5;
			do {
				GlobalData.recommendedCases[1] = authorMatches[UnityEngine.Random.Range(0, authorMatches.Count)];
				tries--;
			} while (GlobalData.recommendedCases[1] == GlobalData.recommendedCases[0] && tries > 0);
		}

		foreach (MenuCase menu in GlobalData.recommendedCases) {
			if (menu == null) {
				print("Null case");
			} else {
				print(menu.patientName);
			}
		}
	}

	private class CaseScores
	{
		public MenuCase mCase;
		public float score;

		public override string ToString()
		{
			return mCase.recordNumber + "_" + mCase.patientName + ": " + score.ToString();
		}
	}

	private void ApplyDifficultyScores(List<CaseScores> cases)
	{
		for(int i = 0; i < cases.Count; i++) {
			if (cases[i].mCase.difficulty.Equals(GlobalData.caseObj.difficulty)) {
				//Best match
				cases[i].score += 2;
			} else {
				switch (cases[i].mCase.difficulty) {
					case "Intermediate":
						//Advanced or beginner are both 1 away
						cases[i].score++;
						break;
					default:
						if (GlobalData.caseObj.difficulty.Equals("Intermediate")) {
							cases[i].score += 1;
						} else {
							cases[i].score += 0;
						}
						break;
				}
			}
		}
	}

	/// <summary>
	/// Add to the score depending on number of tag matches
	/// Adds 2^(n-1) for number of tag matches
	/// </summary>
	/// <param name="cases"></param>
	/// <param name="currentTag"></param>
	private void ApplyTagScores(List<CaseScores> cases, string currentTag)
	{
		List<string> otherTags = new List<string>(GlobalData.caseObj.tags);
		otherTags.Remove(currentTag);

		//If no other tags to check from the current case, return
		if (otherTags.Count == 0) {
			return;
		}

		for (int i = 0; i < cases.Count; i++) {
			float valueToAdd = 1;
			foreach(string tag in cases[i].mCase.tags) {
				if (otherTags.Contains(tag)) {
					//Add 1 for each matching tag
					valueToAdd += 1f;
					cases[i].score += valueToAdd;
				}
			}
		}
	}

	/// <summary>
	/// Add (1-5)/3 to the score depending on rating.
	/// Add 1 (rating of 3) to cases with no rating for fairness.
	/// </summary>
	/// <param name="cases"></param>
	private void ApplyRatingScores(List<CaseScores> cases)
	{
		//Increment the scores more for higher rated cases.
		//Unrated cases unfavorable with this model though...
		for (int i = 0; i < cases.Count; i++) {
			if (cases[i].mCase.rating == 0) {
				//Add the default score boost to cases which have no rating yet.
				cases[i].score += 1;
			} else {
				cases[i].score += (cases[i].mCase.rating / 3f);
			}
		}
	}

	/// <summary>
	/// Reduce score by 1 for a completed case
	/// </summary>
	/// <param name="cases"></param>
	private void ApplyCompletedScore(List<CaseScores> cases)
	{
		for (int i = 0; i < cases.Count; i++) {
			if (cases[i].mCase.completed) {
				cases[i].score -= 1;
			}
		}
	}

	private void LoadInfoPanel(MenuCase m)
	{

	}

	private void LoadReader(MenuCase m)
	{
		GlobalData.createCopy = false;
		GlobalData.resourcePath = "Reader";
		//Set case
		//set ds.filename
		//ds.ReloadFile

		ReaderDataScript ds = GameObject.Find("GaudyBG").GetComponent<ReaderDataScript>();
		ds.loadingScreen.gameObject.SetActive(true);
		Destroy(GameObject.Find("FinishedCasePanel"));
		GlobalData.caseObj = m;
		ds.SetFileName(m.filename);
		ds.ReloadFile();
	}

	public void SendEmail()
	{
		if (!emailField.text.Equals("")) {
			StartCoroutine(SendEmailCoroutine());
		} else {
			print("Please enter your email");
		}
	}

	private IEnumerator SendEmailCoroutine()
	{
		WWWForm form = new WWWForm();
		form.AddField("email", emailField.text);
		form.AddField("code", GlobalData.fileName);
		form.AddField("case", GlobalData.firstName + " " + GlobalData.lastName);
		form.AddField("joinEmail", addToEmailList.isOn.ToString());

		
		using (UnityWebRequest webRequest = UnityWebRequest.Post(serverAddress, form)) {
			yield return webRequest.SendWebRequest();
			print(webRequest.downloadHandler.text);

			print(webRequest.downloadHandler.text.Split(new string[] { "--" }, System.StringSplitOptions.None)[0]);
			if (webRequest.downloadHandler.text.Split(new string[] { "--" }, System.StringSplitOptions.None)[0].Equals("Email sent!")) {
				nextScreen.SetActive(true);
				gameObject.SetActive(false);
			} else {
				print("Could not send email");
				GameObject.Find("GaudyBG").GetComponent<ReaderDataScript>().ShowMessage("Could not send email", true);
			}
		}
	}
}
