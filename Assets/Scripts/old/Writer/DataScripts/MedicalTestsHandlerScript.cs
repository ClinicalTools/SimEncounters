using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MedicalTestsHandlerScript : MonoBehaviour
{
    public Transform entryParent;
    public TextMeshProUGUI ddLabel;
    private string csvPath;
	private GlobalDataScript gds;

    // Use this for initialization
    void Start()
    {
		csvPath = Application.streamingAssetsPath + "/Medical Panels/";
		gds = GameObject.Find ("Canvas").GetComponent<GlobalDataScript> ();

        Debug.Log(csvPath);
    }

	public void singleInput() {
		DirectoryInfo dInfo;
		try {
			dInfo = new DirectoryInfo(csvPath);
		} catch (System.ArgumentNullException e) {
			print(e.Message);
			return;
		}
		FileInfo[] fInfo = dInfo.GetFiles ("*.csv");
		foreach (FileInfo f in fInfo) {
			StreamReader reader = new StreamReader(csvPath + f.Name);
			string file = reader.ReadToEnd();
			string[] lines = file.Split("\n"[0]);
			for (int i = 0; i < lines.Length; i++) {
				string[] lineData = (lines[i].Trim()).Split(',');
				if (lineData [0].Equals (ddLabel.text)) {
					Transform entry = transform.Find("MedicalTestColumns/MedicalTestRows/Row1/NormalRangeValue");
					if (gds.gender == "Male")
						entry.GetComponent<TMP_InputField> ().text = lineData [1];
					else if (gds.gender == "Female")
						entry.GetComponent<TMP_InputField> ().text = lineData [2];
					else if (gds.gender == "Other")
						entry.GetComponent<TMP_InputField> ().text = lineData [2];
					else 
						Debug.Log ("A gender was not entered in Patient Info");
					break;
				}
			}
		}
	}

    public void panelInput()
    {
		/*
		for (int i = 0; i < entryParent.childCount; i++) {
			entryParent.GetComponentInChildren<RemoveEntryScript> ().ApprovedRemove (entryParent.GetChild(i).gameObject);
			Debug.Log("DELETING "+ entryParent.GetChild(i).name);
			//Destroy (entryParent.GetChild (i).gameObject);
		}
		*/
        if (this.GetComponent<SaveCaseScript>().tags.Contains(ddLabel.text))
        {
			//When destroying elements, unity removes them at the end of the frame.
			//Since we're adding information to the spawned entries based on transform child index,
			//the index of the new, not to be destroyed entries must be upped by the number of previous children
			int oldChildren = entryParent.childCount;
			entryParent.GetComponent<HistoryFieldManagerScript>().RemoveEntries(entryParent);
			StreamReader reader = new StreamReader(csvPath + ddLabel.text + ".csv");
            string file = reader.ReadToEnd();
            string[] lines = file.Split("\n"[0]);

            for (int i = 0; i < lines.Length; i++)
            {
                entryParent.GetComponent<HistoryFieldManagerScript>().AddEntryFromButton("LabTestEntry");
                string[] lineData = (lines[i].Trim()).Split(',');
                Transform entry = entryParent.GetChild(i + oldChildren).Find("Content/Row0");
                entry.GetChild(0).GetComponent<TMP_InputField>().text = lineData[0];
				if (gds.gender == "Male")
					entry.GetChild (2).GetComponent<TMP_InputField> ().text = lineData [1];
				else if (gds.gender == "Female")
					entry.GetChild (2).GetComponent<TMP_InputField> ().text = lineData [2];
				else if (gds.gender == "Other")
					entry.GetChild (2).GetComponent<TMP_InputField> ().text = lineData [2];
				else 
					Debug.Log ("A gender was not entered in Patient Info");
            }
            reader.Close();
        }

        else if (!ddLabel.text.Equals("") && entryParent.childCount == 0)
        { 
            // Create a single entry
            entryParent.GetComponent<HistoryFieldManagerScript>().AddEntryFromButton("LabTestEntry");
        } else {
			entryParent.GetComponent<HistoryFieldManagerScript>().RemoveEntries(entryParent);
		}
    }
		
}

