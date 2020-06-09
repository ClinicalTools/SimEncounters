namespace ClinicalTools.SimEncounters
{
    public interface IPasswordLoginManager
    {
        WaitableResult<User> Login(string username, string email, string password);
    }
}