using UnityEngine;

namespace ClinicalTools.SimEncounters.MainMenu
{
    public class AutoLogin : ILogin
    {
        public event LoggedInEventHandler LoggedIn;
        protected IDeviceIdLogin DeviceIdLogin { get; }

        public AutoLogin(IDeviceIdLogin deviceIdLogin)
        {
            DeviceIdLogin = deviceIdLogin;
        }

        public void Begin()
        {
            if (PlayerPrefs.GetInt("StayLoggedIn", 0) == 0) {
                LoggedIn?.Invoke(this, new LoggedInEventArgs("Stay logged in is not true."));
                return;
            }

            DeviceIdLogin.LoggedIn += DeviceIdLogin_LoggedIn;
            DeviceIdLogin.DeviceId = SystemInfo.deviceUniqueIdentifier;
            DeviceIdLogin.Begin();
        }

        private void DeviceIdLogin_LoggedIn(object sender, LoggedInEventArgs e)
        {

            LoggedIn?.Invoke(this, e);
        }
    }
}