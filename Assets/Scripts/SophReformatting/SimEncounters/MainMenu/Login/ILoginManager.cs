namespace ClinicalTools.SimEncounters
{
    public interface ILoginManager
    {
        WaitableResult<User> Login();
    }
}