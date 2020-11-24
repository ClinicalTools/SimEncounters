namespace ClinicalTools.SimEncounters
{
    public class WriterSceneInfoSelector : Selector<WriterSceneInfoSelectedEventArgs>
    {
        protected ISelector<EncounterSelectedEventArgs> EncounterSelector { get; }
        public WriterSceneInfoSelector(ISelector<EncounterSelectedEventArgs> encounterSelector)
            => EncounterSelector = encounterSelector;

        public override void Select(object sender, WriterSceneInfoSelectedEventArgs eventArgs)
        {
            base.Select(sender, eventArgs);
            EncounterSelector.Select(this, new EncounterSelectedEventArgs(eventArgs.SceneInfo.Encounter));
        }
    }
    public class ReaderSceneInfoSelector : Selector<ReaderSceneInfoSelectedEventArgs>
    {
        protected ISelector<UserEncounterSelectedEventArgs> UserEncounterSelector { get; }
        public ReaderSceneInfoSelector(ISelector<UserEncounterSelectedEventArgs> userEncounterSelector)
            => UserEncounterSelector = userEncounterSelector;

        public override void Select(object sender, ReaderSceneInfoSelectedEventArgs eventArgs)
        {
            base.Select(sender, eventArgs);
            UserEncounterSelector.Select(this, new UserEncounterSelectedEventArgs(eventArgs.SceneInfo.Encounter));
        }
    }
    public class MenuSceneInfoSelector : Selector<MenuSceneInfoSelectedEventArgs>
    {
        
        public override void Select(object sender, MenuSceneInfoSelectedEventArgs eventArgs)
        {
            base.Select(sender, eventArgs);
        }
    }
}