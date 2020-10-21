namespace ClinicalTools.SimEncounters
{
    public class NewReaderEncounterInfoPopupUI : BaseReaderEncounterInfoPopup
    {
        public override void ShowEncounterInfo(UserEncounter userEncounter) => gameObject.SetActive(true);
    }
}