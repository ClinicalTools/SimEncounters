namespace ClinicalTools.SimEncounters
{
    public abstract class BaseInitialLoginHandler : BaseLoginHandler
    {
        public abstract WaitableResult<User> InitialLogin(ILoadingScreen loadingScreen);
    }
}