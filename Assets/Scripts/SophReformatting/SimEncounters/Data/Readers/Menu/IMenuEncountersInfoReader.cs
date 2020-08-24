namespace ClinicalTools.SimEncounters
{
    public interface IMenuEncountersInfoReader
    {
        WaitableResult<IMenuEncountersInfo> GetMenuEncountersInfo(User user);
    }
}