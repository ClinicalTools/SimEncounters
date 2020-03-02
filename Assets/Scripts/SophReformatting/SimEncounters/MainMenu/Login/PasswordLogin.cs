using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class PasswordLogin : IPasswordLogin
    {
        public event LoggedInEventHandler LoggedIn;

        protected IWebAddress WebAddress { get; }
        protected UserParser UserParser { get; }

        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public PasswordLogin(IWebAddress webAddress, UserParser userParser)
        {
            WebAddress = webAddress;
            UserParser = userParser;
        }


        protected virtual LoggedInEventArgs NoUsernameOrEmailArgs { get; } = new LoggedInEventArgs("No username or email provided.");
        protected virtual LoggedInEventArgs NoPasswordArgs { get; } = new LoggedInEventArgs("No password provided.");
        public void Begin()
        {
            var form = CreateForm();
            if (form == null)
                return;

            var address = WebAddress.GetUrl("Login.php");
            var webRequest = UnityWebRequest.Post(address, form);
            var requestOperation = webRequest.SendWebRequest();
            requestOperation.completed += (asyncOperation) => RequestOperation_completed(webRequest);
        }

        protected virtual WWWForm CreateForm()
        {

            if (string.IsNullOrWhiteSpace(Username) && string.IsNullOrWhiteSpace(Email)) {
                LoggedIn?.Invoke(this, NoUsernameOrEmailArgs);
                return null;
            }
            if (string.IsNullOrWhiteSpace(Password)) {
                LoggedIn?.Invoke(this, NoPasswordArgs);
                return null;
            }

            WWWForm form = new WWWForm();
            form.AddField("ACTION", "login");
            if (Email != null)
                form.AddField("email", Email);
            if (Username != null)
                form.AddField("username", Username);
            form.AddField("password", Password);

            return form;
        }

        private void RequestOperation_completed(UnityWebRequest webRequest)
        {
            if (!string.IsNullOrWhiteSpace(webRequest.error)) {
                LoggedIn?.Invoke(this, new LoggedInEventArgs(webRequest.error));
                return;
            }

            var user = UserParser.Parse(webRequest.downloadHandler.text);
            if (user == null)
                LoggedIn?.Invoke(this, new LoggedInEventArgs($"Could not parse user: {webRequest.downloadHandler.text}"));
            else
                LoggedIn?.Invoke(this, new LoggedInEventArgs(user));
        }
    }
}