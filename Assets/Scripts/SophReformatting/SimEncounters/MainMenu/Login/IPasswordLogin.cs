namespace ClinicalTools.SimEncounters.MainMenu
{
    public interface IPasswordLogin : ILogin
    {
        string Email { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}