namespace ClinicalTools.SimEncounters.MainMenu
{
    public delegate void LoggedInEventHandler(object sender, LoggedInEventArgs e);
    public class LoggedInEventArgs
    {
        public User User { get; }
        public string Message { get; }
        public LoggedInEventArgs(User user)
        {
            User = user;
        }
        public LoggedInEventArgs(string message)
        {
            Message = message;
        }
        public LoggedInEventArgs(User user, string message)
        {
            User = user;
            Message = message;
        }
    }

    public interface ILogin
    {
        event LoggedInEventHandler LoggedIn;

        void Begin();
    }
}