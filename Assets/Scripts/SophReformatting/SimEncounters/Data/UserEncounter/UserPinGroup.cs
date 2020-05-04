namespace ClinicalTools.SimEncounters.Data
{
    public class UserPinGroup
    {
        public UserEncounter Encounter { get; }
        public PinData Data { get; }

        public UserDialoguePin DialoguePin { get; }
        public UserQuizPin QuizPin { get; }
        public UserPinGroup(UserEncounter encounter, PinData pinGroup)
        {
            Encounter = encounter;
            Data = pinGroup;

            if (pinGroup.Dialogue != null)
                DialoguePin = new UserDialoguePin(encounter, pinGroup.Dialogue);
            if (pinGroup.Quiz != null)
                QuizPin = new UserQuizPin(encounter, pinGroup.Quiz);
        }
    }
}