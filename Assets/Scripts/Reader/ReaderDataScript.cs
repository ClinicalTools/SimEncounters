using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
#if UNITY_EDITOR
#endif


/**
 * This script manages Data. It acts as the Intermediary data container.
 * Data is loaded from the chosen XML file into here.
 * Data is loaded from here into Tabs as needed.
 * Data is updated here and then sent to the XML file from here as well.
 */

public class ReaderDataScript : MonoBehaviour
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
    [SerializeField]
    private bool readerOnlyBuild = false;
    public UploadToServer ServerUploader;
    public CanvasGroup loadingScreen;
    public bool disableDownload = false;

    public ReaderCaseOverviewScript caseOverviewScript;

    public bool forceInOrder;

    void Awake()
    {
        if (Dict == null) {
            imgDict = new Dictionary<string, SpriteHolderScript>();
            Dict = new Dictionary<string, SectionDataScript>();
            dialogueDict = new Dictionary<string, string>();
            flagDict = new Dictionary<string, string>();
            eventDict = new Dictionary<string, string>();
            quizDict = new Dictionary<string, string>();
            newTabs = new List<Transform>();
        }
        if (GlobalData.showLoading) {
            //SceneManager.UnloadSceneAsync(0);
            //loadingScreen = SceneManager.GetSceneByName("LoadingScreen").GetRootGameObjects()[2].transform.Find("LoadingScreenNew").GetComponent<CanvasGroup>();
            loadingScreen = GameObject.Find("LoadingScreenNew").GetComponent<CanvasGroup>();
            if (loadingScreen == null) {
                loadingScreen = LoadingScreenManager.Instance.GetComponent<CanvasGroup>();
            }
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
    }

    // Use this for initialization
    IEnumerator Start()
    {
        //! READER: potentially get demo case
        if (fileName.Equals("") && readerOnlyBuild && GlobalData.caseObj == null) {
            GetDemoCase();
        }

        path = GlobalData.filePath;
        Debug.Log(path);
        fileName = GlobalData.fileName;
        Debug.Log(fileName);

        //PrintDebugMessages();
        /*
		XmlDocument xmlDoc = new XmlDocument();
		WWW www = new WWW(Application.streamingAssetsPath + "/DemoCases/Chad_Wright.ced");
		yield return www;
		print(www.text);
		try {
			xmlDoc.LoadXml(www.text);
		} catch (Exception e) {
			string text = "";
			AesManaged aes = new AesManaged();
			aes.Padding = PaddingMode.None;
			ICryptoTransform decrypt = aes.CreateDecryptor(Encoding.UTF8.GetBytes(GlobalData.encryptionKey), Encoding.UTF8.GetBytes(GlobalData.encryptionIV));
			using (MemoryStream ms = new MemoryStream(www.bytes)) {
				using (CryptoStream cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read)) {
					using (StreamReader sr = new StreamReader(cs)) {
						text = sr.ReadToEnd();
						print(text);
					}
				}
			}
			xmlDoc.LoadXml(text);
		}
		*/

        //! READER: check absolute url
        if (Application.absoluteURL.Contains("?patient=")) {
            string patient = Application.absoluteURL.Substring(Application.absoluteURL.IndexOf("?patient="));
            patient = patient.Remove(0, "?patient=".Length);
            fileName = patient + ".ced";
            GlobalData.fileName = patient + ".ced";
            GlobalData.caseObj.patientName = patient.Replace("_", " ");
            print(patient);
        }

        transform.gameObject.AddComponent<UploadToServer>();
        ServerUploader = transform.GetComponent<UploadToServer>();
        serverCaseData = "";
        serverImageData = "";

        //! READER: download data if not disabled
        if (!disableDownload) {//Application.platform != RuntimePlatform.WebGLPlayer) {//&& Application.platform != RuntimePlatform.Android) { 
                               //if (fileName.Length > 5 && !GlobalData.loadLocal && !System.IO.File.Exists (path + fileName.Remove (fileName.Length - 3) + "ced")) {
            if (!GlobalData.loadLocal || !File.Exists(path + fileName)) {
                yield return StartCoroutine(ServerUploader.DownloadFromServer("xmlData"));
                serverCaseData = ServerUploader.GetDownloadedText();
            }
            if (!GlobalData.loadLocal || !File.Exists(path + fileName)) {
                yield return StartCoroutine(ServerUploader.DownloadFromServer("imgData"));
                serverImageData = ServerUploader.GetDownloadedText();
            }
        }
        correctlyOrderedDialogues = new Dictionary<string, string>();
        correctlyOrderedQuizes = new Dictionary<string, string>();
        correctlyOrderedFlags = new Dictionary<string, string>();
        /*if (fileName.Length > 5) {
			LoadImages ();
			PopulateDict ();
		}
		transform.GetComponent<ReaderTabManager> ().FirstTimeLoad ();
		*/

        yield return StartCoroutine(LoadCase());

        //! READER: set patient information
        GlobalData.firstName = "";
        GlobalData.lastName = "";
        if (GlobalData.caseObj != null) {
            string[] nameSplit = GlobalData.caseObj.patientName.Split('_');
            GlobalData.firstName = nameSplit[0];
            if (nameSplit.Length > 1) {
                GlobalData.lastName = nameSplit[1];
            }

            var patientInformation = transform.Find("ContentPanel /Section/PatientInformation");
            if (patientInformation != null) {
                patientInformation.Find("PatientName").GetComponent<TextMeshProUGUI>().text = GlobalData.caseObj.patientName.Replace("_", " ");
                if (Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.Android) {
                    patientInformation.Find("RecordNumber").gameObject.SetActive(false);
                } else {
                    if (!GlobalData.fileName.StartsWith("[CHECKFORDUPLICATE]")) {
                        patientInformation.Find("RecordNumber").GetComponent<TextMeshProUGUI>().text = "Record Number: #" + GlobalData.caseObj.recordNumber;
                    } else {
                        patientInformation.Find("RecordNumber").GetComponent<TextMeshProUGUI>().text = "Record Number: ######";
                    }
                }
            }
        }


        if (loadingScreen == null) {
            loadingScreen = LoadingScreenManager.Instance.GetComponent<CanvasGroup>();
        }
        yield return null;
        if (loadingScreen != null && GlobalData.showLoading) {
            loadingScreen.blocksRaycasts = false;
            LoadingScreenManager.Instance.Fade();
        }

        //! READER: Refresh case overview
        if (caseOverviewScript != null) {
            caseOverviewScript.RefreshOverview();
        }

        yield return null;
    }

    // READER ONLY METHOD
    public void DisplayMessageFromJavascript(string message)
    {
        Debug.Log("I'm printing because I was called from javascript!");
        Debug.Log(message);
    }

    private void PrintDebugMessages()
    {
        Debug.Log(Application.absoluteURL);
    }

    //! READER ONLY METHOD
    private IEnumerator LoadCase()
    {
        if (fileName.Length > 5) {
            yield return StartCoroutine(LoadImages());
            yield return StartCoroutine(PopulateDict());
        }
        transform.GetComponent<ReaderTabManager>().FirstTimeLoad();
        yield break;
    }

    //! ???
    public void CallStart()
    {
        ClearAllData();
        Awake();
        StartCoroutine(Start());
    }

    /**
	 * Manually entering in the variables needed to load the chad case for the reader
	 */
     //! READER ONLY METHOD
    public void GetDemoCase()
    {
        disableDownload = true;
        GlobalData.fileName = "Chad_Wright.ced";
        GlobalData.filePath = Application.streamingAssetsPath + "/DemoCases/";
        Debug.Log("[Open File] Selected file: " + GlobalData.filePath + GlobalData.fileName);

        GlobalData.loadLocal = true;
    }

    /**
	 * Loads images from the images XML file
	 */
     //! LOADIMAGES
    public IEnumerator LoadImages()
    {
        string imageFileName = fileName.Remove(fileName.Length - 3) + "cei";
        if (imgDict == null) {
            GlobalData.showLoading = false;
            Awake();
            GlobalData.showLoading = true;
        }

        xmlDoc = new XmlDocument();
        print(Application.platform.ToString());
        print(Application.platform == RuntimePlatform.WebGLPlayer);
        if (!disableDownload) {//if (Application.platform != RuntimePlatform.WebGLPlayer && Application.platform != RuntimePlatform.Android) {
            if (serverImageData.Equals("") && File.Exists(path + imageFileName)) {
                StreamReader read = new StreamReader(path + imageFileName);
                string s = read.ReadToEnd();
                if (s == null || s.Equals("")) {
                    Debug.Log("No images to load!");
                    read.Close();
                    //Maybe ask if they want to check the server here?
                    //return;
                    yield break;
                }
                try {
                    xmlDoc.LoadXml(s); //this loads the local file
                    read.Close();
                } catch (Exception) {
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
                    //return;
                    yield break;
                }
                xmlDoc.LoadXml(serverImageData); //this loads from the server

                //Debug.Log("File does not exist. Cannot load Images!");
                //return;
            }
        } else { //Load for WebGL
			using (UnityWebRequest webRequest = UnityWebRequest.Get(GlobalData.filePath + imageFileName)) {
				yield return webRequest.SendWebRequest();
				try {
					xmlDoc.LoadXml(webRequest.downloadHandler.text);
				} catch (Exception e) {
					print(e.Message);
					print(webRequest.downloadHandler.text);
					string text = "";
					AesManaged aes = new AesManaged();
					ICryptoTransform decrypt = aes.CreateDecryptor(Encoding.UTF8.GetBytes(GlobalData.encryptionKey), Encoding.UTF8.GetBytes(GlobalData.encryptionIV));
					using (MemoryStream ms = new MemoryStream(webRequest.downloadHandler.data)) {
						using (CryptoStream cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read)) {
							using (StreamReader sr = new StreamReader(cs)) {
								text = sr.ReadToEnd();
								print(text);
							}
						}
					}
					xmlDoc.LoadXml(text);
				}
			}
        }
        //imgDict = new Dictionary<string, Sprite> ();

        XmlNode node = xmlDoc.FirstChild;
        while (node != null) {
            if (node.Name.ToLower().StartsWith("image")) {
                while (!node.Name.Equals("key")) {
                    node = xmlDoc.AdvNode(node);
                }
                string key = node.InnerText;
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
                        if (!newColor) {
                            c = new Color(20 / 255.0f, 178 / 255.0f, 163 / 255.0f, 1.0f);
                            newColor = true;
                        }
                    }
                } else {
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
        //! READER: No character image ????
        } else {
            Debug.Log("No character image");
            GameObject.Find("CharacterCamera").transform.Find("Canvas").gameObject.SetActive(false);
        }

        yield return null;
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
        GlobalData.fileName = fileName;
    }

    public void SetFileName(string filename)
    {
        if (!filename.EndsWith(".ced")) {
            filename += ".ced";
        }
        fileName = filename;
        GlobalData.fileName = fileName;
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
        StartCoroutine(Start());
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
        data += "</Body>";
        return data;
    }

    public SpriteHolderScript GetImage(string uid)
    {
        if (imgDict.ContainsKey(uid)) {
            return imgDict[uid];
        } else {
            SpriteHolderScript spr = new SpriteHolderScript("null") { color = GlobalData.GDS.defaultGreen };
            return spr;
            //return null;
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

    /**
	 * Populates the Dictionary from the XML file
	 * Used to load in new data or data at startup
	 */
    public IEnumerator PopulateDict()
    {
        if (fileName == null) {
            yield break;
        }
        //Finds the file and loads data into xmlDoc
        xmlDoc = new XmlDocument();
        if (!disableDownload) {//if (Application.platform != RuntimePlatform.WebGLPlayer && Application.platform != RuntimePlatform.Android) {
            if (serverCaseData.Equals("") && System.IO.File.Exists(path + fileName)) {
                if (overwrite) {
                    SpawnDefaultSection();
                    //ClearVitalsPanel ();
                    yield break;
                }
                StreamReader read = new StreamReader(path + fileName);
                string s = read.ReadToEnd();
                if (s == null || s.Equals("")) {
                    Debug.Log("No data to load locally!");
                    read.Close();
                    //Maybe ask if they want to check the server here?
                    yield break;
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
            } else {
                if (serverCaseData.Equals("") || serverCaseData == null) {
                    Debug.Log("No case data to load from server");
                    SpawnDefaultSection();
                    yield break;
                }

                xmlDoc.LoadXml(serverCaseData); //This loads from the server
            }
        } else { //Load for WebGL
			using (UnityWebRequest webRequest = UnityWebRequest.Get(GlobalData.filePath + GlobalData.fileName)) {
				yield return webRequest.SendWebRequest();

				try {
					xmlDoc.LoadXml(webRequest.downloadHandler.text);
				} catch (Exception) {
					string text = "";
					AesManaged aes = new AesManaged();
					ICryptoTransform decrypt = aes.CreateDecryptor(Encoding.UTF8.GetBytes(GlobalData.encryptionKey), Encoding.UTF8.GetBytes(GlobalData.encryptionIV));
					using (MemoryStream ms = new MemoryStream(webRequest.downloadHandler.data)) {
						using (CryptoStream cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read)) {
							using (StreamReader sr = new StreamReader(cs)) {
								text = sr.ReadToEnd();
							}
						}
					}
					xmlDoc.LoadXml(text);
				}
			}
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
                if (characterPanel == null) {
                    node = node.NextSibling;
                    print("No editor panel. Continuing");
                    continue;
                }
                element.LoadXml(node.OuterXml);
                children = characterPanel.GetComponentsInChildren<Transform>(true);

                XmlDocument tempXml = new XmlDocument();
                print(xmlDoc.GetElementsByTagName("Personal_InfoTab")[0].InnerXml);
                tempXml.LoadXml("<rootElement>" + xmlDoc.GetElementsByTagName("Personal_InfoTab")[0].InnerXml + "</rootElement>");
                string gend = tempXml.GetElementsByTagName("Gender").Item(0).InnerText;
                if (transform.parent.GetComponent<CharacterManagerScript>()) {
                    //NextFrame.Function(delegate { transform.parent.GetComponent<CharacterManagerScript>().setCharacter(gend); });
                }
                GlobalData.gender = gend;
            } else if (transform.Find(node.Name) != null) {
                element.LoadXml(node.OuterXml);
                children = transform.Find(node.Name).GetComponentsInChildren<Transform>(true);
            } else if (node.Name.Equals("Sections")) {
                inSections = true;
                continue;
            } else if (node.Name.Equals("SaveCaseBG")) {
                if (GlobalData.caseObj == null) {
                    node = node.FirstChild;
                    string patientName = null, description = null, summary = null, tagsStr = null, audience = null, difficulty = null;

                    while (true) {
                        if (node.Name == "PatientNameValue")
                            patientName = node.InnerText.Replace('+', ' ');
                        else if (node.Name == "DescriptionValue")
                            description = node.InnerText;
                        else if (node.Name == "SummaryValue")
                            summary = node.InnerText;
                        else if (node.Name == "TagsValue")
                            tagsStr = node.InnerText;
                        else if (node.Name == "TargetAudienceValue")
                            audience = node.InnerText;
                        else if (node.Name == "DifficultyValue")
                            difficulty = node.InnerText;

                        if (node.NextSibling == null) break;
                        node = node.NextSibling;
                    }

                    string[] tagsArr = tagsStr.Split(';');
                    List<string> tagsList = new List<string>();
                    foreach (var tag in tagsArr) {
                        var t = tag.Trim();
                        if (t != string.Empty)
                            tagsList.Add(t);
                    }

                    GlobalData.caseObj = new MenuCase(GlobalData.fileName) {
                        patientName = patientName,
                        description = description,
                        recordNumber = "######",
                        summary = summary,
                        tags = tagsList.ToArray(),
                        audience = audience,
                        difficulty = difficulty
                    };

                    if (caseOverviewScript != null)
                        caseOverviewScript.RefreshOverview();

                    node = node.ParentNode;
                }
                node = node.NextSibling;
                continue;
            } else {
                print(node.ParentNode.OuterXml);
                print("ERROR. NO PANEL FOUND. RETURNING");
                node = node.NextSibling;
                continue;
                //yield break;
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
                } else if (child.gameObject.GetComponent<Dropdown>() != null) {
                    int indexValue = 0;
                    foreach (Dropdown.OptionData myOptionData in child.gameObject.GetComponent<Dropdown>().options) {
                        if (myOptionData.text.Equals(UnityWebRequest.UnEscapeURL(xmlValue))) {
                            break;
                        }
                        indexValue++;
                    }
                    child.gameObject.GetComponent<Dropdown>().value = indexValue;
                } else if (child.gameObject.GetComponent<Toggle>() != null && xmlValue != null && !xmlValue.Equals("")) {
                    child.gameObject.GetComponent<Toggle>().isOn = bool.Parse(xmlValue);
                } else if (child.gameObject.GetComponent<Text>() != null) {
                    child.gameObject.GetComponent<Text>().text = UnityWebRequest.UnEscapeURL(xmlValue);
                } else if (child.name.Equals("Image") && child.GetComponent<ReaderOpenImageUploadPanelScript>()) {
                    Debug.Log("LOADING IMAGE: " + xmlValue);
                    child.GetComponent<ReaderOpenImageUploadPanelScript>().SetGuid(xmlValue);

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
                    child.gameObject.GetComponent<Slider>().value = float.Parse(xmlValue);
                }
            }

            while (node != null || endCurrentXMLSection) {
                if (node.PreviousSibling != null && node.PreviousSibling.Name.Equals(children[0].name)) {// node.Name.ToLower ().EndsWith ("section")) {
                    endCurrentXMLSection = true;
                    if (node.Name.Equals("Sections") && !inSections) {
                        inSections = true;
                    }
                    print(node.Name + ", " + node.Value + ", " + node.OuterXml);
                    break;
                }
                node = xmlDoc.AdvNode(node);
            }
        }

        node = xmlDoc.AdvNode(node); //Go inside sections
        if (node == null) {
            print("Null node");
        }
        print("Entering sections...");
        Tracker.CaseData caseTrackingData = Tracker.GetCaseData(GlobalData.caseObj.recordNumber);
        //string[] trackingDataSplit = caseTrackingData.caseProgress.Split(new string[] { "-" }, StringSplitOptions.None);
        while (node != null) {
            if (node.Name.ToLower().EndsWith("section")) {
                if (sectionName != null) {
                    if (Dict.ContainsKey(sectionName)) {
                        Dict[sectionName] = xmlDict;
                    } else {
                        Dict.Add(sectionName, xmlDict);
                    }
                    //Dict.Count may not be the way to go, but will test it
                    //if ^p:#-, # >= xmlDict.GetPosition(), visit all tabs
                    //Check if there's a number and compare it
                    if (caseTrackingData.caseFinished) {
                        Dict[sectionName].VisitAllTabs();
                    } else {
                        if (caseTrackingData.IsSectionRead(xmlDict.GetPosition(), xmlDict.GetCount()))
                            Dict[sectionName].VisitAllTabs();
                    }
                }

                xmlDict = new SectionDataScript();
                xmlDict.Initiate();
                xmlDict.SetPosition(Dict.Count);
                sectionName = ConvertNameFromXML(node.Name);
                while (node != null && !node.Name.ToLower().EndsWith("tab")) {
                    node = xmlDoc.AdvNode(node);
                }
            }
            if (node?.Name?.ToLower().EndsWith("tab") == true && tabName == null) {
                tabName = node.Name.Replace("_", " ").Substring(0, node.Name.Length - 3); //Unformat tabType
                XmlNode tempNode = xmlDoc.AdvNode(node);
                if (tempNode.Name.Equals("customTabName")) {
                    tempNode = xmlDoc.AdvNode(tempNode);
                    customTabName = tempNode.Value;

                } else {
                    customTabName = tabName;
                }
                while (node != null && !node.Name.ToLower().Equals("data")) {
                    node = xmlDoc.AdvNode(node);
                }
            }
            if (tabName != null && sectionName != null && node.Name.Equals("data")) {
                //if (Dict.Count == 0 && xmlDict.GetCount () == 0) {
                if (tabName.Equals("Personal Info")) {
                    xmlDict.AddPersistingData(tabName, customTabName, node.OuterXml);
                    /*XmlDocument tempXml = new XmlDocument ();
                    tempXml.LoadXml (node.OuterXml);
                    string gend = tempXml.GetElementsByTagName ("Gender").Item (0).InnerText;
                    transform.parent.GetComponent<CharacterManagerScript> ().setCharacter (gend);
                    GlobalData.gender = gend;*/
                } else {
                    xmlDict.AddData(tabName, customTabName, node.OuterXml);
                }

                /*if (caseTrackingData.caseFinished) {
                    xmlDict.GetTabInfo(customTabName).Visit();
                }*/

                //See if the tab has been visited
                //We subtract 1 because the listed number is the index of the last completed section
                //We're looking for the in-progress section which falls after that
                if (caseTrackingData.IsTabRead(xmlDict.GetPosition(), xmlDict.GetTabInfo(customTabName).n)) {
                    xmlDict.GetTabInfo(customTabName).Visit();
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
            yield break;
        }

        //Add/update the last section from the file with it's Tab data. (It did not get saved in the loop above)
        if (Dict.ContainsKey(sectionName)) {
            Dict[sectionName] = xmlDict;
        } else {
            Dict.Add(sectionName, xmlDict);
        }

        //Check last section against progress
        if (caseTrackingData.caseFinished) {
            Dict[sectionName].VisitAllTabs();
            if (caseTrackingData.IsSectionRead(xmlDict.GetPosition(), xmlDict.GetCount()))
                Dict[sectionName].VisitAllTabs();
        }

        if (SectionButtonPar != null) {
            //Load in the section buttons
            GameObject parent = SectionButtonPar;

            //Remove all previously existing section buttons (Since we'll have new sections)
            foreach (Transform child in parent.transform) {
                if (child.name.EndsWith("SectionButton")) {
                    Destroy(child.gameObject);
                }
            }

            //Spawn the section buttons
            int i = 0;
            Transform iconHolder = GameObject.Find("GaudyBG").transform;
            iconHolder = iconHolder.Find("SectionImages");
            GlobalData.resourcePath = "Reader";
            foreach (string key in Dict.Keys) {
                GameObject newSection = Resources.Load(GlobalData.resourcePath + "/Prefabs/SectionButton") as GameObject;
                TextMeshProUGUI[] children = newSection.GetComponentsInChildren<TextMeshProUGUI>(true);
                string buttonName = null;
                string imageKey = null;
                foreach (TextMeshProUGUI child in children) {
                    if (child.name.Equals("SectionLinkToText")) { //Where the button links to
                        imageKey = key;
                        child.text = key;
                    } else if (child.name.Equals("SectionDisplayText")) { //What the button displays
                        child.text = key.Replace('_', ' ').Substring(0, key.Length - "Section".Length);
                        buttonName = child.text.Replace(" ", "_") + "SectionButton";
                    }
                }

                newSection = Instantiate(newSection, parent.transform);
                newSection.name = buttonName;
                newSection.transform.SetSiblingIndex(i);
                newSection.GetComponent<Image>().color = imgDict.ContainsKey(key) ? imgDict[key].color : GlobalData.GDS.defaultGreen;// new Color(20.0f / 255.0f, 178.0f / 255.0f, 163.0f / 255.0f, 1);
                i++;

                //Edit this later
                //continue;

                if (imgDict.Count > 0) {
                    Image[] images = newSection.GetComponentsInChildren<Image>(true);
                    foreach (Image img in images) {
                        if (img.transform.name.Equals("StepIcon")) {
                            img.sprite = null;
                            if (!imgDict.ContainsKey(imageKey)) { //Load default image if it's not found in the dictionary
                                img.sprite = iconHolder.transform.Find("IconPanel1/Icon").GetComponent<Image>().sprite;
                            } else {
                                if (imgDict[imageKey].referenceName != null && !imgDict[imageKey].referenceName.Equals("")) {
                                    img.sprite = iconHolder.transform.Find(imgDict[imageKey].referenceName + "/Icon").GetComponent<Image>().sprite;
                                } else {
                                    img.sprite = imgDict[imageKey].sprite;
                                }
                                if (imgDict[imageKey].useColor) {
                                    //img.color = imgDict [imageKey].color;
                                    //img.gameObject.AddComponent<Outline>();
                                    img.color = new Color(1f, 1f, 1f, 1f);
                                }
                            }
                        }
                    }
                }

                if (Dict[key].AllTabsVisited()) {
                    newSection.transform.Find("AllTabsVisitedCheck").gameObject.SetActive(true);
                }
                if (forceInOrder) {
                    /*if (Tracker.GetCaseData(GlobalData.caseObj.recordNumber).caseFinished) {
					    newSection.transform.Find("AllTabsVisitedCheck").gameObject.SetActive(true);
				    } else if (Tracker.GetCaseData(GlobalData.caseObj.recordNumber).caseProgress.StartsWith("p:"+ (newSection.transform.GetSiblingIndex() - 1) + "-")) {
					    newSection.transform.Find("AllTabsVisitedCheck").gameObject.SetActive(true);
				    }*/
                    if (!Dict[key].AllTabsVisited()) {
                        if (caseTrackingData.IsSectionRead(xmlDict.GetPosition(), xmlDict.GetCount())) {
                            if (Dict[key].GetPosition() != 0) { //Don't darken the first step
                                                                //newSection.transform.Find("Overlay").GetComponent<Image>().color = new Color(0, 0, 0, (float)175 / 255);
                                newSection.transform.Find("Lock").gameObject.SetActive(true);
                                newSection.transform.Find("Overlay").GetComponent<HideOnMouseHoverScript>().enabled = false;
                                newSection.GetComponent<Button>().interactable = false;
                            }
                        } else {
                            if (caseTrackingData.IsSectionRead(Dict[key].GetPosition(), Dict[key].GetCount())) {
                                Dict[sectionName].VisitAllTabs();
                                //} else if (Dict[key].GetPosition() == lastVisitedSection + 1 || Dict[key].GetPosition() == 0) {
                                //Continue
                            } else if (Dict[key].GetPosition() > 0) { //Don't show lock on first step
								if (newSection.transform.Find("Overlay")) {
									newSection.transform.Find("Overlay").GetComponent<Image>().color = new Color(0, 0, 0, (float)175 / 255);
									newSection.transform.Find("Overlay").GetComponent<HideOnMouseHoverScript>().enabled = false;
								}
								newSection.GetComponent<Button>().interactable = false;
                                newSection.transform.Find("Lock").gameObject.SetActive(true);
                            }
                        }
                    }
                }
            }
            if (SectionButtonPar.transform.Find("StepSelectionTitle")) {
                SectionButtonPar.transform.Find("StepSelectionTitle").SetAsFirstSibling();
            }
        }

        yield break;
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
        Text[] children = newSection.GetComponentsInChildren<Text>();
        string buttonName = null;
        foreach (Text child in children) {
            if (child.name.Equals("SectionLinkToText")) { //Where the button links to
                child.text = key;
            } else if (child.name.Equals("SectionDisplayText")) { //What the button displays
                child.text = key.Replace('_', ' ').Substring(0, key.Length - "Section".Length);
                buttonName = child.text.Replace(" ", "_") + "SectionButton";
            }
        }

        SectionDataScript sds = new SectionDataScript();
        sds.SetPosition(0);
        sds.AddPersistingData(dds.defaultTab, null);//.Replace(" ", "_") + "Tab", null);
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
        if (!Dict.ContainsKey(Section) || !Dict[Section].ContainsKey(Tab)) {
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
                        if (child.gameObject.GetComponent<Toggle>() != null && child.gameObject.GetComponent<Toggle>().isOn) {
                            longertext += "<" + child.name + ">";
                            longertext += child.gameObject.GetComponent<Toggle>().isOn;
                            longertext += "</" + child.name + ">";
                        } else if (child.name.ToLower().EndsWith("toggle")) {
                            continue;
                        } else {
                            longertext += "<" + child.name + ">";

                            //Handle reading the child depending on the input type
                            if (child.gameObject.GetComponent<InputField>() != null) {
                                string tempText = child.gameObject.GetComponent<InputField>().text;
                                //tempText = tempText.Replace("<", "[");
                                //tempText = tempText.Replace(">", "]");

                                longertext += tempText;
                            } else if (child.gameObject.GetComponent<Slider>() != null) {

                                longertext += child.gameObject.GetComponent<Slider>().value;
                            } else if (child.gameObject.GetComponent<Dropdown>() != null) {
                                longertext += child.gameObject.GetComponent<Dropdown>().captionText.text;
                            } else if (child.gameObject.GetComponent<Text>() != null) {
                                longertext += child.gameObject.GetComponent<Text>().text;
                            }

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
                string hex = "." + String.Format("{0:X}", a) + ".";
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
                string character = Char.ConvertFromUtf32(Convert.ToInt32(name.Substring(pos + 1, 2), 16));
                name = name.Replace(name.Substring(pos, 4), character);
            }
        }
        return name;
    }

    /**
     * Returns the data of the specified Section
     */
    public SectionDataScript GetData(string Section)
    {
        if (Section == null || Section.Length == 0) {
            return new SectionDataScript();
        }
        if (Dict != null && Dict.ContainsKey(Section)) {
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
        return Dict.Keys.ToArray();
    }

    /**
     * Edits a tab's display name
     */
    public void EditTab(string oldName, string newName)
    {
        string Section = transform.GetComponent<ReaderTabManager>().getCurrentSection();
        Debug.Log("SWAPPING TEXT NAME. OLDNAME: " + oldName + ", NEWNAME: " + newName);
        if (Dict[Section].ContainsKey(oldName)) {
            //Debug.Log ("MATCH FOUND");
            Dict[Section].Replace(oldName, newName);
            //Dict [Section].GetTabList (); //For debugging

            ReaderTabManager tm = GetComponent<ReaderTabManager>();
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

            Dictionary<string, string> tempDict = new Dictionary<string, string>();
            tempDict = dialogueDict;
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
     * Edits a section's display name
     */
    public void EditSection(string oldName, string newName)
    {
        if (Dict.ContainsKey(oldName)) {
            if (!Dict.ContainsKey(newName)) {
                SectionDataScript temp = Dict[oldName];
                Dict.Remove(oldName);

                //Update newName to fit the conventions of section names and update the Dict reference.
                string linkToName = newName.Replace(" ", "_") + "Section";
                Dict.Add(linkToName, temp);
                Dict[linkToName].SetSectionDisplayName(newName);
                transform.GetComponent<ReaderTabManager>().setCurrentSection(linkToName);

                Debug.Log(oldName);
                Dict[linkToName].GetCurrentTab().data = Dict[linkToName].GetCurrentTab().data.Replace(oldName, linkToName);
                foreach (string k in temp.GetTabList()) {
                    //Debug.Log (Dict [linkToName].GetData (k));
                    Dict[linkToName].AddData(k, temp.GetData(k).Replace(oldName, linkToName));
                    //Debug.Log (Dict [linkToName].GetData (k));
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
                throw new System.Exception("Cannot have two sections with matching names!");
            }
        } else {
            throw new System.Exception("Current section does not exist!");
        }
    }

    /**
     * Removes a tab from a section
     */
    public void RemoveTab(string Tab)
    {
        Dict[transform.GetComponent<ReaderTabManager>().getCurrentSection()].Remove(Tab);
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
        //Dict = null;
        dialogueDict.Clear();
        quizDict.Clear();
        flagDict.Clear();
        newTabs.Clear();
    }

    /**
     * Method to check if the provided name has any special characters that XML wouldn't like
     */
    public bool IsValidName(string name)
    {
        if (name.ToLower().StartsWith("xml")) {
            Debug.Log("Name cannot start with xml");
            return false;
        } else if (Regex.IsMatch(name, "(//)+|[*<>&]")) {
            Debug.Log("Name cannot contain //, *, <, >, or &. Please rename");
            return false;
        }

        return true; //Comment this line out to prevent special characters from being used.
                     /*
                     if (!Regex.IsMatch (name, "^[a-zA-Z_]")) {
                         Debug.Log ("Name must start with letters or an underscore");
                         return false;
                     } else if (!Regex.IsMatch (name, "^([a-zA-Z0-9-_. ])*$")) {
                         Debug.Log ("Name contains special characters");
                         return false;
                     }
                     return true;
                     */
    }

    public bool GetReaderOnly()
    {
        return readerOnlyBuild;
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
                notification = Instantiate(Resources.Load("Writer" + "/Prefabs/NotificationMessage") as GameObject, transform);
                notification.name = "NotificationPanel";
            }
        } else if (transform.Find("ErrorPanel") == null) {
            Destroy(notification);
            notification = Instantiate(Resources.Load("Writer" + "/Prefabs/ErrorMessage") as GameObject, transform);
            notification.name = "ErrorPanel";
        }
        CancelInvoke("Fade");
        fade = false;
        notification.GetComponent<CanvasGroup>().alpha = 1;
        notification.SetActive(true);
        Text messageText = notification.transform.Find("BG/Message").GetComponent<Text>();
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

    //Used for testing
    public void buttonprintdata()
    {
        Debug.Log(GetData());
        Debug.Log(GetImagesXML());
        Debug.Log(GetData(transform.GetComponent<ReaderTabManager>().getCurrentSection()).GetAllPositions());
    }

    public void Reload()
    {
        ClearAllData();
        Awake();
        StartCoroutine(Start());
    }
}