using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Xml;
using System.IO;
using UnityEngine.UI;
using System.Text;
using System.Security.Cryptography;
using UnityEngine.Networking;
using SimEncounters;

public class UploadToServer : MonoBehaviour
{
    //private WWW www;
    //private WWW w2;
    private WriterHandler ds;
    private string fileName;
    private string path;
    private string shrunkCharacterImageData;

    // Use this for initialization
    void Start()
    {
        ds = WriterHandler.WriterInstance;
        fileName = GlobalData.fileName;
        path = GlobalData.filePath;
    }

    /**
	 * Method to call when saving the file from SubmitToXML.cs to avoid stopping coroutines
	 */
    public void StartUpload()
    {
        StartCoroutine(SubmitToFile());
    }

    /**
	 * Handles uploading all data to the server.
	 * Currently it's a mess because I'm not sure if we'll need to store more than the max_allowed_packet variable
	 * but it can do that if needed. Just remove most of the commented things/format them and it should work for that.
	 */
    private IEnumerator Upload()
    {
        print("Uploading to server...");
        string fileName = GlobalData.fileName.Replace(" ", "_");
        string serverURL = GlobalData.serverAddress + "Test.php";
        string urlParams = "?webfilename=" + fileName + "&webusername=clinical&webpassword=encounters&mode=download";
        string wwwText = "";
        string imagesXML = ds.GetImagesXml();
        int i = -1;
        int MAP = 0;
        do {
            //For uploading
            WWWForm form = new WWWForm();
            form.AddField("mode", "upload");
            form.AddField("fileN", fileName);
            form.AddField("account_id", GlobalData.accountId);
            form.AddField("index", i); //Used for segmenting image uploads

            //form.AddField ("column", "xmlData");

            byte[] fileBytes = GetFileAsByteArray("<body>" + ds.GetXml() + "</body>");
            Debug.Log("Case file size (in bytes): " + fileBytes.Length);
            form.AddBinaryData("xmlData", fileBytes, fileName, "text/xml");

            if (MAP == 0) {
                wwwText = imagesXML; //use "Placeholder" if trying to segment data;
            } else if (MAP * i + MAP > imagesXML.Length) {
                wwwText = imagesXML.Substring(MAP * i);
            } else {
                wwwText = imagesXML.Substring(MAP * i, MAP);
            }
            byte[] fileBytesImg = GetFileAsByteArray(wwwText);
            Debug.Log("Image file size (in bytes): " + fileBytesImg.Length);
            if (imagesXML.Length > 10000000) { //If the xml length is greater than max_allowed_packet
                print("Error: Images exceed upload size limit");
            } else {
                form.AddBinaryData("imgData", fileBytesImg, fileName, "text/xml");
            }

			using (UnityWebRequest webRequest = UnityWebRequest.Post(serverURL, form)) {
				yield return webRequest.SendWebRequest();
				while (!webRequest.isDone) {
					Debug.Log(webRequest.uploadProgress);
					yield return new WaitForSeconds(0.5f);
				}

				if (webRequest.error != null) {
					print("Error: " + webRequest.error);
				} else {
					if (webRequest.uploadProgress == 1 && webRequest.isDone) {
						Debug.Log("Returned text from PHP: \n" + webRequest.downloadHandler.text);
					}
				}

				try {
					MAP = int.Parse(webRequest.downloadHandler.text);
				} catch (Exception) {
					//print ("Number was not returned in www.text");
				}
				i++;
			}            
        } while (false);//MAP * i < imagesXML.Length);
        serverURL = GlobalData.serverAddress + "Test.php";

        //Upload the truncated case preview for the main menu
        yield return StartCoroutine(UploadMenuEntry());

        //ShowConfirmation ("Upload finished!");
        ds.ShowMessage("Upload finished!");
    }

	/**
	 * We want to make sure we have a unique case. Go ahead and shuffle the record number and filename then check them on the server
	 */
	public IEnumerator AvoidServerDuplicates()
	{
		if (GlobalData.fileName.StartsWith("[CHECKFORDUPLICATE]")) {
			//Remove any existing files if the user saved 
			if (File.Exists(GlobalData.filePath + GlobalData.fileName)) {
				File.Delete(GlobalData.filePath + GlobalData.fileName);
				File.Delete(GlobalData.filePath + GlobalData.fileName.Substring(0, fileName.Length - 3) + "cei");
				File.Delete(GlobalData.filePath + GlobalData.fileName.Substring(0, fileName.Length - 3) + "xml");
				File.Delete(GlobalData.filePath + GlobalData.fileName.Substring(0, fileName.Length - 4) + " menu.txt");
			}
			GlobalData.fileName = GlobalData.fileName.Remove(0, "[CHECKFORDUPLICATE]".Length);

		}
		print("filename: " + GlobalData.fileName + ", recordNumber: " + GlobalData.caseObj.recordNumber);
		string serverURL = GlobalData.serverAddress + "Menu.php";

		//Assign a new record number for the new case
		string recordNumber = (Math.Floor(UnityEngine.Random.value * 999999) + "").PadLeft(6, '0');
		GlobalData.fileName = GlobalData.caseObj.recordNumber + GlobalData.firstName + " " + GlobalData.lastName + ".ced";
		GlobalData.caseObj.recordNumber = recordNumber;
		Debug.Log(GlobalData.accountId);

		//Check filename and record number for duplicates

		WWWForm form = new WWWForm();
		form.AddField("mode", "checkFields");
		string outputText;
		do {
			print("checking " + recordNumber);
			form = new WWWForm();
			form.AddField("mode", "checkFields");
			form.AddField("filename", GlobalData.fileName.Replace(" ", "_"));
			form.AddField("recordNumber", recordNumber);
			using (UnityWebRequest webRequest = UnityWebRequest.Post(serverURL, form)) {
				yield return webRequest.SendWebRequest();
				outputText = webRequest.downloadHandler.text;
				Debug.Log(outputText);
			}
			if (outputText.Equals("duplicate")) {
				GlobalData.caseObj.recordNumber = (Math.Floor(UnityEngine.Random.value * 99999) + "").PadLeft(6, '0');
				GlobalData.fileName = GlobalData.caseObj.recordNumber + GlobalData.firstName + " " + GlobalData.lastName + ".ced";
				recordNumber = GlobalData.caseObj.recordNumber;
			}
		} while (!outputText.Equals("No cases")) ;
        GlobalData.caseObj.recordNumber = recordNumber;
        GlobalData.fileName = GlobalData.caseObj.recordNumber + GlobalData.firstName + " " + GlobalData.lastName + ".ced";
        print("filename: " + GlobalData.fileName + ", recordNumber: " + GlobalData.caseObj.recordNumber);
    }

    /**
	 * Uploades a truncated version of the main file to be used for the main menu
	 * List of SQL Table columns: account_id, filename, caseID, patientName, difficulty, topic
	 * May add reviews, rating, feedback, categories
	 */
    private IEnumerator UploadMenuEntry()
    {
        string serverURL = GlobalData.serverAddress + "Menu.php";

        //AvoidServerDuplicates() code was here

        //For uploading
        WWWForm form = new WWWForm();
        form.AddField("mode", "upload");
        form.AddField("account_id", GlobalData.accountId);
        form.AddField("filename", GlobalData.fileName.Replace(" ", "_"));
        //form.AddField ("authorName", "Author Name Here");
        form.AddField("authorName", GetAuthorName()); //The authorname is actually set in PHP but that can be changed
        form.AddField("patientName", GlobalData.firstName + "_" + GlobalData.lastName);
        form.AddField("recordNumber", GlobalData.caseObj.recordNumber);
        form.AddField("difficulty", GlobalData.caseObj.difficulty);
        form.AddField("description", GlobalData.caseObj.description);
        form.AddField("summary", GlobalData.caseObj.summary);
        form.AddField("tags", GlobalData.caseObj.GetTagsAsOneString());
        form.AddField("modified", GlobalData.caseObj.dateModified + "");
        form.AddField("audience", GlobalData.caseObj.audience);
        form.AddField("version", "Early Access");
        form.AddField("rating", GlobalData.caseObj.rating);
        form.AddField("caseType", GlobalData.caseObj.caseType.GetHashCode() + "");
        //I'll have to figure out how tags are stored/handled. When uploding, I only need to pass in a 1 or 0 for up and down for each tag
        //That way I don't have to locally store the rating, although this may prove easiest in the long run.
        //We'll just have to wait and see.

        print("Uploading Menu entry...");
        //Use below for image data if needed
        //byte[] fileBytes = GetFileAsByteArray ("base64 text of image goes here");
        //Debug.Log ("Image file size (in bytes): " + fileBytes.Length);
        //form.AddBinaryData ("image", fileBytes, GlobalData.fileName.Replace (" ", "_"), "text/base64"); //Use this to upload patient pictures. Keep these small
        if (GlobalData.uploadCharacterImage) {
            yield return StartCoroutine(GetCharacterImageData());
            form.AddField("image", shrunkCharacterImageData);
        }

		using (UnityWebRequest webRequest = UnityWebRequest.Post(serverURL, form)) {
			yield return webRequest.SendWebRequest();

			if (webRequest.error != null) {
				print("Error: " + webRequest.error);
			} else {
				if (webRequest.uploadProgress == 1 && webRequest.isDone) {
					Debug.Log("Returned text from PHP: \n" + webRequest.downloadHandler.text);
					GlobalData.caseObj.localOnly = false;
					GlobalData.caseObj.server = true;
				}
			}
		}
    }

    private IEnumerator TestCharThingy(Image i)
    {
        yield return StartCoroutine(GetCharacterImageData());
        Texture2D temp = new Texture2D(2, 2);
        temp.LoadImage(Convert.FromBase64String(shrunkCharacterImageData));
        Sprite newSprite = Sprite.Create(temp, new Rect(0, 0, 100, 100), new Vector2(0, 0), 1);
        i.sprite = newSprite;

    }

    public void TestingShrink(Image i)
    {
        StartCoroutine(TestCharThingy(i));
    }

    private IEnumerator GetCharacterImageData()
    {
        //Camera cam = GetComponentInParent<CharacterManagerScript>().faceCamera;
        //RenderTexture bigCharacter = cam.targetTexture;
        RenderTexture smallCharacter = Resources.Load("CharacterPortraitSmall") as RenderTexture;

        //cam.targetTexture = smallCharacter;
        yield return new WaitForEndOfFrame();

        //Texture newTexture = transform.Find("SidePanel/MainPanel/MenuPanel/CharacterCreation/Image").GetComponent<RawImage>().mainTexture;
        Texture2D nt2 = new Texture2D(100, 100, TextureFormat.ARGB32, false);

        //RenderTexture toBeActive = new RenderTexture(newTexture.width, newTexture.height, 0);
        //Graphics.Blit(newTexture, toBeActive);
        //RenderTexture.active = toBeActive;
        RenderTexture.active = smallCharacter;

        nt2.ReadPixels(new Rect(0, 0, 100, 100), 0, 0);
        nt2.Apply();

        byte[] bytes = nt2.EncodeToPNG();
        string imageData = Convert.ToBase64String(bytes);
        print(imageData.Length + ", " + imageData);
        shrunkCharacterImageData = imageData;
        //cam.targetTexture = bigCharacter;



        /*
		Camera cam = GameObject.Find("CameraTest").GetComponent<Camera>();
		RenderTexture rtBig = cam.targetTexture;
		RenderTexture rtSmall = Resources.Load("CharacterPortraitSmall") as RenderTexture;

		print(cam.targetTexture.width);
		cam.targetTexture = rtSmall;
		yield return new WaitForSecondsRealtime(5f);
		print(cam.targetTexture.width);
		cam.targetTexture = rtBig;
		*/
    }

    //Change this in ServerControls too if changed
    public string GetAuthorName()
    {
        if (GlobalData.userTitle.Equals("--") || GlobalData.userTitle.Equals("")) {
            return GlobalData.userFirstName + " " + GlobalData.userLastName;
        }
        return GlobalData.userTitle + " " + GlobalData.userFirstName + " " + GlobalData.userLastName;
    }

    public string GetDownloadedText()
    {
        return downloadedData;
    }

	private string downloadedData;

    /**
	 * Download a column's data from the SQL server
	 * @param column: the column whose data is to be downloaded
	 */
    public IEnumerator DownloadFromServer(string column)
    {
        print("Downloading " + column + "'s data...");
        string fileName = GlobalData.fileName.Replace(" ", "_");
        string serverURL = GlobalData.serverAddress + "Test.php";
        string urlParams = "?webfilename=" + fileName + "&webusername=clinical&webpassword=encounters&mode=download";
		string address = serverURL + urlParams + "&column=" + column + "&accountId=" + GlobalData.caseObj.accountId;
		using (UnityWebRequest webRequest = UnityWebRequest.Get(address)) {
			yield return webRequest.SendWebRequest();

			if (webRequest.error != null) {
				print("Error: " + webRequest.error);
				if (webRequest.error.Equals("Error: Cannot connect to destination host")) {
					DownloadFromServer(column);
				}
			}

			//Ensure that the download starts
			while (webRequest.downloadProgress == 0) {
				yield return null;
			}

			//Get the loading screen and progress slider
			Transform loadingScreen = null;
			if (ds == null) {
				loadingScreen = GetComponent<ReaderDataScript>().loadingScreen.transform;
			} else {
                // TODO: handle loading screen
				//loadingScreen = ds.loadingScreen.transform;
			}
			loadingScreen.transform.Find("LoadingBar").gameObject.SetActive(true);
			Slider slider = loadingScreen.GetComponentInChildren<Slider>();

			//Show progress in the slider
			while (!webRequest.isDone) {// && contentLength > 0) {
				//Want to show progress here but cannot do it until we get Content-Length header to be more consistant
				Debug.Log(string.Format("{0:P1}", webRequest.downloadProgress)); //(w2.progress / float.Parse(w2.responseHeaders["Content-Length"])) + ""));
				slider.value = webRequest.downloadProgress;
				yield return new WaitForSeconds(0.2f);
			}

			//Set the slider value to max
			loadingScreen.GetComponentInChildren<Slider>().value = 0;

			if (webRequest.downloadHandler.text != null && webRequest.downloadHandler.text != "") {
				downloadedData = webRequest.downloadHandler.text;
				Debug.Log("Data from server: " + downloadedData);
			} else {
				print("File " + fileName + " is Empty");
			}
		}
    }

    /**
	 * Returns the passed in string as a byte array. Makes code easier to read
	 */
    private byte[] GetFileAsByteArray(string data)
    {
        return System.Text.Encoding.UTF8.GetBytes(data);
    }

    private MenuCase.CaseType newCaseType;
    public void SetNewCaseType(int caseType)
    {
        newCaseType = (MenuCase.CaseType)caseType;
    }


    /**
	 * Saves the dictionary as an XML file (both formatted/easy to read and non-formatted)
	 */
    private IEnumerator SubmitToFile()
    {
        //Toggle overwriteToggle = transform.Find("SaveCaseBG/SaveCasePanel/Content/Row1/OverwriteTemplateToggle").GetComponent<Toggle>();

        print(GlobalData.caseObj.IsTemplate()); //Is template
                                                //print(!overwriteToggle.isOn); //Overwrite template?

        if (GlobalData.createCopy || GlobalData.fileName.StartsWith("[CHECKFORDUPLICATE]")) {
            //if ((GlobalData.caseObj.caseType.GetHashCode() & GlobalData.caseObj.templateCompare) == GlobalData.caseObj.templateCompare &&
            //!overwriteToggle.isOn) {
            yield return StartCoroutine(ds.ServerUploader.AvoidServerDuplicates());
            GlobalData.createCopy = false;
        }

        //We set the case type to the new case type after we have handled any case relying on previous case type

        GlobalData.caseObj.caseType = newCaseType;

        fileName = GlobalData.fileName;
        string tempFileName = fileName.Remove(fileName.Length - 3);

        string data = "<body>" + ds.GetXml() + "</body>";
        Debug.Log(data);



        //Outputting regular file
        StreamWriter sw = new StreamWriter(path + tempFileName + "ced", false);
        //sw.WriteLine(data);
        sw.Close();
        sw.Dispose();
        File.WriteAllBytes(path + tempFileName + "ced", EncryptStringToBytes_Aes(data));

        //Formatted, easy to read version
        sw = new StreamWriter(path + tempFileName + "xml", false);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(data);
        xmlDoc.Save(sw);
        sw.Close();
        sw.Dispose();

        //Images
        sw = new StreamWriter(path + tempFileName + "cei", false);
        //sw.WriteLine (ds.GetImagesXML ());
        sw.Close();
        sw.Dispose();
        File.WriteAllBytes(path + tempFileName + "cei", EncryptStringToBytes_Aes(ds.GetImagesXml()));

        /* Non-encrypted local saving
		//Outputting regular file
		StreamWriter sw = new StreamWriter(path + tempFileName + "ced", false);
		sw.WriteLine(data);
		sw.Close();
		sw.Dispose ();

		//Formatted, easy to read version
		data = replaceValues(data);
		XmlDocument xmlDoc = new XmlDocument();
		//data = WWW.UnEscapeURL (data);
		xmlDoc.LoadXml(data);

		//XmlWriter xw = XmlWriter.Create(path + tempFileName + "xml");
		sw = new StreamWriter(path + tempFileName + "xml", false);
		xmlDoc.Save(sw);
		sw.Close();
		sw.Dispose ();

		//Images
		sw = new StreamWriter(path + tempFileName + "cei", false);
		sw.WriteLine (ds.GetImagesXML());
		sw.Close ();
		sw.Dispose ();
		*/


        //Delete the menu text file since we will have the information up on the server
        File.Delete(GlobalData.filePath + fileName.Remove(fileName.Length - 4) + " menu.txt");

        //Delete the autosave as well
        File.Delete(path + tempFileName + "auto");
        File.Delete(path + tempFileName + "iauto");
        ds.RestartAutosave();


        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime localFileModified = File.GetLastWriteTime(GlobalData.filePath + GlobalData.fileName);

        //GlobalData.caseObj.dateModified = (long) DateTime.UtcNow.Subtract (unixEpoch).TotalSeconds;// Old method
        GlobalData.caseObj.dateModified = (long)localFileModified.ToUniversalTime().Subtract(unixEpoch).TotalSeconds;

        Debug.Log("Saved: " + path + tempFileName);


        //ShowConfirmation("Case saved locally! Uploading to server...");
        ds.ShowMessage("Saved locally! Uploading to server...");
        yield return StartCoroutine(Upload());
        //b.interactable = false;
        //Debug.Log("Data successfully submitted!");
    }

    public string GetMenuText()
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

        return GlobalData.accountId + "--" +
                GlobalData.fileName.Replace(" ", "_") + "--" +
                GetAuthorName() + "--" +
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

    private string replaceValues(string s)
    {
        StringBuilder sb = new StringBuilder(s);

        sb.Replace("%26", "&amp;");
        sb.Replace("%22", "&quot;");
        sb.Replace("%3c", "&lt;");
        sb.Replace("%3e", "&gt;");


        return sb.ToString();
    }
}
