using ClinicalTools.SimEncounters;
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


/**
 * Comments that start with "//!" should be replaced before this code is used given to anyone outside of me
 * "//! READER:" indicates code that only occurred in the reader and is matched with a comment in ReaderDataScript.cs
 * "//! WRITER:" indicates code that only occurred in the writer and is matched with a comment in DataScript.cs
 * "//! TODO:" indicates a coding task that needs to be completed 
 * "//! DIFF:" indicates an area of the code that does the same thing but in a different way depending on whether it's in the Reader or Writer.
 */

namespace ClinicalTools.SimEncountersOld
{
    public class SophData : MonoBehaviour
    {
        // There should only ever be one data script in the scene, so it's safe to use a static instance
        public static SophData Instance { get; private set; }

        private OldSectionCollection sectionData;
        private ImgCollection imgData;
        // These need to not be static so their methods can be overridden
        // private CondData condData;
        // private VarData varData;

        private Dictionary<string, SectionDataScript> Dict = new Dictionary<string, SectionDataScript>();     //A dictionary of Sections. Key=sectionName. Value=sectionData (all tab info)
        private Dictionary<string, SpriteHolderScript> imgDict = new Dictionary<string, SpriteHolderScript>(); //A dictionary of any and all images. Key=Section(.Tab(.Entry name))

        private Dictionary<string, string> dialogueDict = new Dictionary<string, string>();
        private Dictionary<string, string> quizDict = new Dictionary<string, string>();
        private Dictionary<string, string> flagDict = new Dictionary<string, string>();
        private Dictionary<string, string> eventDict = new Dictionary<string, string>();
        public XmlDocument xmlDoc;                          //Holds data from the XML file. Easy to parse.
        private string path = "";                           //Default XML path
        private string fileName = "";                       //Chosen filename for current XML file.
        private bool overwrite = false;                     //Whether or not to load or overwrite the file
        public GameObject SectionButtonPar;                 //The parent gameobject for section buttons
        public string[] nonSectionDataPanels;               //Names of the panels whose data must be saved yet are not a part of any section
        public Dictionary<string, string> correctlyOrderedDialogues = new Dictionary<string, string>();
        public Dictionary<string, string> correctlyOrderedQuizes = new Dictionary<string, string>();
        public Dictionary<string, string> correctlyOrderedFlags = new Dictionary<string, string>();
        public List<Transform> newTabs;                     //
        public Transform characterPanel;                    //

        public UploadToServer ServerUploader;
        public CanvasGroup loadingScreen;

        #region Variables only in reader
        public bool readerOnlyBuild;

        #endregion

        #region Variables only in writer
        #endregion

        #region Functions only in reader
        public void GetDemoCase() { }
        #endregion

        #region Functions only in writer
        #endregion



        // May make this not a monobehavior and use a real initializer
        // Keeping it as a monobehavior leaves a bit less to change and requires this to be inhereted from, rather than used as an instance
        // But I generally don't feel that "data" should be tied to a particular gameobject, although I suppose it inherently has to in some way
        // The methods of a monobehavior, in my opinion, should serve as a link between the codebase and the scene
        // Monobehavior also means that more things can be set in the editor if relevant
        private void Awake()
        {
            //! why is this in awake and the other stuff in start
            if (GlobalData.showLoading) {
                loadingScreen = GameObject.Find("LoadingScreenNew").GetComponent<CanvasGroup>();
                if (loadingScreen == null) {
                    loadingScreen = LoadingScreenManager.Instance.GetComponent<CanvasGroup>();
                }
            } else if (loadingScreen != null && GlobalData.showLoading) {
                loadingScreen.gameObject.SetActive(true);
            } else if (loadingScreen == null) {
                //! This line was only in the writer's start function, so I'm unsure whether it's used
                //! TODO: Debug to see if this line is ever used

                loadingScreen = LoadingScreenManager.Instance.GetComponent<CanvasGroup>();
            }
        }

        private IEnumerator Start()
        {
            //! READER: potentially get demo case

            path = GlobalData.filePath;
            fileName = GlobalData.fileName;

            //! READER: check absolute url

            transform.gameObject.AddComponent<UploadToServer>();
            ServerUploader = transform.GetComponent<UploadToServer>();

            //! WRITER: check if new case

            //! WRITER: set loading screen if it's not a new case and download

            //! READER: download data if not disabled



            //! WRITER: handle loading autosaves

            //! WRITER: load tab manager

            //! WRITER: guests can't show a case in the reader from the writer??????

            //! WRITER: set up initial handling of patient name info

            //! WRITER: handle template

            //! WRITER: handle saving autosaves

            //! WRITER: handle load into writer button

            //! READER: load case

            //! READER: set patient information

            if (loadingScreen != null && GlobalData.showLoading) {
                loadingScreen.blocksRaycasts = false;
                LoadingScreenManager.Instance.Fade();
            }

            //! READER: Refresh case overview

            yield return null;
        }


        /**
         * Loads images from the images XML file
         */
        public void LoadImages()
        {
            //! DIFF: get filename for images file


            xmlDoc = new XmlDocument();

            //! DIFF: load xml

            VarData.Reset();
            CondData.Reset();


            XmlNode node = xmlDoc.FirstChild;
            while (node != null) {
                //! TODO: use a better way of getting xml data. May be slightly slower but more flexible
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

            // Unique to clinical encountners
            if (GetImage(GlobalData.patientImageID) != null) {
                GameObject.Find("CharacterCamera").transform.Find("Canvas/Image").GetComponent<Image>().sprite = GetImage(GlobalData.patientImageID).sprite;
                GameObject.Find("CharacterCamera").transform.Find("Canvas").gameObject.SetActive(true);
            }
            //Debug.Log (string.Join (",", imgDict.Keys.ToArray ()));
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
                //! DIFF
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
            return null;//sectionData.GetSectionsList();
        }

        // TODO: This method is 500 lines long and I don't want to have to deal with that kind of negativity right now
        public IEnumerator PopulateDict()
        {
            if (fileName == null) {
                yield break;
            }
            //Finds the file and loads data into xmlDoc
            xmlDoc = new XmlDocument();

            //! DIFF: load xml


            //Loads the data into the Dictionary variable
            XmlNode node = xmlDoc.FirstChild;
            node = xmlDoc.AdvNode(node);
            string sectionName = null;
            
            SectionDataScript xmlDict = new SectionDataScript();
            xmlDict.Initiate();
            xmlDict.SetPosition(0);
            bool inSections = false;


            while (!inSections) { //Load in any panel's data that is not in a section
                print(node.Name + ", " + node.Value + ", " + node.OuterXml);
                Transform[] children = null;

                XmlDocument element = new XmlDocument();

                //! TODO: check node name

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
                        //! DIFF: image 
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

                        //! DIFF: slider
                    } else if (child.gameObject.GetComponent<Slider>() != null) {
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

                        print(node.Name + ", " + node.Value + ", " + node.OuterXml);
                        break;
                    }
                    node = xmlDoc.AdvNode(node);
                }
            }


            node = xmlDoc.AdvNode(node); //Go inside sections

            //! READER: tracking data


            while (node != null) {
                if (node.Name.ToLower().EndsWith("section")) {
                    //sectionData.ReadSection(node);
                }

                node = node.NextSibling;
            }


            //If there were no sections in the file then loading is done. Return.
            if (sectionName == null) {
                yield break;
            }

            //! READER: tracking data

            if (SectionButtonPar == null)
                yield break;

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
            foreach (string key in GetSectionsList()) {
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

                //! READER: tabs visitied
                //! READER: force visited

                //! WRITER: set SectionDisplayTMP to active
            }

            //! READER: step selection title

            //! WRITER: get keys
            //! WRITER: set section button/filler as siblings

            //! TODO: complete the rest of this
        }

        /**
         * Load in the default section buttons
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
                throw new Exception("Cannot add two duplicate sections!");
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
            string newName = "";
            foreach (char c in name) {
                string character = c + "";
                if (!Regex.IsMatch(character, "[a-zA-Z0-9_ ]")) {
                    int a = Convert.ToInt32(c);
                    if (a > 255) {
                        continue;
                    }
                    string hex = "." + string.Format("{0:X}", a) + ".";
                    newName += hex;
                } else {
                    newName += c;
                }
            }

            return "_" + newName;
        }


        /**
         * Convert's a section's formatted XML name into regular text
         */
        public string ConvertNameFromXML(string name)
        {
            if (name.StartsWith("_")) {
                name = name.Substring(1); //Remove starting underscore
            }
            for (int pos = 0; pos < name.Length; pos++) {
                string currentChar = name.ToCharArray()[pos] + "";
                if (currentChar.Equals(".")) {
                    string character = char.ConvertFromUtf32(Convert.ToInt32(name.Substring(pos + 1, 2), 16));
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
            if (Section == null || Section.Equals("")) {
                return new SectionDataScript();
            }
            if (Dict.ContainsKey(Section)) {
                return Dict[Section];
            } else {
                print("No section found with the given section name");
                return new SectionDataScript();
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
         * 
         * why isn't this writer only
         */
        public void EditTab(string oldName, string newName)
        {
            string Section = transform.GetComponent<TabManager>().GetCurrentSectionKey();
            Debug.Log("SWAPPING TEXT NAME. OLDNAME: " + oldName + ", NEWNAME: " + newName);
            if (Dict[Section].ContainsKey(oldName)) {
                Dict[Section].Replace(oldName, newName);

                TabManager tm = GetComponent<TabManager>();
                tm.setTabName(newName);

                foreach (string imgName in imgDict.Keys.ToArray()) {
                    string[] splitKey = imgName.Split('.');
                    if (splitKey.Count() >= 3) {
                        if (splitKey[0].Equals(tm.GetCurrentSectionKey())) {
                            if (splitKey[1].Equals(oldName + "Tab")) {
                                string newImgName = imgName.Replace(splitKey[1], newName + "Tab");
                                imgDict.Add(newImgName, imgDict[imgName]);
                                imgDict.Remove(imgName);
                            }
                        }
                    }
                }

                Dictionary<string, string> tempDict = dialogueDict;
                foreach (string key in tempDict.Keys.ToList()) {
                    if (key.StartsWith(tm.GetCurrentSectionKey() + "/" + oldName + "Tab")) {
                        string newDialogue = dialogueDict[key].Replace(oldName + "Tab", newName + "Tab");
                        dialogueDict.Remove(key);
                        dialogueDict.Add(key.Replace(oldName + "Tab", newName + "Tab"), newDialogue);
                    }
                }

                tempDict = quizDict;
                foreach (string key in tempDict.Keys.ToList()) {
                    if (key.StartsWith(tm.GetCurrentSectionKey() + "/" + oldName + "Tab")) {
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
                            Dict[linkToName].AddData(k, temp.GetData(k).Replace(oldName, linkToName));
                        }
                    }


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
            Dict[transform.GetComponent<TabManager>().GetCurrentSectionKey()].Remove(Tab);
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

            return true;
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


        //Used for testing
        public void buttonprintdata()
        {
            Debug.Log(GetData());
            Debug.Log(GetImagesXML());
            Debug.Log(GetData(transform.GetComponent<TabManager>().GetCurrentSectionKey()).GetAllPositions());
        }

        public void Reload()
        {
            ClearAllData();
            Awake();
            StartCoroutine(Start());
        }

    }
}