using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;
using System;
using TMPro;
using UnityEngine.Networking;

/**
 * Handles submitting all information to an XML file
 */
public class SubmitToXML : MonoBehaviour
{

    private string path = "";               //Default XML path
    public string fileName = "";            //Chosen filename for current XML file.
    public int minutesBetweenSaves;         //Minutes between autosaves
    private DataScript ds;                  //Data Manager
    private TabManager TabManager;			//Section and Tab manager
    private GameObject BG;
    private bool autosaving;

    // Use this for initialization
    void Awake()
    {
        //fileName.text = "test.txt";
        BG = GameObject.Find("GaudyBG");
        TabManager = BG.GetComponentInChildren<TabManager>();
        ds = BG.GetComponentInChildren<DataScript>();
        fileName = GlobalData.fileName;
        path = GlobalData.filePath;
        if (GlobalData.demo) {
            GetComponent<CollapseContentScript>().SetTarget(false);
            if (gameObject.name == "SaveCaseBG") {
                transform.Find("SaveCasePanel/RowTitle/PublishButton").GetComponent<Button>().interactable = false;
            }
        } else {
            if (GetComponent<CollapseContentScript>()) {
                GetComponent<CollapseContentScript>().SetTarget(true);
            }
        }

        if (gameObject.name == "SaveCaseBG") {
            if (GlobalData.role.Equals(GlobalData.Roles.Admin)) {
                transform.Find("SaveCasePanel/Content/Row1/TemplateToggle").gameObject.SetActive(true);
            } else {
                transform.Find("SaveCasePanel/Content/Row1/TemplateToggle").GetComponent<Toggle>().isOn = false;
                transform.Find("SaveCasePanel/Content/Row1/TemplateToggle").gameObject.SetActive(false);
            }
        }
        autosaving = false;
    }

    void OnEnable()
    {
        print("Enabled");
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
        if (ds == null) {
            return;
        }
        SetSaveCaseBGPatientName();

        if (gameObject.name == "SaveCaseBG") {
            if (GlobalData.role.Equals(GlobalData.Roles.Admin)) {
                transform.Find("SaveCasePanel/Content/Row1/TemplateToggle").gameObject.SetActive(true);
            } else {
                transform.Find("SaveCasePanel/Content/Row1/TemplateToggle").GetComponent<Toggle>().isOn = false;
                transform.Find("SaveCasePanel/Content/Row1/TemplateToggle").gameObject.SetActive(false);
            }
        }
        //CheckIfTemplate();
    }

    public void OpenSavePanel()
    {
        if (!GlobalData.GDS.developer && GlobalData.role.Equals(GlobalData.Roles.Guest) && !GlobalData.demo) {
            transform.parent.Find("QuickStartRegistration").gameObject.SetActive(true);
        } else {
            gameObject.SetActive(true);
        }
    }

    public void Autosave()
    {
        if (TabManager == null) {
            Awake();
        }
        autosaving = true;
        SubmitToFile();
    }

    private void CheckIfTemplate()
    {
        if (GlobalData.caseObj.IsTemplate()) {
            transform.Find("SaveCasePanel/Content/Row1/OverwriteTemplateToggle").gameObject.SetActive(true);
        } else {
            transform.Find("SaveCasePanel/Content/Row1/OverwriteTemplateToggle").gameObject.SetActive(false);
        }
    }

    private void CheckDuplicateFiles()
    {
        print("filename: " + GlobalData.fileName + ", recordNumber: " + GlobalData.caseObj.recordNumber);

        string recordNumber = (System.Math.Floor(UnityEngine.Random.value * 999999) + "").PadLeft(6, '0');
        GlobalData.caseObj.recordNumber = recordNumber;
        GlobalData.fileName = GlobalData.caseObj.recordNumber + GlobalData.firstName + " " + GlobalData.lastName + ".ced";
        bool exit = false;
        do {
            if (File.Exists(GlobalData.fileName)) {
                recordNumber = (System.Math.Floor(UnityEngine.Random.value * 999999) + "").PadLeft(6, '0');
                GlobalData.caseObj.recordNumber = recordNumber;
                GlobalData.fileName = GlobalData.caseObj.recordNumber + GlobalData.firstName + " " + GlobalData.lastName + ".ced";
            } else {
                exit = true;
                break;
            }
        } while (exit == false);

        print("filename: " + GlobalData.fileName + ", recordNumber: " + GlobalData.caseObj.recordNumber);
    }

    /**
	 * Saves the dictionary as an XML file to the local system (both formatted/easy to read and non-formatted)
	 */
    public void SubmitToFile()
    {
        if (TabManager == null) {
            BG = GameObject.Find("GaudyBG");
            TabManager = BG.GetComponentInChildren<TabManager>();
            ds = BG.GetComponentInChildren<DataScript>();
            fileName = GlobalData.fileName;
            path = GlobalData.filePath;
        }

        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }

        TabManager.sameTab = true;
        TabManager.AddToDictionary();
        TabManager.GetSectionImages();

        fileName = GlobalData.fileName;
        string tempFileName = fileName.Remove(fileName.Length - 3);

        string data = "<body>" + ds.GetData() + "</body>";
        Debug.Log(data);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(data);

        string textDataExtension = "ced";
        if (autosaving) {
            textDataExtension = "auto";
        }

        //Outputting regular file
        StreamWriter sw = new StreamWriter(path + tempFileName + textDataExtension, false);
        //sw.WriteLine(data);
        sw.Close();
        sw.Dispose();
        File.WriteAllBytes(path + tempFileName + textDataExtension, EncryptStringToBytes_Aes(data));

        if (!autosaving) { //Don't want to save images when autosaving, just text for now.
                           //Formatted, easy to read version
            sw = new StreamWriter(path + tempFileName + "xml", false);
            xmlDoc.Save(sw);
            sw.Close();
            sw.Dispose();

            //Easy to read images (For testing)
            sw = new StreamWriter(path + "ImageTest" + tempFileName + "xml", false);
            xmlDoc.LoadXml(ds.GetImagesXML());
            xmlDoc.Save(sw);
            sw.Close();
            sw.Dispose();

            File.Delete(path + tempFileName + "auto");
            File.Delete(path + tempFileName + "iauto");
            ds.RestartAutosave();
        }

        //Images
        string imgDataExtension = "cei";
        if (autosaving) {
            textDataExtension = "iauto";

            // Autosaving should have the most recent save
            GlobalData.caseObj.dateModified = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
        sw = new StreamWriter(path + tempFileName + imgDataExtension, false);
        //sw.WriteLine (ds.GetImagesXML ());
        sw.Close();
        sw.Dispose();
        File.WriteAllBytes(path + tempFileName + imgDataExtension, EncryptStringToBytes_Aes(ds.GetImagesXML()));

        //Update the caseObj to create the menu file
        Transform content = transform.Find("SaveCasePanel/Content");
        GlobalData.caseObj.description = content.Find("Row3/TMPInputField/DescriptionValue").GetComponent<TMP_InputField>().text;
        GlobalData.caseObj.summary = content.Find("Row5/TMPInputField/SummaryValue").GetComponent<TMP_InputField>().text;
        GlobalData.caseObj.tags = GetComponent<AutofillTMP>().enteredTags.ToArray();
        GlobalData.caseObj.audience = content.Find("Row7/TargetAudienceValue").GetComponent<TMP_Dropdown>().captionText.text;
        GlobalData.caseObj.difficulty = content.Find("Row7/DifficultyValue").GetComponent<TMP_Dropdown>().captionText.text;

        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime localFileModified = File.GetLastWriteTime(GlobalData.filePath + GlobalData.fileName);

        //GlobalData.caseObj.dateModified = (long) DateTime.UtcNow.Subtract (unixEpoch).TotalSeconds;// Old method
        GlobalData.caseObj.dateModified = (long)localFileModified.ToUniversalTime().Subtract(unixEpoch).TotalSeconds;

        File.WriteAllText(GlobalData.filePath + GlobalData.fileName.Remove(GlobalData.fileName.Length - 4) + " menu.txt", ds.ServerUploader.GetMenuText());

        //Old demo menu appending system
        /*if (GlobalData.demo) {
			bool append = true;

			StreamReader reader = new StreamReader(Application.streamingAssetsPath + GlobalData.filePath + GlobalData.fileName);
			string fileText = reader.ReadToEnd();
			string[] cases = fileText.Split(new string[] { "::" }, System.StringSplitOptions.RemoveEmptyEntries);
			foreach (string s in cases) {
				string[] caseSplit = s.Split(new string[] { "--" }, System.StringSplitOptions.None);
				if (caseSplit[1].Equals(GlobalData.fileName)) {
					caseSplit[3] = GlobalData.firstName + "_" + GlobalData.lastName;
					fileText = fileText.Replace(s, string.Join("--", caseSplit));
					append = false;
					break;
				}
			}
			reader.Close();
			StreamWriter writer = new StreamWriter(Application.streamingAssetsPath + "/DemoCases/Cases/MenuCases/MenuCases.txt", append);
			if (append) {
				writer.Write("0--{0}--ECGC Guest--{1}--000000--Beginner--User Custom Case--This case was created by one of our guests!--N/A--0--ECGC Guests--1.0--N/A::", tempFileName + "ced", GlobalData.firstName + "_" + GlobalData.lastName);
			} else {
				writer.Write(fileText); //Write all of the cases including the edited one
			}
			writer.Close();
		}*/

        Debug.Log("Saved: " + path + tempFileName);

        //b.interactable = false;
        if (autosaving) {
            autosaving = false;
            Debug.Log("Data Autosaved!");
            ds.ShowMessage("Data Autosaved!", false);
        } else {
            Debug.Log("Data successfully submitted!");
            ds.ShowMessage("Data saved successfully!", false);
        }
    }

    private string GetHexStringFromBytes(byte[] bytes)
    {
        StringBuilder hex = new StringBuilder(bytes.Length * 2);
        foreach (byte b in bytes)
            hex.AppendFormat("{0:x2}", b);
        return hex.ToString();
    }

    private string EncryptData(ICryptoTransform encrypt, byte[] bytes)
    {
        StringBuilder encryptedString = new StringBuilder();
        byte[] encryptedBytes = new byte[encrypt.OutputBlockSize];
        for (int i = 0; i < bytes.Length / encrypt.OutputBlockSize; i++) {
            encrypt.TransformBlock(bytes, i * encrypt.InputBlockSize, encrypt.InputBlockSize, encryptedBytes, 0);
            encryptedString.Append(System.Text.Encoding.UTF8.GetString(encrypt.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length)));
            //print(encryptedBytes);
            encryptedBytes = new byte[encrypt.OutputBlockSize];
        }
        encryptedString.Append(System.Text.Encoding.UTF8.GetString(encrypt.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length)));

        return encryptedString.ToString();
    }

    /**
	 * The following two methods are taken from Microsoft's page on managed aes encryption
	 * https://msdn.microsoft.com/en-us/library/system.security.cryptography.aesmanaged(v=vs.110).aspx
	 */
    static byte[] EncryptStringToBytes_Aes(string plainText)
    {
        byte[] encrypted;
        // Create an AesManaged object
        // with the specified key and IV.
        using (AesManaged aesAlg = new AesManaged()) {
            aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(GlobalData.encryptionKey);
            aesAlg.IV = System.Text.Encoding.UTF8.GetBytes(GlobalData.encryptionIV);

            // Create a decrytor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream()) {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)) {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt)) {
                        if (plainText.Length % aesAlg.BlockSize > 0) {
                            plainText.PadRight(plainText.Length % aesAlg.BlockSize, '0');
                        }
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }




        // Return the encrypted bytes from the memory stream.
        return encrypted;

    }

    /**
	 * This is called when the Publish button is pressed
	 */
    public void SubmitXML()
    {
        TabManager.sameTab = true;
        TabManager.AddToDictionary();
        TabManager.GetSectionImages();

        //Check to see if the entered tags are valid. safeToPublish will be false if they are not all correct
        if (!transform.GetComponent<SaveCaseScript>().IsSafeToPublish()) {
            ds.ShowMessage("One or more of your tags are invalid. Please fix before publishing!", true);
            return;
        }
        //SubmitToFile ();
        //ShowConfirmation ();

        Transform content = transform.Find("SaveCasePanel/Content");
        if (GlobalData.caseObj == null) {
            GlobalData.caseObj = new MenuCase(GlobalData.fileName);
        }
        GlobalData.caseObj.description = content.Find("Row3/TMPInputField/DescriptionValue").GetComponent<TMP_InputField>().text;
        GlobalData.caseObj.summary = content.Find("Row5/TMPInputField/SummaryValue").GetComponent<TMP_InputField>().text;
        GlobalData.caseObj.tags = GetComponent<AutofillTMP>().enteredTags.ToArray();
        //GlobalData.caseObj.tags = content.Find ("Row6/TMPAutoSuggest/TagsValue").GetComponent<TMP_InputField> ().text.Split (new string[] {"; "}, System.StringSplitOptions.RemoveEmptyEntries);
        GlobalData.caseObj.audience = content.Find("Row7/TargetAudienceValue").GetComponent<TMP_Dropdown>().captionText.text;
        GlobalData.caseObj.difficulty = content.Find("Row7/DifficultyValue").GetComponent<TMP_Dropdown>().captionText.text;

        GlobalData.caseObj.patientName = GameObject.Find("GaudyBG").transform.Find("SaveCaseBG/SaveCasePanel/Content/Row0/PatientNameValue").GetComponent<TextMeshProUGUI>().text;

        //We will set the case type to what is selected after we act upon what the current type and choices are, but we calculate the choice here
        //A not private case is 0x01
        //A template case is 0x02
        int caseType = 0x00;
        if (!content.Find("Row1/PrivateToggle").GetComponent<Toggle>().isOn) {
            caseType = caseType | 0x01;
        }
        if (content.Find("Row1/TemplateToggle").GetComponent<Toggle>().isOn) {
            caseType = caseType | 0x02;
        }
        ds.ServerUploader.SetNewCaseType(caseType);

        if (autosaving) {
            autosaving = false;
            Debug.Log("Autosaving...");
            ds.ShowMessage("Autosaving...", false);
        }

        ds.ServerUploader.StartUpload();
        gameObject.SetActive(false);
    }

    private string GetMenuText()
    {
        /* MENU CASE OBJECT ORDER
		 * accountID,
		 * filename,
		 * authorName,
		 * patientName,
		 * recordNumber,
		 * difficulty,
		 * description,
		 * summary,
		 * tags,
		 * modified,
		 * audience,
		 * version,
		 * rating,
		 * caseType
		 */

        if (GlobalData.caseObj == null) {
            GlobalData.caseObj = new MenuCase(GlobalData.fileName);
            CheckDuplicateFiles();
        }

        return GlobalData.accountId + "--" +
                GlobalData.fileName.Replace(" ", "_") + "--" +
                "Author Name Here" + "--" +
                GlobalData.firstName + "_" + GlobalData.lastName + "--" +
                GlobalData.caseObj.recordNumber + "--" +
                GlobalData.caseObj.difficulty + "--" +
                GlobalData.caseObj.description + "--" +
                GlobalData.caseObj.summary + "--" +
                GlobalData.caseObj.GetTagsAsOneString() + "--" +
                GlobalData.caseObj.dateModified + "--" +
                GlobalData.caseObj.audience + "--" +
                "version" + "--" +
                "Clearly the best" + "--" +
                GlobalData.caseObj.caseType.GetHashCode();
    }

    /**
	 * Finds the patient's name for this case and updates the SaveCaseBG Panel
	 */
    public void SetSaveCaseBGPatientName()
    {
        /*
         * Uncomment here if you want to instantiate SaveCaseBG. 
         * It has a dependency in DataScript().
         */
        //GameObject test = Instantiate(Resources.Load("Writer/Prefabs/Panels/SaveCaseBG")) as GameObject;
        //test.transform.SetParent(BG.transform, false);

        List<TabInfoScript> tis = new List<TabInfoScript>();

        TabManager.sameTab = true;
        TabManager.AddToDictionary();
        foreach (string key in ds.getKeys()) {
            foreach (string tabKey in ds.GetData(key).GetTabList()) {
                if (ds.GetData(key).GetTabInfo(tabKey).persistant) {
                    tis.Add(ds.GetData(key).GetTabInfo(tabKey));
                }
            }
        }
        string name = "";
        foreach (TabInfoScript tab in tis) {
            if (tab.customName.Equals("Personal Info")) {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(tab.data);
                GlobalData.firstName = UnityWebRequest.UnEscapeURL(xmlDoc.GetElementsByTagName("FirstNameValue")[0].InnerText);
                GlobalData.lastName = UnityWebRequest.UnEscapeURL(xmlDoc.GetElementsByTagName("LastNameValue")[0].InnerText);
                name = GlobalData.firstName + " " + GlobalData.lastName;
                break;
            }
        }
        Transform nameValue = GameObject.Find("GaudyBG").transform.Find("SaveCaseBG/SaveCasePanel/Content/Row0/PatientNameValue");
        nameValue.GetComponent<TextMeshProUGUI>().text = name;
    }
}
