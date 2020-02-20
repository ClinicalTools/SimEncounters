using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderEncounterInfo
    {
        public ReaderEncounterInfo(ReaderScene reader, ReaderEncounterInfoUI encounterInfoUI, EncounterInfo encounterInfo)
        {
            encounterInfoUI.EncounterTitle.text = encounterInfo.Title;
            var infoPopup = new ReaderInfoPopup(reader, encounterInfoUI.InfoPopup, encounterInfo);
        }
    }
}