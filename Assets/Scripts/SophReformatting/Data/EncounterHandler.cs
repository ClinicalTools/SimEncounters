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
        protected virtual UploadToServer ServerUploader { get; set; }
        protected virtual EncounterData EncounterData { get; set; }

        private Dictionary<string, string> dialogueDict = new Dictionary<string, string>();
        private Dictionary<string, string> quizDict = new Dictionary<string, string>();
        private Dictionary<string, string> flagDict = new Dictionary<string, string>();
        private Dictionary<string, string> eventDict = new Dictionary<string, string>();

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
            var encounterXml = globalData.EncounterXml;

            // TODO: move the wait to where most accurate
            var ced = encounterXml.CurrentEncounterCed;
            var cei = encounterXml.CurrentEncounterCei;

            // If there is no case xml loaded, default encounter information should be used
            // This should only be hit if someone didn't go through the main menu
            if (ced == null || cei == null) {
                yield return ShowDefaultEncounter();
            } else {
                while (!ced.IsCompleted || !cei.IsCompleted)
                    yield return null;

                EncounterData = new EncounterData(ced.Result, cei.Result);
            }

            //transform.GetComponent<ReaderTabManager>().FirstTimeLoad();
            transform.GetComponent<TabManager>().FirstTimeLoad();

            HideLoadingScreen();


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

        public abstract IEnumerator ShowDefaultEncounter();



        protected virtual void StartLoadingScreen()
        {
            if (GlobalData.showLoading) {
                LoadingScreen = GameObject.Find("LoadingScreenNew").GetComponent<CanvasGroup>();
                if (LoadingScreen == null) {
                    LoadingScreen = LoadingScreenManager.Instance.GetComponent<CanvasGroup>();
                }
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

            //! WRITER: get keys
            //! WRITER: set section button/filler as siblings

            return;
        }

        [SerializeField]
        protected Sprite defaultIcon;
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

            //! WRITER: set SectionDisplayTMP to active

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

        protected abstract void UpdatePatientImage(SpriteHolderScript imgData, Image img, GameObject sectionBtn);
        /*{
            img.sprite = null;
            if (imgData == null) { //Load default image if it's not found in the dictionary
                var bg = GameObject.Find("GaudyBG").transform;
                var iconHolder = bg.Find("SectionCreatorBG/SectionCreatorPanel/Content/ScrollView/Viewport/Content/IconPanel1/Icon");
                defaultIcon = iconHolder.GetComponent<Image>().sprite;

                img.sprite = defaultIcon;
                return;
            }


            if (imgData.referenceName != null && !imgData.referenceName.Equals("")) {
                img.sprite = imgData.iconHolder.transform.Find(
                    imgData.referenceName + "/Icon")
                    .GetComponent<Image>().sprite;
            } else {
                img.sprite = imgData.sprite;
            }

            if (imgData.useColor) {
                sectionBtn.GetComponent<Image>().color = imgData.color;
            }
        }*/

        #region TEMP
        // This stuff will use the data classes more directly going forward

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

        #endregion

    }
}