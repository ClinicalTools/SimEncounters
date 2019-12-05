using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FilePickerScript : MonoBehaviour
{

    private string[] extensions = new string[] { "ced" };
    private GlobalData myGlobals;

    public Image testImage;

    void Awake()
    {

    }


    // Use this for initialization
    IEnumerator Start()
    {
        //myGlobals = GameObject.Find ("Canvas").GetComponent<GlobalData> ();

        //The "View in reader/writer" buttons will have this script as a component
        if (SceneManager.GetActiveScene().name.Equals("Writer")) {
            yield break;
        } else if (SceneManager.GetActiveScene().name.Equals("CassReader")) {
            if (GlobalData.caseObj == null || GlobalData.caseObj.accountId != GlobalData.accountId) {
                gameObject.SetActive(false);
            }
            yield break;
        }

        //This is the image shrinking that works
        //ImageCompressingNewer()

        //NextFrame.Function(delegate { TestImageCompression(); });
        //StartCoroutine(TestImageCompression2());

        //TestEncryption2();
        //TestEncryption3();

        //Application.backgroundLoadingPriority = ThreadPriority.Low;
        //NextFrame.Function(StartLoading);
    }

    public void OpenFile()
    {
        Cursor.visible = true;
        GameObject.Find("BrowseFiles").GetComponent<Button>().interactable = false;

        string filePath = null;// FileBrowser.OpenSingleFile("Open case file", Application.persistentDataPath, extensions);
        if (string.IsNullOrWhiteSpace(filePath)) {
            GlobalData.fileName = null;
            Debug.Log("[Open File] Canceled");
            Cursor.visible = false;
            GameObject.Find("BrowseFiles").GetComponent<Button>().interactable = true;
            return;
        }

        string[] explodedPath = filePath.Split(new string[] { "\\" }, StringSplitOptions.None);

        GlobalData.fileName = explodedPath[explodedPath.Length - 1];
        /*
        GlobalData.filePath = "";
        for(int i = 0; i < explodedPath.Length - 1; i++){
            GlobalData.filePath += explodedPath[i] + "\\";
        }
        */


        Debug.Log("[Open File] Selected file: " + GlobalData.filePath + GlobalData.fileName);
        GameObject.Find("BrowseFiles").GetComponent<Button>().interactable = true;
        ActivateScene();
    }

    public void OpenFromWriter()
    {
        Cursor.visible = true;
        string filePath = null;// FileBrowser.OpenSingleFile("Open case file", Application.persistentDataPath, extensions);
        if (string.IsNullOrWhiteSpace(filePath)) {
            GlobalData.fileName = null;
            Debug.Log("[Open File] Canceled");
            Cursor.visible = false;
            GameObject.Find("BrowseFiles").GetComponent<Button>().interactable = true;
            return;
        }

        string[] explodedPath = filePath.Split(new string[] { "\\" }, StringSplitOptions.None);

        GlobalData.fileName = explodedPath[explodedPath.Length - 1];

        GlobalData.filePath = "";
        for (int i = 0; i < explodedPath.Length - 1; i++) {
            GlobalData.filePath += explodedPath[i] + "\\";
        }
        Cursor.visible = false;
        Debug.Log("[Open File] Selected file: " + GlobalData.filePath + GlobalData.fileName);
        GlobalData.caseObj = new MenuCase(GlobalData.fileName);
        // TODO: reload
        //GameObject.Find("GaudyBG").GetComponent<DataScript>().Reload();
    }

    public void NewFile()
    {
        Cursor.visible = true;
        Button newButton = null;

        if (GameObject.Find("NewCase") != null) {
            newButton = GameObject.Find("NewCase").GetComponent<Button>();
            newButton.interactable = false;
        }
        if (GlobalData.demo) {
            /*string recordNumber = (Math.Floor(UnityEngine.Random.value * 99999) + "").PadLeft(6, '0');
            while (File.Exists(recordNumber + ".ced")) {
                recordNumber = (Math.Floor(UnityEngine.Random.value * 99999) + "").PadLeft(6, '0');
            }

            GlobalData.fileName = recordNumber + ".ced";
            GlobalData.filePath = Application.streamingAssetsPath + "/DemoCases/Cases/";*/
            GlobalData.newCase = true;
            ActivateScene();
        } else {
            string filePath = null;// FileBrowser.OpenSingleFile("Open case file", Application.persistentDataPath, extensions);
            if (string.IsNullOrWhiteSpace(filePath)) {
                GlobalData.fileName = "[Save File] Canceled";
                Debug.Log("[Save File] Canceled");
                Cursor.visible = false;
                if (newButton != null) {
                    newButton.interactable = true;
                }
                return;
            }

            GlobalData.createCopy = true;

            string[] explodedPath = filePath.Split(new string[] { "\\" }, StringSplitOptions.None);

            GlobalData.fileName = explodedPath[explodedPath.Length - 1];
            if (!GlobalData.fileName.Substring(GlobalData.fileName.Length - 4).Equals(".ced")) {
                GlobalData.fileName += ".ced";
            }
            if (GlobalData.accountId == 0) {
                GlobalData.filePath = Application.persistentDataPath + "\\LocalSaves\\" + GameObject.Find("GaudyBG").GetComponent<ServerControls>().GetLocalFolder(GlobalData.accountId + "") + "\\";
            }

            /*
            GlobalData.filePath = "";
            for (int i = 0; i < explodedPath.Length - 1; i++) {
                GlobalData.filePath += explodedPath[i] + "\\";
            }
            */

            Debug.Log(GlobalData.fileName);
            Debug.Log(GlobalData.filePath);
            Debug.Log("[Save File] Selected file: " + GlobalData.filePath + GlobalData.fileName);
            if (File.Exists(filePath)) {
                File.Delete(filePath);
            }
            GlobalData.newCase = true;
            GlobalData.caseObj = new MenuCase(GlobalData.fileName);
            //ActivateScene();
            transform.parent.GetComponent<ServerControls>().loadingScreen.SetActive(true);
            StartCoroutine(OpenScene("Writer"));
        }
    }

    public void NewFileAvoidFilePicker()
    {
        GlobalData.createCopy = true;
        if (GlobalData.accountId == 0 && !GlobalData.demo) {
            GlobalData.filePath = Application.persistentDataPath + "\\LocalSaves\\" + GameObject.Find("GaudyBG").GetComponent<ServerControls>().GetLocalFolder(GlobalData.accountId + "") + "\\";
        }

        /*
        GlobalData.filePath = "";
        for (int i = 0; i < explodedPath.Length - 1; i++) {
            GlobalData.filePath += explodedPath[i] + "\\";
        }
        */
        GlobalData.fileName = "I need a fake filename"; //This gets past a check in the writer to allow running from the writer
        GlobalData.newCase = true;
        GlobalData.caseObj = new MenuCase(GlobalData.fileName) {
            patientName = GlobalData.firstName + "_" + GlobalData.lastName
        };
        //ActivateScene();
        transform.parent.GetComponent<ServerControls>().loadingScreen.SetActive(true);
        GameObject.Find("GaudyBG").GetComponent<ServerControls>().TestShowLoading();
        StartCoroutine(OpenScene("Writer"));
    }

    //Loading level at start of game, but waiting to activate
    public string levelName;
    AsyncOperation asyncReader;
    AsyncOperation asyncWriter;

    public void StartLoading()
    {
        StartCoroutine("load");
    }

    public void WriterToReader()
    {
        GlobalData.loadLocal = true;
        levelName = "CassReader";
        GlobalData.resourcePath = "Reader";
        if (GlobalData.caseObj == null) {
            GlobalData.caseObj = new MenuCase(GlobalData.fileName);
        }
        GlobalData.caseObj.accountId = GlobalData.accountId;
        // TODO: start loading screen 
        //GameObject.Find("GaudyBG").GetComponent<DataScript>().loadingScreen.gameObject.SetActive(true);
        SceneManager.LoadScene(levelName);
    }

    public void ReaderToWriter()
    {
        //If you are the owner/creator of the case
        if (GlobalData.caseObj != null && GlobalData.caseObj.accountId == GlobalData.accountId) {
            //GlobalData.loadLocal = true;
            levelName = "Writer";
            GlobalData.resourcePath = "Writer";
            GameObject.Find("GaudyBG").GetComponent<ReaderDataScript>().loadingScreen.gameObject.SetActive(true);
            SceneManager.LoadScene(levelName);
        }
    }

    IEnumerator load()
    {
        Application.backgroundLoadingPriority = ThreadPriority.Low;
        Debug.LogWarning("ASYNC LOAD STARTED - " +
            "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
        asyncWriter = SceneManager.LoadSceneAsync("Writer");
        asyncWriter.allowSceneActivation = false;
        asyncReader = SceneManager.LoadSceneAsync("CassReader");
        asyncReader.allowSceneActivation = false;
        yield return asyncWriter;
        yield return asyncReader;
        Debug.Log("Level has finished loading");
    }

    public void OpenDemoScene(string fileName)
    {
        GlobalData.fileName = fileName;
        GlobalData.filePath = Application.streamingAssetsPath + "/DemoCases/Cases/";
        NextFrame.Function(ActivateScene);
    }

    public IEnumerator OpenScene(string sceneName)
    {
        Debug.Log("Activating scene");
        if (GlobalData.GDS.isMobile) {
            // TODO: Hide cursor offscreen
        }

        //yield return SceneManager.LoadSceneAsync("LoadingScreen", LoadSceneMode.Additive);

        if (levelName.Equals("CassReader")) {
            GlobalData.resourcePath = "Reader";
        }
        Application.backgroundLoadingPriority = ThreadPriority.Normal;
        AsyncOperation loader = SceneManager.LoadSceneAsync(sceneName);
        while (!loader.isDone) {
            //GameObject.Find("Canvas").transform.Find("LoadingScreen").GetComponentInChildren<Slider>().value = loader.progress;
            //SceneManager.GetSceneByName("DontDestroyOnLoad").GetRootGameObjects()[0].GetComponentInChildren<Slider>().value = loader.progress;
            //GameObject.Find("LoadingScreenNew").GetComponentInChildren<Slider>().value = loader.progress;
            LoadingScreenManager.Instance.GetComponentInChildren<Slider>().value = loader.progress;
            yield return null;
        }
        yield return loader;
    }

    public void ActivateScene()
    {
        Debug.Log("Activating scene");
        if (levelName.Equals("CassReader")) {
            GlobalData.resourcePath = "Reader";
            asyncReader.allowSceneActivation = true;
            Application.backgroundLoadingPriority = ThreadPriority.Normal;
        } else {
            SceneManager.UnloadSceneAsync("CassReader");
            asyncWriter.allowSceneActivation = true;
            Application.backgroundLoadingPriority = ThreadPriority.Normal;
        }
    }

    /**
     * The following two methods are taken from Microsoft's page on managed aes encryption
     * https://msdn.microsoft.com/en-us/library/system.security.cryptography.aesmanaged(v=vs.110).aspx
     */
    static byte[] EncryptStringToBytes_Aes(string plainText)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
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

    static string DecryptStringFromBytes_Aes(byte[] cipherText)
    {
        // Check arguments.
        if (cipherText == null || cipherText.Length <= 0)
            throw new ArgumentNullException("cipherText");

        // Declare the string used to hold
        // the decrypted text.
        string plaintext = null;

        // Create an AesManaged object
        // with the specified key and IV.
        using (AesManaged aesAlg = new AesManaged()) {
            aesAlg.Key = System.Text.Encoding.UTF8.GetBytes(GlobalData.encryptionKey);
            aesAlg.IV = System.Text.Encoding.UTF8.GetBytes(GlobalData.encryptionIV);

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
        char[] chars = hex.ToCharArray();
        byte[] bytes = new byte[hex.Length];
        for (int i = 0; i < hex.Length; i++) {
            bytes[i] = byte.Parse(chars[i].ToString(), System.Globalization.NumberStyles.HexNumber);
        }
        return bytes;
    }

    public void ImportFile()
    {
        extensions = new string[] { "ced" };
        string filePath = null; // FileBrowser.OpenSingleFile("Open case file", Application.persistentDataPath, extensions);
        if (string.IsNullOrWhiteSpace(filePath)) {
            GlobalData.fileName = "[Import] Canceled";
            Debug.Log("[Import] Canceled");
            Cursor.visible = false;
            return;
        }

        XmlDocument xmlDoc = new XmlDocument();
        try {
            string s = File.ReadAllText(filePath);
            print(s);
            xmlDoc.LoadXml(s); //This loads the locally saved file
        } catch (Exception) {
            string text = "";
            AesManaged aes = new AesManaged();
            ICryptoTransform decrypt = aes.CreateDecryptor(Encoding.UTF8.GetBytes(GlobalData.encryptionKey), Encoding.UTF8.GetBytes(GlobalData.encryptionIV));
            using (FileStream fs = new FileStream(filePath, FileMode.Open)) {
                using (CryptoStream cs = new CryptoStream(fs, decrypt, CryptoStreamMode.Read)) {
                    using (StreamReader sr = new StreamReader(cs)) {
                        text = sr.ReadToEnd();
                    }
                }
            }
            //xmlDoc.LoadXml(DecryptStringFromBytes_Aes(GetBytesFromHex(s)));
            try {
                xmlDoc.LoadXml(text);
            } catch (XmlException xmlError) {
                Debug.LogWarning(xmlError.Message);
                return;
            }
        }

        XmlDocument subXmlDoc = new XmlDocument();
        subXmlDoc.LoadXml(xmlDoc.DocumentElement.ChildNodes[0].OuterXml);
        string menuText = subXmlDoc.GetElementsByTagName("menu")[0].InnerText;
        string[] menuSplit = menuText.Split(new string[] { "--" }, StringSplitOptions.None);
        menuSplit[0] = GlobalData.accountId.ToString();
        menuSplit[1] = "[CHECKFORDUPLICATE]" + menuSplit[1];
        bool addCase = true;
        if (File.Exists(GameObject.Find("GaudyBG").GetComponent<ServerControls>().GetLocalSavesFolderPath() + menuSplit[1])) {
            addCase = false;
        }
        //Change the record number to avoid local conflicts or maybe remove that check for the menu
        menuText = string.Join("--", menuSplit);
        MenuCase m = new MenuCase(menuSplit);
        string savePathNoExtension = GameObject.Find("GaudyBG").GetComponent<ServerControls>().GetLocalSavesFolderPath() + m.filename.Remove(m.filename.Length - ".ced".Length);

        //Save local menu preview
        File.WriteAllText(savePathNoExtension + " menu.txt", menuText);
        //Save .ced
        File.WriteAllBytes(savePathNoExtension + ".ced", EncryptStringToBytes_Aes(xmlDoc.DocumentElement.ChildNodes[1].OuterXml));
        //Save .cei
        File.WriteAllBytes(savePathNoExtension + ".cei", EncryptStringToBytes_Aes(xmlDoc.DocumentElement.ChildNodes[2].OuterXml));

        if (addCase) {
            GameObject.Find("GaudyBG").GetComponent<ServerControls>().AddCase(m, true);
        } else {
            GameObject.Find("GaudyBG").GetComponent<LoginManager>().ShowMessage("Reimported case");
        }
    }

    public void ExportFile(TextMeshProUGUI filename)
    {
        ExportFile(filename.text);
    }

    public void ExportFile(string filename)
    {
        extensions = new string[] { "ced" };
        string filePath = null;// FileBrowser.OpenSingleFile("Open case file", Application.persistentDataPath, extensions);
        if (string.IsNullOrWhiteSpace(filePath)) {
            GlobalData.fileName = "[Save File] Canceled";
            Debug.Log("[Save File] Canceled");
            Cursor.visible = false;
            return;
        }

        //The user has chosen where to save the export, so we continue

        if (!File.Exists(GameObject.Find("GaudyBG").GetComponent<ServerControls>().GetLocalSavesFolderPath() + filename)) {
            //download the files if they aren't downloaded locally
            StartCoroutine(ExportFileCoroutine(filename, filePath));
            return;
        }
        //If local files exist, copy their contents to an export file

        //Create the string of data to be encrypted/saved
        string data = "<exportData>";
        data += "<menu>" + GameObject.Find("GaudyBG").GetComponent<ServerControls>().GetCaseByFilename(filename).ToString() + "</menu>";

        //Must decrypt the local files before re-encrypting them
        using (Aes aes = Aes.Create()) {
            aes.Key = System.Text.Encoding.UTF8.GetBytes(GlobalData.encryptionKey);
            aes.BlockSize = 128;
            aes.IV = System.Text.Encoding.UTF8.GetBytes(GlobalData.encryptionIV);
            aes.Padding = PaddingMode.None;
            ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, aes.IV);

            //Decrypt .ced
            using (FileStream fs = new FileStream(GameObject.Find("GaudyBG").GetComponent<ServerControls>().GetLocalSavesFolderPath() + filename, FileMode.Open)) {
                using (CryptoStream cs = new CryptoStream(fs, decrypt, CryptoStreamMode.Read)) {
                    using (StreamReader sr = new StreamReader(cs)) {
                        data += sr.ReadToEnd();
                        fs.Close();
                        cs.Close();
                        sr.Close();
                    }
                }
            }
            data = data.TrimEnd(new char[] { '\n', '\u0008', '' });

            decrypt = aes.CreateDecryptor(aes.Key, aes.IV);

            //Decrypt .cei
            string imageXml = "";
            using (FileStream fs = new FileStream(GameObject.Find("GaudyBG").GetComponent<ServerControls>().GetLocalSavesFolderPath() + filename.Remove(filename.Length - ".ced".Length) + ".cei", FileMode.Open)) {
                using (CryptoStream cs = new CryptoStream(fs, decrypt, CryptoStreamMode.Read)) {
                    using (StreamReader sr = new StreamReader(cs)) {
                        imageXml = sr.ReadToEnd();
                    }
                }
            }
            data += imageXml;
            data = data.TrimEnd(new char[] { '\n', '\u0008' });

            //data += WWW.EscapeURL(File.ReadAllText(GameObject.Find("GaudyBG").GetComponent<ServerControls>().GetLocalSavesFolderPath() + filename));
            //data += WWW.EscapeURL(File.ReadAllText(GameObject.Find("GaudyBG").GetComponent<ServerControls>().GetLocalSavesFolderPath() + filename.Remove(filename.Length - ".ced".Length) + ".cei"));
            data += "</exportData>";
            print(data.Substring(data.Length - 820));
            /*sw = new StreamWriter("C:/Users/Will/Desktop/TEXTFILEEXPORTLOCAL.txt");
            sw.Write(data);
            sw.Close();
            sw.Dispose();*/
            File.WriteAllText("C:/Users/Will/Desktop/TEXTFILEEXPORTLOCAL.txt", data);

            //Add the correct extension if it isn't present
            if (!filePath.EndsWith(extensions[0])) {
                filePath += extensions[0];
            }

            //Encrypt and save the new export file
            File.WriteAllBytes(filePath, EncryptStringToBytes_Aes(data));
        }
    }

    private IEnumerator ExportFileCoroutine(string filename, string filePath)
    {
        print("Downloading files for export...");

        //Create the string of data to be encrypted/saved
        string data = "<exportData>";
        data += "<menu>" + GameObject.Find("GaudyBG").GetComponent<ServerControls>().GetCaseByFilename(filename).ToString() + "</menu>";
        print(data);

        yield return StartCoroutine(GetComponent<UploadToServer>().DownloadFromServer("xmlData"));
        print(GetComponent<UploadToServer>().GetDownloadedText());
        data += GetComponent<UploadToServer>().GetDownloadedText();

        yield return StartCoroutine(GetComponent<UploadToServer>().DownloadFromServer("imgData"));
        print(GetComponent<UploadToServer>().GetDownloadedText());
        data += GetComponent<UploadToServer>().GetDownloadedText();

        data += "</exportData>";
        File.WriteAllText("C:/Users/Will/Desktop/TEXTFILEEXPORTSERVER.txt", data);
        //Encrypt the data and save it
        using (Aes aes = Aes.Create()) {
            /*aes.Key = System.Text.Encoding.UTF8.GetBytes(GlobalData.encryptionKey);
            aes.BlockSize = 128;
            aes.IV = System.Text.Encoding.UTF8.GetBytes(GlobalData.encryptionIV);
            aes.Padding = PaddingMode.None;
            ICryptoTransform encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
            ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, aes.IV);*/

            //Outputting regular file
            if (!filePath.EndsWith(extensions[0])) {
                filePath += extensions[0];
            }
            //StreamWriter sw = new StreamWriter(filePath, false);
            //sw.WriteLine(data);
            //sw.Close();
            //sw.Dispose();
            File.WriteAllBytes(filePath, EncryptStringToBytes_Aes(data));
        }
    }

    //-------------------------------------------------------------------------------------------------------------------
    //---------------------------------------------------------------------------------------

    void ChooseScenePopup()
    {
        GameObject confirm;
        //If there's a way to tell a user exited from a case, this code *should* work, but case object is null. Will return to
        if (GlobalData.caseObj != null) {
            print("You are from the " + GlobalData.resourcePath);
            GlobalData.caseObj = null;
            if (GlobalData.demo) {
                confirm = GameObject.Find("GaudyBG").transform.Find("ReaderWriterSelector").gameObject;
            } else {
                confirm = GameObject.Find("GaudyBG").transform.Find("LocalOrServerConfirmation").gameObject;
            }

            if (GlobalData.resourcePath.Equals("Writer")) {
                levelName = "Writer";
                GlobalData.resourcePath = "Writer";
                Application.backgroundLoadingPriority = ThreadPriority.Low;
                StartLoading();
                confirm.SetActive(false);
                return;
            } else if (GlobalData.resourcePath.Equals("Reader")) {
                levelName = "CassReader";
                GameObject.Find("GaudyBG").GetComponent<ServerControls>().DisableNewCase();
                //GameObject.Find("GaudyBG").GetComponent<ServerControls>().ChangeEditButtons();
                GlobalData.resourcePath = "Reader";
                Application.backgroundLoadingPriority = ThreadPriority.Low;
                StartLoading();
                return;
            }
        }


        Button serverButton;
        Button localButton;

        if (GlobalData.demo) {
            confirm = GameObject.Find("GaudyBG").transform.Find("ReaderWriterSelector").gameObject;
            serverButton = confirm.transform.Find("CaseWriter").GetComponent<Button>();
            localButton = confirm.transform.Find("CaseReader").GetComponent<Button>();
        } else {
            confirm = GameObject.Find("GaudyBG").transform.Find("LocalOrServerConfirmation").gameObject;
            confirm.transform.Find("ConfirmActionPanel/Content/Row0/ActionValue").GetComponent<Text>().text = "Use Reader or Writer?";
            serverButton = confirm.transform.Find("ConfirmActionPanel/Content/Row1/ServerButton").GetComponent<Button>();
            localButton = confirm.transform.Find("ConfirmActionPanel/Content/Row1/LocalButton").GetComponent<Button>();
            Button bothButton = confirm.transform.Find("ConfirmActionPanel/Content/Row1/BothButton").GetComponent<Button>();
            bothButton.gameObject.SetActive(false);

            serverButton.onClick.RemoveAllListeners();
            localButton.onClick.RemoveAllListeners();
            bothButton.onClick.RemoveAllListeners();

            serverButton.GetComponentInChildren<Text>().text = "Writer";
            localButton.GetComponentInChildren<Text>().text = "Reader";
        }

        serverButton.onClick.AddListener(delegate {
            print("writer");
            confirm.SetActive(false);
            levelName = "Writer";
            if (!GlobalData.demo) {
                serverButton.GetComponentInChildren<Text>().text = "Server";
                localButton.GetComponentInChildren<Text>().text = "Local";
            }
            GlobalData.resourcePath = "Writer";
            Application.backgroundLoadingPriority = ThreadPriority.Low;
            NextFrame.Function(StartLoading);
        });

        localButton.onClick.AddListener(delegate {
            print("reader");
            confirm.SetActive(false);
            levelName = "CassReader";
            if (!GlobalData.demo) {
                serverButton.GetComponentInChildren<Text>().text = "Server";
                localButton.GetComponentInChildren<Text>().text = "Local";
            }
            GameObject.Find("GaudyBG").GetComponent<ServerControls>().DisableNewCase();
            //GameObject.Find("GaudyBG").GetComponent<ServerControls>().ChangeEditButtons();
            GlobalData.resourcePath = "Reader";
            Application.backgroundLoadingPriority = ThreadPriority.Low;
            NextFrame.Function(StartLoading);
        });

        confirm.SetActive(true);
    }

    private void ImageCompressingNewer()
    {
        GameObject image = Instantiate(new GameObject("IMAGE"));
        image.transform.parent = GameObject.Find("Canvas").transform;
        Image i = image.AddComponent<Image>();
        Texture2D newTexture;
        byte[] bytes2;
        //string imageData = Encoding.UTF32.GetString(bytes2);
        foreach (string filename in Directory.GetFiles("C:/Users/Will/Desktop/png test/")) {
            Texture2D texture = new Texture2D(2, 2);
            byte[] bytes = File.ReadAllBytes(filename);
            ImageConversion.LoadImage(texture, bytes);

            //To sprite and back
            Sprite newSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0), 100);
            i.sprite = newSprite;
            if (i.sprite.texture.format == TextureFormat.RGB24) {
                newTexture = new Texture2D(i.sprite.texture.width, i.sprite.texture.height, TextureFormat.RGB24, false);
            } else {
                newTexture = new Texture2D(i.sprite.texture.width, i.sprite.texture.height, TextureFormat.ARGB32, false);
            }
            newTexture.SetPixels(0, 0, i.sprite.texture.width, i.sprite.texture.height, i.sprite.texture.GetPixels());
            newTexture.Apply();

            bytes2 = ImageConversion.EncodeToPNG(newTexture);
            if (filename.EndsWith("jpg")) {
                bytes2 = ImageConversion.EncodeToJPG(newTexture);
            }
            print("Filepath: " + filename);
            print("Width/height: " + newTexture.width + "/" + newTexture.height);
            print("Byte array: " + bytes2.Length);
            print("String length: " + Convert.ToBase64String(bytes2).Length);

            if (newTexture.height > 400) {
                float heightRatio = (float)400.0 / newTexture.height;
                TextureScale.Bilinear(newTexture, (int)Math.Floor(newTexture.width * heightRatio), 400);
            }
            if (newTexture.width > 720) {
                float widthRatio = (float)720.0 / newTexture.width;
                TextureScale.Bilinear(newTexture, 720, (int)Math.Floor(newTexture.height * widthRatio));
            }
            //yield return new WaitForSecondsRealtime(1.0f);
            bytes2 = ImageConversion.EncodeToPNG(newTexture);
            if (filename.EndsWith("jpg")) {
                bytes2 = ImageConversion.EncodeToJPG(newTexture);
            }
            print("--");
            print("New Width/height: " + newTexture.width + "/" + newTexture.height);
            print("Compressed byte array size: " + bytes2.Length);
            print("String length: " + Convert.ToBase64String(bytes2).Length);


            Texture2D temp2 = new Texture2D(2, 2);
            temp2.LoadImage(Convert.FromBase64String(Convert.ToBase64String(bytes2)));
            Sprite newSprite3 = Sprite.Create(temp2, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0, 0), 100);
            i.sprite = newSprite3;

            print("format: " + i.sprite.texture.format);
            /*print(Encoding.ASCII.GetString(bytes2).Length);
            print(Encoding.Unicode.GetString(bytes2).Length);
            print(Encoding.UTF7.GetString(bytes2).Length);
            print(Encoding.UTF8.GetString(bytes2).Length);
            print(Encoding.UTF32.GetString(bytes2).Length);*/
            print("------------");
            /*print(Convert.FromBase64String(Convert.ToBase64String(bytes2)).Length);
            print(Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(bytes2)).Length);
            print(Encoding.Unicode.GetBytes(Encoding.Unicode.GetString(bytes2)).Length);
            print(Encoding.UTF7.GetBytes(Encoding.UTF7.GetString(bytes2)).Length);
            print(Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(bytes2)).Length);
            print(Encoding.UTF32.GetBytes(Encoding.UTF32.GetString(bytes2)).Length);*/


        }
    }

    private void TestImageCompression()
    {
        Sprite sprite = GameObject.Find("BigLoginPanel/Mushroom").GetComponent<Image>().sprite;
        Texture texture = GameObject.Find("BigLoginPanel/Mushroom").GetComponent<Image>().mainTexture;

        //Encrypt
        Texture2D newTexture = new Texture2D(sprite.texture.width, sprite.texture.height, TextureFormat.ARGB32, false);

        //newTexture.SetPixels(0, 0, sprite.texture.width, sprite.texture.height, sprite.texture.GetPixels());
        //---------
        RenderTexture toBeActive = new RenderTexture(texture.width, texture.height, 0);
        Graphics.Blit(texture, toBeActive);
        RenderTexture.active = toBeActive;
        newTexture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);

        newTexture.Apply();
        byte[] bytes = newTexture.EncodeToPNG();
        string imageData = Convert.ToBase64String(bytes);

        //Decrypt
        Texture2D temp = new Texture2D(2, 2);
        temp.LoadImage(Convert.FromBase64String(imageData));
        Sprite newSprite = Sprite.Create(temp, new Rect(0, 0, sprite.texture.width / 2, sprite.texture.height / 2), new Vector2(0, 0), 1);

        testImage.sprite = newSprite;

        /*
        Texture newTexture = transform.parent.Find("BigLoginPanel/Mushroom").GetComponent<Image>().mainTexture;
        Texture2D nt2 = new Texture2D(256, 256, TextureFormat.ARGB32, false);
        RenderTexture.active = newTexture as RenderTexture;
        nt2.ReadPixels(new Rect(0, 0, newTexture.width, newTexture.height), 0, 0);
        nt2.Apply();

        byte[] bytes = nt2.EncodeToPNG();
        string imageData = Convert.ToBase64String(bytes);
        print(imageData.Length + ", " + imageData);

        Texture2D temp = new Texture2D(2, 2);
        temp.LoadImage(Convert.FromBase64String(imageData));

        testImage.sprite = Sprite.Create(temp, new Rect(0, 0, 256, 256), new Vector2(0, 0), 1);
        //testImage.sprite = Sprite.Create(nt2, new Rect(0, 0, 256, 256), new Vector2(0, 0), 1);
        */
    }

    public IEnumerator TestImageCompression2()
    {
        Camera cam = GameObject.Find("CameraTest").GetComponent<Camera>();
        RenderTexture rtBig = cam.targetTexture;
        RenderTexture rtSmall = Resources.Load("CharacterPortraitSmall") as RenderTexture;

        print(cam.targetTexture.width);
        print(rtBig.width);
        cam.targetTexture = rtSmall;
        yield return new WaitForSecondsRealtime(5f);
        print(cam.targetTexture.width);
        cam.targetTexture = rtBig;
    }

    private void TestEncryption3()
    {
        byte[] encrypted;
        string plainText = "And this is going to be another longer message that should hopefully use a few blocks and requrie padding.123456";
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
            File.WriteAllBytes(Application.persistentDataPath + "\\LocalSaves\\" + "TestCaseSave.ced", encrypted);

            string text = "";
            ICryptoTransform decrypt = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using (FileStream fs = new FileStream(Application.persistentDataPath + "\\LocalSaves\\" + "TestCaseSave.ced", FileMode.Open)) {
                using (CryptoStream cs = new CryptoStream(fs, decrypt, CryptoStreamMode.Read)) {
                    using (StreamReader sr = new StreamReader(cs)) {
                        text = sr.ReadToEnd();
                    }
                }
            }
            print(text);
        }
    }

    private void TestEncryption1()
    {
        try {

            string textToEncrypt = "This is going to be a really long message that I will use for encryption. It should hopefully take around 3 or more blocks" +
                    "of encrypting to actually work.";


            // Encrypt the string to an array of bytes.
            byte[] encrypted = EncryptStringToBytes_Aes(textToEncrypt);

            byte[] encryptedCopy = new byte[encrypted.Length];
            for (int i = 0; i < encrypted.Length - 1; i++) {
                encryptedCopy[i] = encrypted[i];
            }

            print(string.Join("", encrypted));
            string testEncryptedString = BitConverter.ToString(encrypted).Replace("-", "");
            print(testEncryptedString);
            print(string.Join("", GetBytesFromHex(testEncryptedString)));


            //encrypted = Encoding.UTF8.GetBytes(testEncryptedString);
            // Decrypt the bytes to a string.
            string roundtrip = DecryptStringFromBytes_Aes(encrypted);

            //Display the original data and the decrypted data.
            print(textToEncrypt);
            print(roundtrip);
            Console.WriteLine("Original:   {0}", textToEncrypt);
            Console.WriteLine("Round Trip: {0}", roundtrip);

        } catch (Exception e) {
            Console.WriteLine("Error: {0}", e.Message);
        }

        return;
    }
    private void TestEncryption2()
    {
        using (Aes aes = Aes.Create()) {
            print(System.Text.Encoding.UTF8.GetBytes(GlobalData.encryptionKey).Length);
            foreach (KeySizes key in aes.LegalKeySizes) {
                print(key.ToString() + ", " + key.MinSize + ", " + key.MaxSize);
            }
            aes.Key = System.Text.Encoding.UTF8.GetBytes(GlobalData.encryptionKey);// GlobalData.encryptionKey);
            aes.BlockSize = 128;
            aes.GenerateIV();
            print(aes.IV.Length + ", " + aes.IV.ToString());
            print(System.Text.Encoding.UTF8.GetBytes(GlobalData.encryptionIV).Length);
            aes.IV = System.Text.Encoding.UTF8.GetBytes(GlobalData.encryptionIV);// GlobalData.encryptionIV);
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform encrypt = aes.CreateEncryptor(aes.Key, aes.IV);

            string textToEncrypt = "This is going to be a really long message that I will use for encryption. It should hopefully take around 3 or more blocks" +
                "of encrypting to actually work.";



            //Encrypt all but last block
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
            StringBuilder encryptedString = new StringBuilder();
            byte[] encryptedBytes = new byte[encrypt.OutputBlockSize];
            List<byte> byteList = new List<byte>();
            for (int i = 0; i < bytes.Length / encrypt.OutputBlockSize; i++) {
                print(i * encrypt.InputBlockSize);
                encrypt.TransformBlock(bytes, i * encrypt.InputBlockSize, encrypt.InputBlockSize, encryptedBytes, 0);
                encryptedString.Append(System.Text.Encoding.UTF8.GetString(encryptedBytes));
                print(System.Text.Encoding.UTF8.GetString(encryptedBytes));

                byteList.AddRange(encryptedBytes);

                encryptedBytes = new byte[encrypt.OutputBlockSize];
            }







            //Setup last block to be encrypted
            byte[] lastBlock = new byte[encrypt.OutputBlockSize];
            int lastBlockStart = ((bytes.Length / encrypt.OutputBlockSize) - 1) * encrypt.OutputBlockSize;
            int lastBlockLen = bytes.Length - lastBlockStart;
            for (int i = 0; i < lastBlockLen; i++) {
                lastBlock[i] = bytes[i + lastBlockStart];
            }
            for (int i = lastBlockLen; i < encrypt.OutputBlockSize; i++) {
                lastBlock[i] = 0;
            }



            encryptedString.Append(System.Text.Encoding.UTF8.GetString(encrypt.TransformFinalBlock(lastBlock, 0, lastBlockLen)));
            byteList.AddRange(encrypt.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length));
            print("Initial string to hex: " + BitConverter.ToString(bytes).Replace("-", ""));
            print("Pure encrypted byte array to hex: " + BitConverter.ToString(byteList.ToArray()).Replace("-", ""));
            print("Encrypted bytes to string: " + encryptedString.ToString());




            ICryptoTransform decrypt = aes.CreateDecryptor(aes.Key, aes.IV);
            bytes = byteList.ToArray();
            StringBuilder decryptedString = new StringBuilder();
            byte[] decryptedBytes = new byte[decrypt.OutputBlockSize];
            byteList = new List<byte>();

            for (int i = 0; i < bytes.Length / decrypt.OutputBlockSize - 1; i++) {
                print(i * decrypt.InputBlockSize);
                decrypt.TransformBlock(bytes, i * decrypt.InputBlockSize, decrypt.InputBlockSize, decryptedBytes, 0);
                decryptedString.Append(System.Text.Encoding.UTF8.GetString(decryptedBytes));
                print(BitConverter.ToString(decryptedBytes).Replace("-", ""));
                print(System.Text.Encoding.UTF8.GetString(decryptedBytes));

                byteList.AddRange(decryptedBytes);

                decryptedBytes = new byte[decrypt.OutputBlockSize];
            }

            //decryptedString.Append(System.Text.Encoding.UTF8.GetString(decrypt.TransformFinalBlock(bytes, 0, 16)));
            decryptedString.Append(System.Text.Encoding.UTF8.GetString(decrypt.TransformFinalBlock(bytes, decrypt.InputBlockSize * (bytes.Length / decrypt.InputBlockSize - 1), decrypt.InputBlockSize)));
            byteList.AddRange(decrypt.TransformFinalBlock(bytes, decrypt.InputBlockSize * (bytes.Length / decrypt.InputBlockSize - 1), decrypt.InputBlockSize));
            print("Decrypted string: " + decryptedString.ToString());
            print("Decrypted byte list to string: " + System.Text.Encoding.UTF8.GetString(byteList.ToArray()));
        }
    }
}
