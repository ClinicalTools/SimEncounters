using ClinicalTools.SimEncounters.Data;

namespace ClinicalTools.SimEncounters
{
    public class EncounterContentStatusParser : IParser<EncounterContentStatus>, ICharEnumeratorParser<EncounterContentStatus>
    {
        private readonly ICharEnumeratorParser<SectionStatus> sectionStatusParser;
        private readonly ICharEnumeratorParser<string> keyParser;
        public EncounterContentStatusParser(ICharEnumeratorParser<SectionStatus> sectionStatusParser,
            ICharEnumeratorParser<string> keyParser)
        {
            this.sectionStatusParser = sectionStatusParser;
            this.keyParser = keyParser;
        }

        public EncounterContentStatus Parse(string text)
        {
            var enumerator = new CharEnumerator(text);
            return Parse(enumerator);
        }

        public EncounterContentStatus Parse(CharEnumerator enumerator)
        {
            var status = new EncounterContentStatus();

            if (!enumerator.MoveNext())
                return status;

            status.Read = enumerator.Current == '1';

            while (enumerator.MoveNext()) {
                var sectionKey = keyParser.Parse(enumerator);
                var sectionStatus = sectionStatusParser.Parse(enumerator);
                status.AddSectionStatus(sectionKey, sectionStatus);
            }

            return status;
        }
    }
}