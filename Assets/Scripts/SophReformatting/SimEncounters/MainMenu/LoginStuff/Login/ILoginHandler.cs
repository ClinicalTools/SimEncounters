namespace ClinicalTools.SimEncounters
{
    public interface ILoginHandler
    {
        WaitableResult<User> Login();
    }
}