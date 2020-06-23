using System;
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

        bool isInjected = false;
        protected ILoginManager AutoLogin { get; set; }
        protected StayLoggedIn StayLoggedIn { get; set; }
        [Inject]
        public virtual void Inject(StayLoggedIn stayLoggedIn, ILoginManager autoLogin)
        {
            isInjected = true;
            Debug.Log("injected");
            StayLoggedIn = stayLoggedIn;
            AutoLogin = autoLogin;
        }

        protected virtual WaitableResult<User> CurrentWaitableResult { get; set; }
        public override WaitableResult<User> InitialLogin(ILoadingScreen loadingScreen)
        {
            Debug.Log($"Is injected: {isInjected}");
            if (CurrentWaitableResult == null || CurrentWaitableResult.IsCompleted)
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

            if (CurrentWaitableResult == null || CurrentWaitableResult.IsCompleted)
                CurrentWaitableResult = new WaitableResult<User>();

            ShowManualLogin();

            return CurrentWaitableResult;
        }

        protected virtual void TryAutoLogin()
        {
            var autoLoginResult = AutoLogin.Login();
            autoLoginResult.AddOnCompletedListener(ProcessAutoLoginResult);
        }
        protected virtual void ProcessAutoLoginResult(User user)
        {
            if (user == null)
                ShowManualLogin();
            else
                CurrentWaitableResult.SetResult(user);
        }

        protected virtual void ShowManualLogin()
        {
            gameObject.SetActive(true);
            var loginResult = ManualLogin.Login();
            loginResult.AddOnCompletedListener(ProcessManualLoginResult);
        }
        protected virtual void ProcessManualLoginResult(User user) => CurrentWaitableResult.SetResult(user);
    }
}