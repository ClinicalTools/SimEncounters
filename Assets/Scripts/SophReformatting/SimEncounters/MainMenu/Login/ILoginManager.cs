namespace ClinicalTools.SimEncounters.MainMenu
{
    public interface ILoginManager
    {
        WaitableResult<User> Login();
    }
}