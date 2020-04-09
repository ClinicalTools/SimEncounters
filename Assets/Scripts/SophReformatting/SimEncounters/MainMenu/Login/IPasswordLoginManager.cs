namespace ClinicalTools.SimEncounters.MainMenu
{
    public interface IPasswordLoginManager
    {
        WaitableResult<User> Login(string username, string email, string password);
    }
}