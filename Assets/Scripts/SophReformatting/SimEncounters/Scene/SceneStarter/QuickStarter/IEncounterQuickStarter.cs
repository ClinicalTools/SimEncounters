namespace ClinicalTools.SimEncounters
{
    public interface IEncounterQuickStarter
    {
        void StartEncounter(User user, ILoadingScreen loadingScreen, int recordNumber);
        void StartEncounter(User user, ILoadingScreen loadingScreen, WaitableResult<IMenuEncountersInfo> encounters, int recordNumber);
    }
}