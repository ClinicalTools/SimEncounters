namespace ClinicalTools.SimEncounters
{
    public class UserEncounterSelector : Selector<UserEncounterSelectedEventArgs>
    {
        protected ISelector<Encounter> EncounterSelector { get; }
        protected ISelector<UserSectionSelectedEventArgs> UserSectionSelector { get; }
        public UserEncounterSelector(
            ISelector<UserSectionSelectedEventArgs> userSectionSelector,
            ISelector<Encounter> encounterSelector)
        {
            UserSectionSelector = userSectionSelector;
            EncounterSelector = encounterSelector;
        }
        public override void Select(object sender, UserEncounterSelectedEventArgs value)
        {
            base.Select(sender, value);
            var sectionArgs = new UserSectionSelectedEventArgs(value.Encounter.GetCurrentSection(), ChangeType.JumpTo);
            UserSectionSelector.Select(this, sectionArgs);
            EncounterSelector.Select(this, value.Encounter.Data);
        }
    }
}