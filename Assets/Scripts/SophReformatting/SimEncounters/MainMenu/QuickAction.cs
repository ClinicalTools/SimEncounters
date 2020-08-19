using ImaginationOverflow.UniversalDeepLinking;

namespace ClinicalTools.SimEncounters
{
    public enum QuickActionType { NA, MainMenu, Reader }
    public class QuickAction
    {
        public QuickActionType Action { get; }
        public int EncounterId { get; }

        public QuickAction()
        {
            Action = QuickActionType.NA;
        }
        public QuickAction(QuickActionType action)
        {
            Action = action;
        }
        public QuickAction(QuickActionType action, int id)
        {
            Action = action;
            EncounterId = id;
        }
    }
    public class QuickActionFactory
    {
        private const string RecordNumberKey = "id";
        public virtual QuickAction GetLinkAction(LinkActivation linkAction)
        {
            if (!linkAction.QueryString.ContainsKey(RecordNumberKey))
                return new QuickAction();

            var recordNumberStr = linkAction.QueryString[RecordNumberKey];
            if (!int.TryParse(recordNumberStr, out int recordNumber))
                return new QuickAction();

            return new QuickAction(QuickActionType.Reader, recordNumber);
        }
    }
}