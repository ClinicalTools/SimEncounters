using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SimEncounters
{
    public class ReaderHandler : EncounterHandler
    {
        public override IEnumerator ShowDefaultEncounter()
        {
            // UploadToServer should already be on the object
            var globalData = SophGlobalData.Instance;
            var encounterXml = globalData.EncounterXml;

            encounterXml.GetDemoCase().Start();

            // TODO: move the wait to where most accurate
            var ced = encounterXml.CurrentEncounterCed;
            var cei = encounterXml.CurrentEncounterCei;

            while (!ced.IsCompleted || !cei.IsCompleted)
                yield return null;

            EncounterData = new EncounterData(ced.Result, cei.Result);
            yield return null;
        }

        protected override void UpdatePatientImage(SpriteHolderScript imgData, Image img, GameObject sectionBtn)
        {
            throw new System.NotImplementedException();
        }
    }
}