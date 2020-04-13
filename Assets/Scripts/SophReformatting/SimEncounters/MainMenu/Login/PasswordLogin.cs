using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class PasswordLogin : IPasswordLoginManager
    {
        protected IServerReader ServerReader { get; }
        protected IUrlBuilder WebAddress { get; }
        protected UserParser UserParser { get; }

        public PasswordLogin(IServerReader serverReader, IUrlBuilder webAddress, UserParser userParser)
        {
            ServerReader = serverReader;
            WebAddress = webAddress;
            UserParser = userParser;
        }


        protected virtual string NoUsernameOrEmailArgs { get; } = "No username or email provided.";
        protected virtual string NoPasswordArgs { get; } = "No password provided.";
        public WaitableResult<User> Login(string username, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(email))
                return new WaitableResult<User>(null, NoUsernameOrEmailArgs, true);
            if (string.IsNullOrWhiteSpace(password))
                return new WaitableResult<User>(null, NoPasswordArgs, true);

            var form = CreateForm(username, email, password);

            var address = WebAddress.BuildUrl("Login.php");
            var webRequest = UnityWebRequest.Post(address, form);
            var serverResult = ServerReader.Begin(webRequest);

            var user = new WaitableResult<User>();
            serverResult.AddOnCompletedListener((result) => ProcessResults(user, result));

            return user;
        }

        protected virtual WWWForm CreateForm(string username, string email, string password)
        {
            WWWForm form = new WWWForm();
            form.AddField("ACTION", "login");
            if (email != null)
                form.AddField("email", email);
            if (username != null)
                form.AddField("username", username);
            form.AddField("password", password);

            return form;
        }

        private void ProcessResults(WaitableResult<User> result, ServerResult serverResult)
        {
            if (serverResult.Outcome != ServerOutcome.Success)
            {
                result.SetError(serverResult.Message);
                return;
            }

            var user = UserParser.Parse(serverResult.Message);
            if (user == null)
                result.SetError($"Could not parse user: {serverResult.Message}");
            else
                result.SetResult(user);
        }
    }
}