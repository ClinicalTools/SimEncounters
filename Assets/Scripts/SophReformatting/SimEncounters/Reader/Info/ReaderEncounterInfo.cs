using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderEncounterInfo
    {
        public ReaderEncounterInfo(EncounterReader reader, ReaderEncounterInfoUI encounterInfoUI, EncounterInfo encounterInfo)
        {
            encounterInfoUI.EncounterTitle.text = encounterInfo.Title;
            var infoPopup = new ReaderInfoPopup(reader, encounterInfoUI.InfoPopup, encounterInfo);
        }
    }
}