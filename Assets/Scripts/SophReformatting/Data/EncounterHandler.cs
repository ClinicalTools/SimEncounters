using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SimEncounters
{
    public abstract class EncounterHandler : MonoBehaviour
    {
        public static EncounterHandler Instance { get; protected set; }

        [SerializeField]
        protected CanvasGroup loadingScreen;
        // Make serializable
        public virtual UploadToServer ServerUploader { get; set; }
        public virtual EncounterData EncounterData { get; protected set; }

        // These need to be yeeted
        public virtual Dictionary<string, string> DialogueDict { get; } = new Dictionary<string, string>();
        public virtual Dictionary<string, string> QuizDict { get; } = new Dictionary<string, string>();
        public virtual Dictionary<string, string> FlagDict { get; } = new Dictionary<string, string>();
        public virtual Dictionary<string, string> EventDict { get; } = new Dictionary<string, string>();
        public virtual Dictionary<string, string> CorrectlyOrderedDialogues { get; } = new Dictionary<string, string>();
        public virtual Dictionary<string, string> CorrectlyOrderedQuizes { get; } = new Dictionary<string, string>();
        public virtual Dictionary<string, string> CorrectlyOrderedFlags { get; } = new Dictionary<string, string>();


        //The parent gameobject for section buttons
        public GameObject SectionButtonPar;

        protected virtual CanvasGroup LoadingScreen {
            get { return loadingScreen; }
            set { loadingScreen = value; }
        }

        protected virtual void Awake()
        {
            Instance = this;

            StartLoadingScreen();

            ServerUploader = transform.GetComponent<UploadToServer>();
        }


        protected virtual IEnumerator Start()
        {
            // UploadToServer should already be on the object
            var globalData = SophGlobalData.Instance;
            yield return GetEncounter(globalData.EncounterXml);


            //transform.GetComponent<ReaderTabManager>().FirstTimeLoad();
            transform.GetComponent<TabManager>().FirstTimeLoad();

            HideLoadingScreen();
            ProcessSections(EncounterData.Sections, EncounterData.Images);

            #region TEMP
            // This stuff will be moved out to CE specific files
            var patientImg = EncounterData.Images[GlobalData.patientImageID];
            if (patientImg != null) {
                var charCamCanvas = GameObject.Find("CharacterCamera").transform.Find("Canvas");
                charCamCanvas.Find("Image").GetComponent<Image>().sprite = patientImg.sprite;
                charCamCanvas.gameObject.SetActive(true);
            }


            #endregion

            yield return null;
        }

        public abstract void GetDefaultEncounter(EncounterXml encounterXml);


        protected virtual IEnumerator GetEncounter(EncounterXml encounterXml)
        {
            var ced = encounterXml.CurrentEncounterCed;
            var cei = encounterXml.CurrentEncounterCei;

            // If there is no case xml loaded, default encounter information should be used
            // This should only be hit if someone didn't go through the main menu
            if (ced == null || cei == null)
                GetDefaultEncounter(encounterXml);

            // Default encounter can sometimes set encounter data and it doesn't need to be read in
            if (EncounterData == null) {
                while (!ced.IsCompleted || !cei.IsCompleted)
                    yield return null;

                EncounterData = new EncounterData(ced.Result, cei.Result);
            }
        }

        protected virtual void StartLoadingScreen()
        {
            if (GlobalData.showLoading) {
                LoadingScreen = GameObject.Find("LoadingScreenNew").GetComponent<CanvasGroup>();
                if (LoadingScreen == null)
                    LoadingScreen = LoadingScreenManager.Instance.GetComponent<CanvasGroup>();
            } else if (LoadingScreen != null && GlobalData.showLoading) {
                LoadingScreen.gameObject.SetActive(true);
            } else if (LoadingScreen == null) {
                //! This line was only in the writer's start function, so I'm unsure whether it's used
                //! TODO: Debug to see if this line is ever used
                Debug.LogError("the line is used");
                LoadingScreen = LoadingScreenManager.Instance.GetComponent<CanvasGroup>();
            }
        }

        public virtual void HideLoadingScreen()
        {
            if (loadingScreen != null) {
                loadingScreen.blocksRaycasts = false;
                LoadingScreenManager.Instance.Fade();
            }
        }

        protected virtual void ProcessSections(SectionCollection sections, ImgCollection imageCollection)
        {
            //Load in the section buttons

            //Spawn the section buttons
            foreach (var key in sections.Keys) {
                ProcessSection(key, imageCollection);
            }

            //! READER: step selection title

            return;
        }

        protected virtual void ProcessSection(string key, ImgCollection imageCollection)
        {
            GameObject newSectionBtn = CreateSectionButton(key);

            if (imageCollection.Count == 0)
                return;

            Image[] images = newSectionBtn.GetComponentsInChildren<Image>();
            foreach (Image img in images) {
                if (!img.transform.name.Equals("Image"))
                    continue;

                var imgData = imageCollection[key];
                UpdatePatientImage(imgData, img, newSectionBtn);
            }

            //! READER: tabs visitied
            //! READER: force visited

            return;
        }

        protected virtual GameObject CreateSectionButton(string key)
        {
            GameObject newSection = Resources.Load(
                    GlobalData.resourcePath + "/Prefabs/SectionButton") as GameObject;

            newSection = Instantiate(newSection, SectionButtonPar.transform);
            newSection.name = GetSectionButtonName(newSection, key);

            newSection.transform.SetAsLastSibling();

            return newSection;
        }

        protected virtual string GetSectionButtonName(GameObject sectionButton, string key)
        {
            TextMeshProUGUI[] labels = sectionButton.GetComponentsInChildren<TextMeshProUGUI>(true);
            string buttonName = null;
            foreach (TextMeshProUGUI label in labels) {
                if (label.name.Equals("SectionLinkToText")) { //Where the button links to
                    label.text = key;
                } else if (label.name.Equals("SectionDisplayTMP")) { //What the button displays
                    label.text = key.Replace('_', ' ')
                                    .Substring(0, key.Length - "Section".Length);
                    buttonName = label.text.Replace(" ", "_") + "SectionButton";
                }
            }

            return buttonName;
        }


        private GameObject notification;
        private bool fade;
        /**
         * Use this to show a confirmation that the case was saved successfully
         */
        public void ShowMessage(string message, bool error = false)
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

        protected abstract void UpdatePatientImage(SpriteHolderScript imgData, Image img, GameObject sectionBtn);

        #region TEMP
        // This stuff will use the data classes more directly going forward

        /**
         * Returns the dictionary of dialogues
         */
        public Dictionary<string, string> GetDialogues()
        {
            return DialogueDict;
        }

        /**
         * Returns the dictionary of flags
         */
        public Dictionary<string, string> GetFlags()
        {
            return FlagDict;
        }

        /**
         * Returns the dictionary of events
         */
        public Dictionary<string, string> GetEvents()
        {
            return EventDict;
        }

        /**
         * Add a dialogue to the dictionary
         */
        public void AddDialogue(string key, string data)
        {
            if (DialogueDict.ContainsKey(key)) {
                DialogueDict[key] = data;
            } else {
                DialogueDict.Add(key, data);
            }
        }

        /**
        * Add a flag to the dictionary
        */
        public void AddFlag(string key, string data)
        {
            if (FlagDict.ContainsKey(key)) {
                FlagDict[key] = data;
            } else {
                FlagDict.Add(key, data);
            }
        }

        public string GetAllQuizData()
        {
            return string.Join("", QuizDict.Select(x => x.Value).ToArray());
        }

        public string GetQuizData(string uniquePath)
        {
            if (QuizDict.ContainsKey(uniquePath)) {
                return QuizDict[uniquePath];
            } else {
                return "<data></data>";
            }
        }

        public Dictionary<string, string> GetQuizes()
        {
            return QuizDict;
        }

        public void AddQuiz(string key, string data)
        {
            if (QuizDict.ContainsKey(key)) {
                QuizDict[key] = data;
            } else {
                QuizDict.Add(key, data);
            }
        }

        #endregion


        /**
         * Adds or updates data for a specified Tab
         * Tab = TabName
         */
        public void NewSectionWithTab(string tabName, string data)
        {
            SectionDataScript section = new SectionDataScript();
            section.AddData(tabName, data);
            EncounterData.Sections.Add(section);
        }

        public void NewSectionWithTab(string key, string tabName, string data)
        {
            var section = EncounterData.Sections[key];
            section.AddData(tabName, data);
        }

        /**
         * Adds or updates data for a specified Tab with customName provided
         */
        public void AddSectionTabData(string tabName, string customName, string data)
        {
            SectionDataScript section = new SectionDataScript();
            section.AddData(tabName, customName, data);
            EncounterData.Sections.Add(section);
        }

        public void AddSectionTabData(string key, string tabName, string customName, string data)
        {
            var section = EncounterData.Sections[key];
            section.AddData(tabName, customName, data);
        }

        /**
         * Returns the data of a specified Tab as a long string
         */
        public string GetTabData(string sectionKey, string tabName)
        {
            var section = EncounterData.Sections[sectionKey];
            if (section == null || !section.ContainsKey(tabName)) {
                return null;
            }
            return section.GetData(tabName);
        }


        /**
         * Adds an image by image key and the name of the image file (a reference to images already saved)
         */
        public string AddImg(string key, string imgRefName)
        {
            if (EncounterData.Images.ContainsKey(key)) {
                EncounterData.Images[key].referenceName = imgRefName;
                return key;
            } else {
                return EncounterData.Images.Add(new SpriteHolderScript(imgRefName));
            }
        }

        public string GetXml()
        {
            return EncounterData.GetXml().OuterXml;
        }
        public string GetImagesXml()
        {
            return EncounterData.GetImageXml().OuterXml;
        }
    }
}