using System.Collections;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

namespace SimEncounters
{
    public class WriterHandler : EncounterHandler
    {
        public static WriterHandler WriterInstance => (WriterHandler)Instance;

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

            // TODO: Move this into saving script
            int minutesBetweenSaves = GlobalData.autosaveRate;
            //If the user is logged in/came from the main menu, they'll have a case object
            if (GlobalData.caseObj != null && minutesBetweenSaves > 0 && GlobalData.enableAutoSave) {
                print("Autosaving started. Saving every " + minutesBetweenSaves + " minutes.");
                InvokeRepeating("Autosaving", minutesBetweenSaves * 60, minutesBetweenSaves * 60);
            }
        }
        
        // TODO: Move this into saving script
        //! WRITER ONLY METHOD
        public void RestartAutosave()
        {
            int minutesBetweenSaves = GlobalData.autosaveRate;
            CancelInvoke("Autosaving");
            InvokeRepeating("Autosaving", minutesBetweenSaves * 60, minutesBetweenSaves * 60);
            print("Restarted autosaving every " + minutesBetweenSaves + " minutes");
        }


        public override IEnumerator ShowDefaultEncounter()
        {
            EncounterData = new EncounterData();

            yield return null;
        }

        protected override void UpdatePatientImage(SpriteHolderScript imgData, Image img, GameObject sectionBtn)
        {
            throw new System.NotImplementedException();
        }
    }
}