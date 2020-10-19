namespace ClinicalTools.SimEncounters
{
    public class ReaderSceneInfoSelector : Selector<ReaderSceneInfo>
    {
        protected ISelector<UserEncounterSelectedEventArgs> UserEncounterSelector { get; }
        public ReaderSceneInfoSelector(ISelector<UserEncounterSelectedEventArgs> userEncounterSelector)
            => UserEncounterSelector = userEncounterSelector;

        public override void Select(object sender, ReaderSceneInfo value)
        {
            base.Select(sender, value);
            UserEncounterSelector.Select(this, new UserEncounterSelectedEventArgs(value.Encounter));
        }
    }
}