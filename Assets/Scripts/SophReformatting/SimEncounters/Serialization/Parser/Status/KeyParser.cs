namespace ClinicalTools.SimEncounters
{
    public class KeyParser : ICharEnumeratorParser<string>
    {
        private const int KEY_LENGTH = 2;

        public string Parse(CharEnumerator enumerator)
        {
            var key = "";

            do {
                key += enumerator.Current;
            } while (enumerator.MoveNext() && key.Length != KEY_LENGTH);

            return key;
        }
    }
}