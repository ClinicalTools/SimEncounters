using UnityEngine;
using UnityEngine.Networking;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class DeviceIdLogin : IDeviceIdLogin
    {
        public event LoggedInEventHandler LoggedIn;
        protected IWebAddress WebAddress { get; }
        protected UserParser UserParser { get; }
        public string DeviceId { get; set; }

        public DeviceIdLogin(IWebAddress webAddress, UserParser userParser)
        {
            WebAddress = webAddress;
            UserParser = userParser;
        }

        public void Begin()
        {
            WWWForm form = new WWWForm();
            form.AddField("ACTION", "checkSession");
            form.AddField("deviceid", DeviceId);

            var address = WebAddress.GetUrl("Login.php");
            var webRequest = UnityWebRequest.Post(address, form);
            var requestOperation = webRequest.SendWebRequest();
            requestOperation.completed += (asyncOperation) => RequestOperation_completed(webRequest);
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