using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncounters
{
    public class WriterHandler : EncounterHandler
    {
        public static WriterHandler WriterInstance => (WriterHandler)Instance;

        // yeet this
        public virtual List<Transform> newTabs { get; set; } = new List<Transform>();

        protected override IEnumerator Start()
        {
            yield return base.Start();

            // TODO: Remove globaldata
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

            StartAutosave();
        }

        // TODO: Move this into saving script
        //! WRITER ONLY METHOD
        public virtual void RestartAutosave()
        {
            CancelInvoke("Autosave");
            StartAutosave();
        }

        protected virtual void StartAutosave()
        {
            int minutesBetweenSaves = GlobalData.autosaveRate; 
            if (GlobalData.caseObj != null && minutesBetweenSaves > 0 && GlobalData.enableAutoSave) {
                print("Autosaving started. Saving every " + minutesBetweenSaves + " minutes.");
                InvokeRepeating(nameof(Autosave), minutesBetweenSaves * 60, minutesBetweenSaves * 60);
            }
        }


        protected virtual void Autosave()
        {
            Debug.Log("Autosaving...");
            ShowMessage("Autosaving...", false);
            transform.Find("SaveCaseBG").GetComponent<SubmitToXML>().Autosave();
        }

        protected override void ProcessSections(OldSectionCollection sections, ImgCollection imageCollection)
        {
            base.ProcessSections(sections, imageCollection);
            // TODO: default section

            SectionButtonPar.transform.Find("AddSectionButton").SetAsLastSibling();
            SectionButtonPar.transform.Find("Filler").SetAsLastSibling();
        }

        protected override GameObject CreateSectionButton(string key)
        {
            var newSectionBtn = base.CreateSectionButton(key);

            // TODO: not entirely sure what this is doing
            Transform[] components = newSectionBtn.GetComponentsInChildren<Transform>();
            foreach (Transform c in components) {
                if (!c.name.Equals(newSectionBtn.name) && !c.name.Equals("SectionDisplayTMP")) {
                    c.gameObject.SetActive(false);
                }
            }
            newSectionBtn.transform.GetChild(0).gameObject.SetActive(true);

            return newSectionBtn;
        }


        public override void GetDefaultEncounter(EncounterXml encounterXml)
        {
            EncounterData = new OldEncounterData();
        }

        protected override void UpdatePatientImage(SpriteHolderScript imgData, Image img, GameObject sectionBtn)
        {
            img.sprite = null;
            if (imgData == null) { //Load default image if it's not found in the dictionary
                var bg = GameObject.Find("GaudyBG").transform;
                var iconHolder = bg.Find("SectionCreatorBG/SectionCreatorPanel/Content/ScrollView/Viewport/Content/IconPanel1/Icon");
                img.sprite = iconHolder.GetComponent<Image>().sprite;

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
        }



        /**
         * Method to check if the provided name has any special characters that XML wouldn't like
         */
         // TODO: Just sandbox these values insteadre
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
    }
}