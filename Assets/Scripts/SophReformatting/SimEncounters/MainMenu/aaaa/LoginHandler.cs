using UnityEngine;
using Zenject;

namespace ClinicalTools.SimEncounters
{
    public class StayLoggedIn
    {
        private const string PLAYER_PREF_KEY = "StayLoggedIn";

        private const int FALSE_VALUE = 0;
        private const int TRUE_VALUE = 1;

        public bool Value => PlayerPrefs.GetInt(PLAYER_PREF_KEY) == TRUE_VALUE;
        public void SetValue(bool value) => PlayerPrefs.SetInt(PLAYER_PREF_KEY, value ? TRUE_VALUE : FALSE_VALUE);
    }

    public abstract class SomethingSomethingLogin : MonoBehaviour
    {
        public abstract WaitableResult<User> InitialLogin(ILoadingScreen loadingScreen);
        public abstract WaitableResult<User> Login();
    }

    public class LoginHandler : SomethingSomethingLogin
    {
        public ManualLogin ManualLogin { get => manualLogin; set => manualLogin = value; }
        [SerializeField] private ManualLogin manualLogin;

        protected ILoginManager AutoLogin { get; set; }
        protected StayLoggedIn StayLoggedIn { get; set; }
        [Inject]
        public virtual void Inject(StayLoggedIn stayLoggedIn, ILoginManager autoLogin)
        {
            StayLoggedIn = stayLoggedIn;
            AutoLogin = autoLogin;
        }

        protected virtual WaitableResult<User> CurrentWaitableResult { get; set; }
        public override WaitableResult<User> InitialLogin(ILoadingScreen loadingScreen)
        {
            CurrentWaitableResult = new WaitableResult<User>();

            if (StayLoggedIn.Value)
                TryAutoLogin();
            else
                ShowManualLogin();

            return CurrentWaitableResult;
        }

        public override WaitableResult<User> Login()
        {
            StayLoggedIn.SetValue(false);

            CurrentWaitableResult = new WaitableResult<User>();

            ShowManualLogin();

            return CurrentWaitableResult;
        }

        protected virtual void TryAutoLogin()
        {
            var autoLoginResult = AutoLogin.Login();
            autoLoginResult.AddOnCompletedListener(ProcessAutoLoginResult);
        }
        protected virtual void ProcessAutoLoginResult(WaitedResult<User> user)
        {
            if (user.IsError() || user.Value == null)
                ShowManualLogin();
            else
                CurrentWaitableResult.SetResult(user.Value);
        }

        protected virtual void ShowManualLogin()
        {
            gameObject.SetActive(true);
            var loginResult = ManualLogin.Login();
            loginResult.AddOnCompletedListener(ProcessManualLoginResult);
        }
        protected virtual void ProcessManualLoginResult(WaitedResult<User> user) => CurrentWaitableResult.SetResult(user.Value);
    }
}