namespace ClinicalTools.SimEncounters
{
    public class UserEncounterMenuSceneStarter : UserMenuSceneStarter, IUserEncounterMenuSceneStarter
    {
        public UserEncounterMenuSceneStarter(
               IMenuSceneStarter menuSceneStarter,
               IMenuEncountersInfoReader menuInfoReader,
               BaseConfirmationPopup confirmationPopup)
            : base(menuSceneStarter, menuInfoReader, confirmationPopup) { }

        public void StartMenuScene(UserEncounter userEncounter, ILoadingScreen loadingScreen)
        {
            var categories = GetMenuEncountersInfo(userEncounter);
            var menuSceneInfo = new LoadingMenuSceneInfo(userEncounter.User, loadingScreen, categories);
            MenuSceneStarter.StartScene(menuSceneInfo);
        }

        protected UserEncounter UserEncounter { get; set; }

        public void ConfirmStartingMenuScene(UserEncounter userEncounter, ILoadingScreen loadingScreen)
        {
            UserEncounter = userEncounter;
            LoadingScreen = loadingScreen;
            ConfirmationPopup.ShowConfirmation(ExitSceneWithEncounter, ExitConfirmationTitle,
                ExitConfirmationDescription);
        }

        protected virtual void ExitSceneWithEncounter() => StartMenuScene(UserEncounter, LoadingScreen);

        protected virtual WaitableTask<IMenuEncountersInfo> GetMenuEncountersInfo(
            UserEncounter userEncounter)
        {
            var task = new WaitableTask<IMenuEncountersInfo>();
            var categories = MenuInfoReader.GetMenuEncountersInfo(userEncounter.User);
            categories.AddOnCompletedListener((result) => 
                CompleteMenuEncountersInfoTask(task, result, userEncounter));
            return task;
        }

        protected virtual void CompleteMenuEncountersInfoTask(WaitableTask<IMenuEncountersInfo> task, 
            TaskResult<IMenuEncountersInfo> result, UserEncounter userEncounter)
        {
            userEncounter.Status.BasicStatus.Completed = userEncounter.Status.ContentStatus.Read;

            if (result.IsError()) { 
                task.SetError(result.Exception);
                return;
            }
            if (result.Value == null) { 
                task.SetResult(null);
                return;
            }
            foreach (var encounter in result.Value.GetEncounters()) {
                if (encounter.GetLatestMetadata().RecordNumber != userEncounter.Data.Metadata.RecordNumber)
                    continue;
                encounter.Status = userEncounter.Status.BasicStatus;
                break;
            }
            task.SetResult(result.Value);
        }
    }
}
