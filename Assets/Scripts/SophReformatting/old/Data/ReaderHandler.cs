using ClinicalTools.SimEncounters;
using ClinicalTools.SimEncounters.Loading;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace ClinicalTools.SimEncountersOld
{
    public class ReaderHandler : EncounterHandler
    {
        public override void GetDefaultEncounter(EncounterXml encounterXml)
        {
            encounterXml.GetDemoCase();//.Start();
        }

        protected override void UpdatePatientImage(SpriteHolderScript imgData, Image img, GameObject sectionBtn)
        {
            throw new System.NotImplementedException();
        }
    }
}