namespace ClinicalTools.SimEncounters
{
    public interface IPasswordLoginHandler
    {
        WaitableResult<User> Login(string username, string email, string password);
    }
}