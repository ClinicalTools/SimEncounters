using System;
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

        private void ProcessResults(WaitableResult<User> result, WaitedResult<string> serverResult)
        {
            if (serverResult.Value == null || serverResult.IsError()) {
                result.SetError(serverResult.Exception);
                return;
            }

            var user = UserParser.Parse(serverResult.Value);
            if (user == null)
                result.SetError(new Exception($"Could not parse user: {serverResult.Value}"));
            else
                result.SetResult(user);
        }
    }
}