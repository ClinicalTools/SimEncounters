using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class AutoLogin : ILoginManager
    {
        protected IServerReader ServerReader { get; }
        protected IUrlBuilder WebAddress { get; }
        protected UserParser UserParser { get; }

        public AutoLogin(IServerReader serverReader, IUrlBuilder webAddress, UserParser userParser)
        {
            ServerReader = serverReader;
            WebAddress = webAddress;
            UserParser = userParser;
        }

        public WaitableResult<User> Login()
        {
            if (PlayerPrefs.GetInt("StayLoggedIn", 0) == 0)
                return new WaitableResult<User>(null, "Stay logged in is not true.", true);

            var webRequest = GetWebRequest();
            var serverResult = ServerReader.Begin(webRequest);

            var user = new WaitableResult<User>();
            serverResult.AddOnCompletedListener((result) => ProcessResults(user, result));

            return user;
        }

        protected virtual UnityWebRequest GetWebRequest()
        {
            var address = WebAddress.BuildUrl("Login.php");
            WWWForm form = new WWWForm();
            form.AddField("ACTION", "checkSession");
            form.AddField("deviceid", SystemInfo.deviceUniqueIdentifier);
            return UnityWebRequest.Post(address, form);
        }

        private void ProcessResults(WaitableResult<User> result, ServerResult2 serverResult)
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