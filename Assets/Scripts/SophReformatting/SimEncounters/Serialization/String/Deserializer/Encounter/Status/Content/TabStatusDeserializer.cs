namespace ClinicalTools.SimEncounters
{
    public class TabStatusDeserializer : ICharEnumeratorDeserializer<TabStatus>
    {
        public TabStatus Deserialize(CharEnumerator enumerator)
        {
            var status = new TabStatus();
            if (enumerator.IsDone)
                return status;

            status.Read = enumerator.Current == '1';
            enumerator.MoveNext();

            return status;
        }
    }
}