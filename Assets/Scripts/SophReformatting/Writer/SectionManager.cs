using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimEncounters
{
    public class SectionManager : MonoBehaviour
    {
        public EncounterData Encounter { get; protected set; }


        // I hate that I can't easily force this to be called
        public void Initiate(EncounterData encounterData)
        {
            Encounter = encounterData;

            foreach (var section in encounterData.Sections) {
                var sectionButton = CreateSectionButton(section.Value.Name, section.Key);

                var sectionImg = encounterData.Images[section.Key];
                if (sectionImg != null) {
                    //sectionButton.SetImage(sectionImg.sprite);
                    sectionButton.SetSprite(sectionImg.sprite);
                }
            }
        }

        private string currentSectionKey;
        /**
         * Returns the current section key
         */
        public string GetCurrentSectionKey()
        {
            return currentSectionKey;
        }

        /**
         * Returns the current section name
         */
        public SectionDataScript GetCurrentSection()
        {
            return Encounter.OldSections[currentSectionKey];
        }


        public virtual string CreateSection(string sectionName, Color color, Image icon)
        {
            var encounterData = WriterHandler.WriterInstance.EncounterData;
            string sectionKey = encounterData.Sections.Add(null); // TODO: this // new Section(sectionName, null, null));

            var sectionButton = CreateSectionButton(sectionName, sectionKey);
            sectionButton.SetImage(color);
            sectionButton.SetSprite(icon.sprite);

            var img = new SpriteHolderScript(icon.name) {
                useColor = true,
                color = color
            };
            encounterData.Images.AddSectionImg(sectionKey, img);

            return sectionKey;
        }

        [field: SerializeField] public Button AddSectionButton { get; set; }
        [field: SerializeField] public Transform SectionFiller { get; set; }
        protected virtual Transform SectionButtonParent => AddSectionButton.transform.parent;
        protected virtual int SectionButtonCount { get; set; }

        // move this all to TabManager or WriterHandler
        protected virtual SwapSectionScript CreateSectionButton(string sectionName, string key)
        {
            //Create the section button and link it accordingly
            GameObject newSection = Resources.Load(GlobalData.resourcePath + "/Prefabs/SectionButton") as GameObject;

            //Spawn in the section button
            newSection = Instantiate(newSection, SectionButtonParent);
            newSection.transform.SetSiblingIndex(SectionButtonCount++);

            var sectionSwapper = newSection.GetComponent<SwapSectionScript>();
            sectionSwapper.Initialize(sectionName, key);

            return sectionSwapper;
        }
    }
}