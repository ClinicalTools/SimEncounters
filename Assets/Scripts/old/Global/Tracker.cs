using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;

public class Tracker : MonoBehaviour
{

    #region Classes
    public enum DataType
    {
        startedCase,
        quizData,
        finishedCase,
        rating,
        progress
    }

    public struct ProgressData
    {
        //public bool isSection;
        public int tabIdx;
        public int sectionIdx;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionIdx"></param>
        /// <param name="tabIdx">Record -1 for completed section</param>
        public ProgressData(int sectionIdx, int tabIdx)
        {
            this.sectionIdx = sectionIdx;
            this.tabIdx = tabIdx;
        }
    }

    public class CaseData
    {
        public bool caseStarted;
        public bool caseFinished;
        public int caseRating;
        public List<string> quizData;
        //public string caseProgress = "p:-"; //string representing only visited the first section
        private bool[][] caseProgress;
        public bool altered = false;

        public CaseData()
        {
            caseStarted = false;
            caseFinished = false;
            caseRating = 0;
            quizData = new List<string>();
        }

        public override string ToString()
        {
            if (caseFinished) {
                return string.Join(DataDivider,
                    caseStarted,
                    caseFinished,
                    caseRating,
                    GetQuizData());
            } else {
                return string.Join(DataDivider,
                    caseStarted,
                    caseFinished,
                    caseRating,
                    GetCaseProgress(),
                    GetQuizData()
                    );
            }
        }


        public void AddCaseProgress(int section, int tab)
        {
            if (section < 0 || tab < 0)
                return;

            if (caseProgress == null)
                caseProgress = new bool[section + 1][];
            else if (caseProgress.Length <= section)
                Array.Resize(ref caseProgress, section + 1);

            if (caseProgress[section] == null)
                caseProgress[section] = new bool[tab + 1];
            else if (caseProgress[section].Length <= tab)
                Array.Resize(ref caseProgress[section], tab + 1);

            caseProgress[section][tab] = true;
        }
        public string GetCaseProgress()
        {
            if (caseProgress == null)
                return "p";

            var str = "p";
            // Iterate through it backwards because that's easier to read when setting up array lengths
            for (int i = caseProgress.Length - 1; i >= 0; i--) {
                if (caseProgress[i] != null) {
                    str += $".{i}";
                    for (int j = caseProgress[i].Length - 1; j >= 0; j--) {
                        if (caseProgress[i][j])
                            str += $",{j}";
                    }
                }
            }

            return str;
        }
        public void ReadCaseProgress(string progress)
        {
            // Basic validation to ensure it's in the correct format
            if (progress == null || progress.Length < 5
                || progress[0] != 'p' || progress[1] != '.' || !char.IsDigit(progress[2])) {

                return;
            }
            caseProgress = null;
            var index = 1;
            while (index < progress.Length && progress[index] == '.') {
                var section = NextInt(progress, ref index);
                if (caseProgress == null)
                    caseProgress = new bool[section + 1][];

                while (index < progress.Length && progress[index] == ',') {
                    var tab = NextInt(progress, ref index);
                    if (caseProgress[section] == null)
                        caseProgress[section] = new bool[tab + 1];

                    caseProgress[section][tab] = true;
                }
            }
        }

        private static int NextInt(string str, ref int index)
        {
            var numStr = "0";
            while (++index < str.Length && char.IsDigit(str[index])) {
                numStr += str[index];
            }

            return int.Parse(numStr);
        }

        public bool IsTabRead(int section, int tab)
        {
            return caseProgress != null
                && caseProgress.Length > section
                && caseProgress[section] != null
                && caseProgress[section].Length > tab
                && caseProgress[section][tab];
        }

        public bool IsSectionRead(int section, int length)
        {
            if (caseProgress != null && caseProgress.Length > section && caseProgress[section] != null && caseProgress[section].Length > length) {
                for (int i = 0; i < length; i++) {
                    if (!caseProgress[section][length])
                        return false;
                }
                return true;
            } else {
                return false;
            }
        }

        public string GetQuizData()
        {
            return string.Join("--", quizData);
        }

        /// <summary>
        /// Returns the idx of the last completed section
        /// </summary>
        /// <returns>the section idx if one is found. -1 if no sections completed</returns>
        public int GetLastCompletedSection()
        {
            // Should never be called
            Debug.LogError("Got last completed section for some reason");
            return -1;
            /*
            int section;
            if (int.TryParse(caseProgress.Split(new char[] { '-' })[0].Substring(2).Trim('-'), out section)) {
                return section;
            } else {
                return -1;
            }*/
        }
    }

    private class TrackedData
    {
        public string userID;
        public Dictionary<string, CaseData> cases;
        public string inProgressKey = "";

        public TrackedData()
        {
            cases = new Dictionary<string, CaseData>();
            userID = GlobalData.username;
        }

        public TrackedData(string recordNumber)
        {
            cases = new Dictionary<string, CaseData>();
            cases.Add(recordNumber, new CaseData());

            userID = GlobalData.username;
        }

        public TrackedData(string recordNumber, bool caseStart = false, bool caseFinish = false, string quizEntry = "", int caseRate = 0)
        {
            cases = new Dictionary<string, CaseData>();
            cases.Add(recordNumber, new CaseData() {
                caseStarted = caseStart,
                caseFinished = caseFinish,
                quizData = new List<string>() { quizEntry },
                caseRating = caseRate
            });

            userID = GlobalData.username;
        }

        public override string ToString()
        {
            string data = "";
            if (inProgressKey.Length > 0)
                data += "inProgress:" + inProgressKey + CaseDivider;
            foreach (string recordNumber in cases.Keys) {
                data += recordNumber + DataDivider;
                data += cases[recordNumber].ToString();
                data += CaseDivider;
            }
            return data;
        }
    }
    #endregion

    public static string DataDivider = "::";
    public static string CaseDivider = "~~";
    private static string PHPAddress = GlobalData.serverAddress + "Track.php";
    private static bool askedToResume = false;
    private static TrackedData data = new TrackedData();
    private static Tracker instance;
    public static Tracker Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<Tracker>();
                if (instance == null) {
                    GameObject obj = new GameObject("Tracker", typeof(Tracker));
                    instance = obj.GetComponent<Tracker>();
                }
            }
            return instance;
        }

        set {
            instance = value;
        }
    }

    #region Record Data
    /// <summary>
    /// Record the data of a certain type of action
    /// </summary>
    /// <param name="type"></param>
    /// <param name="recordNumber"></param>
    /// <param name="data"></param>
    public static void RecordData(DataType type, string recordNumber, dynamic data)
    {
#if (UNITY_WEBGL == false)
        if (!GlobalData.GDS.developer && GlobalData.username.Equals("Guest")) {
            Debug.LogWarning("Cannot track data for a guest account");
            return;
        }

        //Null checks
        if (Tracker.data == null) {
            Tracker.data = new TrackedData(recordNumber);
        }
        if (Tracker.data.cases == null) {
            Tracker.data.cases = new Dictionary<string, CaseData>();
        }
        if (!Tracker.data.cases.ContainsKey(recordNumber)) {
            Tracker.data.cases.Add(recordNumber, new CaseData());
        }
        //Set this to true because after starting a case, make sure not to ask about resuming it when returning
        askedToResume = true;

        Tracker.data.inProgressKey = recordNumber;
        // Cases that were started via suggested cases don't mark this properly
        // Adding it here is a simple fix
        Tracker.data.cases[recordNumber].caseStarted = true;
        Tracker.data.cases[recordNumber].altered = true;

        switch (type) {
            case DataType.startedCase:
                RecordStartCase(recordNumber, (bool)data);
                break;
            case DataType.finishedCase:
                RecordFinishedCase(recordNumber, (bool)data);
                break;
            case DataType.quizData:
                RecordQuizData(recordNumber, (string)data);
                break;
            case DataType.rating:
                RecordCaseRating(recordNumber, (int)data);
                break;
            case DataType.progress:
                RecordProgress(recordNumber, (ProgressData)data);
                break;
        }

        SaveData();
#endif
    }

    public static void ClearInProgressKey()
    {
        data.inProgressKey = "";
    }

    public static string GetInProgressKey()
    {
        return data.inProgressKey;
    }

    /// <summary>
    /// Handles adding an entire case to tracked data
    /// </summary>
    /// <param name="caseData">Data-divided string from the server (or local)</param>
    /// <remarks>Data order: record number, started, finished, rating, progress, quiz data</remarks>
    public static void RecordCase(string[] caseData, bool overwrite = true)
    {
        if (!data.cases.ContainsKey(caseData[0])) {
            data.cases.Add(caseData[0], new CaseData());
        } else if (!overwrite) {
            return;
        }
        int arrayIdx = 1;
        if (!bool.TryParse(caseData[arrayIdx++], out data.cases[caseData[0]].caseStarted)) {
            data.cases[caseData[0]].caseStarted = !caseData[arrayIdx - 1].Equals("0");
        }
        if (!bool.TryParse(caseData[arrayIdx++], out data.cases[caseData[0]].caseFinished)) {
            data.cases[caseData[0]].caseFinished = !caseData[arrayIdx - 1].Equals("0");
        }
        int.TryParse(caseData[arrayIdx++], out data.cases[caseData[0]].caseRating);

        if (data.cases[caseData[0]].caseFinished) {
            arrayIdx++;
        } else if (caseData.Length > arrayIdx) {
            if (caseData[arrayIdx].StartsWith("p")) {
                //data.cases[caseData[0]].caseProgress = caseData[arrayIdx++];
                data.cases[caseData[0]].ReadCaseProgress(caseData[arrayIdx]);
            }
            arrayIdx++;
        }

        if (caseData.Length > arrayIdx) {
            string[] quizzes = caseData[arrayIdx++].Split(new string[] { "--" }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in quizzes) {
                data.cases[caseData[0]].quizData.Add(s);
            }
        }

        print(data.cases[caseData[0]].ToString());
    }

    /// <summary>
    /// Handles adding an entire case to tracked data
    /// </summary>
    /// <param name="caseData">Data-divided string from the server (or local)</param>
    public static void RecordCase(string caseData, bool overwrite = true)
    {
        string[] dataSplit = caseData.Split(new string[] { DataDivider }, System.StringSplitOptions.RemoveEmptyEntries);
        RecordCase(dataSplit, overwrite);
    }

    /// <summary>
    /// Records when a case was started
    /// </summary>
    /// <param name="recordNumber">The case being recorded</param>
    /// <param name="started">If the case was started</param>
    private static void RecordStartCase(string recordNumber, bool started)
    {
        Debug.Log("Case Started");
        data.cases[recordNumber].caseStarted = started;
    }

    /// <summary>
    /// Records when a case was finished
    /// </summary>
    /// <param name="recordNumber">The case being recorded</param>
    /// <param name="finished">If the case was finished</param>
    private static void RecordFinishedCase(string recordNumber, bool finished)
    {
        Debug.Log("Case Finished");
        data.cases[recordNumber].caseFinished = finished;
        ClearInProgressKey();
    }

    /// <summary>
    /// Records the data for a single quiz answer
    /// </summary>
    /// <param name="recordNumber">The case being recorded</param>
    /// <param name="quiz">Use Tracker.GetQuizData(...) for this value</param>
    private static void RecordQuizData(string recordNumber, string quiz)
    {
        bool replacePreviousQuizAnswers = false;
        if (replacePreviousQuizAnswers) {
            //Find the index of the question being answered (if it exists in the list)
            string quizAnswers = Regex.Split(quiz, "[0-9]*-[0-9]*-")[1];
            string questionID = quiz.Substring(0, quiz.IndexOf(quizAnswers));
            int quizIdx = data.cases[recordNumber].quizData.FindIndex((string s) => s.StartsWith(questionID));

            if (quizIdx >= 0) {
                //Replace the entry
                data.cases[recordNumber].quizData[quizIdx] = quiz;
            } else {
                //Add the new entry
                data.cases[recordNumber].quizData.Add(quiz);
            }
        } else {
            data.cases[recordNumber].quizData.Add(quiz);
        }
    }

    /// <summary>
    /// Formats the data for a quiz answer appropriately
    /// </summary>
    /// <param name="pageNumber">The page the quiz was on</param>
    /// <param name="questionNumber">The index of the question being answered</param>
    /// <param name="answers">The index of the answers selected</param>
    /// <returns></returns>
    public static string GetQuizData(int pageNumber, int questionNumber, params int[] answers)
    {
        return pageNumber + "-" + questionNumber + "-" + string.Join(",", answers);
    }

    /// <summary>
    /// Records the rating for a given case. 
    /// </summary>
    /// <param name="recordNumber">The case being recorded</param>
    /// <param name="rating">The rating, given as an int to represent stars</param>
    private static void RecordCaseRating(string recordNumber, int rating)
    {
        data.cases[recordNumber].caseRating = rating;
    }

    private static void RecordProgress(string recordNumber, ProgressData progressData)
    {
        data.cases[recordNumber].AddCaseProgress(progressData.sectionIdx, progressData.tabIdx);

        /*
        int listedNum;
        if (int.TryParse(data.cases[recordNumber].caseProgress.Split(new char[] { '-' })[0].Substring(2).Trim('-'), out listedNum)) {
            if (progressData.sectionIdx <= listedNum) {
                return; //Trying to record previous progress.
            }
        }

        if (progressData.tabIdx == -1) {
            //Moved to a new section
            data.cases[recordNumber].caseProgress = "p:" + progressData.sectionIdx + "-";
            print(data.cases[recordNumber].caseProgress);
        } else {
            //Update tabs in current section
            if (data.cases[recordNumber].caseProgress.Equals("")) {
                data.cases[recordNumber].caseProgress = "p:-";
            }

            string[] split = Regex.Split(data.cases[recordNumber].caseProgress, "-");
            //(#,|#$)
            if (Regex.IsMatch(split[1], "(" + progressData.tabIdx + ",|" + progressData.tabIdx + "$)")) {
                //Tab has already been recorded
                return;
            } else {
                data.cases[recordNumber].caseProgress += "," + progressData.tabIdx;
            }
        }*/
    }


    #endregion

    #region Upload
    /// <summary>
    /// Uploads and/or saves any tracked data
    /// </summary>
    /// <remarks>Do I use threads? One thread? many threads?</remarks>
    public static void Upload(bool clearAfterUpload = false)
    {
        if (data == null) {
            data = new TrackedData();
            Debug.Log("No data to upload. Returning");
            return;
        }

        bool single = true;
        //Begin uploading the data to the server
        Instance.StartCoroutine(Instance.UploadData(single, clearAfterUpload));

        //Also save some data locally (For dev, save all locally for now)
        SaveData();
    }

    public static void SaveData()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer) {
            string localData = data.ToString();

            StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/" + GetTrackingFile(), false);
            sw.WriteLine(localData);
            //sw.WriteLine("(RecordNumber)::(caseStarted)::(caseFinished)::(caseRating)~~(New Case)");
            sw.Close();
            sw.Dispose();

            print("Saved Data");
        }
    }

    private IEnumerator UploadData(bool singleRoutine, bool clearAfterUpload = false)
    {
        print("Uploading...");
        if (singleRoutine) {
            StartCoroutine(UploadTrackerDataCo());
        } else {
            foreach (string recordNumber in data.cases.Keys) {
                StartCoroutine(UploadSoloTrackCo(recordNumber));
            }
        }

        //Wait for the data to be saved and the uploading to begin
        yield return null;
        if (clearAfterUpload) {
            ResetData();
        }
    }

    #region coroutine/thread methods

    //This uploads case data one at a time
    private IEnumerator UploadTrackerDataCo()
    {
        //NEW WAY
        //The new way will be to cycle through new tracked data
        //and to send off those cases one at a time
        if (data.userID == null) {
            Debug.LogWarning("No user assigned for tracking!");
            yield break;
        }
        WWWForm form;
        if (data != null) {
            print(data.ToString());
        }
        foreach (string caseKey in data.cases.Keys) {
            if (!data.cases.ContainsKey(caseKey)) {
                continue;
            }
            CaseData d = data.cases[caseKey];
            print(caseKey + ": " + d);
            if (true || data.cases[caseKey].altered) {
                form = new WWWForm();
                form.AddField("ACTION", "upload");
                form.AddField("username", data.userID == null ? "" : data.userID);
                form.AddField("inProgress", data.inProgressKey);
                form.AddField("recordNumber", caseKey);
                form.AddField("started", data.cases[caseKey].caseStarted ? 1 : 0);
                form.AddField("finished", data.cases[caseKey].caseFinished ? 1 : 0);
                form.AddField("rating", data.cases[caseKey].caseRating);
                form.AddField("progress", data.cases[caseKey].GetCaseProgress());
                form.AddField("quizData", data.cases[caseKey].GetQuizData());
                print(data.cases[caseKey].GetQuizData());
				using (UnityWebRequest webRequest = UnityWebRequest.Post(PHPAddress, form)) {
					yield return webRequest.SendWebRequest();
				}
			}
        }
    }

    //Uploads one case. To be used by many coroutines
    private IEnumerator UploadSoloTrackCo(string recordNumber)
    {
        if (recordNumber == null) {
            Debug.Log("Record number null. Skipping");
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("ACTION", "upload");
        form.AddField("RecordNumber", recordNumber);
        form.AddField("username", data.userID == null ? "" : data.userID);

        form.AddField("Quizes", string.Join("::", data.cases[recordNumber].quizData));
        form.AddField("Rating", data.cases[recordNumber].caseRating);
        form.AddField("Started", data.cases[recordNumber].caseStarted.ToString());
        form.AddField("Finished", data.cases[recordNumber].caseStarted.ToString());

		//form.AddField("data", data.ToString());

		using (UnityWebRequest webRequest = UnityWebRequest.Post(PHPAddress, form)) {
			yield return webRequest.SendWebRequest();
			while (webRequest.uploadProgress < 1) {
				yield return null;
			}
		}

        Debug.Log("Form data for " + recordNumber + " uploaded");
        yield break;
    }
    #endregion

    #endregion

    /// <summary>
    /// This is for loading data saved locally
    /// </summary>
    public static void ReloadLocalData()
    {
        //Instance.GetComponent<ServerControls>().GetLocalFolder("a");
        StreamReader sr;
        try {
            sr = new StreamReader(Application.persistentDataPath + "/" + GetTrackingFile());
        } catch (FileNotFoundException e) {
            Debug.LogWarning("File could not be found! Local data not loaded\n" + e.Message);
            return;
        } catch (DirectoryNotFoundException e) {
            Debug.LogWarning("Directory not found!\n" + e.Message);
            return;
        }
        string text = sr.ReadLine();
        sr.Close();
        sr.Dispose();
        print(text);
        ResetData();
        string[] splitCases = text.Split(new string[] { CaseDivider }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string s in splitCases) {
            if (s.StartsWith("inProgress")) {
                data.inProgressKey = s.Substring("inProgress:".Length);
            } else {
                RecordCase(s, false);
            }
        }
        Debug.Log("Data: " + data.ToString());
    }

    public static void LoadData()
    {
        Instance.StartCoroutine(Instance.InstanceLoadData());
    }

    private IEnumerator InstanceLoadData()
    {
        WWWForm form = new WWWForm();
        form.AddField("ACTION", "download");
        form.AddField("username", GlobalData.username);


		string[] splitCases;
		using (UnityWebRequest webRequest = UnityWebRequest.Post(PHPAddress, form)) {
			yield return webRequest.SendWebRequest();
			if (webRequest.error != null || webRequest.downloadHandler.text.StartsWith("Error: ")) {
				Debug.Log(webRequest.downloadHandler.text);
				yield break;
			}
			print(webRequest.downloadHandler.text);
			splitCases = webRequest.downloadHandler.text.Split(new string[] { CaseDivider }, StringSplitOptions.None);
		}
        //If the server return is successful, update the data

        //Handle anything related to local files
        if (File.Exists(Application.persistentDataPath + "/" + GetTrackingFile())) {
            //See which save is newer and load that menu preview
            DateTime localFileModified = File.GetLastWriteTime(Application.persistentDataPath + "/" + GetTrackingFile());
            DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); //Convert from seconds from unix epoch to local DateTime
            if (double.TryParse(splitCases[0], out double timestamp)) {
                DateTime serverFileModified = unixEpoch.AddSeconds(timestamp); //Add the seconds as specified by the download
                if (DateTime.Compare(serverFileModified.ToLocalTime(), localFileModified.ToLocalTime()) < 0) {
                    yield break; //Don't add the old data from the server
                }
            } else {
                //An error may have been thrown. Either way, something's not right
                yield break;
            }
        }

        if (int.TryParse(splitCases[1], out _)) {
            data.inProgressKey = splitCases[1];
        }

        ResetData();
        for (int i = 2; i < splitCases.Length; i++) {
            if (splitCases[i].Length == 0) {
                continue;
            } else {
                RecordCase(splitCases[i], false);
            }
        }
    }

    private static string GetTrackingFile()
    {
        return GlobalData.accountId + ".txt";
    }

    public static bool InProgress(string recordNumber)
    {
        if (!data.cases.ContainsKey(recordNumber)) {
            return false;
        }
        if (data.cases[recordNumber].caseStarted && !data.cases[recordNumber].caseFinished) {
            return true;
        }
        return false;
        /*
		//Works, but inefficient
		if (HasCaseData(recordNumber)) {
			if (data.cases[recordNumber].GetLastCompletedSection() >= 0) {
				return true;
			}
		}
		return false;
        */
    }

    public static bool GetAskedToResume()
    {
        return askedToResume;
    }

    public static void Resumed()
    {
        askedToResume = true;
    }

    /// <summary>
    /// Wipes the tracked data
    /// </summary>
    public static void ResetData()
    {
        data = new TrackedData();
        askedToResume = false;
    }

    /// <summary>
    /// Updates the id of the logged in user. Call when logging in
    /// </summary>
    public static void UpdateUserID()
    {
        data.userID = GlobalData.username;
    }

    public static bool HasCaseData(string recordNumber)
    {
        return data.cases.ContainsKey(recordNumber);
    }

    public static CaseData GetCaseData(string recordNumber)
    {
        try {
            return data.cases[recordNumber];
        } catch (KeyNotFoundException e) {
            Debug.LogWarning("No case listed: " + e.Message);
            data.cases.Add(recordNumber, new CaseData());
            return data.cases[recordNumber];
        }
    }

    public static void PrintAllData()
    {
        print(data.ToString());
        return;
        /*
		string spaces = "    ";
		string allData = "---ALL DATA HERE---\nUsername: " + data.userID + "\n";
		foreach (string key in data.cases.Keys) {
			allData += "#" + key + ":\n";
			allData += spaces + "Case Started: " + data.cases[key].caseStarted + "\n";
			allData += spaces + "Case Finished: " + data.cases[key].caseFinished + "\n";
			allData += spaces + "Case Rating: " + data.cases[key].caseRating + "\n";
			allData += spaces + "Quiz Data:\n";
			foreach (string quizData in data.cases[key].quizData) {
				allData += spaces + spaces + quizData + "\n";
			}
		}
		Debug.Log(allData);
        */
    }
}
