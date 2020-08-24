using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public class TabStatusParser : ICharEnumeratorParser<TabStatus>
    {
        public TabStatus Parse(CharEnumerator enumerator)
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