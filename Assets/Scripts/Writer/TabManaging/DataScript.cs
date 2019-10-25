using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#if UNITY_EDITOR
#endif


/**
 * This script manages Data. It acts as the Intermediary data container.
 * Data is loaded from the chosen XML file into here.
 * Data is loaded from here into Tabs as needed.
 * Data is updated here and then sent to the XML file from here as well.
 */

public class DataScript : MonoBehaviour
{

    private Dictionary<string, SectionDataScript> Dict;     //A dictionary of Sections. Key=sectionName. Value=sectionData (all tab info)
    private Dictionary<string, SpriteHolderScript> imgDict; //A dictionary of any and all images. Key=Section(.Tab(.Entry name))

    private Dictionary<string, string> dialogueDict;
    private Dictionary<string, string> quizDict;
    private Dictionary<string, string> flagDict;
    private Dictionary<string, string> eventDict;
    public XmlDocument xmlDoc;                          //Holds data from the XML file. Easy to parse.
    private string path = "";                           //Default XML path
    private string fileName = "";                       //Chosen filename for current XML file.
    private bool overwrite = false;                     //Whether or not to load or overwrite the file
    public GameObject SectionButtonPar;                 //The parent gameobject for section buttons
    public string[] nonSectionDataPanels;               //Names of the panels whose data must be saved yet are not a part of any section
    public Dictionary<string, string> correctlyOrderedDialogues, correctlyOrderedQuizes, correctlyOrderedFlags;
    public List<Transform> newTabs;						//
    public Transform characterPanel;                    //
    private string serverCaseData;                      //Variable to hold loaded case data from the server
    private string serverImageData;                     //Variable to hold loaded image data from the server									
    public UploadToServer ServerUploader;
    public CanvasGroup loadingScreen;

    public static DataScript instance;

    [ContextMenu("Print dict")]
    private void PrintDict()
    {
        string keys = "";
        foreach (string key in dialogueDict.Keys) {
            keys += key + ",";
        }
        print(keys);
    }

    void Awake()
    {
        imgDict = new Dictionary<string, SpriteHolderScript>();
        Dict = new Dictionary<string, SectionDataScript>();
        dialogueDict = new Dictionary<string, string>();
        flagDict = new Dictionary<string, string>();
        eventDict = new Dictionary<string, string>();
        quizDict = new Dictionary<string, string>();
        newTabs = new List<Transform>();
        if (GlobalData.showLoading && SceneManager.sceneCount > 1) {
            //SceneManager.UnloadSceneAsync(0);
            //loadingScreen = SceneManager.GetSceneByName("LoadingScreen").GetRootGameObjects()[2].transform.Find("LoadingScreenNew").GetComponent<CanvasGroup>();
            if (GameObject.Find("LoadingScreenNew") != null) {
                loadingScreen = GameObject.Find("LoadingScreenNew").GetComponent<CanvasGroup>();
            }
            loadingScreen = LoadingScreenManager.Instance.GetComponent<CanvasGroup>();
        } else if (loadingScreen != null && GlobalData.showLoading) {
            loadingScreen.gameObject.SetActive(true);
        }
        // Use code below if you want to default to a specific file on application load

        /*
		if (fileName == null || fileName.Equals ("")) {
			fileName = "test.txt";
		} else if (!fileName.EndsWith (".txt")) {
			fileName = fileName + ".txt";
		}
        */
        instance = this;
    }

    // Use this for initialization
    IEnumerator Start()
    {
        path = GlobalData.filePath;
        Debug.Log(path);
        fileName = GlobalData.fileName;
        Debug.Log(fileName);
        transform.gameObject.AddComponent<UploadToServer>();
        ServerUploader = transform.GetComponent<UploadToServer>();
        serverCaseData = "";
        serverImageData = "";

        //! WRITER: check if new case
        if (GlobalData.caseObj == null) {
            GlobalData.newCase = true;
        }
        //if (fileName.Length > 5 && !GlobalData.loadLocal && !System.IO.File.Exists (path + fileName.Remove (fileName.Length - 3) + "ced")) {
        if (loadingScreen == null) {
            loadingScreen = LoadingScreenManager.Instance.GetComponent<CanvasGroup>();
        }

        //! WRITER: set loading screen if it's not a new case and download
        if (!GlobalData.newCase) {
            if (!GlobalData.loadLocal || !File.Exists(path + fileName)) {
                loadingScreen.transform.Find("LoadingBar").gameObject.SetActive(false);
                loadingScreen.transform.Find("LoadingBarDescription").GetComponent<TextMeshProUGUI>().text = "Connecting to server";
                yield return StartCoroutine(ServerUploader.DownloadFromServer("xmlData"));
                serverCaseData = ServerUploader.GetDownloadedText();
            }
            if (!GlobalData.loadLocal || !File.Exists(path + fileName)) {
                yield return StartCoroutine(ServerUploader.DownloadFromServer("imgData"));
                serverImageData = ServerUploader.GetDownloadedText();
            }
            loadingScreen.transform.Find("LoadingBar").gameObject.SetActive(false);
            loadingScreen.transform.Find("LoadingBarDescription").gameObject.SetActive(false);
        }
        correctlyOrderedDialogues = new Dictionary<string, string>();
        correctlyOrderedQuizes = new Dictionary<string, string>();
        correctlyOrderedFlags = new Dictionary<string, string>();

        //! WRITER: handle loading autosaves
        if (fileName.Length > 5) {
            LoadImages();

            //Since we're only autosaving the ced file, we swap the filename from ced to auto when loading data
            if (GlobalData.loadAutosave) {
                fileName = fileName.Remove(GlobalData.fileName.Length - "ced".Length) + "auto";
                GlobalData.fileName = fileName;
                PopulateDict();
                fileName = fileName.Remove(GlobalData.fileName.Length - "auto".Length) + "ced";
                GlobalData.fileName = fileName;
                GlobalData.loadAutosave = false;
            } else {
                PopulateDict();
            }
        }

        //! WRITER: load tab manager
        transform.GetComponent<TabManager>().FirstTimeLoad();

        //! WRITER: guests can't show a case in the reader from the writer??????
        if (GlobalData.role == GlobalData.Roles.Guest) {
            transform.Find("SidePanel/MainPanel/MenuPanel/ShowCaseInReader").gameObject.SetActive(false);
        }

        //GlobalData.firstName = "";
        //GlobalData.lastName = "";

        //! WRITER: set up initial handling of patient name info
        if (GlobalData.firstName != null && GlobalData.firstName.Equals("") && GlobalData.caseObj != null) {
            if (!GlobalData.caseObj.patientName.Equals("")) {
                string[] nameSplit = GlobalData.caseObj.patientName.Split('_');
                GlobalData.firstName = nameSplit[0];
                if (nameSplit.Length > 1) {
                    GlobalData.lastName = nameSplit[1];
                }
            } else {
                GlobalData.firstName = "";
                GlobalData.lastName = "";
            }
        }

        //! WRITER: handle template
        //Check to see if the case opened is a template. If it is, set it up so the user saves a copy
        if (GlobalData.createCopy) {// || (GlobalData.caseObj.caseType.GetHashCode() & GlobalData.caseObj.templateCompare) == GlobalData.caseObj.templateCompare) {
            CheckDuplicateFiles();
            GlobalData.createCopy = false;
        }

        //! WRITER: handle saving autosaves
        int minutesBetweenSaves = GlobalData.autosaveRate;
        //int minutesBetweenSaves = transform.Find("SaveCaseBG").GetComponent<SubmitToXML>().minutesBetweenSaves;
        if (GlobalData.caseObj != null && minutesBetweenSaves > 0 && GlobalData.enableAutoSave) { //If the user is logged in/came from the main menu, they'll have a case object
            print("Autosaving started. Saving every " + minutesBetweenSaves + " minutes.");
            InvokeRepeating("Autosaving", minutesBetweenSaves * 60, minutesBetweenSaves * 60);
        }

        //! WRITER: handle load into writer button
        //When playing just the writer
        //print((GlobalData.caseObj == null) + ", " + GlobalData.GDS.developer);
        if (GlobalData.caseObj == null && GlobalData.GDS.developer) {
            transform.Find("SidePanel/MainPanel/MenuPanel/LoadIntoWriterButton").gameObject.SetActive(true);
            GlobalData.filePath = "C:/Users/Will/AppData/LocalLow/Clinical Tools Inc/Clinical Encounters_ Creator";

        }
        if (loadingScreen != null && GlobalData.showLoading) {
            loadingScreen.blocksRaycasts = false;
            LoadingScreenManager.Instance.Fade();
        }
        yield return null;
    }

    //! WRITER ONLY METHOD
    private void CheckDuplicateFiles()
    {
        if (GlobalData.caseObj == null) {
            GlobalData.caseObj = new MenuCase(GlobalData.fileName);
        }
        print("filename: " + GlobalData.fileName + ", recordNumber: " + GlobalData.caseObj.recordNumber);

        string recordNumber = (Math.Floor(UnityEngine.Random.value * 999999) + "").PadLeft(6, '0');
        GlobalData.caseObj.recordNumber = recordNumber;
        GlobalData.fileName = "[CHECKFORDUPLICATE]" + GlobalData.caseObj.recordNumber + GlobalData.firstName + " " + GlobalData.lastName + ".ced";
        bool exit = false;
        do {
            if (File.Exists(GlobalData.fileName)) {
                recordNumber = (Math.Floor(UnityEngine.Random.value * 999999) + "").PadLeft(6, '0');
                GlobalData.caseObj.recordNumber = recordNumber;
                GlobalData.fileName = "[CHECKFORDUPLICATE]" + GlobalData.caseObj.recordNumber + GlobalData.firstName + " " + GlobalData.lastName + ".ced";
            } else {
                exit = true;
                break;
            }
        } while (exit == false);

        print("filename: " + GlobalData.fileName + ", recordNumber: " + GlobalData.caseObj.recordNumber);
    }

    private void Autosaving()
    {
        Debug.Log("Autosaving...");
        ShowMessage("Autosaving...", false);
        transform.Find("SaveCaseBG").GetComponent<SubmitToXML>().Autosave();
    }

    //! WRITER ONLY METHOD
    public void RestartAutosave()
    {
        int minutesBetweenSaves = GlobalData.autosaveRate;
        CancelInvoke("Autosaving");
        InvokeRepeating("Autosaving", minutesBetweenSaves * 60, minutesBetweenSaves * 60);
        print("Restarted autosaving every " + minutesBetweenSaves + " minutes");
    }

    /**
	 * Loads images from the images XML file
	 */
    //! LOADIMAGES
    public void LoadImages()
    {
        string imageFileName = fileName.Remove(fileName.Length - 3);
        if (GlobalData.loadAutosave && File.Exists(path + imageFileName + "iauto")) {
            imageFileName += "iauto";
        } else {
            imageFileName += "cei";
        }

        xmlDoc = new XmlDocument();
        if (serverImageData.Equals("") && File.Exists(path + imageFileName)) {
            StreamReader read = new StreamReader(path + imageFileName);
            string s = read.ReadToEnd();
            if (s == null || s.Equals("")) {
                Debug.Log("No images to load!");
                read.Close();
                //Maybe ask if they want to check the server here?
                return;
            }

            try {
                xmlDoc.LoadXml(s); //this loads the local file
                read.Close();
            } catch (Exception e) {
                print(e.Message);
                read.Close();
                string text = "";
                AesManaged aes = new AesManaged();
                ICryptoTransform decrypt = aes.CreateDecryptor(Encoding.UTF8.GetBytes(GlobalData.encryptionKey), Encoding.UTF8.GetBytes(GlobalData.encryptionIV));
                using (FileStream fs = new FileStream(path + imageFileName, FileMode.Open)) {
                    using (CryptoStream cs = new CryptoStream(fs, decrypt, CryptoStreamMode.Read)) {
                        using (StreamReader sr = new StreamReader(cs)) {
                            text = sr.ReadToEnd();
                            print(text);
                        }
                    }
                }
                xmlDoc.LoadXml(text);
            }





        } else {
            if (serverImageData.Equals("") || serverImageData == null) {
                Debug.Log("No image data to load from server");
                return;
            }
            xmlDoc.LoadXml(serverImageData); //this loads from the server

            //Debug.Log("File does not exist. Cannot load Images!");
            //return;
        }
        //imgDict = new Dictionary<string, Sprite> ();

        VarData.Reset();
        CondData.Reset();
        XmlNode node = xmlDoc.FirstChild;
        while (node != null) {
            if (node.Name.ToLower().StartsWith("image")) {
                while (!node.Name.Equals("key")) {
                    node = xmlDoc.AdvNode(node);
                }

                string key = node.InnerText.Replace(GlobalData.EMPTY_WIDTH_SPACE + "", "");
                while (!node.Name.Equals("imgData")) {
                    node = xmlDoc.AdvNode(node);
                }

                node = xmlDoc.AdvNode(node);
                Color c = new Color();
                bool newColor = false;
                if (node.Name.Equals("iconColor")) {
                    node = xmlDoc.AdvNode(node);
                    string colorValue = node.Value;
                    string[] vars = Regex.Split(colorValue, ",");
                    c.r = float.Parse(vars[0]);
                    c.g = float.Parse(vars[1]);
                    c.b = float.Parse(vars[2]);
                    c.a = float.Parse(vars[3]);
                    //c = (node = AdvNode (node)).Value;
                    newColor = true;
                    node = xmlDoc.AdvNode(node);
                }

                if (node.Name.Equals("reference")) {
                    if (imgDict.ContainsKey(key)) {
                        Debug.Log("Image " + key + " not added. Another image already has that key.");
                    } else {
                        //If using special characters in sections, replace key here with formatted version
                        imgDict.Add(key, new SpriteHolderScript(node.InnerText));
                    }
                } else {
                    if (imgDict.ContainsKey(key)) {
                        continue;
                    }
                    while (!node.Name.Equals("width")) {
                        node = xmlDoc.AdvNode(node);
                    }
                    int width = int.Parse(node.InnerText);

                    while (!node.Name.Equals("height")) {
                        node = xmlDoc.AdvNode(node);
                    }
                    int height = int.Parse(node.InnerText);

                    while (!node.Name.Equals("data")) {
                        node = xmlDoc.AdvNode(node);
                    }

                    Texture2D temp = new Texture2D(2, 2);
                    temp.LoadImage(Convert.FromBase64String(node.InnerText));
                    Sprite newSprite = Sprite.Create(temp, new Rect(0, 0, width, height), new Vector2(0, 0), 100);
                    imgDict.Add(key, new SpriteHolderScript(newSprite));
                    //gObjText.sprite = newSprite;
                }
                if (newColor) {
                    imgDict[key].useColor = true;
                    imgDict[key].color = c;
                }
            }

            node = VarData.ReadNode(xmlDoc, node);
            node = CondData.ReadNode(xmlDoc, node);

            node = xmlDoc.AdvNode(node);
        }

        if (GetImage(GlobalData.patientImageID) != null && GetImage(GlobalData.patientImageID).sprite != null) {
            GameObject.Find("CharacterCamera").transform.Find("Canvas/Image").GetComponent<Image>().sprite = GetImage(GlobalData.patientImageID).sprite;
            GameObject.Find("CharacterCamera").transform.Find("Canvas").gameObject.SetActive(true);
        }
        //! READER: No character image ????

        //Debug.Log (string.Join (",", imgDict.Keys.ToArray ()));
    }


    /**
	 * Used to pull the data from the filename input text box I had
	 */
    public void SetFileName(Text file)
    {
        string s = file.text;
        if (!s.EndsWith(".ced")) {
            s += ".ced";
        }
        fileName = s;
    }

    /**
	 * Returns the case save file name
	 */
    public string GetFileName()
    {
        return fileName;
    }

    /**
	 * Whether or not to overwrite the save file. If overwrite = true, then we do not load the file
	 */
    public void Overwrite(Toggle overwrite)
    {
        this.overwrite = overwrite.isOn;
    }

    /**
	 * Clears all changes and loads in the file specified by fileName
	 */
    public void ReloadFile()
    {
        ClearAllData();
        //ClearVitalsPanel ();
        Awake();
        Start();
    }

    /**
	 * Adds an image by image key and the name of the image file (a reference to images already saved)
	 */
    public void AddImg(string key, string imgRefName)
    {
        if (imgDict.ContainsKey(key)) {
            imgDict[key].referenceName = imgRefName;
        } else {
            imgDict.Add(key, new SpriteHolderScript(imgRefName));
        }
    }

    /**
	 * Adds an image by image key and by the sprite of the image itself
	 */
    public void AddImg(string key, Sprite s)
    {
        // Remove all empty space characters from the string
        key = key.Replace("​", "");
        if (imgDict.ContainsKey(key)) {
            imgDict[key].sprite = s;
        } else {
            imgDict.Add(key, new SpriteHolderScript(s));
        }
    }

    /**
	 * Returns the dictionary of images (No longer using due to helper methods)
	 */
    public Dictionary<string, SpriteHolderScript> GetImages()
    {
        return imgDict;
    }

    /**
	 * Returns the dictionary of dialogues
	 */
    public Dictionary<string, string> GetDialogues()
    {
        return dialogueDict;
    }

    /**
	 * Returns the dictionary of flags
	 */
    public Dictionary<string, string> GetFlags()
    {
        return flagDict;
    }

    /**
	 * Returns the dictionary of events
	 */
    public Dictionary<string, string> GetEvents()
    {
        return eventDict;
    }

    /**
	 * Add a dialogue to the dictionary
	 */
    public void AddDialogue(string key, string data)
    {
        if (dialogueDict.ContainsKey(key)) {
            dialogueDict[key] = data;
        } else {
            dialogueDict.Add(key, data);
        }
    }

    /**
    * Add a flag to the dictionary
    */
    public void AddFlag(string key, string data)
    {
        if (flagDict.ContainsKey(key)) {
            flagDict[key] = data;
        } else {
            flagDict.Add(key, data);
        }
    }

    public string GetAllQuizData()
    {
        return string.Join("", quizDict.Select(x => x.Value).ToArray());
    }

    public string GetQuizData(string uniquePath)
    {
        if (quizDict.ContainsKey(uniquePath)) {
            return quizDict[uniquePath];
        } else {
            return "<data></data>";
        }
    }

    public Dictionary<string, string> GetQuizes()
    {
        return quizDict;
    }

    public void AddQuiz(string key, string data)
    {
        if (quizDict.ContainsKey(key)) {
            quizDict[key] = data;
        } else {
            quizDict.Add(key, data);
        }
    }

    /**
	 * Returns the XML string to represent all images
	 */
    public string GetImagesXML()
    {
        string data = "<Body>";
        int i = 0;
        foreach (string key in imgDict.Keys) {
            data += "<image" + i + ">";
            //If using section special characters, replace key with formatted version
            data += "<key>" + key + "</key>";
            data += "<imgData>" + imgDict[key].GetXMLText() + "</imgData>";
            data += "</image" + i + ">";
            i++;
        }

        data += VarData.GetXML();
        data += CondData.GetXML();

        data += "</Body>";
        return data;
    }

    public SpriteHolderScript GetImage(string uid)
    {
        if (uid != null && imgDict.ContainsKey(uid)) {
            return imgDict[uid];
        } else {
            return null;
        }
    }


    public List<string> GetImageKeys()
    {
        return imgDict.Keys.ToList();
    }


    public bool RemoveImage(string uid)
    {
        if (imgDict.ContainsKey(uid)) {
            imgDict.Remove(uid);
            return true;
        }
        return false;
    }

    public List<string> GetSectionsList()
    {
        return Dict.Keys.ToList();
    }

    static string DecryptStringFromBytes_Aes(byte[] cipherText)
    {
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");

        // Declare the string used to hold
        // the decrypted text.
        string plaintext;

        // Create an AesManaged object
        // with the specified key and IV.
        using (AesManaged aesAlg = new AesManaged()) {
            aesAlg.Key = Encoding.UTF8.GetBytes(GlobalData.encryptionKey);
            aesAlg.IV = Encoding.UTF8.GetBytes(GlobalData.encryptionIV);

            // Create a decrytor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText)) {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)) {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {

                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                        return plaintext;
                    }
                }
            }

        }
    }

    private byte[] GetBytesFromHex(string hex)
    {
        StringBuilder sb = new StringBuilder(hex);
        byte[] bytes = new byte[hex.Length];
        for (int i = 0; i < hex.Length / 2; i++) {
            bytes[i] = byte.Parse(sb.Remove(i, 1).ToString(), System.Globalization.NumberStyles.HexNumber);
        }
        return bytes;
    }

    /**
	 * Populates the Dictionary from the XML file
	 * Used to load in new data or data at startup
	 */
    public void PopulateDict()
    {
        if (fileName == null) {
            return;
        }
        //Finds the file and loads data into xmlDoc
        xmlDoc = new XmlDocument();
        if (serverCaseData.Equals("") && File.Exists(path + fileName)) {
            if (overwrite) {
                SpawnDefaultSection();
                //ClearVitalsPanel ();
                return;
            }
            StreamReader read = new StreamReader(path + fileName);
            string s = read.ReadToEnd();
            if (s == null || s.Equals("")) {
                Debug.Log("No data to load locally!");
                read.Close();
                //Maybe ask if they want to check the server here?
                return;
            }

            try {
                xmlDoc.LoadXml(s); //This loads the locally saved file
                read.Close();
            } catch (Exception) {
                read.Close();
                string text = "";
                AesManaged aes = new AesManaged();
                ICryptoTransform decrypt = aes.CreateDecryptor(Encoding.UTF8.GetBytes(GlobalData.encryptionKey), Encoding.UTF8.GetBytes(GlobalData.encryptionIV));
                using (FileStream fs = new FileStream(GlobalData.filePath + GlobalData.fileName, FileMode.Open)) {
                    using (CryptoStream cs = new CryptoStream(fs, decrypt, CryptoStreamMode.Read)) {
                        using (StreamReader sr = new StreamReader(cs)) {
                            text = sr.ReadToEnd();
                        }
                    }
                }


                //xmlDoc.LoadXml(DecryptStringFromBytes_Aes(GetBytesFromHex(s)));
                xmlDoc.LoadXml(text);
            }
            //xmlDoc.LoadXml(serverCaseData); //This loads from the server
        } else {
            if (serverCaseData.Equals("") || serverCaseData == null) {
                Debug.Log("No case data to load from server");
                SpawnDefaultSection();
                return;
            }
            xmlDoc.LoadXml(serverCaseData); //This loads from the server
        }

        //Loads the data into the Dictionary variable
        XmlNode node = xmlDoc.FirstChild;
        node = xmlDoc.AdvNode(node);
        string sectionName = null;
        string tabName = null;
        SectionDataScript xmlDict = new SectionDataScript();
        xmlDict.Initiate();
        xmlDict.SetPosition(0);
        string customTabName = null;
        bool inSections = false;


        while (!inSections) { //Load in any panel's data that is not in a section
            print(node.Name + ", " + node.Value + ", " + node.OuterXml);
            Transform[] children;

            XmlDocument element = new XmlDocument();

            if (node.Name.Equals("CharacterEditorPanel")) {
                element.LoadXml(node.OuterXml);
                children = characterPanel.GetComponentsInChildren<Transform>(true);

                XmlDocument tempXml = new XmlDocument();
                tempXml.LoadXml("<rootElement>" + xmlDoc.GetElementsByTagName("Personal_InfoTab")[0].InnerXml + "</rootElement>");
                string gend = tempXml.GetElementsByTagName("Gender").Item(0).InnerText;
                if (transform.parent.GetComponent<CharacterManagerScript>()) {
                    //transform.parent.GetComponent<CharacterManagerScript>().setCharacter(gend);
                }
                GlobalData.gender = gend;
            } else if (transform.Find(node.Name) != null) {
                element.LoadXml(node.OuterXml);
                children = transform.Find(node.Name).GetComponentsInChildren<Transform>(true);
            } else {
                print(node.ParentNode.OuterXml);
                print("ERROR. NO PANEL FOUND. RETURNING");
                return;
            }
            bool endCurrentXMLSection = false;
            while (node != null && node.Value == null && !inSections) {
                node = xmlDoc.AdvNode(node);
                if (node.Name.Equals("Sections")) {
                    inSections = true;
                    print(node.Name + ", " + node.Value + ", " + node.OuterXml);
                    break;
                } else if (node.PreviousSibling != null && node.PreviousSibling.Name.Equals(children[0].name)) {
                    endCurrentXMLSection = true;
                    break;
                }
            }

            foreach (Transform child in children) {
                int pos = 0;
                if (element.GetElementsByTagName(child.name).Count == 0) {
                    continue;
                } else if (element.GetElementsByTagName(child.name).Count > 1) {
                    //If there is more than one match, find where the current child sits compared to its siblings which share the same name
                    List<Transform> sameNames = children.ToList().FindAll((Transform obj) => obj.name.Equals(child.name));
                    pos = sameNames.FindIndex((Transform obj) => obj.Equals(child));
                }

                //Assign the value in the XML to a string to make the code easier to read
                string xmlValue = element.GetElementsByTagName(child.name).Item(pos).InnerText;

                //Set the data according to the type of data field
                if (child.gameObject.GetComponent<InputField>() != null) {
                    child.gameObject.GetComponent<InputField>().text = UnityWebRequest.UnEscapeURL(xmlValue);
                    if (child.GetComponent<InputFieldResizer>()) {
                        child.GetComponent<InputFieldResizer>().ResizeField();
                    }
                } else if (child.gameObject.GetComponent<TMP_InputField>() != null) {
                    child.gameObject.GetComponent<TMP_InputField>().text = UnityWebRequest.UnEscapeURL(xmlValue);
                    if (child.GetComponent<InputFieldResizer>()) {
                        child.GetComponent<InputFieldResizer>().ResizeField();
                    }
                } else if (child.gameObject.GetComponent<Dropdown>() != null) {
                    int indexValue = 0;
                    foreach (Dropdown.OptionData myOptionData in child.gameObject.GetComponent<Dropdown>().options) {
                        if (myOptionData.text.Equals(UnityWebRequest.UnEscapeURL(xmlValue))) {
                            break;
                        }
                        indexValue++;
                    }
                    child.gameObject.GetComponent<Dropdown>().value = indexValue;
                } else if (child.gameObject.GetComponent<TMP_Dropdown>() != null) {
                    int indexValue = 0;
                    foreach (TMP_Dropdown.OptionData myOptionData in child.gameObject.GetComponent<TMP_Dropdown>().options) {
                        if (myOptionData.text.Equals(UnityWebRequest.UnEscapeURL(xmlValue))) {
                            break;
                        }
                        indexValue++;
                    }
                    child.gameObject.GetComponent<TMP_Dropdown>().value = indexValue;
                } else if (child.gameObject.GetComponent<Toggle>() != null && xmlValue != null && !xmlValue.Equals("")) {
                    child.gameObject.GetComponent<Toggle>().isOn = bool.Parse(xmlValue);
                } else if (child.gameObject.GetComponent<Text>() != null) {
                    child.gameObject.GetComponent<Text>().text = UnityWebRequest.UnEscapeURL(xmlValue);
                } else if (child.gameObject.GetComponent<TextMeshProUGUI>() != null) {
                    child.gameObject.GetComponent<TextMeshProUGUI>().text = UnityWebRequest.UnEscapeURL(xmlValue);
                } else if (child.name.Equals("Image") && child.GetComponent<OpenImageUploadPanelScript>()) {
                    Debug.Log("LOADING IMAGE: " + xmlValue);
                    child.GetComponent<OpenImageUploadPanelScript>().SetGuid(xmlValue);

                    Image img = child.GetComponent<Image>();
                    img.sprite = null;

                    if (imgDict.ContainsKey(element.GetElementsByTagName(child.name).Item(0).InnerText)) { //Load image
                        img.sprite = imgDict[xmlValue].sprite;
                    }

                    if (img.sprite == null) {
                        img.GetComponent<CanvasGroup>().alpha = 0f;
                        img.transform.parent.GetComponent<Image>().enabled = true;
                    } else {
                        img.GetComponent<CanvasGroup>().alpha = 1f;
                        img.transform.parent.GetComponent<Image>().enabled = false;
                    }
                } else if (child.gameObject.GetComponent<Slider>() != null) {
                    //child.gameObject.GetComponent<Slider> ().value = float.Parse (xmlValue);
                    if (child.name.Equals("AgeSlider")) {
                        NextFrame.Function(delegate { child.gameObject.GetComponent<Slider>().value = float.Parse(xmlValue); });
                    } else {
                        child.gameObject.GetComponent<Slider>().value = float.Parse(xmlValue);
                    }
                }
            }

            while (node != null || endCurrentXMLSection) {
                if (node.PreviousSibling != null && node.PreviousSibling.Name.Equals(children[0].name)) {// node.Name.ToLower ().EndsWith ("section")) {
                    endCurrentXMLSection = true;
                    if (node.Name.Equals("Sections") && !inSections) {
                        inSections = true;
                    }
                    //print (node.Name + ", " + node.Value + ", " + node.OuterXml);
                    break;
                }
                node = xmlDoc.AdvNode(node);
            }
        }

        //Special case, set case description from caseObj due to case copies
        transform.Find("SaveCaseBG/SaveCasePanel/Content/Row3/TMPInputField/DescriptionValue").GetComponent<TMP_InputField>().text = GlobalData.caseObj.description;

        node = xmlDoc.AdvNode(node); //Go inside sections

        while (node != null) {
            if (node.Name.ToLower().EndsWith("section")) {
                if (sectionName != null) {
                    if (Dict.ContainsKey(sectionName)) {
                        Dict[sectionName] = xmlDict;
                    } else {
                        Dict.Add(sectionName, xmlDict);
                    }
                }
                xmlDict = new SectionDataScript();
                xmlDict.Initiate();
                xmlDict.SetPosition(Dict.Count);
                sectionName = ConvertNameFromXML(node.Name);
                while (node != null && node.Name.ToLower() != "conditionals" && !node.Name.ToLower().EndsWith("tab")) {
                    node = xmlDoc.AdvNode(node);
                }


                if (node != null && node.Name.ToLower() == "conditionals") {
                    node = xmlDoc.AdvNode(node);

                    var sectionCondKeys = new List<string>();
                    while (node.Name.Equals("cond")) {
                        node = xmlDoc.AdvNode(node);
                        sectionCondKeys.Add(node.Value);
                        node = xmlDoc.AdvNode(node);
                    }
                    xmlDict.Conditions = sectionCondKeys;

                    XmlNode tempNode = xmlDoc.AdvNode(node);
                    while (node != null && !node.Name.ToLower().EndsWith("tab"))
                        node = xmlDoc.AdvNode(node);
                }
            }

            List<string> condKeys = null;
            if (node != null && node.Name.ToLower().EndsWith("tab") && tabName == null) {
                tabName = node.Name.Replace("_", " ").Substring(0, node.Name.Length - 3); //Unformat tabType
                XmlNode tempNode = xmlDoc.AdvNode(node);
                if (tempNode.Name.Equals("customTabName")) {
                    tempNode = xmlDoc.AdvNode(tempNode);
                    customTabName = tempNode.Value;
                } else {
                    customTabName = tabName;
                }

                tempNode = xmlDoc.AdvNode(tempNode);
                if (tempNode.Name.Equals("conditionals")) {
                    tempNode = xmlDoc.AdvNode(tempNode);

                    condKeys = new List<string>();
                    while (tempNode.Name.Equals("cond")) {
                        tempNode = xmlDoc.AdvNode(tempNode);
                        condKeys.Add(tempNode.Value);
                        tempNode = xmlDoc.AdvNode(tempNode);
                    }
                }

                while (node != null && !node.Name.ToLower().Equals("data")) {
                    node = xmlDoc.AdvNode(node);
                }
            }
            if (tabName != null && sectionName != null && node.Name.Equals("data")) {
                //if (Dict.Count == 0 && xmlDict.GetCount () == 0) {
                if (tabName.Equals("Personal Info") || tabName.Equals("Office Visit")) {
                    xmlDict.AddPersistingData(tabName, customTabName, node.OuterXml);
                    /*XmlDocument tempXml = new XmlDocument ();
                    tempXml.LoadXml (node.OuterXml);
                    string gend = tempXml.GetElementsByTagName ("Gender").Item (0).InnerText;
                    transform.parent.GetComponent<CharacterManagerScript> ().setCharacter (gend);
                    GlobalData.gender = gend;*/
                } else {
                    xmlDict.AddData(tabName, customTabName, node.OuterXml, condKeys);
                }
                tabName = null;
                customTabName = null;
                while (node != null) { //Skip the data section
                    if (node.ParentNode.NextSibling != null) {
                        node = node.ParentNode.NextSibling;
                        break;
                    }
                    node = node.ParentNode;
                    if (node.ParentNode == null) {
                        node = null;
                    }
                }
            }
            if (node != null) {
                if (node.Name.Equals("sectionName")) { //Section's custom name
                    node = xmlDoc.AdvNode(node);
                    if (node == null || node.Value == null || node.Value.Equals("")) {
                        xmlDict.SetSectionDisplayName(sectionName);
                    } else {
                        xmlDict.SetSectionDisplayName(node.Value);
                    }
                } else {
                    if (node.ParentNode.Name.Equals("Sections") && !node.Name.EndsWith("Section")) {
                        node = node.NextSibling;
                    }
                    //node = AdvNode (node);
                }
            }
        }

        //If there were no sections in the file then loading is done. Return.
        if (sectionName == null) {
            return;
        }

        //Add/update the last section from the file with it's Tab data. (It did not get saved in the loop above)
        if (Dict.ContainsKey(sectionName)) {
            Dict[sectionName] = xmlDict;
        } else {
            Dict.Add(sectionName, xmlDict);
        }

        //Load in the section buttons
        GameObject parent = SectionButtonPar;

        //Remove all previously existing section buttons (Since we'll have new sections)
        foreach (Transform child in parent.transform) {
            if (child.name != "Filler" && child.name != "AddSectionButton") {
                GameObject.Destroy(child.gameObject);
            }
        }

        //Spawn the section buttons
        int i = 0;
        Transform iconHolder = GameObject.Find("GaudyBG").transform;
        iconHolder = iconHolder.Find("SectionCreatorBG/SectionCreatorPanel/Content/ScrollView/Viewport/Content");
        foreach (string key in Dict.Keys) {
            GameObject newSection = Resources.Load(GlobalData.resourcePath + "/Prefabs/SectionButton") as GameObject;
            TextMeshProUGUI[] children = newSection.GetComponentsInChildren<TextMeshProUGUI>(true);
            string buttonName = null;
            string imageKey = null;
            foreach (TextMeshProUGUI child in children) {
                if (child.name.Equals("SectionLinkToText")) { //Where the button links to
                    imageKey = key;
                    child.text = key;
                } else if (child.name.Equals("SectionDisplayTMP")) { //What the button displays
                    child.text = key.Replace('_', ' ').Substring(0, key.Length - "Section".Length);
                    buttonName = child.text.Replace(" ", "_") + "SectionButton";
                }
            }
            //Just in case
            newSection.transform.Find("SectionDisplayText").GetComponent<Text>().text = key.Replace('_', ' ').Substring(0, key.Length - "Section".Length);

            newSection = Instantiate(newSection, parent.transform);
            newSection.name = buttonName;
            newSection.transform.SetSiblingIndex(i);
            i++;

            if (imgDict.Count > 0) {
                Image[] images = newSection.GetComponentsInChildren<Image>();
                foreach (Image img in images) {
                    if (img.transform.name.Equals("Image")) {
                        img.sprite = null;
                        if (!imgDict.ContainsKey(imageKey)) { //Load default image if it's not found in the dictionary
                            img.sprite = iconHolder.transform.Find("IconPanel1/Icon").GetComponent<Image>().sprite;
                        } else {
                            if (imgDict[imageKey].referenceName != null && !imgDict[imageKey].referenceName.Equals("")) {
                                img.sprite = imgDict[imageKey].iconHolder.transform.Find(imgDict[imageKey].referenceName + "/Icon").GetComponent<Image>().sprite;
                            } else {
                                img.sprite = imgDict[imageKey].sprite;
                            }
                            if (imgDict[imageKey].useColor) {
                                newSection.GetComponent<Image>().color = imgDict[imageKey].color;
                                //img.color = imgDict [imageKey].color;
                            }
                        }
                    }
                }
            }

            Transform[] components = newSection.GetComponentsInChildren<Transform>();
            foreach (Transform c in components) {
                if (!c.name.Equals(newSection.name) && !c.name.Equals("SectionDisplayTMP")) {
                    c.gameObject.SetActive(false);
                }
            }
            newSection.transform.GetChild(0).gameObject.SetActive(true);
        }

        //This code spawns in an Office Visit tab for any case that doesn't have one.
        //Change line 617 to treat the Office visit like personal info
        foreach (string key in getKeys()) {
            foreach (string tabKey in Dict[key].GetTabList()) {
                if (Dict[key].GetTabInfo(tabKey).type.Equals("Personal Info")) { //Add an office visit to any 
                    if (!Dict[key].GetTabList().Contains("Office Visit")) {
                        Dict[key].AddPersistingData("Office Visit", "Office Visit", "<data></data>");
                        break;
                    } else {
                        //Office visit already exists
                        break;
                    }
                }
            }
        }

        parent.transform.Find("AddSectionButton").SetAsLastSibling();
        parent.transform.Find("Filler").SetAsLastSibling();

        Debug.Log(GetData());
    }

    /**
     * Clears the Vitals panel information
     *
    public void ClearVitalsPanel() {
        //Clear out the Vitals panel as well
        Transform[] children = GameObject.Find ("VitalsPanel").transform.GetComponentsInChildren<Transform> ();
        foreach (Transform child in children) {
            if (child.tag.Equals("Value")) {
                if (child.gameObject.GetComponent<InputField> () != null) {
                    child.gameObject.GetComponent<InputField> ().text = "";
                } else if (child.gameObject.GetComponent<Dropdown> () != null) {
                    child.gameObject.GetComponent<Dropdown> ().value = 0;
                    child.gameObject.GetComponent<Dropdown> ().captionText.text = child.gameObject.GetComponent<Dropdown> ().options [0].text;
                } else if (child.gameObject.GetComponent<Toggle> () != null) {
                    child.gameObject.GetComponent<Toggle> ().isOn = false;
                }
            }
        }
    }*/

    /**
     * 	Load in the default section buttons
     */
    public void SpawnDefaultSection()
    {

        GameObject parent = SectionButtonPar;

        int i = 0;
        DefaultDataScript dds = new DefaultDataScript();
        string key = dds.defaultSection;
        GameObject newSection = Resources.Load(GlobalData.resourcePath + "/Prefabs/SectionButton") as GameObject;
        TextMeshProUGUI[] children = newSection.GetComponentsInChildren<TextMeshProUGUI>(true);
        string buttonName = null;
        foreach (TextMeshProUGUI child in children) {
            if (child.name.Equals("SectionLinkToText")) { //Where the button links to
                child.text = key;
            } else if (child.name.Equals("SectionDisplayText") || child.name.Equals("SectionDisplayTMP")) { //What the button displays
                child.text = key.Replace('_', ' ').Substring(0, key.Length - "Section".Length);
                buttonName = child.text.Replace(" ", "_") + "SectionButton";
            }
        }

        newSection.transform.Find("SectionDisplayTMP").GetComponent<TMPro.TextMeshProUGUI>().text = key.Replace('_', ' ').Substring(0, key.Length - "Section".Length);

        AddImg(key, dds.defaultIcon);
        GetImage(key).color = dds.defaultColor;
        GetImage(key).useColor = true;
        newSection.transform/*.Find("Image")*/.GetComponent<Image>().color = dds.defaultColor;

        SectionDataScript sds = new SectionDataScript();
        sds.SetPosition(0);
        sds.AddPersistingData(dds.defaultTab, null);//.Replace(" ", "_") + "Tab", null); //Personal Info will always be saved
        sds.AddPersistingData("Office Visit", "<data><EntryData><Parent></Parent><Entry0><PanelType>OfficeVisitPanel</PanelType><PanelData></PanelData></Entry0></EntryData></data>"); //office visit may never be visited, so construct null data
        sds.SetCurrentTab(dds.defaultTab);
        Dict.Add(key, sds);

        newSection = Instantiate(newSection, parent.transform);
        newSection.name = buttonName;
        newSection.transform.SetSiblingIndex(i);
        Debug.Log(GetData());

        parent.transform.Find("Filler").SetAsLastSibling();
    }

    /**
     * Adds a new Section to the Dictionary
     */
    public void AddSection(string sectionName)
    {
        if (Dict.ContainsKey(sectionName)) {
            throw new System.Exception("Cannot add two duplicate sections!");
        } else {
            Dict.Add(sectionName, new SectionDataScript());
            Dict[sectionName].SetPosition(Dict.Count - 1);
        }
    }

    /**
     * Adds or updates data for a specified Tab
     * Tab = TabName
     */
    public void AddData(string Section, string Tab, string Data)
    {
        if (!Dict.ContainsKey(Section)) {
            SectionDataScript xmlDict = new SectionDataScript();
            xmlDict.Initiate();
            xmlDict.SetPosition(Dict.Count);
            xmlDict.AddData(Tab, Data);
            Dict.Add(Section, xmlDict);
        } else {
            //Debug.Log ("Section: " + Section + ", Tab: " + Tab + ", Data: " + Data);
            if (Dict[Section] == null) {
                Dict[Section] = new SectionDataScript();
                Dict[Section].SetPosition(Dict.Count - 1);
            }
            Dict[Section].AddData(Tab, Data);
        }
    }

    /**
     * Adds or updates data for a specified Tab with customName provided
     */
    public void AddData(string Section, string Tab, string customName, string Data)
    {
        if (!Dict.ContainsKey(Section)) {
            SectionDataScript xmlDict = new SectionDataScript();
            xmlDict.Initiate();
            xmlDict.SetPosition(Dict.Count);
            xmlDict.AddData(Tab, customName, Data);
            Dict.Add(Section, xmlDict);
        } else {
            //Debug.Log ("Section: " + Section + ", Tab: " + Tab + ", Data: " + Data);
            if (Dict[Section] == null) {
                Dict[Section] = new SectionDataScript();
                Dict[Section].SetPosition(Dict.Count - 1);
            }
            Dict[Section].AddData(Tab, customName, Data);
        }
    }

    /**
     * Returns the data of a specified Tab as a long string
     */
    public string GetData(string Section, string Tab)
    {
        if (Section == null || !Dict.ContainsKey(Section) || !Dict[Section].ContainsKey(Tab)) {
            return null;
        }
        return Dict[Section].GetData(Tab);
    }

    /**
     * Returns the data of all Sections in the Dictionary
     */
    public string GetData()
    {
        string longertext = "";
        if (nonSectionDataPanels == null) {
            nonSectionDataPanels = new string[] { "SaveCaseBG", "CharacterEditorPanel" };
        }
        foreach (string s in nonSectionDataPanels) {
            longertext += "<" + s + ">";
            Transform[] children;
            if (s == "CharacterEditorPanel") {
                children = characterPanel.GetComponentsInChildren<Transform>(true);
            } else if (transform.Find(s) != null) {
                children = transform.Find(s).GetComponentsInChildren<Transform>(true);
            } else {
                children = GameObject.Find(s).transform.GetComponentsInChildren<Transform>(true);
            }
            foreach (Transform child in children) {

                if (child != null) {
                    if ((child.name.ToLower().EndsWith("value") || child.tag.Equals("Value") || child.name.ToLower().EndsWith("toggle"))) { //&& child.gameObject.activeInHierarchy) {
                                                                                                                                            //Debug.Log("CHILD: "+child.name);
                        if (child.gameObject.GetComponent<Toggle>() != null) {
                            longertext += "<" + child.name + ">";
                            longertext += child.gameObject.GetComponent<Toggle>().isOn;
                            longertext += "</"
                                + child.name + ">";
                        } else if (child.name.ToLower().EndsWith("toggle")) {
                            continue;
                        } else {
                            longertext += "<" + child.name + ">";

                            string tempText = "";
                            //Handle reading the child depending on the input type
                            if (child.gameObject.GetComponent<InputField>() != null) {
                                tempText = child.gameObject.GetComponent<InputField>().text.Replace("<", "[");
                                tempText = tempText.Replace(">", "]");
                            } else if (child.gameObject.GetComponent<Slider>() != null) {
                                tempText += child.gameObject.GetComponent<Slider>().value;
                            } else if (child.gameObject.GetComponent<Dropdown>() != null) {
                                tempText = child.gameObject.GetComponent<Dropdown>().captionText.text;
                            } else if (child.gameObject.GetComponent<Text>() != null) {
                                tempText = child.gameObject.GetComponent<Text>().text;
                            } else if (child.gameObject.GetComponent<TMP_InputField>() != null) {
                                tempText = child.gameObject.GetComponent<TMP_InputField>().text;
                            } else if (child.gameObject.GetComponent<TMP_Dropdown>() != null) {
                                tempText = child.gameObject.GetComponent<TMP_Dropdown>().captionText.text;
                            } else if (child.gameObject.GetComponent<TextMeshProUGUI>() != null) {
                                tempText = child.gameObject.GetComponent<TextMeshProUGUI>().text;
                            }
                            longertext += UnityWebRequest.EscapeURL(tempText);

                            longertext += "</" + child.name + ">";
                        }
                    }
                }
            }
            longertext += "</" + s + ">";
        }
        longertext += "<Sections>";



        foreach (string key in Dict.Keys.OrderBy((string arg) => Dict[arg].GetPosition())) {
            //Debug.Log (key);
            string formattedKey = ConvertNameForXML(key);
            longertext += "<" + formattedKey + ">";
            longertext += "<sectionName>" + Dict[key].GetSectionDisplayName() + "</sectionName>";
            longertext += Dict[key].GetAllData();
            longertext += "</" + formattedKey + ">";
        }
        longertext += "</Sections>";
        return longertext;
    }

    /**
     * This method will convert special characters into an xml friendly format so they can be used
     */
    private string ConvertNameForXML(string name)
    {

        //return name; //Comment this line out to handle special characters. Must comment out ConvertNameFromXML's return too.
        string newName = "";
        foreach (char c in name) {
            string character = c + "";
            if (!Regex.IsMatch(character, "[a-zA-Z0-9_ ]")) {
                int a = Convert.ToInt32(c);
                if (a > 255) {
                    continue;
                }
                string hex = "." + string.Format("{0:X}", a) + ".";
                //name = name.Replace (character, hex);
                newName += hex;
            } else {
                newName += c;
            }
        }
        //Debug.Log (name);
        return "_" + newName;
    }

    /**
     * Convert's a section's formatted XML name into regular text
     */
    public string ConvertNameFromXML(string name)
    {

        //return name; //Comment this line out to handle special characters. Must comment out ConvertNameForXML's return too.

        if (name.StartsWith("_")) {
            name = name.Substring(1); //Remove starting underscore
        }
        for (int pos = 0; pos < name.Length; pos++) {
            string currentChar = name.ToCharArray()[pos] + "";
            if (currentChar.Equals(".")) {
                if (name.ToCharArray()[pos + 3].Equals('.')) {
                    string character = Char.ConvertFromUtf32(Convert.ToInt32(name.Substring(pos + 1, 2), 16));
                    name = name.Replace(name.Substring(pos, 4), character);
                } else {
                    name = name.Replace(name.Substring(pos, name.Substring(pos + 1).IndexOf('.') + 2), ""); //+2 to avoid first . and include second
                }
            }
        }
        return name;
    }

    /**
     * Returns the data of the specified Section
     */
    public SectionDataScript GetData(string Section)
    {
        if (Section == null || Section.Equals("")) {
            return new SectionDataScript();
        }
        if (Dict.ContainsKey(Section)) {
            return Dict[Section];
        } else {
            print("No section found with the given section name");
            return new SectionDataScript();
            /*Dict [Section] = new SectionDataScript ();
            Dict [Section].SetPosition (Dict.Count - 1);
            return Dict[Section];*/
        }
    }

    /**
     * Returns the specified Section's custom Display name
     */
    public string getSectionCustomName(string sectionName)
    {
        return Dict[sectionName].GetSectionDisplayName();
    }

    /**
     * Updates the specified Section's custom Display name
     */
    public void setSectionCustomName(string sectionName, string customName)
    {
        Dict[sectionName].SetSectionDisplayName(customName);
    }

    /**
     * Returns the list of keys in the Dictionary. This will give section LinkTo names
     */
    public string[] getKeys()
    {
        if (Dict != null)
            return Dict.Keys.ToArray();
        return null;
    }

    /**
     * Edits a tab's display name
     */
    public void EditTab(string oldName, string newName)
    {
        string Section = transform.GetComponent<TabManager>().getCurrentSection();
        Debug.Log("SWAPPING TEXT NAME. OLDNAME: " + oldName + ", NEWNAME: " + newName);
        if (Dict[Section].ContainsKey(oldName)) {
            //Debug.Log ("MATCH FOUND");
            Dict[Section].Replace(oldName, newName);
            //Dict [Section].GetTabList (); //For debugging

            TabManager tm = GetComponent<TabManager>();
            tm.setTabName(newName);

            //Debug.Log ("ImgDict Keys: " + string.Join (",", imgDict.Keys.ToArray ()));
            foreach (string imgName in imgDict.Keys.ToArray()) {
                string[] splitKey = imgName.Split('.');
                if (splitKey.Count() >= 3) {
                    if (splitKey[0].Equals(tm.getCurrentSection())) {
                        if (splitKey[1].Equals(oldName + "Tab")) {
                            string newImgName = imgName.Replace(splitKey[1], newName + "Tab");//tm.getCurrentSection () + "." + newName + "." + splitKey [2];
                            imgDict.Add(newImgName, imgDict[imgName]);
                            imgDict.Remove(imgName);
                        }
                    }
                }
            }

            Dictionary<string, string> tempDict = dialogueDict;
            foreach (string key in tempDict.Keys.ToList()) {
                //Debug.Log ("Key     : " + key);
                //Debug.Log ("Matching: " + oldName + "Tab");
                if (key.StartsWith(tm.getCurrentSection() + "/" + oldName + "Tab")) {
                    string newDialogue = dialogueDict[key].Replace(oldName + "Tab", newName + "Tab");
                    dialogueDict.Remove(key);
                    dialogueDict.Add(key.Replace(oldName + "Tab", newName + "Tab"), newDialogue);
                }
            }

            tempDict = quizDict;
            foreach (string key in tempDict.Keys.ToList()) {
                if (key.StartsWith(tm.getCurrentSection() + "/" + oldName + "Tab")) {
                    string newQuiz = quizDict[key].Replace(oldName + "Tab", newName + "Tab");
                    quizDict.Remove(key);
                    quizDict.Add(key.Replace(oldName + "Tab", newName + "Tab"), newQuiz);
                }
            }
        }
    }

    /**
     * Updates a section's conditions
     */
    public void UpdateSectionConds(string name, List<string> conditions)
    {
        if (Dict.ContainsKey(name)) {
            Dict[name].Conditions = conditions;
        }
    }
    /**
     * Edits a section's display name
     */
    public void EditSection(string oldName, string newName)
    {
        if (Dict.ContainsKey(oldName)) {
            //Update newName to fit the conventions of section names and update the Dict reference.
            string linkToName = newName.Replace(" ", "_") + "Section";
            if (oldName == linkToName || (!Dict.ContainsKey(newName) && !Dict.ContainsKey(linkToName))) {
                SectionDataScript temp = Dict[oldName];
                Dict.Remove(oldName);

                Dict.Add(linkToName, temp);
                Dict[linkToName].SetSectionDisplayName(newName);
                transform.GetComponent<TabManager>().setCurrentSection(linkToName);

                Debug.Log(oldName);
                string data;
                if ((data = Dict[linkToName].GetCurrentTab().data) != null && !data.Equals("")) {
                    Dict[linkToName].GetCurrentTab().data = data.Replace(oldName, linkToName);

                    foreach (string k in temp.GetTabList()) {
                        //Debug.Log (Dict [linkToName].GetData (k));
                        Dict[linkToName].AddData(k, temp.GetData(k).Replace(oldName, linkToName));
                        //Debug.Log (Dict [linkToName].GetData (k));
                    }
                }

                /*Transform currentTab = transform.Find ("ContentPanel/TabContentPanel").GetChild (0);
                HistoryFieldManagerScript[] listToUpdate = currentTab.GetComponentsInChildren<HistoryFieldManagerScript> ();
                foreach (HistoryFieldManagerScript HFM in listToUpdate) {
                    Debug.Log (HFM.RefreshUniquePath ());
                }*/



                Dictionary<string, string> tempDict = new Dictionary<string, string>();
                tempDict = dialogueDict;
                foreach (string key in tempDict.Keys.ToList()) {
                    if (key.StartsWith(oldName)) {
                        string newDialogue = dialogueDict[key].Replace(oldName, linkToName);
                        dialogueDict.Remove(key);
                        dialogueDict.Add(key.Replace(oldName, linkToName), newDialogue);
                    }
                }

                tempDict = quizDict;
                foreach (string key in tempDict.Keys.ToList()) {
                    if (key.StartsWith(oldName)) {
                        string newQuiz = quizDict[key].Replace(oldName, linkToName);
                        quizDict.Remove(key);
                        quizDict.Add(key.Replace(oldName, linkToName), newQuiz);
                    }
                }

                tempDict = flagDict;
                foreach (string key in tempDict.Keys.ToList()) {
                    if (key.StartsWith(oldName)) {
                        string newFlag = flagDict[key].Replace(oldName, linkToName);
                        flagDict.Remove(key);
                        flagDict.Add(key.Replace(oldName, linkToName), newFlag);
                    }
                }
                /*
                    tempDict = eventDict;
                    foreach (string key in tempDict.Keys.ToList()) {
                        if (key.StartsWith (oldName)) {
                            string newEvent = eventDict [key].Replace (oldName, linkToName);
                            eventDict.Remove (key);
                            eventDict.Add (key.Replace (oldName, linkToName), newEvent);
                        }
                    }
                */
            } else {
                ShowMessage("Cannot have two steps with matching names!", true);
                throw new Exception("Cannot have two steps with matching names!");
            }
        } else {
            ShowMessage("Current step does not exist!", true);
            throw new Exception("Current step does not exist!");
        }
    }

    /**
     * Removes a tab from a section
     */
    public void RemoveTab(string Tab)
    {
        Dict[transform.GetComponent<TabManager>().getCurrentSection()].Remove(Tab);
    }

    /**
     * Removes a section from the Dictionary
     */
    public void RemoveSection(string Section)
    {
        Dict.Remove(Section);
    }

    /**
     * Clears all data
     */
    public void ClearAllData()
    {
        Dict.Clear();
        imgDict.Clear();
        Dict = null;
        dialogueDict.Clear();
        quizDict.Clear();
        flagDict.Clear();
    }

    /**
     * Method to check if the provided name has any special characters that XML wouldn't like
     */
    public bool IsValidName(string name, string field)
    {
        if (name.ToLower().StartsWith("xml")) {
            ShowMessage($"{field} name not valid. Cannot start with 'xml'.", true);
            return false;
        } else if (Regex.IsMatch(name, "(//)+|[*<>&]")) {
            ShowMessage($"{field} name not valid. Cannot use:\n*, &, <, >, or //", true);
            return false;
        }

        return true; //Comment this line out to prevent special characters from being used.

        /*if (!Regex.IsMatch (name, "^[a-zA-Z_]")) {
            Debug.Log ("Name must start with letters or an underscore");
            return false;
        } else if (!Regex.IsMatch (name, "^([a-zA-Z0-9-_. ])*$")) {
            Debug.Log ("Name contains special characters");
            return false;
        }
        return true;*/
    }


    private GameObject notification;
    private bool fade;
    public void ShowMessage(string message)
    {
        ShowMessage(message, false);
    }

    /**
	 * Use this to show a confirmation that the case was saved successfully
	 */
    public void ShowMessage(string message, bool error)
    {
        if (!error) {
            if (transform.Find("NotificationPanel") == null) {
                Destroy(notification);
                notification = Instantiate(Resources.Load(GlobalData.resourcePath + "/Prefabs/NotificationMessage") as GameObject, transform);
                notification.name = "NotificationPanel";
            }
        } else if (transform.Find("ErrorPanel") == null) {
            Destroy(notification);
            notification = Instantiate(Resources.Load(GlobalData.resourcePath + "/Prefabs/ErrorMessage") as GameObject, transform);
            notification.name = "ErrorPanel";
        }
        CancelInvoke("Fade");
        fade = false;
        notification.GetComponent<CanvasGroup>().alpha = 1;
        notification.SetActive(true);
        TextMeshProUGUI messageText = notification.transform.Find("BG/Message").GetComponent<TextMeshProUGUI>();
        messageText.text = message;
        Invoke("Fade", 5f);
        NextFrame.Function(delegate { fade = true; });

    }

    /**
	 * Used temporarily as a delay for the save confirmation message
	 */
    private void Fade()
    {
        if (notification.GetComponent<CanvasGroup>().alpha > 0) {
            if (!fade) {
                return;
            }
            //t.color = new Color (t.color.r, t.color.g, t.color.b, t.color.a - Time.deltaTime / 6f);
            notification.GetComponent<CanvasGroup>().alpha = (notification.GetComponent<CanvasGroup>().alpha - Time.deltaTime / 3f);
            NextFrame.Function(Fade);
            return;
        }
        Destroy(notification);
    }

    IEnumerator LoadFilesAsync()
    {
        AssetBundleCreateRequest bundleRequest = AssetBundle.LoadFromFileAsync(GlobalData.filePath + GlobalData.fileName);
        yield return bundleRequest;

        AssetBundle bundle = bundleRequest.assetBundle;
        if (bundle == null) {
            Debug.Log("Failed to load!");
            yield break;
        }

        AssetBundleRequest obj = bundle.LoadAssetAsync<string>("Yes");
        yield return obj;

        yield break;
    }

    //Used for testing
    public void buttonprintdata()
    {
        Debug.Log(GetData());
        Debug.Log(GetImagesXML());
        Debug.Log(GetData(transform.GetComponent<TabManager>().getCurrentSection()).GetAllPositions());
    }

    public void Reload()
    {
        ClearAllData();
        Awake();
        StartCoroutine(Start());
    }
}