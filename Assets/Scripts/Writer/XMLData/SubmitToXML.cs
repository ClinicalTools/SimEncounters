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
    private TabManager TabManager;			//Section and Tab manager
    private GameObject BG;

    // Use this for initialization
    void Awake()
    {
        //fileName.text = "test.txt";
        BG = GameObject.Find("GaudyBG");
        TabManager = BG.GetComponentInChildren<TabManager>();
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
    }

    void OnEnable()
    {
        print("Enabled");
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
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
