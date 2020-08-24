using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public class SectionStatusParser : ICharEnumeratorParser<SectionStatus>
    {
        private const char END_CHAR = ' ';

        private readonly ICharEnumeratorParser<TabStatus> tabStatusParser;
        private readonly ICharEnumeratorParser<string> keyParser;
        public SectionStatusParser(
            ICharEnumeratorParser<TabStatus> tabStatusParser,
            ICharEnumeratorParser<string> keyParser)
        {
            this.tabStatusParser = tabStatusParser;
            this.keyParser = keyParser;
        }

        public SectionStatus Parse(CharEnumerator enumerator)
        {
            var status = new SectionStatus();

            if (enumerator.IsDone)
                return status;

            status.Read = enumerator.Current == '1';

            while (enumerator.MoveNext() && enumerator.Current != END_CHAR) {
                var sectionKey = keyParser.Parse(enumerator);
                var sectionStatus = tabStatusParser.Parse(enumerator);
                status.AddTabStatus(sectionKey, sectionStatus);
            }

            return status;
        }
    }
}