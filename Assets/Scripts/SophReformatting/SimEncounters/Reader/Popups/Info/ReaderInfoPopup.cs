using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters.Reader
{
    public class ReaderInfoPopup : ReaderPopup
    {
        public ReaderInfoPopup(EncounterReader reader, ReaderInfoPopupUI infoPopupUI, EncounterInfo encounterInfo) : base(reader, infoPopupUI)
        {
            infoPopupUI.Title.text = encounterInfo.Title;
            infoPopupUI.Subtitle.text = encounterInfo.Subtitle;
            infoPopupUI.Description.text = encounterInfo.Description;
            infoPopupUI.Categories.text = string.Join(", ", encounterInfo.Categories); ;
            infoPopupUI.Audience.text = encounterInfo.Audience;
            var difficulty = new ReaderDifficulty(reader, infoPopupUI.Difficulty, encounterInfo.Difficulty);
        }
    }
}